using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace ShalicoSoundKit.Runtime
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager s_instance;
        private static readonly LinkedList<SoundHandler> s_soundHandlers = new();

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

        private void OnDestroy()
        {
            s_soundHandlers.Clear();
        }

        public static IEnumerable<ISoundHandler> GetAllHandlers()
        {
            return s_soundHandlers;
        }

        public static IEnumerable<ISoundHandler> GetHandlers(int soundID)
        {
            return s_soundHandlers.Where(x => x.SoundID == soundID);
        }


        private static SoundPlayer GetSoundPlayer()
        {
            return SoundPlayer.GetOrCreate(Instance.transform);
        }

        public static ISoundHandler GetPlayer(AudioClip clip, AudioMixerGroup group = null)
        {
            return GetSoundPlayer().Initialize(clip, group);
        }

        internal static void RegisterHandler(SoundHandler soundHandler)
        {
            s_soundHandlers.AddLast(soundHandler);
        }

        internal static void UnregisterHandler(SoundHandler soundHandler)
        {
            s_soundHandlers.Remove(soundHandler);
        }
    }
}