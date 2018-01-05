using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    public static class StringExtensions
    {
        /// <summary>
        /// Parse he segments of the UPI
        /// </summary>
        /// <param name="upi"></param>
        /// <returns></returns>
        public static List<string> ParseUpi(this string upi)
        {
            var segments = upi.ToUpper().Split('-').Select(s => s.Trim()).ToList();
            return segments;
        }

        public static ICountryUpi ToCountryUpi(this List<string> segments)
        {
            if (segments.Any())
            {
                var countryId = segments[0];

                if (IsoCountry.TryParse(countryId, out IsoCountry isoCountry))
                {
                    var countryUpi = (ICountryUpi)Activator.CreateInstance(isoCountry.UpiType(), segments);
                    return countryUpi;
                }

            }
            return null;
        }
    }

}
