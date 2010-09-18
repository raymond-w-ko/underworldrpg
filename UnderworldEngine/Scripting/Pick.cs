using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class Pick : IInterpretable
    {
        #region IInterpretable Members

        public void run(string function)
        {
            string[] command = function.Split(Game1.interpreter.Mask);

            //TODO whatever logic you want
        }

        #endregion
    }
}
