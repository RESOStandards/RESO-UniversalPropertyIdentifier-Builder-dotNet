using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;

namespace Reso.Upi.Core.US
{
    
    public class Fips
    {
        public List<FipsState> States { get; set; } = new List<FipsState>();
    }

    public class FipsState
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public List<FipsCounty> Counties { get; set; } = new List<FipsCounty>();

        public override string ToString()
        {
            return $"{Code} {Name} ";
        }
    }

    public class FipsCounty
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string StatusCode { get; set; }
        public FipsState State { get; set; }
        public List<FipsSubCounty> SubCounties { get; set; } = new List<FipsSubCounty>();
        public override string ToString()
        {
            return $"{Code} [{StatusCode}] {Name} ";
        }
    }

    public class FipsSubCounty
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FunctionalStatus { get; set; }
        public FipsCounty County { get; set; }
        public override string ToString()
        {
            return $"{Code} [{FunctionalStatus}] {Name} ";
        }
    }

    public class FipsCountyEntry
    {
        //AL,01,001,Autauga County,H1
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
        public string StatusCode { get; set; }
    }

    public class FipsSubCountyEntry
    {
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
        public string SubCountyCode { get; set; }
        public string SubCountyName { get; set; }
        public string FunctionalStatus { get; set; }
    }

    public static class FipsCache
    {
        private const string FipsCountyResource = @"US\national_cousub-by-function.txt";
        private const string FipsSubCountyResource = @"US\national_county.txt";
        public static readonly string UNVALID = "INVLAID";

        public static List<FipsCountyEntry> FipsCodes { get; set; } = new List<FipsCountyEntry>();
        public static List<FipsSubCountyEntry> FipsSubCodes { get; set; } = new List<FipsSubCountyEntry>();

        public static Fips FIPS { get; set; } = new Fips();

        static FipsCache()
        {
            LoadCache();
        }

        public static FipsCountyEntry GetCounty(string fipsCode)
        {
            if (!string.IsNullOrEmpty(fipsCode))
            {
                var fipsState = fipsCode.Substring(0, 2);
                var fipsCounty = fipsCode.Substring(2, 3);

                return FipsCodes.FirstOrDefault(s => s.CountyCode == fipsCounty && s.StateCode == fipsState)
                       ?? UnknownCounty(fipsCode);
            }

            return UnknownCounty("Unspecified");

        }

        static FipsCountyEntry UnknownCounty(string countyCode)
        {
            return new FipsCountyEntry()
            {
                StateName = UNVALID,
                StateCode = "UNDEFINED",
                CountyName = UNVALID,
                CountyCode = countyCode
            };
        }

        public static FipsSubCountyEntry GetSubCounty(string subCountyCode)
        {
            if (!string.IsNullOrEmpty(subCountyCode))
            {
                if (subCountyCode.ToUpper() == "N")
                    return NotApplicableSubCounty();

                return FipsSubCodes.FirstOrDefault(s => s.SubCountyCode == subCountyCode)
                       ?? UnknownSubCounty(subCountyCode);
            }

            return UnknownSubCounty("Unspecified");

        }

        static FipsSubCountyEntry UnknownSubCounty(string subCountyCode)
        {
            return new FipsSubCountyEntry()
            {
                SubCountyName = UNVALID,
                SubCountyCode = subCountyCode
            };
        }

        private static FipsSubCountyEntry NotApplicableSubCounty()
        {
            return new FipsSubCountyEntry()
            {
                SubCountyName = "N/A",
                SubCountyCode = "N"
            };
        }

        public static bool IsInvalid(this FipsSubCountyEntry entry)
        {
            return entry.SubCountyName == UNVALID;
        }

        public static bool IsInvalid(this FipsCountyEntry entry)
        {
            return entry.CountyName == UNVALID;
        }

        static void LoadCache()
        {
            try
            {
                var fipsData = System.IO.File.ReadAllLines(FipsSubCountyResource);

                var fipsLines = fipsData.Select(l => l.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));

                FipsCodes = fipsLines.Select(l => new FipsCountyEntry()
                {
                    StateName = l[0],
                    StateCode = l[1],
                    CountyCode = l[2],
                    CountyName = l[3],
                    StatusCode = l[4]
                })
                    .ToList();

                var fipsSubData = System.IO.File.ReadAllLines(FipsCountyResource);

                foreach (var line in fipsSubData)
                {
                    if (line.Trim().Length == 0) continue;

                    var cols = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    if (cols.Length != 7 && cols.Length != 8)
                    {
                        Debugger.Break();
                    }

                    if (cols.Length == 8)
                    {
                        FipsSubCodes.Add(new FipsSubCountyEntry()
                        {
                            StateName = cols[0],
                            StateCode = cols[1],
                            CountyCode = cols[2],
                            CountyName = cols[3],
                            SubCountyCode = cols[4],
                            SubCountyName = $"{cols[5]}, {cols[6]}",
                            FunctionalStatus = cols[7]
                        });

                    }
                    else if (cols.Length == 7)
                    {
                        FipsSubCodes.Add(new FipsSubCountyEntry()
                        {
                            StateName = cols[0],
                            StateCode = cols[1],
                            CountyCode = cols[2],
                            CountyName = cols[3],
                            SubCountyCode = cols[4],
                            SubCountyName = cols[5],
                            FunctionalStatus = cols[6]
                        });
                    }
                    else
                    {
                        Debugger.Break();
                    }
                }

                foreach (var code in FipsCodes)
                {
                    var thisState = FIPS.States.FirstOrDefault(s => s.Name == code.StateName);

                    if (thisState == null)
                    {
                        thisState = new FipsState() { Name = code.StateName, Code = code.StateCode };
                        FIPS.States.Add(thisState);
                    }

                    thisState.Counties.Add(new FipsCounty() { Name = code.CountyName, Code = code.CountyCode, StatusCode = code.StatusCode, State = thisState });
                }

                foreach (var code in FipsSubCodes)
                {
                    var thisState = FIPS.States.FirstOrDefault(s => s.Name == code.StateName);

                    var thisCounty = thisState?.Counties.FirstOrDefault(c => c.Code == code.CountyCode);

                    thisCounty?.SubCounties.Add(new FipsSubCounty()
                    {
                        County = thisCounty,
                        Name = code.SubCountyName,
                        Code = code.SubCountyCode,
                        FunctionalStatus = code.FunctionalStatus
                    });
                }

                Debug.WriteLine("(Loaded FIPS data)\n");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to read FIPS files to populate Cache: {ex.Message}", ex);

            }
        }
    }
}
