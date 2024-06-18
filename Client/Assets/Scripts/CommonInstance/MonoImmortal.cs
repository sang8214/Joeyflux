using UnityEngine;

namespace Flux.CommonInstance
{
    public class MonoImmortal<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static bool IsValidate  { get { return s_Instance != null; } }
        private static T s_Instance = null;
        private static bool s_AppIsQuit = false;
        private static object s_Sync = new object();
    
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
    
                            DontDestroyOnLoad(gameObject);
                        }
                        else
                        {
                            DontDestroyOnLoad(s_Instance.gameObject);
                        }
                    }
                    return s_Instance;
                }
            }
        }
    
        protected bool AwakeInstance()
        {
            if (s_Instance == null)
            {
                s_Instance = Instance;
                return true;
            }
            else if (s_Instance != this)
            {
                Destroy(this.gameObject);
                return false;
            }
    
            return true;
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
