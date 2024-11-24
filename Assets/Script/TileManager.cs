using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CGC.App
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField]
        private List<Tile> Tiles;

        public bool useRedDora = false;

        void Start()
        {
            Tiles = new List<Tile>();
            InitializeTiles();
            ShuffleTiles();
        }

        // Update is called once per frame
        void Update()
        {

        }

        // 牌の初期化
        private void InitializeTiles()
        {
            Tiles.Clear();

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
                    Tiles.Add(new Tile(suit, n));
                }
            }
        }

        // 字牌を追加する
        private void AddHonorTiles(TileSuit suit, int number)
        {
            for (int i = 0; i < 4; i++)
            {
                Tiles.Add(new Tile(suit, number));
            }
        }

        public void ShuffleTiles()
        {

            System.Random rng = new();
            for (int i = Tiles.Count - 1; i > 0; i--)
            {
                int swapIndex = rng.Next(i + 1);
                (Tiles[swapIndex], Tiles[i]) = (Tiles[i], Tiles[swapIndex]);
            }
        }
    }
}
