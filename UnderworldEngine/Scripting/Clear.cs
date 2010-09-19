using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class Clear : IInterpretable
    {
        #region IInterpretable Members

        public void run(string function)
        {
            for (int i = 0; i < 10; i++)
            {
                Game1.console.Log("\n");
            }
        }

        #endregion
    }
}
