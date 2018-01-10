using System.Collections.Generic;

namespace Reso.Upi.Core.US
{
    public interface ICountryUpi
    {
        string CountryName { get; }
        string ToUpi();
        string Description { get; }
        bool IsValid();

        IsoCountryCode Country { get; }
        string SubCountry { get; }
        string Property { get; }
        SubPropertyTypeCode PropertyType { get;  }
        string SubProperty { get;  }
    }

    public sealed class InvalidCountry : CountryUpi
    {
        public override bool IsValid()
        {
            return false;
        }

        public override string SubCountry { get { return "INVALID";  } }

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

        protected CountryUpi(IsoCountryCode country)
        {
            Country = country;
        }

        protected readonly List<string> ValidationErrors = new List<string>();

        public string CountryName { get; protected set; }

        public virtual string Description { get; protected set; }

        public IsoCountryCode Country { get; protected set; }
        public abstract string SubCountry { get;  }
        public string Property { get;  set; }

        // RESO-defined sub property type
        public SubPropertyTypeCode PropertyType { get;  set; }

        public string SubProperty { get;  set; }

        public abstract bool IsValid();

        public virtual string ToUpi()
        {
            return string.Empty;
        }
    }
}