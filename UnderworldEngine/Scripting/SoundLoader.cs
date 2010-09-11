using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnderworldEngine.Audio;

namespace UnderworldEngine.Scripting
{
    class SoundLoader : ILoader
    {
        AudioManager audio;

        public SoundLoader(AudioManager audio)
        {
            this.audio = audio;
        }

        public bool load(string name)
        {
            bool success = false;
            try
            {
                audio.AddSoundLibrary(name);
                success = true;
            }
            catch
            { }

            return success;
        }
    }
}
