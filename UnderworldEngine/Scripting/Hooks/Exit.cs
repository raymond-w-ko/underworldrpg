using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class Exit : IInterpretable
    {
        #region IInterpretable Members

        public void run(string function)
        {
            Game1.exit = true ;
        }

        #endregion
    }
}
