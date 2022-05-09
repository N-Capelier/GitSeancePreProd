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
        [Header("Initial Volumes")]
        [Range(-80f, 20f), Tooltip("Volume in decibel")] public float masterVolume;
        [Space(10)]
        [Range(-80f, 20f), Tooltip("Volume in decibel")] public float musicVolume;
        [Range(-80f, 20f), Tooltip("Volume in decibel")] public float sfxVolume;
        [Range(-80f, 20f), Tooltip("Volume in decibel")] public float ambienceVolume;
        [Range(-80f, 20f), Tooltip("Volume in decibel")] public float voiceVolume;
        [Range(-80f, 20f), Tooltip("Volume in decibel")] public float interfaceVolume;

        [Space(25)]
        public AudioMixer mainAudioMixer;
        public Mixer[] mixers;
        public Sound[] sounds;

        void Awake()
        {
            CreateSingleton();
            DontDestroyOnLoad(gameObject);

            CreateAudioBank();
        }

        private void Start()
        {
            masterVolume = PlayerPrefs.GetFloat("masterVolume", 0.5f);
            musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
            sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
            ambienceVolume = PlayerPrefs.GetFloat("ambienceVolume", 0.5f);
            voiceVolume = PlayerPrefs.GetFloat("voiceVolume", 0.5f); ;
            interfaceVolume = PlayerPrefs.GetFloat("interfaceVolume", 0.5f); ;
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

        // Method taken from Riverflow project
        public void SetVolumes()
        {
            ChangeVolume(masterVolume, "masterVolume");
            ChangeVolume(musicVolume, "musicVolume");
            ChangeVolume(sfxVolume, "sfxVolume");
            ChangeVolume(ambienceVolume, "ambienceVolume");
            ChangeVolume(voiceVolume, "voiceVolume");
            ChangeVolume(interfaceVolume, "interfaceVolume");
        }

        // Method taken from Riverflow project
        public void ChangeVolume(float value, string targetGroup)
        {
            mainAudioMixer.SetFloat(targetGroup, value != 0 ? Mathf.Log10(value) * 20 : -80);
            PlayerPrefs.SetFloat(targetGroup, value);
        }

        // Method taken from Riverflow project
        public void ChangePitch(float value, AudioSource audioSource)
        {
            audioSource.pitch = value;
        }
    }
}
