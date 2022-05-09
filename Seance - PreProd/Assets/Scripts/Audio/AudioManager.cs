// Edouard Murat

// Similar to Brackys Audio Manager : https://www.youtube.com/watch?v=6OT43pvUyfY


using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

namespace Seance.SoundManagement
{
    public class AudioManager : Singleton<AudioManager>
    {
        public Mixer[] mixers;
        public Sound[] sounds;

        void Awake()
        {
            CreateSingleton();
            DontDestroyOnLoad(gameObject);

            CreateAudioBank();
        }

        private void CreateAudioBank()
        {
            foreach(Sound s in sounds)
            {
                AudioSource _source = gameObject.AddComponent<AudioSource>();
                _source.clip = s.clip;
                _source.outputAudioMixerGroup = Array.Find(mixers, mixer => mixer.category == s.category).mixerGroup;
                _source.volume = s.volume;
                _source.pitch = s.pitch;
                _source.loop = s.loop;

                s.source = _source;
            }
        }

        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            
            if(s == null)
            {
                Debug.LogWarning("AudioManager : Sound '" + name + "' is unknown. Check name spelling");
            }
            else
            {
                s.source.Play();
            }
        }
    }
}
