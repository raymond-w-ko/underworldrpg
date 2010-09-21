using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace UnderworldEngine.Scripting
{
    class Console : IInterpretable
    {
        #region IInterpretable Members

        public void run(string function)
        {
            string[] command = function.Split(Game1.interpreter.Mask);

            
            Game1.console.Open(Keys.OemTilde);
        }

        #endregion
    }
}
