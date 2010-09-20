using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnderworldEngine.Audio;
using UnderworldEngine.Game;

namespace UnderworldEngine.Scripting
{
    class Load : IInterpretable
    {
        public void run(string function)
        {
            string[] command = function.Split(Game1.interpreter.Mask);
            
            switch (command[1])
            {
                case "sndpkg":
                    Game1.audioManager.AddSoundLibrary(command[2]);
                    break;

                case "map": {
                        Level level = new Level(command[2]);
                        Game1.screenManager.AddScreen("level", level);
                        break;
                    }
            }
        }
    }
}
