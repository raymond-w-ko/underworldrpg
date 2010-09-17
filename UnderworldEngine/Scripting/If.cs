using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class If : IInterpretable
    {
        #region IInterpretable Members

        public void run(string function)
        {
            string[] command = function.Split(Game1.interpreter.Mask);
            if (command[1].Trim() == "1")
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 2; i < command.Length; i++)
                {
                    sb.Append(command[i]);
                    sb.Append(" ");
                }
                Game1.interpreter.run(sb.ToString());
            }
            else
            {
                throw new ArgumentException("Does not evaluate to 1", command[1]);
            }
        }

        #endregion
    }
}
