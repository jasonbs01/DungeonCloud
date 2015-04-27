using DungeonCloud.DungeonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DungeonServiceConsole
{
    class Program
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            //ServiceHost sh = new ServiceHost(typeof(StateStore),
            //    new Uri("http://localhost:8089/"));
            ServiceHost sh = new ServiceHost(typeof(DungeonService));
            bool openSucceeded = false;
            //TRY OPENINNG, IF FAILS THE HOST WILL BE ABORTED 
            try
            {
                //ServiceEndpoint sep = sh.AddServiceEndpoint(typeof(IStateStore),
                //    new WebHttpBinding(),
                //    "StateStore");
                //sep.Behaviors.Add(new WebHttpBehavior {HelpEnabled = true});
                sh.Open();
                openSucceeded = true;
            }
            catch (Exception ex)
            {
                log.Error("Service host failed to open", ex);
            }
            finally
            {
                if (!openSucceeded)
                {
                    sh.Abort();
                }
            }
            if (sh.State == CommunicationState.Opened)
            {
                log.InfoFormat("Service was successfully started at {0}", sh.Description.Endpoints[0].Address);
                Console.WriteLine("The Service is running. Press Enter to stop.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Server failed to open");
                log.Error("Service host failed to open");
            }
            //TRY CLOSING, IF FAILS THE HOST WILL BE ABORTED 
            bool closeSucceed = false;
            try
            {
                sh.Close();
                closeSucceed = true;
            }
            catch (Exception ex)
            {
                log.Error("Service host failed to close", ex);
            }
            finally
            {
                if (!closeSucceed)
                {
                    sh.Abort();
                }
            }
        }
    }
}
