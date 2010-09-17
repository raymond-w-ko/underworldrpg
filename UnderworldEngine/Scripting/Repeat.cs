using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class Repeat : IInterpretable
    {
        #region IInterpretable Members

        public void run(string function)
        {
            string[] command = function.Split(new Char[] { ' ' });
            //TODO implement multiline parsing
            int repeats = Convert.ToInt32(command[1]);
            StringBuilder sb = new StringBuilder();
            for (int i = 2; i < command.Length; i++)
            {
                sb.Append(command[i]);
                sb.Append(" ");
            }

            string func = sb.ToString();

            for (int i = 0; i < repeats; i++)
            {
                Game1.console.Log("Running " + func);
                Game1.interpreter.run(func);
            }
        }

        #endregion
    }
}
