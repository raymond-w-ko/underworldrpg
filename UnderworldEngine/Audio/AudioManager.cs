using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.Collections;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.Audio
{
    class AudioManager
    {
        /// <summary>
        /// Manages the loading, fetching, playing, and pausing of sounds and music
        /// </summary>
        
        List<AudioInstance> instances;
        LinkedList<Cue> currentlyPlaying;
        Dictionary<Cue, AudioEmitter> cueToEmitter;
        Dictionary<Cue, AudioListener> cueToListener;
        Vector3 currentEmitterPosition;
        Vector3 currentListenerPosition;
        float volume;

        public Vector3 CurrentEmitterPosition
        {
            get
            {
                return currentEmitterPosition;
            }
            set
            {
                currentEmitterPosition = value;
            }
        }

        public Vector3 CurrentListeneverPosition
        {
            get
            {
                return currentListenerPosition;
            }
            set { currentListenerPosition = value; }
        }

        public float Volume
        {
            get
            {
                return volume;
            }
            set
            {
                volume = value;
                UpdateVolume();
            }
        }

        public AudioManager()
        {
            instances = new List<AudioInstance>();
            currentlyPlaying = new LinkedList<Cue>();
            cueToEmitter = new Dictionary<Cue, AudioEmitter>();
            cueToListener = new Dictionary<Cue, AudioListener>();
            currentListenerPosition = Vector3.Zero;
            currentEmitterPosition = Vector3.Zero;
        }

        public bool AddSoundLibrary(string name)
        {
            bool success;
            AudioInstance inst;
            inst = new AudioInstance(name);
            instances.Add(inst);
            success = true;

            return success;
        }


        public void PauseAll()
        {
            foreach (Cue cue in currentlyPlaying)
            {
                cue.Pause();
            }
        }

        public void PlayAll()
        {
            foreach (Cue cue in currentlyPlaying)
            {
                cue.Play();
            }
        }
        

        public void ResumeAll()
        {
            foreach (Cue cue in currentlyPlaying)
            {
                cue.Resume();
            }
        }

        public void StopAll()
        {
            foreach (Cue cue in currentlyPlaying)
            {
                cue.Stop(AudioStopOptions.Immediate);
            }
        }

        private void UpdateVolume()
        {
            foreach(AudioInstance inst in instances)
            {
                inst.AudioEngine.GetCategory("Default").SetVolume(volume);
            }
        }

        public Cue PlaySound(string name)
        {
            Cue sound = FetchSound(name);
            sound.Play();
            return sound;
        }

        public Cue FetchSound(string name)
        {
            Cue temp = null;

            foreach (AudioInstance inst in instances)
            {
                try
                {
                    temp = inst.SoundBank.GetCue(name);
                }
                catch 
                {
                    temp = null;
                }
            }

            if (temp == null)
            {
                throw new ArgumentNullException(name, "The requested sound was not found");
            }

            return temp;
        }
    }
}
