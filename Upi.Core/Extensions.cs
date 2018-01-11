using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Reso.Upi.Core.US;

namespace Reso.Upi.Core
{
    internal static class IsoExtensions
    {
        public static string AsText(this IsoCountryCode country)
        {
            switch (country)
            {
                case IsoCountryCode.US: return "United States";
                default: return "";
            }
        }

        public static Type UpiType(this IsoCountryCode country)
        {
            switch (country)
            {
                case IsoCountryCode.US: return typeof(UnitedStatesUpi);
                default: return null;
            }
        }
    }

    internal static class ResoExtensions
    {
        public static string AsText(this SubPropertyTypeCode propertyType)
        {
            switch (propertyType)
            {
                case SubPropertyTypeCode.B: return "Building";
                case SubPropertyTypeCode.S: return "Cooperative/Apartment";
                case SubPropertyTypeCode.R: return "Real Property";
                case SubPropertyTypeCode.T: return "Temporary Designation";
                default: return SubPropertyTypeCode.N.ToString();
            }
        }
    }

    internal static class StringExtensions
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

                if (IsoCountryCode.TryParse(countryId, out IsoCountryCode isoCountry))
                {
                    var countryUpi = (ICountryUpi)Activator.CreateInstance(isoCountry.UpiType(), segments);
                    return countryUpi;
                }

            }
            return null;
        }
        public static ICountryUpi ToCountryUpi(this IsoCountryCode isoCountry)
        {
            var countryUpi = (ICountryUpi)Activator.CreateInstance(isoCountry.UpiType());
            return countryUpi;
        }
    }

}
