﻿global using System;
global using System.Reflection;
global using BuildingBlocks.Behaviors;
global using BuildingBlocks.CQRS;
global using BuildingBlocks.Exceptions;
global using BuildingBlocks.Exceptions.Handler;
global using Carter;
global using FluentValidation;
global using HealthChecks.ApplicationStatus.DependencyInjection;
global using HealthChecks.UI.Client;
global using Mapster;
global using Marten;
global using Marten.Pagination;
global using Marten.Schema;
global using MediatR;
global using Membership.Shared.Discovery;
global using Membership.Shared.Email;
global using Membership.Shared.Logging;
global using Membership.Shared.Secrets;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using OpenTelemetry.Metrics;
global using OpenTelemetry.Resources;
global using Person.BackEnd.API.Data;
global using Person.BackEnd.API.Exceptions;
global using Person.BackEnd.API.Models;
global using Prometheus;
global using Serilog;
global using VaultSharp.V1.SecretsEngines;