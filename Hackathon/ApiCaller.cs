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
        private long TimeSinceSameSyncPoint = 0;
        private int NeutralStateTime = 0;
        private int NeutralStateCycles = 0;
        private PLCInstance PLCInstance;

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
            if (time < 0)
                return CheckVariable(name, value);
            if (!CheckVariable(name, value))
                return false;
            RunToNextSyncPoint();
            int TimeSinceSameSyncPoint_ms = (int) TimeSinceSameSyncPoint / 1000000;
            return CheckVariableTimeDependent(name, value, time - TimeSinceSameSyncPoint_ms);
        }

        public bool CheckVariableTimeDependent(string name, int value, int time)
        {
            if(time < 0)
                return CheckVariable(name, value);
            if (!CheckVariable(name, value))
                return false;
            RunToNextSyncPoint();
            int TimeSinceSameSyncPoint_ms = (int)TimeSinceSameSyncPoint / 1000000;
            return CheckVariableTimeDependent(name, value, time - TimeSinceSameSyncPoint_ms);
        }

        public bool CheckVariableCycleDependent(string name, bool value, int cycles)
        {
            if (cycles < 0)
                return CheckVariable(name, value);
            if (!CheckVariable(name, value))
                return false;
            RunToNextSyncPoint();
            return CheckVariableCycleDependent(name, value, cycles - 1);
        }

        public bool CheckVariableCycleDependent(string name, int value, int cycles)
        {
            if (cycles < 0)
                return CheckVariable(name, value);
            if (!CheckVariable(name, value))
                return false;
            RunToNextSyncPoint();
            return CheckVariableCycleDependent(name, value, cycles - 1);
        }
        public bool SetVariable(string name, bool value)
        {
            SDataValue sValue = new SDataValue(){ Type = EPrimitiveDataType.Bool, Bool = value };
            return PLCInstance.InstanceWrite(name, sValue);
        }

        public bool SetVariable(string name, int value)
        {
            EPrimitiveDataType Type = GetEPrimitiveDataType(name);
            SDataValue sValue = new SDataValue();
            switch (Type)
            {
                case EPrimitiveDataType.Int16:
                    sValue = new SDataValue() { Type = Type, Int16 = (short) value };
                    break;
                case EPrimitiveDataType.UInt16:
                    sValue = new SDataValue() { Type = Type, UInt16 = (ushort) value };
                    break;
            }
            return PLCInstance.InstanceWrite(name, sValue);
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
            TimeSinceSameSyncPoint = in_TimeSinceSameSyncPoint_ns;
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
                    // Do Nothing
                    break;
            }
         }

        private EPrimitiveDataType GetEPrimitiveDataType(string name)
        {
            foreach(var variable in PLCInstance.TagInfos())
            {
                if (variable.Name.Equals(name))
                    return variable.PrimitiveDataType;
            }
            return EPrimitiveDataType.Unspecific;
        }
    }

    enum ApiCallerState
    {
        TestRunning = 0,
        NeutralTime = 1,
        NeutralCycles = 2,
    }

}
