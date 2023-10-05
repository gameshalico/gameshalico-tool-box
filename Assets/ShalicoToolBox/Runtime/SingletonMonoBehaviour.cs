using UnityEngine;

namespace Shalico.ToolBox
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T s_instance;

        public static bool IsValid => s_instance != null;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = FindFirstObjectByType<T>();

                    if (s_instance == null)
                    {
                        Debug.LogError($"{typeof(T)} is not found.");
                    }
                    else
                    {
                        s_instance.Initialize();
                    }
                }

                return s_instance;
            }
        }

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = (T)this;
                s_instance.Initialize();
            }

            if (s_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (s_instance == this)
            {
                s_instance = null;
            }
        }

        protected virtual void Initialize() { }
    }
}