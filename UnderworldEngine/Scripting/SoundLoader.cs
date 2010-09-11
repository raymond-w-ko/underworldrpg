using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnderworldEngine.Audio;

namespace UnderworldEngine.Scripting
{
    class SoundLoader : ILoader
    {
        public bool load(string name)
        {
            bool success = false;
            try
            {
                Game1.audioManager.AddSoundLibrary(name);
                success = true;
            }
            catch
            { }

            return success;
        }
    }
}
