using System;
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
            functions["load"] = new Load();
            functions["play"] = new Play();
        }

        public void run(string function)
        {
            string[] command = function.Split(new Char[] { ' ' });

            try
            {
                functions[command[0]].run(function);
            }
            catch
            {
                throw new ArgumentException("Command " + command[0] + " is unrecognized");
            }
        }


    }
}
