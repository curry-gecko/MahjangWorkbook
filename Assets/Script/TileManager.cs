using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CGC.App
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField]
        private List<Tile> _tiles;

        public bool useRedDora = false;

        //
        [SerializeField]
        private GameObject _tileObjectPrefab;
        [SerializeField]
        private HandManager _handManager;

        void Start()
        {
            _tiles = new List<Tile>();
            InitializeTiles();
            ShuffleTiles();

            if (_tileObjectPrefab == null)
            {
                Debug.Log("OnStart" + ": TileObjectPrefab is null.");
            }

            if (_handManager == null)
            {
                Debug.Log("OnStart" + ": _handManager is null.");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        // 牌の初期化
        private void InitializeTiles()
        {
            _tiles.Clear();

            // 数牌
            AddNumberedTiles(TileSuit.Manzu);
            AddNumberedTiles(TileSuit.Pinzu);
            AddNumberedTiles(TileSuit.Souzu);

            // 風牌
            AddHonorTiles(TileSuit.Fonpai, (int)Fonpai.Ton);
            AddHonorTiles(TileSuit.Fonpai, (int)Fonpai.Nan);
            AddHonorTiles(TileSuit.Fonpai, (int)Fonpai.Sha);
            AddHonorTiles(TileSuit.Fonpai, (int)Fonpai.Pei);

            // 三元牌
            AddHonorTiles(TileSuit.Sangenpai, (int)Sangenpai.Haku);
            AddHonorTiles(TileSuit.Sangenpai, (int)Sangenpai.Hatsu);
            AddHonorTiles(TileSuit.Sangenpai, (int)Sangenpai.Chun);

            // 赤ドラ追加 TODO (省略)

        }

        // 数牌を追加する
        private void AddNumberedTiles(TileSuit suit)
        {
            for (int n = 1; n <= 9; n++)
            {
                for (int i = 0; i < 4; i++)
                {
                    _tiles.Add(new Tile(suit, n));
                }
            }
        }

        // 字牌を追加する
        private void AddHonorTiles(TileSuit suit, int number)
        {
            for (int i = 0; i < 4; i++)
            {
                _tiles.Add(new Tile(suit, number));
            }
        }

        public void ShuffleTiles()
        {

            System.Random rng = new();
            for (int i = _tiles.Count - 1; i > 0; i--)
            {
                int swapIndex = rng.Next(i + 1);
                (_tiles[swapIndex], _tiles[i]) = (_tiles[i], _tiles[swapIndex]);
            }
        }

        public GameObject DistributeTile()
        {
            Tile tile = PopTile(0);

            return InstantiateTileObject(tile);
        }

        private Tile PopTile(int tileIndex)
        {
            Tile tile = _tiles[tileIndex];
            _tiles.RemoveAt(tileIndex);
            return tile;
        }
        private GameObject InstantiateTileObject(Tile tile)
        {
            if (_tileObjectPrefab == null)
            {
                Debug.Log("InstantiateTileObject " + ":" + " TileObjectPrefab is null.");
                return null;
            }

            GameObject prefab = Instantiate(_tileObjectPrefab, null);

            if (prefab.TryGetComponent<TileObject>(out var _tileObject))
            {
                _tileObject.SetTile(tile);
            }

            return prefab;
        }
    }
}
