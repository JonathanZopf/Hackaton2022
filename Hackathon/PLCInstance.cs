using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siemens.Simatic.Simulation.Runtime;

namespace Hackathon
{
    
    class PLCInstance
    {
        private readonly string InstanceName = "001";
        private readonly EOperatingMode OperatingMode = EOperatingMode.SingleStep_Bus;
        private readonly bool SyncEventInDefaultModeEnabled = true;

        private IInstance Instance;
        private bool IsConfigured = false;

        public PLCInstance()
        {
            try {
                Instance = SimulationRuntimeManager.CreateInterface(InstanceName); //hooks into existing VirtualPLC, works

                Instance.CommunicationInterface = ECommunicationInterface.TCPIP;
                Instance.IsSendSyncEventInDefaultModeEnabled = SyncEventInDefaultModeEnabled;
                Instance.OperatingMode = OperatingMode;

                Instance.OnSyncPointReached += OnEndOfCycle;
                Instance.OnOperatingStateChanged += OnOperatingStateChanged;

                Instance.Run();
                Console.WriteLine("PLCInstance created.");
            }catch(SimulationRuntimeException ex)
            {
                Console.WriteLine("Failed registering PLCInstance: " + ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void OnEndOfCycle(IInstance in_Sender, ERuntimeErrorCode in_ErrorCode, DateTime in_DateTime, UInt32 in_PipId, Int64 in_TimeSinceSameSyncPoint_ns, Int64 in_TimeSinceAnySyncPoint_ns, UInt32 in_SyncPointCount)
        {
            Instance.RunToNextSyncPoint();
        }

        void OnOperatingStateChanged(IInstance in_Sender, ERuntimeErrorCode in_ErrorCode, DateTime in_DateTime, EOperatingState in_PrevState, EOperatingState in_OperatingState)
        {
            Console.WriteLine($"Operating Mode changed to {in_OperatingState}");
            if(in_OperatingState == EOperatingState.Run)
            {
                try
                {
                    Instance.UpdateTagList();
                    IsConfigured = true;
                    Console.WriteLine("Simulation is configured");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error when updating TagList:", ex.Message);
                }
            }
        }
    }
}
