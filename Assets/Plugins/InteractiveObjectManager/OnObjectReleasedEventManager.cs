using System;
using UniRx;
using UnityEngine;

namespace InteractiveObjectManager
{
    public class OnObjectReleasedEventManager : MonoBehaviour
    {
        private static string NAME_EVENT_MANAGER = "OnObjectReleasedEventManager";
        // サブジェクトの定義
        public Subject<(IClickableObject, IClickableObject)> OnObjectsReleased = new();

        // シングルトンのインスタンス
        private static OnObjectReleasedEventManager _instance;
        public static OnObjectReleasedEventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    CreateInstance(); // インスタンスを確実に作成
                }
                return _instance;
            }
        }

        private static void CreateInstance()
        {
            // 新しい GameObject を作成してコンポーネントを追加
            GameObject obj = new GameObject(NAME_EVENT_MANAGER);
            _instance = obj.AddComponent<OnObjectReleasedEventManager>();
            DontDestroyOnLoad(obj); // シーン間で保持
        }

        private void Awake()
        {
            // インスタンスが既に存在する場合、このオブジェクトを破棄
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            // オブジェクトが破棄された時にサブジェクトを完了
            OnObjectsReleased.OnCompleted();
        }
    }
}
