using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siemens.Simatic.Simulation.Runtime;

namespace Hackathon
{
    class Program
    {
        public static IInstance instance { get; set; }

        static void Main(string[] args)
        {
            instance = SimulationRuntimeManager.CreateInterface("001"); //hooks into existing VirtualPLC, works

          
            instance.CommunicationInterface = ECommunicationInterface.TCPIP;
            instance.IsSendSyncEventInDefaultModeEnabled = true;

            instance.UpdateTagList();
            Console.WriteLine(instance.Read("QX_VGR_ValveVacuum_Q8"));
            Console.ReadKey();

        }
    }
}
