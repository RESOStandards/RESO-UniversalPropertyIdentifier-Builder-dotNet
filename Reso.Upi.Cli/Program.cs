using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Reso.Upi.Core;
using Reso.Upi.Core.US;

namespace Reso.Upi.Cli
{
    class Program
    {
        static void Main(string[] args)
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
                "US-123331-N-87-99",
                "XX-123331-N-N-99798987-99",
                "OIOASPODASDO APOSAPSCAS"
            };

            if (args.Any())
            {
                Console.WriteLine((new UnitedStatesUpi(args[0])).Description);
            }
            else
            {
                foreach (var upiText in sampleUpis)
                {
                    try
                    {
                        UniversalPropertyIdentifier upi = upiText;
                        Console.WriteLine(upi.Description);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
