using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class Interpreter
    {
        StringBuilder commandBuffer;
        Dictionary<string, IInterpretable> functions;
        Dictionary<string, string> env;
        public Dictionary<string, string> Env
        {
            get
            {
                return env;
            }
        }


        public Char[] Mask = new Char[] { ' ' };

        public Interpreter()
        {
            functions = new Dictionary<string, IInterpretable>();
            env = new Dictionary<string,string>();
            commandBuffer = new StringBuilder();
            loadFunctions();
        }

        private void loadFunctions()
        {
            #region interpreterBinds

            functions["bind"] = new Bind();
            functions["clear"] = new Clear();
            functions["console"] = new Console();
            functions["exit"] = new Exit();
            functions["if"] = new If();
            functions["let"] = new Let();
            functions["load"] = new Load();
            functions["music"] = new Music();
            functions["repeat"] = new Repeat();
            functions["pick"] = new Pick();
            functions["run"] = new Run();
            functions["setv"] = new SetV();
            functions["setc"] = new SetC();
            functions["show"] = new Show();
            functions["switch"] = new Switch();
            
            #endregion

            /*foreach (string key in functions.Keys)
            {
                Game1.console.BindCommandHandler(key, new ConsoleCommandHandler(run), new Char[] { ' ' });
            }*/
        }

        public void run(string function)
        {
            if (function == null)
            {
                return;
            }

            string[] command = function.Split(new Char[] { ' ' });

            //multilining stuff
            #region multiline
            if (command[0] == "\\\\")
                return;

            if (command[command.Length - 1] == "\\\\" || commandBuffer.Length > 0)
            {
                if (commandBuffer.Length > 0 && command[command.Length - 1] != "\\\\")
                {
                    for (int i = 0; i < command.Length; i++)
                    {
                        commandBuffer.Append(command[i]);
                        commandBuffer.Append(" ");
                    }
                }
                else
                {
                    for (int i = 0; i < command.Length - 1; i++)
                    {
                        commandBuffer.Append(command[i]);
                        commandBuffer.Append(" ");
                    }
                    return;
                }
            }
            #endregion

            //replace marked variables
            #region bash replace
            for (int i = 0; i < command.Length; i++)
            {
                if (command[i].Contains('$'))
                {
                    try
                    {
                        command[i] = command[i].Split(new Char[] { '$' })[1];
                        command[i] = env[command[i]];
                    }
                    catch (Exception e)
                    {
                        Game1.console.Log(e.Message);
                    }
                }
            }
            #endregion
            
            try
            {
                //check to see if function is null
                if (command[0].Contains("None"))
                {
                    return;
                }
                else if (commandBuffer.Length > 0) //then check to see if multilined
                {
                    function = commandBuffer.ToString(); 
                    command = function.Split(Game1.interpreter.Mask);
                    commandBuffer = new StringBuilder();
                }

                //put function back together from command array
                StringBuilder f = new StringBuilder();
                for (int i = 0; i < command.Length; i++)
                {
                    f.Append(command[i]);
                    f.Append(" ");
                }

                //else just run
                functions[command[0]].run(f.ToString());
            }
            catch (Exception e)
            {
                if (function == String.Empty)
                    return;

                Game1.console.Log(e.Message); 
            }
        }

        public void run(Microsoft.Xna.Framework.GameTime
            t, string function)
        {
            run(function);
        }


    }
}
