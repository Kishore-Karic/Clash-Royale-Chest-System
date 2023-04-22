using UnityEngine;

namespace ChestSystem.GenericSingleton
{
    public class GenericSingleton<T> : MonoBehaviour where T : GenericSingleton<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
    }
}