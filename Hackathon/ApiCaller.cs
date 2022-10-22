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
        PLCInstance PLCInstance;
        
        public ApiCaller()
        {
            PLCInstance = new PLCInstance();
        }

        public bool CheckVariable(string name, bool value)
        {
            throw new NotImplementedException();
        }

        public bool CheckVariable(string name, int value)
        {
            throw new NotImplementedException();
        }


        public bool CheckVariableTimeDependent(string name, bool value, int time)
        {
            throw new NotImplementedException();
        }

        public bool CheckVariableTimeDependent(string name, int value, int time)
        {
            throw new NotImplementedException();
        }

        public void SetVariable(string name, bool value)
        {
            throw new NotImplementedException();
        }

        public void SetVariable(string name, int value)
        {
            throw new NotImplementedException();
        }

        public void RunToNextSyncPoint()
        {
            throw new NotImplementedException();
        }

        public void RunToNextSyncPoint(int time)
        {
            throw new NotImplementedException();
        }
    }
}
