using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace Hackathon.Interpreter
{
    /// <summary>
    /// Zusammenfassung f?r MyCompiler.
    /// </summary>
    class MyCompiler
    {
        YYARec[] yya;
        YYARec[] yyg;
        YYRRec[] yyr;
        int[] yyd;
        int[] yyal;
        int[] yyah;
        int[] yygl;
        int[] yygh;

        int yyn = 0;
        int yystate = 0;
        int yychar = -1;
        int yynerrs = 0;
        int yyerrflag = 0;
        int yysp = 0;
        int yymaxdepth = 1024;
        int yyflag = 0;
        int yyfnone = 0;
        int[] yys = new int[1024];
        string[] yyv = new string[1024];

        string yyval = "";

        FileStream OutputStream;
        StreamWriter Output;

        class YYARec
        {
            public int sym;
            public int act;
            public YYARec(int s, int a) { sym = s; act = a; }
        }

        class YYRRec
        {
            public int len;
            public int sym;
            public YYRRec(int l, int s) { sym = s; len = l; }
        }

        ////////////////////////////////////////////////////////////////
        /// Constant values / tokens
        ////////////////////////////////////////////////////////////////
        int t_braceL = 257;
        int t_braceR = 258;
        int t_assert = 259;
        int t_set = 260;
        int t_Underscore = 261;
        int t_for = 262;
        int t_cycles = 263;
        int t_ms = 264;
        int t_Number = 265;
        int t_Boolean = 266;
        int t_wait = 267;
        int t_ignore = 256;
        int t_IX = 269;
        int t_IW = 270;
        int t_QX = 271;
        int t_QW = 272;
        ///////////////////////////////////////////////////////////
        /// Global settings: 
        ///////////////////////////////////////////////////////////
        // VCC prevents imports here - which is why we can't use dynamicly resizing arrays and we use hacks

        static int MAX_ARRAY_LENGHT = 10000; //IMPLICATION: if a single block execution contains more than 10k sensors / actors to check / set this will break the code
                                             //static int INITIAL_ARRAY_SIZE = 8;
        static int SUB_ARRAY_LENGHT = 4;
        static ApiCaller api = new ApiCaller();

        int actualArrayLength = 0;
        string[,] instructionStack = new string[MAX_ARRAY_LENGHT, SUB_ARRAY_LENGHT];
        //string[,] instructionStack = new string[INITIAL_ARRAY_SIZE, SUB_ARRAY_LENGHT];


        /* For Re-Initialization (Resetting) of the instructionStack and its components-
        belongs to VCC implied no imports restriction */
        void ResetInstructionStack()
        {
            //instructionStack = new string[INITIAL_ARRAY_SIZE, SUB_ARRAY_LENGHT];
            instructionStack = new string[MAX_ARRAY_LENGHT, SUB_ARRAY_LENGHT];
            actualArrayLength = 0;
        }

        /* Adds all elements to the instructionStack at the currently last index -
        belongs to VCC implied no imports restriction */
        void PushToInstructionStack(string action, string identifier, string type, string interval)
        {
            // increase array size if needed (double it)
            /*
                 if (actualArrayLength >= instructionStack.Length-1){
                    string[,] copyArray = new string[actualArrayLength*2, SUB_ARRAY_LENGHT];
                    instructionStack.CopyTo(copyArray, 0);
                    instructionStack = copyArray;
                 }
            */

            // insert values
            instructionStack[actualArrayLength, 0] = action;
            instructionStack[actualArrayLength, 1] = identifier;
            instructionStack[actualArrayLength, 2] = type;
            instructionStack[actualArrayLength, 3] = interval;
            actualArrayLength += 1;
        }

        /* Reduces the instruction stack to its real number of elements -
        belongs to VCC implied no imports restriction */
        void InstructionStackReduce(int i)
        {
            //Poor and ugly way to reduce this arrays size
            string[,] array = new string[i, SUB_ARRAY_LENGHT];
            for (int a = 0; a < i; a++)
            {
                for (int b = 0; b < SUB_ARRAY_LENGHT; b++)
                {
                    array[a, b] = instructionStack[a, b];
                }
            }
            instructionStack = array;
        }

        /*
        Calls the API-Methods Provided by ApiCaller Interface Implementer based on the parsed input.
        */
        void makeApiCalls()
        {
            // interval is concatenated String in form of Number-Unit

            // loop for all instructions on stack
            for (int j = 0; j < actualArrayLength; j++)
            {
                string action = instructionStack[j, 0];
                string identifier = instructionStack[j, 1];
                string type = instructionStack[j, 2];
                string interval = instructionStack[j, 3];


                //Call API-Methods responsible for setting sensors
                if (action.Equals("set"))
                {
                    if (type.Equals("false"))
                    {
                        Console.WriteLine($"set {identifier} {api.SetVariable(identifier, false)}");
                    }
                    else if (type.Equals("true"))
                    {
                        Console.WriteLine($"set {identifier} {api.SetVariable(identifier, true)}");
                    }
                    // set a number
                    else
                    {
                        Console.WriteLine($"set {identifier} {api.SetVariable(identifier, Int16.Parse(type))}");
                    }
                }
                //Call API-Methods repsonsible for asserting sensor state - without time interval
                else if (interval.Length == 0)
                {
                    if (type.Equals("false"))
                    {
                        Console.WriteLine($"check {identifier} {api.CheckVariable(identifier, false)}");
                    }
                    else if (type.Equals("true"))
                    {
                        Console.WriteLine($"check {identifier} {api.CheckVariable(identifier, true)}");
                    }
                    // set a number
                    else
                    {
                        Console.WriteLine($"check {identifier} {api.CheckVariable(identifier, Int16.Parse(type))}");
                    }
                }
                //Call API-Methods repsonsible for asserting sensor state - with time interval
                else
                {
                    string[] intervalSplit = interval.Split("-".ToCharArray());
                    int time = Int16.Parse(intervalSplit[0]);
                    string timeUnit = intervalSplit[1];

                    // we stop time using ms
                    if (timeUnit.Equals("ms"))
                    {
                        // data is boolean
                        if (type.Equals("false"))
                        {
                            Console.WriteLine($"check time dependent {api.CheckVariableTimeDependent(identifier, false, time)}");
                        }
                        else if (type.Equals("true"))
                        {
                            Console.WriteLine($"check time dependent {api.CheckVariableTimeDependent(identifier, true, time)}");
                        }
                        // data is integer
                        else
                        {
                            Console.WriteLine($"check time dependent {api.CheckVariableTimeDependent(identifier, Int16.Parse(type), time)}");
                        }
                    }
                    // we stop time using cycles
                    else
                    {
                        if (type.Equals("false"))
                        {
                            Console.WriteLine($"check cycle dependent {api.CheckVariableCycleDependent(identifier, false, time)}");
                        }
                        else if (type.Equals("true"))
                        {
                            Console.WriteLine($"check time dependent {api.CheckVariableCycleDependent(identifier, true, time)}");
                        }
                        // data is integer
                        else
                        {
                            Console.WriteLine($"check time dependent {api.CheckVariableCycleDependent(identifier, Int16.Parse(type), time)}");
                        }
                    }
                }
            }

            api.RunToNextSyncPoint(); //TODO - as of now does not allow for multiple 'for x time' statements! 
            // clear stack
            ResetInstructionStack();
        }

        ///////////////////////////////////////////////////////////


        /// <summary>
        /// Der Haupteinstiegspunkt f?r die Anwendung.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {

            bool ShowTokens = false;
            string InputFilename = "../../TestData/test_one.txt";
            string OutputFilename = "idc.txt";

            foreach (string s in args)
            {
                if (s.ToLower() == "-t")
                {
                    ShowTokens = true;
                }
                else
                {
                    if (InputFilename == "") InputFilename = s;
                    else
                    if (OutputFilename == "") OutputFilename = s;
                    else
                    {
                        Console.WriteLine("Too many arguments!");
                        return 1;
                    }
                }
            }
            if (InputFilename == "")
            {
                System.Console.WriteLine("You need to specify input and outputfile: compiler.exe input.txt output.txt");
                return 1;
            }

            StreamReader in_s = File.OpenText(InputFilename);
            string inputstream = in_s.ReadToEnd();
            in_s.Close();

            ////////////////////////////////////////////////////////////////
            /// Compiler Code:
            ////////////////////////////////////////////////////////////////
            MyCompiler compiler = new MyCompiler();
            compiler.Output = null;
            if (OutputFilename != "")
            {
                File.Delete(OutputFilename);
                compiler.OutputStream = File.OpenWrite(OutputFilename);
                compiler.Output = new StreamWriter(compiler.OutputStream, new System.Text.UTF8Encoding(false));
            }
            else
            {
                compiler.Output = new StreamWriter(Console.OpenStandardOutput(), new System.Text.UTF8Encoding(false));

            }

            if (!compiler.Scanner(inputstream)) return 1;
            if (ShowTokens)
            {
                foreach (AToken t in compiler.TokenList)
                {
                    Console.WriteLine("TokenID: " + t.token + "  =  " + t.val);
                }
            }
            compiler.InitTables();
            if (!compiler.yyparse()) return 1;

            if (compiler.Output != null) compiler.Output.Close();
            return 0;
        }
        public void yyaction(int yyruleno)
        {
            switch (yyruleno)
            {
                ////////////////////////////////////////////////////////////////
                /// YYAction code:
                ////////////////////////////////////////////////////////////////
                case 1:
                    yyval = yyv[yysp - 1] + yyv[yysp - 0];
                    //Output.WriteLine(yyval);

                    break;
                case 2:
                    yyval = yyv[yysp - 1] + yyv[yysp - 0];

                    break;
                case 3:
                    yyval = yyv[yysp - 0];
                    // stack should only have one instruction, regardless we use the same method call as for ExecuteTogetheer NT (simplicity) 

                    // Called after each sequential execution (this will most commonly be used).
                    // By convention sequential executions happen on a per line basis. (use braces to schedule actions to be executed together)
                    makeApiCalls();

                    break;
                case 4:
                    yyval = yyv[yysp - 1] + yyv[yysp - 0];

                    // todo call wait befehl
                    string[] intervalSplit1 = yyv[yysp - 0].Split("-".ToCharArray());
                    string time1 = intervalSplit1[0];
                    string timeUnit1 = intervalSplit1[1];

                    // Call API-Method specific for ms
                    if (timeUnit1.Equals("ms"))
                    {
                        api.RunMilliSeconds(Int16.Parse(time1));
                    }
                    // Call API-Method specific for cycles
                    else
                    {
                        api.RunCycles(Int16.Parse(time1));
                    }

                    break;
                case 5:
                    yyval = yyv[yysp - 0];

                    break;
                case 6:
                    yyval = "";

                    break;
                case 7:
                    yyval = yyv[yysp - 1] + yyv[yysp - 0];

                    break;
                case 8:
                    yyval = "";
                    // todo talk to co-framework, use everything from stack
                    // interval is concatenated String in form of Number-Unit

                    // called when ExecuteTogether Block ends - This is when at least two instructions 
                    // where marked as checked together using braces.
                    makeApiCalls();

                    break;
                case 9:
                    yyval = yyv[yysp - 4] + yyv[yysp - 3] + yyv[yysp - 2] + yyv[yysp - 1] + yyv[yysp - 0];

                    break;
                case 10:
                    yyval = yyv[yysp - 3] + yyv[yysp - 2] + yyv[yysp - 1] + yyv[yysp - 0];
                    PushToInstructionStack(yyv[yysp - 3], yyv[yysp - 2], yyv[yysp - 1], yyv[yysp - 0]);


                    break;
                case 11:
                    yyval = yyv[yysp - 3] + yyv[yysp - 2] + yyv[yysp - 1] + yyv[yysp - 0];
                    PushToInstructionStack(yyv[yysp - 3], yyv[yysp - 2], yyv[yysp - 1], yyv[yysp - 0]);

                    break;
                case 12:
                    yyval = yyv[yysp - 2] + yyv[yysp - 1] + yyv[yysp - 0];
                    PushToInstructionStack(yyv[yysp - 2], yyv[yysp - 1], yyv[yysp - 0], "");

                    break;
                case 13:
                    yyval = yyv[yysp - 2] + yyv[yysp - 1] + yyv[yysp - 0];
                    PushToInstructionStack(yyv[yysp - 2], yyv[yysp - 1], yyv[yysp - 0], "");

                    break;
                case 14:
                    //yyval = yyv[yysp-2] + yyv[yysp-1] + yyv[yysp-0]; we drop 'for' and concat the rest (we keep units for identification)
                    yyval = yyv[yysp - 1] + "-" + yyv[yysp - 0];

                    break;
                case 15:
                    //yyval = yyv[yysp-2] + yyv[yysp-1] + yyv[yysp-0]; we drop 'for' and concat the rest (we keep units for identification)
                    yyval = yyv[yysp - 1] + "-" + yyv[yysp - 0];

                    break;
                case 16:
                    yyval = "";

                    break;
                default: return;
            }
        }

        public void InitTables()
        {
            ////////////////////////////////////////////////////////////////
            /// Init Table code:
            ////////////////////////////////////////////////////////////////

            int yynacts = 57;
            int yyngotos = 23;
            int yynstates = 33;
            int yynrules = 16;
            yya = new YYARec[yynacts + 1]; int yyac = 1;
            yyg = new YYARec[yyngotos + 1]; int yygc = 1;
            yyr = new YYRRec[yynrules + 1]; int yyrc = 1;

            yya[yyac] = new YYARec(257, 5); yyac++;
            yya[yyac] = new YYARec(259, 6); yyac++;
            yya[yyac] = new YYARec(260, 7); yyac++;
            yya[yyac] = new YYARec(267, 8); yyac++;
            yya[yyac] = new YYARec(257, 5); yyac++;
            yya[yyac] = new YYARec(259, 6); yyac++;
            yya[yyac] = new YYARec(260, 7); yyac++;
            yya[yyac] = new YYARec(267, 8); yyac++;
            yya[yyac] = new YYARec(0, -6); yyac++;
            yya[yyac] = new YYARec(257, 5); yyac++;
            yya[yyac] = new YYARec(259, 6); yyac++;
            yya[yyac] = new YYARec(260, 7); yyac++;
            yya[yyac] = new YYARec(267, 8); yyac++;
            yya[yyac] = new YYARec(0, -6); yyac++;
            yya[yyac] = new YYARec(0, 0); yyac++;
            yya[yyac] = new YYARec(259, 6); yyac++;
            yya[yyac] = new YYARec(260, 7); yyac++;
            yya[yyac] = new YYARec(271, 13); yyac++;
            yya[yyac] = new YYARec(272, 14); yyac++;
            yya[yyac] = new YYARec(269, 15); yyac++;
            yya[yyac] = new YYARec(270, 16); yyac++;
            yya[yyac] = new YYARec(262, 18); yyac++;
            yya[yyac] = new YYARec(0, -16); yyac++;
            yya[yyac] = new YYARec(257, -16); yyac++;
            yya[yyac] = new YYARec(259, -16); yyac++;
            yya[yyac] = new YYARec(260, -16); yyac++;
            yya[yyac] = new YYARec(267, -16); yyac++;
            yya[yyac] = new YYARec(259, 6); yyac++;
            yya[yyac] = new YYARec(260, 7); yyac++;
            yya[yyac] = new YYARec(266, 20); yyac++;
            yya[yyac] = new YYARec(265, 21); yyac++;
            yya[yyac] = new YYARec(266, 22); yyac++;
            yya[yyac] = new YYARec(265, 23); yyac++;
            yya[yyac] = new YYARec(265, 24); yyac++;
            yya[yyac] = new YYARec(259, 6); yyac++;
            yya[yyac] = new YYARec(260, 7); yyac++;
            yya[yyac] = new YYARec(258, -8); yyac++;
            yya[yyac] = new YYARec(262, 18); yyac++;
            yya[yyac] = new YYARec(0, -16); yyac++;
            yya[yyac] = new YYARec(257, -16); yyac++;
            yya[yyac] = new YYARec(258, -16); yyac++;
            yya[yyac] = new YYARec(259, -16); yyac++;
            yya[yyac] = new YYARec(260, -16); yyac++;
            yya[yyac] = new YYARec(267, -16); yyac++;
            yya[yyac] = new YYARec(262, 18); yyac++;
            yya[yyac] = new YYARec(0, -16); yyac++;
            yya[yyac] = new YYARec(257, -16); yyac++;
            yya[yyac] = new YYARec(258, -16); yyac++;
            yya[yyac] = new YYARec(259, -16); yyac++;
            yya[yyac] = new YYARec(260, -16); yyac++;
            yya[yyac] = new YYARec(267, -16); yyac++;
            yya[yyac] = new YYARec(263, 29); yyac++;
            yya[yyac] = new YYARec(264, 30); yyac++;
            yya[yyac] = new YYARec(258, 31); yyac++;
            yya[yyac] = new YYARec(259, 6); yyac++;
            yya[yyac] = new YYARec(260, 7); yyac++;
            yya[yyac] = new YYARec(258, -8); yyac++;

            yyg[yygc] = new YYARec(-6, 1); yygc++;
            yyg[yygc] = new YYARec(-5, 2); yygc++;
            yyg[yygc] = new YYARec(-3, 3); yygc++;
            yyg[yygc] = new YYARec(-2, 4); yygc++;
            yyg[yygc] = new YYARec(-6, 1); yygc++;
            yyg[yygc] = new YYARec(-5, 2); yygc++;
            yyg[yygc] = new YYARec(-4, 9); yygc++;
            yyg[yygc] = new YYARec(-3, 3); yygc++;
            yyg[yygc] = new YYARec(-2, 10); yygc++;
            yyg[yygc] = new YYARec(-6, 1); yygc++;
            yyg[yygc] = new YYARec(-5, 2); yygc++;
            yyg[yygc] = new YYARec(-4, 11); yygc++;
            yyg[yygc] = new YYARec(-3, 3); yygc++;
            yyg[yygc] = new YYARec(-2, 10); yygc++;
            yyg[yygc] = new YYARec(-6, 12); yygc++;
            yyg[yygc] = new YYARec(-7, 17); yygc++;
            yyg[yygc] = new YYARec(-6, 19); yygc++;
            yyg[yygc] = new YYARec(-8, 25); yygc++;
            yyg[yygc] = new YYARec(-6, 26); yygc++;
            yyg[yygc] = new YYARec(-7, 27); yygc++;
            yyg[yygc] = new YYARec(-7, 28); yygc++;
            yyg[yygc] = new YYARec(-8, 32); yygc++;
            yyg[yygc] = new YYARec(-6, 26); yygc++;

            yyd = new int[yynstates];
            yyd[0] = 0;
            yyd[1] = -3;
            yyd[2] = 0;
            yyd[3] = 0;
            yyd[4] = 0;
            yyd[5] = 0;
            yyd[6] = 0;
            yyd[7] = 0;
            yyd[8] = 0;
            yyd[9] = -2;
            yyd[10] = -5;
            yyd[11] = -1;
            yyd[12] = 0;
            yyd[13] = 0;
            yyd[14] = 0;
            yyd[15] = 0;
            yyd[16] = 0;
            yyd[17] = -4;
            yyd[18] = 0;
            yyd[19] = 0;
            yyd[20] = 0;
            yyd[21] = 0;
            yyd[22] = -12;
            yyd[23] = -13;
            yyd[24] = 0;
            yyd[25] = 0;
            yyd[26] = 0;
            yyd[27] = -10;
            yyd[28] = -11;
            yyd[29] = -14;
            yyd[30] = -15;
            yyd[31] = -9;
            yyd[32] = -7;

            yyal = new int[yynstates];
            yyal[0] = 1;
            yyal[1] = 5;
            yyal[2] = 5;
            yyal[3] = 10;
            yyal[4] = 15;
            yyal[5] = 16;
            yyal[6] = 18;
            yyal[7] = 20;
            yyal[8] = 22;
            yyal[9] = 28;
            yyal[10] = 28;
            yyal[11] = 28;
            yyal[12] = 28;
            yyal[13] = 30;
            yyal[14] = 31;
            yyal[15] = 32;
            yyal[16] = 33;
            yyal[17] = 34;
            yyal[18] = 34;
            yyal[19] = 35;
            yyal[20] = 38;
            yyal[21] = 45;
            yyal[22] = 52;
            yyal[23] = 52;
            yyal[24] = 52;
            yyal[25] = 54;
            yyal[26] = 55;
            yyal[27] = 58;
            yyal[28] = 58;
            yyal[29] = 58;
            yyal[30] = 58;
            yyal[31] = 58;
            yyal[32] = 58;

            yyah = new int[yynstates];
            yyah[0] = 4;
            yyah[1] = 4;
            yyah[2] = 9;
            yyah[3] = 14;
            yyah[4] = 15;
            yyah[5] = 17;
            yyah[6] = 19;
            yyah[7] = 21;
            yyah[8] = 27;
            yyah[9] = 27;
            yyah[10] = 27;
            yyah[11] = 27;
            yyah[12] = 29;
            yyah[13] = 30;
            yyah[14] = 31;
            yyah[15] = 32;
            yyah[16] = 33;
            yyah[17] = 33;
            yyah[18] = 34;
            yyah[19] = 37;
            yyah[20] = 44;
            yyah[21] = 51;
            yyah[22] = 51;
            yyah[23] = 51;
            yyah[24] = 53;
            yyah[25] = 54;
            yyah[26] = 57;
            yyah[27] = 57;
            yyah[28] = 57;
            yyah[29] = 57;
            yyah[30] = 57;
            yyah[31] = 57;
            yyah[32] = 57;

            yygl = new int[yynstates];
            yygl[0] = 1;
            yygl[1] = 5;
            yygl[2] = 5;
            yygl[3] = 10;
            yygl[4] = 15;
            yygl[5] = 15;
            yygl[6] = 16;
            yygl[7] = 16;
            yygl[8] = 16;
            yygl[9] = 17;
            yygl[10] = 17;
            yygl[11] = 17;
            yygl[12] = 17;
            yygl[13] = 18;
            yygl[14] = 18;
            yygl[15] = 18;
            yygl[16] = 18;
            yygl[17] = 18;
            yygl[18] = 18;
            yygl[19] = 18;
            yygl[20] = 20;
            yygl[21] = 21;
            yygl[22] = 22;
            yygl[23] = 22;
            yygl[24] = 22;
            yygl[25] = 22;
            yygl[26] = 22;
            yygl[27] = 24;
            yygl[28] = 24;
            yygl[29] = 24;
            yygl[30] = 24;
            yygl[31] = 24;
            yygl[32] = 24;

            yygh = new int[yynstates];
            yygh[0] = 4;
            yygh[1] = 4;
            yygh[2] = 9;
            yygh[3] = 14;
            yygh[4] = 14;
            yygh[5] = 15;
            yygh[6] = 15;
            yygh[7] = 15;
            yygh[8] = 16;
            yygh[9] = 16;
            yygh[10] = 16;
            yygh[11] = 16;
            yygh[12] = 17;
            yygh[13] = 17;
            yygh[14] = 17;
            yygh[15] = 17;
            yygh[16] = 17;
            yygh[17] = 17;
            yygh[18] = 17;
            yygh[19] = 19;
            yygh[20] = 20;
            yygh[21] = 21;
            yygh[22] = 21;
            yygh[23] = 21;
            yygh[24] = 21;
            yygh[25] = 21;
            yygh[26] = 23;
            yygh[27] = 23;
            yygh[28] = 23;
            yygh[29] = 23;
            yygh[30] = 23;
            yygh[31] = 23;
            yygh[32] = 23;

            yyr[yyrc] = new YYRRec(2, -2); yyrc++;
            yyr[yyrc] = new YYRRec(2, -2); yyrc++;
            yyr[yyrc] = new YYRRec(1, -5); yyrc++;
            yyr[yyrc] = new YYRRec(2, -5); yyrc++;
            yyr[yyrc] = new YYRRec(1, -4); yyrc++;
            yyr[yyrc] = new YYRRec(0, -4); yyrc++;
            yyr[yyrc] = new YYRRec(2, -8); yyrc++;
            yyr[yyrc] = new YYRRec(0, -8); yyrc++;
            yyr[yyrc] = new YYRRec(5, -3); yyrc++;
            yyr[yyrc] = new YYRRec(4, -6); yyrc++;
            yyr[yyrc] = new YYRRec(4, -6); yyrc++;
            yyr[yyrc] = new YYRRec(3, -6); yyrc++;
            yyr[yyrc] = new YYRRec(3, -6); yyrc++;
            yyr[yyrc] = new YYRRec(3, -7); yyrc++;
            yyr[yyrc] = new YYRRec(3, -7); yyrc++;
            yyr[yyrc] = new YYRRec(0, -7); yyrc++;
        }

        public bool yyact(int state, int sym, ref int act)
        {
            int k = yyal[state];
            while (k <= yyah[state] && yya[k].sym != sym) k++;
            if (k > yyah[state]) return false;
            act = yya[k].act;
            return true;
        }
        public bool yygoto(int state, int sym, ref int nstate)
        {
            int k = yygl[state];
            while (k <= yygh[state] && yyg[k].sym != sym) k++;
            if (k > yygh[state]) return false;
            nstate = yyg[k].act;
            return true;
        }

        public void yyerror(string s)
        {
            System.Console.Write(s);
        }

        int yylexpos = -1;
        string yylval = "";

        public int yylex()
        {
            yylexpos++;
            if (yylexpos >= TokenList.Count)
            {
                yylval = "";
                return 0;
            }
            else
            {
                yylval = ((AToken)TokenList[yylexpos]).val;
                return ((AToken)TokenList[yylexpos]).token;
            }
        }

        public bool yyparse()
        {

        parse:

            yysp++;
            if (yysp > yymaxdepth)
            {
                yyerror("yyparse stack overflow");
                goto abort;
            }

            yys[yysp] = yystate;
            yyv[yysp] = yyval;

        next:

            if (yyd[yystate] == 0 && yychar == -1)
            {
                yychar = yylex();
                if (yychar < 0) yychar = 0;
            }

            yyn = yyd[yystate];
            if (yyn != 0) goto reduce;


            if (!yyact(yystate, yychar, ref yyn)) goto error;
            else if (yyn > 0) goto shift;
            else if (yyn < 0) goto reduce;
            else goto accept;

            error:

            if (yyerrflag == 0) yyerror("syntax error");

            errlab:

            if (yyerrflag == 0) yynerrs++;

            if (yyerrflag <= 2)
            {
                yyerrflag = 3;
                while (yysp > 0 && !(yyact(yys[yysp], 255, ref yyn) && yyn > 0)) yysp--;

                if (yysp == 0) goto abort;
                yystate = yyn;
                goto parse;
            }
            else
            {
                if (yychar == 0) goto abort;
                yychar = -1; goto next;
            }

        shift:

            yystate = yyn;
            yychar = -1;
            yyval = yylval;
            if (yyerrflag > 0) yyerrflag--;
            goto parse;

        reduce:

            yyflag = yyfnone;
            yyaction(-yyn);
            yysp -= yyr[-yyn].len;

            if (yygoto(yys[yysp], yyr[-yyn].sym, ref yyn)) yystate = yyn;

            switch (yyflag)
            {
                case 1: goto accept;
                case 2: goto abort;
                case 3: goto errlab;
            }

            goto parse;

        accept:

            return true;

        abort:

            return false;
        }
        ////////////////////////////////////////////////////////////////
        /// Scanner
        ////////////////////////////////////////////////////////////////

        public class AToken
        {
            public int token;
            public string val;
        }

        ArrayList TokenList = new ArrayList();

        public bool Scanner(string Input)
        {
            if (Input.Length == 0) return true;
            TokenList = new ArrayList();
            while (1 == 1)
            {
                AToken lasttoken = FindToken(Input);
                if (lasttoken.token == 0) break;
                if (lasttoken.token != t_ignore) TokenList.Add(lasttoken);
                if (Input.Length > lasttoken.val.Length)
                    Input = Input.Substring(lasttoken.val.Length);
                else return true;
            }
            System.Console.WriteLine(Input);
            System.Console.WriteLine();
            System.Console.WriteLine("No matching token found!");
            return false;
        }
        public AToken FindToken(string Rest)
        {
            ArrayList Results = new ArrayList();
            ArrayList ResultsV = new ArrayList();
            try
            {

                if (Regex.IsMatch(Rest, "^(\\()"))
                {
                    Results.Add(t_braceL);
                    ResultsV.Add(Regex.Match(Rest, "^(\\()").Value);
                }

                if (Regex.IsMatch(Rest, "^(\\))"))
                {
                    Results.Add(t_braceR);
                    ResultsV.Add(Regex.Match(Rest, "^(\\))").Value);
                }

                if (Regex.IsMatch(Rest, "^(assert)"))
                {
                    Results.Add(t_assert);
                    ResultsV.Add(Regex.Match(Rest, "^(assert)").Value);
                }

                if (Regex.IsMatch(Rest, "^(set)"))
                {
                    Results.Add(t_set);
                    ResultsV.Add(Regex.Match(Rest, "^(set)").Value);
                }

                if (Regex.IsMatch(Rest, "^(_)"))
                {
                    Results.Add(t_Underscore);
                    ResultsV.Add(Regex.Match(Rest, "^(_)").Value);
                }

                if (Regex.IsMatch(Rest, "^(for)"))
                {
                    Results.Add(t_for);
                    ResultsV.Add(Regex.Match(Rest, "^(for)").Value);
                }

                if (Regex.IsMatch(Rest, "^(cycles)"))
                {
                    Results.Add(t_cycles);
                    ResultsV.Add(Regex.Match(Rest, "^(cycles)").Value);
                }

                if (Regex.IsMatch(Rest, "^(ms)"))
                {
                    Results.Add(t_ms);
                    ResultsV.Add(Regex.Match(Rest, "^(ms)").Value);
                }

                if (Regex.IsMatch(Rest, "^([1-9][0-9]*)"))
                {
                    Results.Add(t_Number);
                    ResultsV.Add(Regex.Match(Rest, "^([1-9][0-9]*)").Value);
                }

                if (Regex.IsMatch(Rest, "^(true|false)"))
                {
                    Results.Add(t_Boolean);
                    ResultsV.Add(Regex.Match(Rest, "^(true|false)").Value);
                }

                if (Regex.IsMatch(Rest, "^(wait)"))
                {
                    Results.Add(t_wait);
                    ResultsV.Add(Regex.Match(Rest, "^(wait)").Value);
                }

                if (Regex.IsMatch(Rest, "^([\\r\\n\\t\\s])"))
                {
                    Results.Add(t_ignore);
                    ResultsV.Add(Regex.Match(Rest, "^([\\r\\n\\t\\s])").Value);
                }

                if (Regex.IsMatch(Rest, "^(IX_[A-Za-z0-9][A-Za-z0-9_]*)"))
                {
                    Results.Add(t_IX);
                    ResultsV.Add(Regex.Match(Rest, "^(IX_[A-Za-z0-9][A-Za-z0-9_]*)").Value);
                }

                if (Regex.IsMatch(Rest, "^(IW_[A-Za-z0-9][A-Za-z0-9_]*)"))
                {
                    Results.Add(t_IW);
                    ResultsV.Add(Regex.Match(Rest, "^(IW_[A-Za-z0-9][A-Za-z0-9_]*)").Value);
                }

                if (Regex.IsMatch(Rest, "^(QX_[A-Za-z0-9][A-Za-z0-9_]*)"))
                {
                    Results.Add(t_QX);
                    ResultsV.Add(Regex.Match(Rest, "^(QX_[A-Za-z0-9][A-Za-z0-9_]*)").Value);
                }

                if (Regex.IsMatch(Rest, "^(QW_[A-Za-z0-9][A-Za-z0-9_]*)"))
                {
                    Results.Add(t_QW);
                    ResultsV.Add(Regex.Match(Rest, "^(QW_[A-Za-z0-9][A-Za-z0-9_]*)").Value);
                }

            }
            catch { }
            int maxlength = 0;
            int besttoken = 0;
            AToken ret = new AToken();
            ret.token = besttoken;
            for (int i = 0; i < Results.Count; i++)
            {
                if (ResultsV[i].ToString().Length > maxlength)
                {
                    maxlength = ResultsV[i].ToString().Length;
                    besttoken = (int)Results[i];
                    ret.token = besttoken;
                    if (besttoken != 0)
                        ret.val = ResultsV[i].ToString();
                }
            }
            return ret;
        }


    }
}
