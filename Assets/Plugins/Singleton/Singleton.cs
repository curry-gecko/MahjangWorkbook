using UnityEngine;

namespace CGC.App.Singleton
{

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object Lock = new object();
        private static bool _applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"{typeof(T)} はアプリケーション終了時にアクセスされています。nullを返します。");
                    return null;
                }

                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindAnyObjectByType<T>();

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString();
                            DontDestroyOnLoad(singletonObject);
                        }
                    }
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _applicationIsQuitting = true;
            }
        }
    }
}