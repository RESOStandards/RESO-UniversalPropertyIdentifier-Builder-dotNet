using System;
using System.Collections.Generic;
using System.Text;
using Reso.Upi.Core.US;

namespace Reso.Upi.Core
{
    public static class IsoExtensions
    {
        public static string AsText(this IsoCountry country)
        {
            switch (country)
            {
                case IsoCountry.US: return "United States";
                default: return "";
            }
        }

        public static Type UpiType(this IsoCountry country)
        {
            switch (country)
            {
                case IsoCountry.US: return typeof(UnitedStatesUpi);
                default: return null;
            }
        }
    }

    public static class ResoExtensions
    {
        public static string AsText(this SubPropertyType propertyType)
        {
            switch (propertyType)
            {
                case SubPropertyType.B: return "Building";
                case SubPropertyType.S: return "Cooperative/Apartment";
                case SubPropertyType.R: return "Real Property";
                case SubPropertyType.T: return "Temporary Designation";
                default: return SubPropertyType.Unknown.ToString();
            }
        }
    }
    
}
