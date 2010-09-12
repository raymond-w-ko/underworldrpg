using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class SetV : IInterpretable
    {
        #region IInterpretable Members

        public void run(string function)
        {
            string[] command = function.Split(new Char[] { ' ' });
            command[1] = command[1].ToLower();
            switch (command[1])
            {
                case "width":
                    Game1.DefaultGraphics.PreferredBackBufferWidth =
                        Convert.ToInt32(command[2]);
                    break;
                case "height":
                    Game1.DefaultGraphics.PreferredBackBufferHeight =
                        Convert.ToInt32(command[2]);
                    break;
                case "fullscreen":
                    Game1.DefaultGraphics.IsFullScreen = true;
                    break;
                case "windowed":
                    Game1.DefaultGraphics.IsFullScreen = false;
                    break;
            }
        }

        #endregion
    }
}
