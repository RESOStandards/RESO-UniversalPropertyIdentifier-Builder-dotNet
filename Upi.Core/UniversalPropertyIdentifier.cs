using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using Reso.Upi.Core.US;

namespace Reso.Upi.Core
{
    public class UniversalPropertyIdentifier: ICountryUpi
    {
        private readonly ICountryUpi _countryUpi;
        public dynamic CountrySpecificUpi => _countryUpi;

        #region ICountryUpi

        public string CountryName => _countryUpi.CountryName;

        public string ToUpi() => _countryUpi.ToUpi();

        public string Description => _countryUpi.Description;

        public bool IsValid() => _countryUpi.IsValid();



        public IsoCountry Country { get; protected  set; }

        // defined by the country
        public string SubCountry { get; protected set; }

        // defined by the sub country
        public string Property { get; protected set; }

        // RESO-defined sub property type
        public SubPropertyType PropertyType { get; protected set; }

        public string SubProperty { get; protected set; }

        #endregion

        // The basic UPI is defined in a very generic way to be compatible with
        // other countries
        public override string ToString()
        {
            return ToUpi();
        }

        #region Construction
        protected UniversalPropertyIdentifier(string upi)
        {
            _countryUpi = upi.ParseUpi().ToCountryUpi() 
                ?? new InvalidCountry($"{upi} is not recognized as a valid UPI. Valid Countries are {string.Join(", ", Enum.GetNames(typeof(IsoCountry)))}");
        }

        protected UniversalPropertyIdentifier(IsoCountry country)
        {
            Country = country;
        }

        protected UniversalPropertyIdentifier(IsoCountry country, string subCountry, string property, SubPropertyType subPropertyType, string subProperty)
        {
            Country = country;
            SubCountry = subCountry;
            Property = property;
            PropertyType = subPropertyType;
            SubProperty = subProperty;
        }

        #endregion

        #region static

        public static UniversalPropertyIdentifier Parse(string upi)
        {
            return new UniversalPropertyIdentifier(upi);
        }

        #region Implicit
        public static implicit operator UniversalPropertyIdentifier(string upi)
        {
            return new UniversalPropertyIdentifier(upi);
        }
        public static implicit operator string(UniversalPropertyIdentifier upi)
        {
            return upi.ToString();
        }
        #endregion

        #endregion

    }


}
