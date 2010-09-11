using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class Bind : IInterpretable
    {
        #region IInterpretable Members

        public void run(string function)
        {
            string[] command = function.Split(new Char[] { ' ' });
            string keyToBind = command[1];
            StringBuilder bindingTemp = new StringBuilder();
            string binding;
            for (int i = 2; i < command.Length; i++)
            {
                bindingTemp.Append(command[i]);
                bindingTemp.Append(" ");
            }

            keyToBind = keyToBind.ToUpper();
            binding = bindingTemp.ToString();
            switch (keyToBind)
            {
                case "A":
                    Game1.controller1.acommand = binding; 
                    break;
                case "B":
                    Game1.controller1.bcommand = binding;
                    break;
                case "X":
                    Game1.controller1.xcommand = binding;
                    break;
                case "Y":
                    Game1.controller1.ycommand = binding;
                    break;
            }
        }

        #endregion
    }
}
