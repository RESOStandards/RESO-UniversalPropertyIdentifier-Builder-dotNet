namespace Reso.Upi.Core.US
{
    public interface ICountryUpi
    {
        string CountryName { get; }
        string ToUpi();
        string Description { get; }
        bool IsValid();
    }
}