﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class Interpreter
    {
        Dictionary<string, IInterpretable> functions;
        LinkedList<string> history;

        public LinkedList<string> History
        {
            get
            {
                return history;
            }
        }

        public Interpreter()
        {
            functions = new Dictionary<string, IInterpretable>();
            history = new LinkedList<string>();

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
            #endregion

            foreach (string key in functions.Keys)
            {
                Game1.console.BindCommandHandler(key, new ConsoleCommandHandler(run), new Char[] { ' ' });
            }
        }

        public void run(string function)
        {
            if (function == null)
            {
                return;
            }

            string[] command = function.Split(new Char[] { ' ' });

            try
            {
                functions[command[0]].run(function);
            }
            catch (Exception e)
            {
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
