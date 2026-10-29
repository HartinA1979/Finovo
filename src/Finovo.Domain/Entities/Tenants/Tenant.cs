using Finovo.Domain.Common;
using Finovo.Domain.Enums;
using Finovo.Domain.ValueObjects;

namespace Finovo.Domain.Entities.Tenants;

public class Tenant : AggregateRoot
{
    public string Name { get; private set; }
    public string Subdomain { get; private set; }
    public LicenseStatus LicenseStatus { get; private set; }
    public DateTime? LicenseExpiresAt { get; private set; }
    public VatCountry VatCountry { get; private set; }

    // Company details for invoice header
    public string? CompanyName { get; private set; }
    public Email? Email { get; private set; }
    public string? Phone { get; private set; }
    public Address? Address { get; private set; }
    public string? VatNumber { get; private set; }

    private Tenant() { Name = string.Empty; Subdomain = string.Empty; } // EF Core

    public Tenant(string name, string subdomain, VatCountry vatCountry = VatCountry.Austria, LicenseStatus licenseStatus = LicenseStatus.Trial)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(subdomain);

        Name = name;
        Subdomain = subdomain.ToLowerInvariant().Trim();
        VatCountry = vatCountry;
        LicenseStatus = licenseStatus;
    }

    public void UpdateLicense(LicenseStatus status, DateTime? expiresAt = null)
    {
        LicenseStatus = status;
        LicenseExpiresAt = expiresAt;
        SetUpdated();
    }

    public void UpdateCompanyDetails(
        string name,
        string? companyName,
        Email? email,
        string? phone,
        Address? address,
        string? vatNumber,
        VatCountry vatCountry)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        CompanyName = companyName;
        Email = email;
        Phone = phone;
        Address = address;
        VatNumber = vatNumber;
        VatCountry = vatCountry;
        SetUpdated();
    }

    public bool IsActive =>
        LicenseStatus is LicenseStatus.Trial or LicenseStatus.Active
        && (LicenseExpiresAt == null || LicenseExpiresAt > DateTime.UtcNow);
}
