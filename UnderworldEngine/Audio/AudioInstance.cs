using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.Audio
{
    class AudioInstance
    {
        /// <summary>
        /// Wrapper for XACT .xap packages
        /// Allows access to Sound and Wave Banks
        /// </summary>
        string name;
        AudioEngine eng;
        SoundBank sb;
        WaveBank wb;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public AudioEngine AudioEngine
        {
            get
            {
                return eng;
            }
        }

        public SoundBank SoundBank
        {
            get
            {
                return sb;
            }
        }

        public WaveBank WaveBank
        {
            get
            {
                return wb;
            }
        }

        public AudioInstance(string name)
        {
            this.name = name;
            Initialize();
        }

        void Initialize()
        {
            eng = new AudioEngine("Content\\Audio\\" + name + ".xgs");
            sb = new SoundBank(eng, "Content\\Audio\\Sound Bank.xsb");
            wb = new WaveBank(eng, "Content\\Audio\\Wave Bank.xwb");
        }

        public Cue GetCue(string cueName)
        {
            return sb.GetCue(cueName);
        }
    }
}
