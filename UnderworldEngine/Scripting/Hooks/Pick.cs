using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class Pick : IInterpretable
    {
        public delegate void Raise();
        public static Raise RaiseHandler;
        public delegate void Lower();
        public static Lower LowerHandler;

        #region IInterpretable Members

        public void run(string function)
        {
            string[] command = function.Split(Game1.interpreter.Mask);

            //TODO whatever logic you want
            switch (command[1]) {
                case "raise": {
                        RaiseHandler();
                        break;
                    }
                case "lower": {
                        LowerHandler();
                        break;
                    }
            }
        }

        #endregion
    }
}
