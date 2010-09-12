﻿using System;
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
            string[] command = function.Split(new Char[] { ' ' });
            switch (command[1])
            {
                case "play":
                    Game1.audioManager.PlaySound(command[2]);
                    break;
                case "pause":
                    Game1.audioManager.PauseAll();
                    break;
                case "stop":
                    Game1.audioManager.StopAll();
                    break;
                case "resume":
                    Game1.audioManager.ResumeAll();
                    break;
                default:
                    throw new ArgumentException("Option " + command[1] + " is an invalid option");
            }
        }

        #endregion
    }
}