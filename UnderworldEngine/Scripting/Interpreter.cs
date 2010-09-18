using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class Interpreter
    {
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

            loadFunctions();
        }

        private void loadFunctions()
        {
            #region interpreterBinds
            functions["load"] = new Load();
            functions["music"] = new Music();
            functions["bind"] = new Bind();
            functions["run"] = new Run();
            functions["setv"] = new SetV();
            functions["setc"] = new SetC();
            functions["show"] = new Show();
            functions["let"] = new Let();
            functions["repeat"] = new Repeat();
            functions["if"] = new If();
            functions["pick"] = new Pick();
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
                functions[command[0]].run(function);
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
