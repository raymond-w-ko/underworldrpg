using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class Play : IInterpretable
    {
        ///<summary>
        ///plays media files
        ///</summary>

        #region IInterpretable Members

        public void run(string function)
        {
            string[] command = function.Split(new Char[] { ' ' });
            Game1.audioManager.PlaySound(command[2]);
        }

        #endregion
    }
}
