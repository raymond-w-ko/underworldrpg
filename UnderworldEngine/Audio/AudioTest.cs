using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace UnderworldEngine.Audio
{
    public class AudioTest
    {
        // Audio objects
        AudioEngine engine;
        SoundBank soundBank;
        WaveBank waveBank;

        public AudioTest()
        {
            // Initialize audio objects.
            engine = new AudioEngine("Content\\Audio\\Test.xgs");
            soundBank = new SoundBank(engine, "Content\\Audio\\Sound Bank.xsb");
            waveBank = new WaveBank(engine, "Content\\Audio\\Wave Bank.xwb");

            // Play the sound.
            Cue cue = soundBank.GetCue("Spy Impossible");
            cue.Play();
        }
    }
}