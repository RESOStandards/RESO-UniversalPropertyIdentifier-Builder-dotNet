using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bessett.SmartConsole;
using Reso.Upi.Core;
using Reso.Upi.Core.US;

namespace Reso.Upi.Cli.Tasks
{
    [NoConfirmation]
    public class Demo : ConsoleTask
    {
        public override TaskResult StartTask()
        {
            string[] sampleUpis =
            {
                "US-04015-N -11022331-R-N",
                "US-42049-49888-1213666-R-N",
                "US-36061-N- 010237502R1-R-N",
                "US-36061-N-010237502R1-S-113",
                "US-06075-N-40010333-T-10",
                "US-13051-N-1122444-R-N",
                "US-36061-N-0122213-S-118",
                "US-04019-N-12401001H-B-65A",
                "US-123331-N-N-99798987-99",
                "US-123331-N-87-99",                // BAD UPI
                "XX-123331-N-N-99798987-99",        // BAD UPI  
                "OIOASPODASDO APOSAPSCAS"           // BAD UPI
            };

            foreach (var upiText in sampleUpis)
            {
                try
                {
                    UniversalPropertyIdentifier upi = upiText;
                    Console.WriteLine($"\n{upi.Description}");
                }
                catch (Exception ex)
                {
                    return TaskResult.Exception(ex);
                }
            }

            // Naked UPI
            var nakedUpi1 = new UniversalPropertyIdentifier(IsoCountryCode.US)
            {
                SubCountry = "13051-N",
                Property = "123-456-789",
                PropertyType = SubPropertyTypeCode.S,
                SubProperty = "909"
            };
            Console.WriteLine($"\n{nakedUpi1}");

            var nakedUpi2 = new UniversalPropertyIdentifier(
                IsoCountryCode.US, "06075-N","144-90-8822"
                );
            Console.WriteLine($"\n{nakedUpi2}");



            // Build a United States UPI

            var testUpi = new UniversalPropertyIdentifier("US-42049-49888-1213666-R-N");
            Console.WriteLine($"\n{testUpi.CountrySpecificUpi.FipsSubCounty.SubCountyName}");

            var upiByCountry = new UnitedStatesUpi()
            {
                FipsCounty = FipsCache.GetCounty("04013"),
                FipsSubCounty = FipsCache.GetSubCounty("N"),
                Property = "508-41-188",
                PropertyType = SubPropertyTypeCode.B,
                SubProperty = "65A"
            };

            Console.WriteLine($"\n{upiByCountry.Description}");

            // minimal property construction by components
            var upiByCountry2 = new UnitedStatesUpi()
            {
                FipsCounty = FipsCache.GetCounty("04015"),
                FipsSubCounty = FipsSubCountyEntry.Undefined(),
                Property = "508-41-188"
                
            };

            Console.WriteLine($"\n{upiByCountry2.Description}");

            var upiByCountry3 = new UnitedStatesUpi()
            {
                FipsCounty = FipsCache.GetCounty("04019"),
                FipsSubCounty = FipsSubCountyEntry.Undefined(),
                Property = "508-41-188",
                PropertyType = SubPropertyTypeCode.B,
                SubProperty = "65A"
            };

            Console.WriteLine($"\n{upiByCountry3.Description}");

            return TaskResult.Complete();

        }
    }
}
