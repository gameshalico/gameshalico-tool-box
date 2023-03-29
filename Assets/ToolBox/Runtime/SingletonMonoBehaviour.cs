using UnityEngine;

namespace GS.ToolBox
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = (T)FindObjectOfType(typeof(T));

                    if (s_instance == null)
                    {
                        Debug.LogError($"{typeof(T)} is not found.");
                    }
                    s_instance.Initialize();
                }
                return s_instance;
            }
        }

        protected virtual void Initialize() { }

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this as T;
                s_instance.Initialize();
            }

            if (s_instance != this)
            {
                Debug.LogError($"{typeof(T)} is already exists.");
            }
        }
        private void OnDestroy()
        {
            if (s_instance == this)
            {
                s_instance = null;
            }
        }
    }
}