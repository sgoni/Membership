﻿namespace Membership.Shared.Secrets;

public record VaultProviderSettings
{
    public string? Address { get; set; }
    public string? Role { get; set; }
    public string? MountPath { get; set; }
    public string? SecretType { get; set; }
    public string? TokenApi { get; set; }
}