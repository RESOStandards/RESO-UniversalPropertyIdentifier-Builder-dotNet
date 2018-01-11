using Reso.Upi.Core.US;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reso.Upi.Core.US
{
    public sealed class UnitedStatesUpi : CountryUpi
    {
        #region ICountryUpi Overrides
        public new string CountryName => Country.AsText();

        public override string ToUpi()
        {
            return $"US-{FipsCounty?.StateCode}{FipsCounty?.CountyCode}-{FipsSubCounty?.SubCountyCode ?? "N"}-{Property.RemoveDashes()}-{PropertyType.ToString()}-{SubProperty}";
        }

        public override string Description
        {
            get
            {
                if (IsValid())
                {
                    var builder = new StringBuilder($"UPI: {ToUpi()}\n");
                    builder.AppendLine($"Country: {CountryName ?? "Undefined"} ");
                    builder.AppendLine($"State: {FipsCounty?.StateName ?? "Undefined"}");
                    builder.AppendLine($"County: {FipsCounty?.CountyName ?? "Undefined"}");
                    builder.AppendLine($"Sub County: {FipsSubCounty?.SubCountyName ?? "Undefined"}");
                    builder.AppendLine($"Property ID: {Property}");
                    builder.AppendLine($"Property Type: {PropertyType.AsText() ?? "Undefined"}");
                    builder.AppendLine($"Sub-Property: {SubProperty}");

                    return builder.ToString();
                }
                return string.Join("\n", ValidationErrors);
            }
        }
        public override bool IsValid()
        {
            return !ValidationErrors.Any();
        }

        #endregion

        public override string SubCountry => $"{FipsCounty.StateCode}{FipsCounty.CountyCode}-{FipsSubCounty.SubCountyCode}";

        public string FipsStateCode => FipsCounty?.StateCode;

        public FipsCountyEntry FipsCounty
        {
            get => _fipsCounty;

            set
            {
                _fipsCounty = value;
                if (_fipsCounty.IsInvalid())
                    ValidationErrors.Add($"Invalid SubCounty ({value.CountyCode})");
            }
        }

        public FipsSubCountyEntry FipsSubCounty
        {
            get => _fipsSubCounty;
            set
            {
                _fipsSubCounty = value;
                if (_fipsCounty.IsInvalid())
                    ValidationErrors.Add($"Invalid SubCounty ({value.SubCountyCode})");
            }
        } 
        
        private FipsCountyEntry _fipsCounty = null;

        private FipsSubCountyEntry _fipsSubCounty = null;


        #region Construction
        public UnitedStatesUpi() : base(IsoCountryCode.US)
        {}

        public UnitedStatesUpi(List<string> segments): this()
        {
            // this constructor is for the case where the upi was parsed, and the upi was
            // determined to be United States
            InitializeUpi(segments);
        }
        
        public UnitedStatesUpi(string upi): this()
        {
            // parse the upi
            var components = upi.ParseUpi();
            if (components.Count() == 6)
            {
                InitializeUpi(components);
            }

            ValidationErrors.Add($"Invalid United States UPI: {upi}");

        }

        public UnitedStatesUpi(string fipsCountyCode, string propertyId) : base(IsoCountryCode.US)
        {
            FipsCounty = FipsCache.GetCounty(fipsCountyCode);
            Property = propertyId;
        }

        public UnitedStatesUpi( string fipsCountyCode, string subCountyCode, string propertyId) : base(IsoCountryCode.US)
        {
            //FipsStateCode = fipsStateCode; 
            FipsCounty = FipsCache.GetCounty(fipsCountyCode);
            FipsSubCounty = FipsCache.GetSubCounty(subCountyCode.ToOptionalUpiComponent());

            Property = propertyId;
        }
        public UnitedStatesUpi(
            string fipsCountyCode, string subCountyCode,
            string propertyId, SubPropertyTypeCode propertyType, string subPropertyId) : base(IsoCountryCode.US)
        {
            //FipsStateCode = fipsStateCode; 
            FipsCounty = FipsCache.GetCounty(fipsCountyCode);
            FipsSubCounty = FipsCache.GetSubCounty(subCountyCode.ToOptionalUpiComponent());

            Property = propertyId.RemoveDashes();
            PropertyType = propertyType;
            SubProperty = subPropertyId.ToOptionalUpiComponent();
        }

        #endregion

        #region Private
        private void InitializeUpi(List<string> components)
        {
            if (components.Count()==6)
                if (components[0] == IsoCountryCode.US.ToString())
                {
                    SubPropertyTypeCode propertyType = SubPropertyTypeCode.N;

                    FipsCounty = FipsCache.GetCounty(components[1]);
                    FipsSubCounty = FipsCache.GetSubCounty(components[2]);
                    Property = components[3];
                    var result = System.Enum.TryParse(components[4], out propertyType);
                    PropertyType = result ? propertyType : SubPropertyTypeCode.N;
                    SubProperty = components[5];


                }
                else
                {
                    ValidationErrors.Add($"First component of US UPI must begin with 'US'  [{components.ToUpi()}]");
                }
            else
            {
                ValidationErrors.Add($"US UPI must have 6 components [{components.ToUpi()}]");
            }
        }

        #endregion

        #region Implicit

        public static implicit operator UnitedStatesUpi(string upi)
        {
            return new UnitedStatesUpi(upi);
        }

        public static implicit operator string(UnitedStatesUpi upi)
        {
            return upi.ToString();
        }
        #endregion
    }

    internal static class StringExtensions
    {
        public static string RemoveDashes(this string target)
        {
            return target?.Replace("-", "");
        }

        public static string ToUpi(this List<string> target)
        {
                return  string.Join("-", target);
        }
        public static string ToOptionalUpiComponent(this string target)
        {
            return string.IsNullOrEmpty(target) ? "N"
                : target.ToUpper() == "N" ? "N"
                    : target.RemoveDashes();
        }
    }
}
