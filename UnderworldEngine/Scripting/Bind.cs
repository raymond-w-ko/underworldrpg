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
            }

            keyToBind = keyToBind.ToUpper();
            switch (keyToBind)
            {
                case "A":
                     
                    break;
            }
        }

        #endregion
    }
}
