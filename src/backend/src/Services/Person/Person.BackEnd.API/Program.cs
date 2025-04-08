var builder = WebApplication.CreateBuilder(args);
var assembly = Assembly.GetExecutingAssembly();

// Add services to the container.
ConfigureServices(builder.Services, builder.Configuration, assembly);

var app = builder.Build();

ConfigureMiddleware(app);
app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration, Assembly assembly)
{
    string? connectionString;

    // Add MediatR
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(assembly);
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        cfg.AddOpenBehavior(typeof(LogginBehavior<,>));
    });

    // Add Validators
    builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

    // Add Carter
    services.AddCarter();

    // Add Exception Handler
    services.AddExceptionHandler<CustomExceptionHandler>();

    // Register the AddVaultService class as a service
    builder.Services.AddVaultService(builder.Configuration);

    using (var scope = builder.Services.BuildServiceProvider().CreateScope())
    {
        var configProvider = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        // Get base configuration
        var server = configProvider["DatabaseConfig:server"];
        var port = configProvider["DatabaseConfig:port"];
        var database = configProvider["DatabaseConfig:database"];

        var usernamePasswordCredentials = scope.ServiceProvider.GetRequiredService<ISecretManager>()
            .GetPostgreSQLCredential<UsernamePasswordCredentials>();

        // Assemble the connection string
        connectionString =
            $"Server={server};Port={port};Database={database};User Id={usernamePasswordCredentials.Result.Username};Password={usernamePasswordCredentials.Result.Password};Include Error Detail=true";
    }

    // Add Marten
    builder.Services
        .AddMarten(opts => { opts.Connection(connectionString); })
        .UseLightweightSessions();

    // Add Serilog
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    builder.Host.UseSerilog();

    if (builder.Environment.IsDevelopment()) builder.Services.InitializeMartenWith<PersonInitialData>();

    // Add Health Checks
    services
        .AddHealthChecks()
        .AddApplicationStatus("api_status", tags: new[] { "api" })
        .AddNpgSql(connectionString!,
            name: "sql",
            failureStatus: HealthStatus.Degraded,
            tags: new[] { "db", "sql", "sqlserver" });

    // Add Discovery
    builder.Services.AddDiscovery(builder.Configuration);

    // Add Graylog
    builder.Services.AddGraylogLogging(builder.Configuration);

    // Add Email
    builder.Services.AddEmail(builder.Configuration);

    // Add Controllers
    builder.Services.AddControllers();

    // Add Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add Prometheus
    builder.Services.UseHttpClientMetrics();
    builder.Services.AddOpenTelemetry()
        .ConfigureResource(resource => resource.AddService(assembly.FullName))
        .WithMetrics(opt =>
            opt
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Member.API"))
                .AddMeter("Member.API.Metrics")
                .AddAspNetCoreInstrumentation() // ASP.NET Core related
                .AddRuntimeInstrumentation() // .NET Runtime metrics like - GC, Memory Pressure, Heap Leaks etc
                .AddPrometheusExporter() // Prometheus Exporter
                .AddProcessInstrumentation()
                .AddOtlpExporter(opt => { opt.Endpoint = new Uri(builder.Configuration["Otel:Endpoint"]); }));
}

void ConfigureMiddleware(WebApplication app)
{
    // Map Carter Endpoints
    app.MapCarter();

    // Configure Swagger for Development
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Person.BackEnd.API v1"));
        // app.UseSwagger(c =>
        // {
        //     // Esto asegura que la versión de OpenAPI esté correctamente especificada
        //     c.SerializeAsV2 = false; // Usa OpenAPI 3.0 (por defecto)
        // });
        //
        // app.UseSwaggerUI(c =>
        // {
        //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Membership API v1");
        //     c.RoutePrefix = string.Empty; // Para servir la UI de Swagger en la raíz
        // });
    }

    // Add Middleware
    app.UseHttpsRedirection();
    app.UseAuthorization();

    // Middleware para Prometheus
    app.UseMetricServer();
    app.MapMetrics(); // Exponer las métricas en "/metrics"
    app.UseHttpMetrics(); // Mide las solicitudes HTTP automáticamente

    // Map Controllers
    app.MapControllers();

    // Use Exception Handler
    app.UseExceptionHandler(options => { });

    // Add Health Checks
    app.UseHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    // Map the /metrics endpoint
    app.UseOpenTelemetryPrometheusScrapingEndpoint();
}