using System.Collections.Generic;

namespace Reso.Upi.Core.US
{
    public interface ICountryUpi
    {
        string CountryName { get; }
        string ToUpi();
        string Description { get; }
        bool IsValid();

        IsoCountry Country { get; }
        string SubCountry { get; }
        string Property { get; }
        SubPropertyType PropertyType { get;  }
        string SubProperty { get;  }
    }

    public class InvalidCountry : CountryUpi
    {
        public override bool IsValid()
        {
            return false;
        }

        public InvalidCountry(string invalidNotes)
        {
            Description = invalidNotes;
        }
    }

    public abstract class CountryUpi: ICountryUpi
    {
        protected Dictionary<string, string> Components { get; set; } = new Dictionary<string, string>();

        protected CountryUpi()
        { }

        protected CountryUpi(IsoCountry country)
        {
            Country = country;
        }

        public string CountryName { get; protected set; }

        public virtual string Description { get; protected set; }

        public IsoCountry Country { get; protected set; }
        public string SubCountry { get; protected set; }
        public string Property { get; protected set; }

        // RESO-defined sub property type
        public SubPropertyType PropertyType { get; protected set; }

        public string SubProperty { get; protected set; }

        public abstract bool IsValid();

        public virtual string ToUpi()
        {
            return string.Empty;
        }
    }
}