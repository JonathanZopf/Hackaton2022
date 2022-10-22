using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Interfaces
{
    interface IApiCaller
    {
        void SetVariable(string name, bool value);
        void SetVariable(string name, int value);
        void SetVariable(string name, float value);

        bool CheckVariable(string name, bool value);
        bool CheckVariable(string name, int value);
        bool CheckVariable(string name, float value);

        bool CheckVariableTimeDependent(string name, bool value, float time);
        bool CheckVariableTimeDependent(string name, int value, float time);
        bool CheckVariableTimeDependent(string name, float value, float time);

    }
}
