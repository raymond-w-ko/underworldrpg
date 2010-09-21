using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class Show : IInterpretable
    {
        #region IInterpretable Members

        public void run(string function)
        {
            string[] command = function.Split(Game1.interpreter.Mask);
            switch (command[1])
            {
                case "fps":
                    Game1.fps.IsVisible = !Game1.fps.IsVisible;
                    break;
            }
        }

        #endregion
    }
}
