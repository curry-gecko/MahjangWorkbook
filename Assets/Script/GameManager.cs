using UniRx;
using UnityEngine;

namespace CGC.App
{
    public class GameManager : MonoBehaviour
    {
        private ReactiveProperty<GameState> _gameState = new();
        public IReadOnlyReactiveProperty<GameState> GameState => _gameState;

        private

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        // 開始準備
        void PreparingStartGame()
        {

        }

        // 開始中
        void StartGameInProgress()
        {

        }

        // 開始完了
        void CompleteGameStart()
        {

        }

        // 手番管理
        void ManagePlayerTurn()
        {

        }

        // 終了準備
        void PreparingEndGame()
        {

        }

        // 終了中
        void EndGameInProgress()
        {

        }

        // 終了
        void CompleteGameEnd()
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
