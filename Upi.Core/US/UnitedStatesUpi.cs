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
        public new string CountryName => Country.AsText();

        public new string ToUpi()
        {
            return $"US-{FipsCounty.StateCode}{FipsCounty.CountyCode}-{FipsSubCounty.SubCountyCode}-{Property}-{PropertyType.ToString()}-{SubProperty}";
        }
        public new string Description
        {
            get
            {
                if (IsValid())
                {
                    var builder = new StringBuilder($"UPI: {ToUpi()}\n");
                    builder.AppendLine($"Country: {CountryName}");
                    builder.AppendLine($"State: {FipsCounty.StateName}");
                    builder.AppendLine($"County: {FipsCounty.CountyName}");
                    builder.AppendLine($"Sub County: {FipsSubCounty.SubCountyName}");
                    builder.AppendLine($"Property ID: {Property}");
                    builder.AppendLine($"Property Type: {PropertyType.AsText()}");
                    builder.AppendLine($"Sub-Property: {SubProperty}");

                    return builder.ToString();
                }
                return string.Join("\n", _validationErrors);
            }
        }
        public new bool IsValid()
        {
            return !_validationErrors.Any();
        }

        #endregion

        public string FipsStateCode => FipsCounty.StateCode;

        public FipsCountyEntry FipsCounty
        {
            get => _fipsCounty;

            protected set
            {
                _fipsCounty = value;
                if (_fipsCounty.IsInvalid())
                    _validationErrors.Add($"Invalid SubCounty ({value.CountyCode})");
            }

        }

        public FipsSubCountyEntry FipsSubCounty {
            get => _fipsSubCounty;
            protected set
            {
                _fipsSubCounty = value;
                if (_fipsCounty.IsInvalid())
                    _validationErrors.Add($"Invalid SubCounty ({value.SubCountyCode})");
            }
        }

        private FipsCountyEntry _fipsCounty = null;

        private FipsSubCountyEntry _fipsSubCounty = null;

        readonly List<string> _validationErrors = new List<string>();

        public UnitedStatesUpi() : base(IsoCountry.US)
        {}
        
        public UnitedStatesUpi(string upi): this()
        {
            // parse the upi
            var components = upi.Split('-');
            if (components.Length == 6)
            {
                if (components[0] == IsoCountry.US.ToString())
                {
                    SubPropertyType propertyType = SubPropertyType.Unknown;

                    FipsCounty = FipsCache.GetCounty(components[1]);
                    FipsSubCounty = FipsCache.GetSubCounty(components[2]);
                    Property = components[3];
                    var result = System.Enum.TryParse(components[4], out propertyType);
                    PropertyType = result ? propertyType : SubPropertyType.Unknown;
                    SubProperty = components[5];

                    base.SubCountry = $"{FipsCounty.StateCode}{FipsCounty.CountyCode}-{FipsSubCounty.SubCountyCode}";

                    return;
                }
            }

            _validationErrors.Add($"Invalid United States UPI: {upi}");
        }

        public UnitedStatesUpi(string fipsCountyCode, string subCountyCode, 
            string propertyId, SubPropertyType propertyType, string subPropertyId) : base(IsoCountry.US)
        {
            //FipsStateCode = fipsStateCode; 
            FipsCounty = FipsCache.GetCounty(fipsCountyCode);
            FipsSubCounty = FipsCache.GetSubCounty(subCountyCode.ToOptionalUpiComponent());

            base.SubCountry = $"{FipsCounty.StateCode}{FipsCounty.CountyCode}-{FipsSubCounty.SubCountyCode}";
            Property = propertyId.RemoveDashes();
            PropertyType = propertyType;
            SubProperty = subPropertyId.ToOptionalUpiComponent();
        }
        
        public static implicit operator UnitedStatesUpi(string upi)
        {
            return new UnitedStatesUpi(upi);
        }
        public static implicit operator string (UnitedStatesUpi upi)
        {
            return upi.ToString();
        }
    }

    internal static class StringExtensions
    {
        public static string RemoveDashes(this string target)
        {
            return target.Replace("-", "");
        }

        public static string ToOptionalUpiComponent(this string target)
        {
            return string.IsNullOrEmpty(target) ? "N"
                : target.ToUpper() == "N" ? "N"
                : target.RemoveDashes();
        }
    }
}
