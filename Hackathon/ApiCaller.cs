using System;
using System.Collections.Generic;   
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hackathon.Interfaces;
using Siemens.Simatic.Simulation.Runtime;

namespace Hackathon
{
    class ApiCaller : IApiCaller
    {

        private ApiCallerState State = ApiCallerState.TestRunning;
        private int NeutralStateTime = 0;
        private int NeutralStateCycles = 0;
        private PLCInstance PLCInstance;
        private IDictionary<string, SDataValue> VariablesSet = new Dictionary<string, SDataValue>();
        private IDictionary<string, (SDataValue value, int time)> WatchListTime = new Dictionary<string, (SDataValue value, int time)>();
        private IDictionary<string, (SDataValue value, int cycles)> WatchListCycle = new Dictionary<string, (SDataValue value, int cycles)>();

        public ApiCaller()
        {
            PLCInstance = new PLCInstance(this);
        }

        public bool CheckVariable(string name, bool value)
        {
            SDataValue result = PLCInstance.InstanceRead(name);
            if (value == result.Bool) 
                return true;
            return false;
        }

        public bool CheckVariable(string name, int value)
        {
            SDataValue result = PLCInstance.InstanceRead(name);
            if (value == result.Int16)
                return true;
            return false;
        }

        public bool CheckVariableTimeDependent(string name, bool value, int time)
        {
            if (CheckVariable(name, value))
            {
                SDataValue result = PLCInstance.InstanceRead(name);
                WatchListTime.Add(name, (result, time - 100));
                return true;
            }
            return false;
        }

        public bool CheckVariableTimeDependent(string name, int value, int time)
        {
            if (CheckVariable(name, value))
            {
                SDataValue result = PLCInstance.InstanceRead(name);
                WatchListTime.Add(name, (result, time - 100));
                return true;
            }
            return false;
        }

        public bool CheckVariableCycleDependent(string name, bool value, int cycles)
        {

            for(int i = 0; i < cycles; i++)
            {
            }
            if (CheckVariable(name, value))
            {
                SDataValue result = PLCInstance.InstanceRead(name);
                WatchListCycle.Add(name, (result, cycles - 1));
                return true;
            }
            return false;
        }

        public bool CheckVariableCycleDependent(string name, int value, int cycles)
        {
            if (CheckVariable(name, value))
            {
                SDataValue result = PLCInstance.InstanceRead(name);
                WatchListCycle.Add(name, (result, cycles - 1));
                return true;
            }
            return false;
        }
        public void SetVariable(string name, bool value)
        {
            SDataValue sValue = new SDataValue(){ Type = EPrimitiveDataType.Bool, Bool = value };
            VariablesSet.Add(name, sValue);
            PLCInstance.InstanceWrite(name, sValue);
        }

        public void SetVariable(string name, int value)
        {
            SDataValue sValue = new SDataValue() { Type = EPrimitiveDataType.Int16, Int16 = (short) value };
            VariablesSet.Add(name, sValue);
            PLCInstance.InstanceWrite(name, sValue);
        }

        public void RunToNextSyncPoint()
        {
            PLCInstance.RunToNextSyncPoint();
        }

        public void RunMilliSeconds(int time)
        {
            this.State = ApiCallerState.NeutralTime;
            this.NeutralStateTime = time;
            RunToNextSyncPoint();
        }

        public void RunCycles(int cycles)
        {
            this.State = ApiCallerState.NeutralCycles;
            this.NeutralStateCycles = cycles;
            RunToNextSyncPoint();
        }
        public void OnEndOfCycle(IInstance in_Sender, ERuntimeErrorCode in_ErrorCode, DateTime in_DateTime, UInt32 in_PipId, Int64 in_TimeSinceSameSyncPoint_ns, Int64 in_TimeSinceAnySyncPoint_ns, UInt32 in_SyncPointCount)
        {
            switch (this.State)
            {
                case ApiCallerState.NeutralTime:
                    if (NeutralStateTime > 0)
                    {
                        NeutralStateTime -= 100;
                        RunToNextSyncPoint();
                    }
                    else
                        State = ApiCallerState.TestRunning;
                    break;
                case ApiCallerState.NeutralCycles:
                    if (NeutralStateCycles > 0)
                    {
                        NeutralStateCycles -= 1;
                        RunToNextSyncPoint();
                    }
                    else
                        State = ApiCallerState.TestRunning;
                    break;
                case ApiCallerState.TestRunning:
                    CheckWatchListCycles();
                    //CheckWatchListTime();
                    break;
            }
         }

        public void CheckWatchListTime()
        {
            foreach(var variable in WatchListTime)
            {

            }
        }

        public void CheckWatchListCycles()
        {
            foreach (var variable in WatchListCycle)
            {
                int cycles = variable.Value.cycles;
                SDataValue newValue = PLCInstance.InstanceRead(variable.Key);
                switch (newValue.Type)
                {
                    case EPrimitiveDataType.Bool:
                        bool value = variable.Value.value.Bool;
                        if(value == newValue.Bool)
                        {

                        }
                        break;
                    case EPrimitiveDataType.Int16:
                        break;
                }
            }
        }
    }

    enum ApiCallerState
    {
        TestRunning = 0,
        NeutralTime = 1,
        NeutralCycles = 2,
    }
}
