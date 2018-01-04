using System;
using System.Collections.Generic;
using System.Text;

namespace Reso.Upi.Core
{
    public static class IsoExtensions
    {
        public static string AsText(this IsoCountry country)
        {
            switch (country)
            {
                case IsoCountry.US: return "UnitedStates";
                default: return "";
            }
        }
    }

    public static class ResoExtensions
    {
        public static string AsText(this PropertyType propertyType)
        {
            switch (propertyType)
            {
                case PropertyType.B: return "Building";
                case PropertyType.S: return "Cooperative/Apartment";
                case PropertyType.R: return "Real Property";
                case PropertyType.T: return "Temporary Designation";
                default: return "";
            }
        }
    }
    
}
