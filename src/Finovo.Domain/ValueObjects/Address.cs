namespace Finovo.Domain.ValueObjects;

public record Address
{
    public string Street { get; }
    public string City { get; }
    public string ZipCode { get; }
    public string Country { get; }

    public Address(string street, string city, string zipCode, string country)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(street);
        ArgumentException.ThrowIfNullOrWhiteSpace(city);
        ArgumentException.ThrowIfNullOrWhiteSpace(zipCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(country);

        Street = street.Trim();
        City = city.Trim();
        ZipCode = zipCode.Trim();
        Country = country.Trim();
    }

    public override string ToString() => $"{Street}, {ZipCode} {City}, {Country}";
}
