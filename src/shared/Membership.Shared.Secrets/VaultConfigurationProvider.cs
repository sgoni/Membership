﻿using Membership.Shared.Secrets.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines;

namespace Membership.Shared.Secrets;

public interface ISecretManager
{
    Task<T> GetCredential<T>(string path) where T : new();
    Task<UsernamePasswordCredentials> GetPostgreSQLCredential<T>() where T : new();
    Task<UsernamePasswordCredentials> GetRabbitMQCredential<T>(string path) where T : new();
}

public class VaultConfigurationProvider : ISecretManager
{
    private readonly ILogger<VaultConfigurationProvider> _logger;
    private readonly VaultProviderSettings _vaultProviderSettings;

    public VaultConfigurationProvider(IOptions<VaultProviderSettings> options,
        ILogger<VaultConfigurationProvider> logger)
    {
        _logger = logger;
        _vaultProviderSettings = options.Value with { TokenApi = GetTokenFromEnvironmentVariable() };
    }

    public async Task<T> GetCredential<T>(string path) where T : new()
    {
        // Configure the Vault client settings, providing the Vault URL and authentication token
        var authMethod = new TokenAuthMethodInfo(_vaultProviderSettings.TokenApi);
        var vaultClientSettings =
            new VaultClientSettings(_vaultProviderSettings.Address, authMethod);

        // Create an instance of the Vault client using the above configuration
        var vaultClient = new VaultClient(vaultClientSettings);

        // Read the secret in the specific path inside Vault (using KV engine version 2)
        // A mountPoint is specified, which is the point where the secrets engine is mounted
        Secret<SecretData> secret =
            await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path,
                mountPoint: _vaultProviderSettings.MountPath + "secrets");

        // Store the data obtained from the secret in a dictionary (key-value)
        var secretValue = secret.Data.Data;

        // The ToDictionary() method does not exist in C#. 
        // If you wanted to convert it to a generic object type T, you would implement a cast
        // Example: return secretValue.ToObject<T>();
        return
            secretValue.ToObject<T>(); // Asume que tienes un método de extensión para convertir el diccionario a un objeto
    }

    public async Task<UsernamePasswordCredentials> GetPostgreSQLCredential<T>() where T : new()
    {
        // Configure the Vault client settings, providing the Vault URL and authentication token
        var authMethod = new TokenAuthMethodInfo(_vaultProviderSettings.TokenApi);
        var vaultClientSettings =
            new VaultClientSettings(_vaultProviderSettings.Address, authMethod);

        // Create an instance of the Vault client using the above configuration
        var vaultClient = new VaultClient(vaultClientSettings);

        Secret<UsernamePasswordCredentials> secret =
            await vaultClient.V1.Secrets.Database.GetCredentialsAsync(
                _vaultProviderSettings.Role, null, null);

        _logger.LogInformation("Credentials obtained from Vault");
        _logger.LogInformation($"secret.Data.Username: {secret.Data.Username}");
        _logger.LogInformation($"secret.Data.Password: {secret.Data.Password}");

        return secret.Data;
    }

    public async Task<UsernamePasswordCredentials> GetRabbitMQCredential<T>(string roleName) where T : new()
    {
        // Configure the Vault client settings, providing the Vault URL and authentication token
        var authMethod = new TokenAuthMethodInfo(_vaultProviderSettings.TokenApi);
        var vaultClientSettings =
            new VaultClientSettings(_vaultProviderSettings.Address, authMethod);

        // Create an instance of the Vault client using the above configuration
        var vaultClient = new VaultClient(vaultClientSettings);

        Secret<UsernamePasswordCredentials> secret =
            await vaultClient.V1.Secrets.RabbitMQ.GetCredentialsAsync(roleName, "rabbitmq");

        return secret.Data;
    }

    private string GetTokenFromEnvironmentVariable()
    {
        return Environment.GetEnvironmentVariable("VAULT_TOKEN") ??
               throw new NotImplementedException("No vault token found");
    }
}