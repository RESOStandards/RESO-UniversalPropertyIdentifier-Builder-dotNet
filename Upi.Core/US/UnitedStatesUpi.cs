using Reso.Upi.Core.US;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reso.Upi.Core.US
{
    public class UnitedStatesUpi : UniversalPropertyIdentifier, ICountryUpi
    {
        #region ICountryUpi
        public string CountryName => "United States";
        #endregion

        public string FipsStateCode {
            get => _FipsStateCode;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _FipsStateCode = FipsCache.FIPS.States.FirstOrDefault(s => s.Code == FipsStateCode)?.Code;
            }
        }

        public string FipsCounty
        {
            get => _FipsCounty;
            set
            {
                _FipsCounty = _FipsStateCode
            }
        }
        public string FipsSubCounty { get; set; }

        string _FipsStateCode = "Invalid:''";
        string _FipsCounty = "Invalid:''";
        string _FipsSubCounty = "Invalid:''";


        public UnitedStatesUpi() : base(IsoCountry.US)
        {}
        
        List<string> ValidationErrors = new List<string>();

        public UnitedStatesUpi(string fipsStateCode, string countyCode, string subCountyCode, string propertyId, PropertyType propertyType, string subPropertyId) : base(IsoCountry.US)
        {
            FipsStateCode = fipsStateCode; 
            FipsCounty = countyCode;
            FipsSubCounty = string.IsNullOrEmpty(subCountyCode)?"N" : subCountyCode;
            SubCountry = $"{FipsCounty}-{FipsSubCounty}";
            Property = propertyId.Replace("-", "");
            SubPropertyType = propertyType;
            SubProperty = string.IsNullOrEmpty(subPropertyId) ? "N" : subPropertyId;
        }

        /// <summary>
        /// Test the UPI's validity
        /// NOTE: this is a formatting check
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            var state = FipsCache.FIPS.States.FirstOrDefault(s => s.Code == FipsStateCode);
            var county = state?.Counties.FirstOrDefault(c => c.Code == FipsCounty);
            var subCounty = _FipsSubCounty == "N" ? "N" : county?.SubCounties.FirstOrDefault(sc => sc.Code == FipsSubCounty).Code;
            
            return !string.IsNullOrEmpty(subCounty);
        }

        public static UnitedStatesUpi Parse(string upi)
        {
            return new UnitedStatesUpi();
        }
    }
}
