
using System;
using CGC.App.Singleton;
using UniRx;
using UnityEngine;

namespace CGC.App
{
    // GameState を管理する
    public class StateManager : Singleton<StateManager>
    {

        private ReactiveProperty<GameState> _gameState = new(App.GameState.None);
        public IReadOnlyReactiveProperty<GameState> GameState => _gameState;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        // State を次のステートに移す
        // TODO 安全なStateに移せない可能性がある。ルールの判定と遷移をそれぞれ用意する。
        public void GoNext()
        {
            int prevState = (int)_gameState.Value;
            _gameState.Value = (GameState)(prevState + 1);
        }

        // 手番を対象プレイヤーに移す
        public void GotoTurnN()
        {
            if (_gameState.Value != App.GameState.Started
                && _gameState.Value != App.GameState.PreparingEnd)
            {
                throw new InvalidOperationException("手番へ移ることができないステートです。");
            }
            _gameState.Value = App.GameState.TurnN;
        }
    }
}
