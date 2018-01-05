using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reso.Upi.Core.US;

namespace Reso.Upi.Core
{
    public class UniversalPropertyIdentifier: ICountryUpi
    {
        private readonly ICountryUpi countryUpi;

        #region ICountryUpi

        public string CountryName => countryUpi.CountryName;

        public string ToUpi() => countryUpi.ToUpi();

        public string Description => countryUpi.Description;

        public bool IsValid() => countryUpi.IsValid();

        #endregion
        
        public IsoCountry Country { get; protected  set; }

        // defined by the country
        public string SubCountry { get; protected set; }

        // defined by the sub country
        public string Property { get; protected set; }

        // RESO-defined sub property type
        public SubPropertyType PropertyType { get; protected set; }

        public string SubProperty { get; protected set; }


        // The basic UPI is defined in a very generic way to be compatible with
        // other countries
        public override string ToString()
        {
            return ToUpi();
        }

        protected UniversalPropertyIdentifier(string upi)
        {
            var segments = upi.Split('-');
            if (segments.Any())
            {
                var countryId = segments[0].ToUpper();

                if (IsoCountry.TryParse(countryId, out IsoCountry isoCountry))
                {
                    countryUpi = (ICountryUpi) Activator.CreateInstance(isoCountry.UpiType(), upi);
                    return;
                }

                throw new ApplicationException($"{upi} is not defined by supported country. Supported countries are: {string.Join(", ", Enum.GetNames(typeof(IsoCountry)))}");
            }

            throw new ApplicationException($"{upi} is not recignized as a valid UPI");
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

        public static implicit operator UniversalPropertyIdentifier(string upi)
        {
            return new UniversalPropertyIdentifier(upi);
        }
        public static implicit operator string(UniversalPropertyIdentifier upi)
        {
            return upi.ToString();
        }

    }


}
