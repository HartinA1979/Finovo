namespace Finovo.Domain.ValueObjects;

/// <summary>
/// Format: RE-{YEAR}-{SEQUENCE:D4}, e.g. RE-2025-0001. Unique per tenant per year.
/// </summary>
public record InvoiceNumber
{
    public int Year { get; }
    public int SequenceNumber { get; }

    public InvoiceNumber(int year, int sequenceNumber)
    {
        if (year < 2000 || year > 9999)
            throw new ArgumentException("Year must be between 2000 and 9999.", nameof(year));
        if (sequenceNumber < 1)
            throw new ArgumentException("Sequence number must be at least 1.", nameof(sequenceNumber));

        Year = year;
        SequenceNumber = sequenceNumber;
    }

    public override string ToString() => $"RE-{Year}-{SequenceNumber:D4}";
}
