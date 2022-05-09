// Edouard Murat

// Similar to Brackys Audio Manager : https://www.youtube.com/watch?v=6OT43pvUyfY


using UnityEngine;

namespace Seance.SoundManagement
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public SoundCategory category;
        [Range(0f, 1f)] public float volume = 0.5f;
        [Range(0.1f, 3f)] public float pitch = 1.0f;
        public bool loop;

        [HideInInspector] public AudioSource source;
    }
}
