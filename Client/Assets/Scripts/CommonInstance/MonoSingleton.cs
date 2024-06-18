using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flux.CommonInstance
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_Instance = null;
        public static bool IsValidate { get { return s_Instance != null; } }
        private static readonly bool s_AppIsQuit = false;
        private static readonly object s_Sync = new object();

        public static T Instance
        {
            get
            {
                if (s_AppIsQuit)
                    return null;

                lock (s_Sync)
                {
                    if (s_Instance == null)
                    {
                        s_Instance = FindFirstObjectByType(typeof(T)) as T;

                        if (s_Instance == null)
                        {
                            var gameObject = new GameObject(typeof(T).ToString());

                            s_Instance = gameObject.AddComponent<T>();
                        }
                    }
                    return s_Instance;
                }
            }
        }

        public virtual void OnApplicationQuit()
        {
#if !UNITY_EDITOR
        s_appIsQuit = true;
#endif
            if (s_Instance != null)
                s_Instance.StopAllCoroutines();
        }
    }
}
