using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class Music : IInterpretable
    {
        ///<summary>
        ///plays media files
        ///</summary>

        #region IInterpretable Members

        public void run(string function)
        {
            string[] command = function.Split(Game1.interpreter.Mask);
            switch (command[1])
            {
                case "play":
                case "p":
                    Game1.audioManager.PlaySound(command[2]);
                    break;
                case "pause":
                case "a":
                    Game1.audioManager.PauseAll();
                    break;
                case "stop":
                case "s":
                    Game1.audioManager.StopAll();
                    break;
                case "resume":
                case "r":
                    Game1.audioManager.ResumeAll();
                    break;
                default:
                    throw new ArgumentException("invalid option", command[1]);
            }
        }

        #endregion
    }
}
