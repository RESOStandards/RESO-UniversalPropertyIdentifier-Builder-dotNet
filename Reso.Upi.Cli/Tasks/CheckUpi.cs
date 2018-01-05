using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bessett.SmartConsole;
using Reso.Upi.Core;

namespace Reso.Upi.Cli.Tasks
{
    [NoConfirmation]
    public class CheckUpi:ConsoleTask
    {
        [ArgumentHelp]
        public string Upi { get; set; }

        public override TaskResult StartTask()
        {
            try
            {
                UniversalPropertyIdentifier upi = Upi;
                if (upi.IsValid())
                {
                    Console.WriteLine($"\n{upi.Description}");
                    return TaskResult.Complete();
                }
                return TaskResult.Failed(upi.Description);
            }
            catch (Exception ex)
            {
                return TaskResult.Exception(ex);
            }

        }
    }
}
