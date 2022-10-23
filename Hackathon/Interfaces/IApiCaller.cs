using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Interfaces
{
    interface IApiCaller
    {
        /// <summary>
        /// Sets a variable to given boolean value
        /// in current cycle.
        /// </summary>
        /// <param name="name">PLC Tag</param>
        /// <param name="value">boolean value</param>
        /// <returns>true/false on success</returns>
        bool SetVariable(string name, bool value);

        /// <summary>
        /// Sets a variable to given int value
        /// in current cycle.
        /// </summary>
        /// <param name="name">PLC Tag</param>
        /// <param name="value">int value</param>
        /// <returns>true/false on success</returns>
        bool SetVariable(string name, int value);

        /// <summary>
        /// Matches given boolean value to the value
        /// of the PLC Tag in current cycle.
        /// </summary>
        /// <param name="name">PLC Tag</param>
        /// <param name="value">boolean value</param>
        /// <returns>true/false</returns>
        bool CheckVariable(string name, bool value);

        /// <summary>
        /// Matches given int value to the value
        /// of the PLC Tag in current cycle.
        /// </summary>
        /// <param name="name">PLC Tag</param>
        /// <param name="value">int value</param>
        /// <returns>true/false</returns>
        bool CheckVariable(string name, int value);

        /// <summary>
        /// Matches the given boolean value to the value
        /// of the PLC Tag over a given period of time.
        /// </summary>
        /// <param name="name">PLC Tag</param>
        /// <param name="value">boolean value</param>
        /// <param name="time">time in ms</param>
        /// <returns>true/false</returns>
        bool CheckVariableTimeDependent(string name, bool value, int time);

        /// <summary>
        /// Matches the given int value to the value
        /// of the PLC Tag over a given period of time.
        /// </summary>
        /// <param name="name">PLC Tag</param>
        /// <param name="value">int value</param>
        /// <param name="time">time in ms</param>
        /// <returns>true/false</returns>
        bool CheckVariableTimeDependent(string name, int value, int time);

        /// <summary>
        /// Matches the given boolean value to the value
        /// of the PLC Tag over a given period of cycles.
        /// </summary>
        /// <param name="name">PLC Tag</param>
        /// <param name="value">boolean value</param>
        /// <param name="time">time in ms</param>
        /// <returns>true/false</returns>
        bool CheckVariableCycleDependent(string name, bool value, int cycles);

        /// <summary>
        /// Matches the given int value to the value
        /// of the PLC Tag over a given period of cycles.
        /// </summary>
        /// <param name="name">PLC Tag</param>
        /// <param name="value">boolean value</param>
        /// <param name="time">time in ms</param>
        /// <returns>true/false</returns>
        bool CheckVariableCycleDependent(string name, int value, int cycles);

        /// <summary>
        /// Simulation runs to next synchronization point and freezes.
        /// </summary>
        void RunToNextSyncPoint();

        /// <summary>
        /// Lets the simulation run a given amount of time.
        /// </summary>
        /// <param name="time">time in ms</param>
        void RunMilliSeconds(int time);

        /// <summary>
        /// Lets the simulation run a given amount of cycles.
        /// </summary>
        /// <param name="time">cycles</param>
        void RunCycles(int cycles);
    }
}
