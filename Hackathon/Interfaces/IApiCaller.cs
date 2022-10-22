﻿using System;
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

        bool CheckVariable(string name, bool value);
        bool CheckVariable(string name, int value);

        bool CheckVariableTimeDependent(string name, bool value, int time);
        bool CheckVariableTimeDependent(string name, int value, int time);

        void RunToNextSyncPoint();
        void RunToNextSyncPoint(int time);
    }
}
