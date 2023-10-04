using UnityEngine;
using UnityEngine.Audio;

namespace SoundKit
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager s_instance;

        private static SoundManager Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;

                s_instance = FindFirstObjectByType<SoundManager>();
                if (s_instance == null) Debug.LogError($"{typeof(SoundManager)} is not found.");

                return s_instance;
            }
        }

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (s_instance == this)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private static SoundPlayer GetSoundPlayer()
        {
            return SoundPlayer.GetOrCreate(Instance.transform);
        }

        public static ISoundHandler GetPlayer(AudioClip clip, AudioMixerGroup group = null)
        {
            return GetSoundPlayer().Initialize(clip, group);
        }
    }
}