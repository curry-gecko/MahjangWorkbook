using System;
using UniRx;
using UnityEngine;

namespace CGC.App
{
    public class GameManager : MonoBehaviour
    {
        private static int TILE_MAX_NUM = 13;

        private StateManager _stateManager;
        [SerializeField]
        private HandManager _handManager;
        [SerializeField]
        private TileManager _tileManager;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _stateManager = StateManager.Instance;
            _stateManager.GameState
                .Subscribe(s => OnStateChanged(s))
                .AddTo(this);

        }

        // Update is called once per frame
        void Update()
        {
            if (_stateManager.GameState.Value == GameState.None)
            {
                // 開始処理
                _stateManager.GoNext();
            }
        }

        private void OnStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.None:
                    break;
                case GameState.PreparingStart:
                    PreparingStartGame();
                    break;
                case GameState.Starting:
                    StartGameInProgress();
                    break;
                case GameState.Started:
                    CompleteGameStart();
                    break;
                case GameState.TurnN:
                    ManagePlayerTurn();
                    break;
                case GameState.PreparingEnd:
                    PreparingEndGame();
                    break;
                case GameState.Ending:
                    EndGameInProgress();
                    break;
                case GameState.Ended:
                    CompleteGameEnd();
                    break;

                default:
                    break;
            }
        }

        // 開始準備
        void PreparingStartGame()
        {
            _stateManager.GoNext();
        }

        // 開始中
        public void StartGameInProgress()
        {
            // player に 牌を配る
            for (int i = 0; i < TILE_MAX_NUM; i++)
            {
                _handManager.AddTiles(_tileManager.DistributeTile());
            }
            _handManager.SortTile();

            _stateManager.GoNext();
        }

        // 開始完了
        void CompleteGameStart()
        {
            _stateManager.GoNext();
        }

        // 手番管理
        public void ManagePlayerTurn()
        {
            // 手番のプレイヤーが牌を切ったら、ステートを進める
            _handManager.ExecuteTurn(() => _stateManager.GoNext());
        }

        // 終了準備
        void PreparingEndGame()
        {
            // 終了条件を満たさない場合、ステートを手番に戻す
            // TODO プレイヤーから和了を実行された際に判定をチェックし、実行する。
            _stateManager.GotoTurnN();
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

}
