using UnityEngine;

namespace CGC.App
{
    public class GameManager : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public enum GameState
    {
        None,           // 状態なし (初期値)
        PreparingStart,  // 開始準備
        Starting,        // 開始中
        Started,         // 開始完了
        TurnN,           // 手番 (Nプレイヤーの手番)
        PreparingEnd,    // 終了準備
        Ending,          // 終了中
        Ended            // 終了
    }
}
