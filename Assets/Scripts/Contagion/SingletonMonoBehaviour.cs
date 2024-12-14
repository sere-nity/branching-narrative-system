using UnityEngine;

namespace Contagion
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T singleton;
        protected static bool isReadonly;

        public static T Singleton
        {
            get
            {
               
                if (!isReadonly && singleton == null)
                {
                    // note this isn't that performant but singleton
                    // will rarely be null as we populate it in Awake
                    singleton = FindObjectOfType<T>();
                }
                return singleton;
            }
            protected set
            {
                // Don't set if readonly or trying to set same value
                if (isReadonly || singleton == value)
                {
                    return;
                }

                // Handle duplicate instances
                if (singleton != null && value != null)
                {
                    Debug.LogWarning($"{typeof(T).Name}.Singleton already exists! Destroying duplicate.");
                    Destroy(value.gameObject);
                    return;
                }

                singleton = value;
            }
        }

        protected virtual void Awake()
        {
            Singleton = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (singleton == this as T)
            {
                singleton = null;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            isReadonly = true;
        }
    }
}