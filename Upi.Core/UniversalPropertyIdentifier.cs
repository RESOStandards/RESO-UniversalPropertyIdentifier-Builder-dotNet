using System;
using System.Collections.Generic;
using System.Text;

namespace Reso.Upi.Core
{
    public abstract class UniversalPropertyIdentifier
    {
        public IsoCountry Country { get; protected  set; }

        // defined by the country
        public string SubCountry { get; protected set; }

        // defined by the sub country
        public string Property { get; protected set; }

        // RESO-defined sub property type
        public PropertyType SubPropertyType { get; protected set; }

        public string SubProperty { get; protected set; }

        // The basic UPI is defined in a very generic way to be compatible with
        // other countries
        public override string ToString()
        {
            return $"{Country.ToString()}-{SubCountry}-{Property}-{SubPropertyType.ToString()}-{SubProperty}";
        }

        protected UniversalPropertyIdentifier()
        {}

        protected UniversalPropertyIdentifier(IsoCountry country)
        {
            Country = country;
        }

        protected UniversalPropertyIdentifier(IsoCountry country, string subCountry, string property, string subPropertyType, string subProperty)
        {
            Country = country;

        }


    }


}
