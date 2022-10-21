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
            instance = SimulationRuntimeManager.CreateInterface("002"); //hooks into existing VirtualPLC, works

            instance.CommunicationInterface = ECommunicationInterface.TCPIP;
            instance.IsSendSyncEventInDefaultModeEnabled = true;

        }
    }
}
