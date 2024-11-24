using System;
using UnityEngine;

namespace CGC.App
{
    [Serializable]
    public class Tile : MonoBehaviour
    {
        public Tile(TileSuit suit, bool isRedDora)
        {
            this.Suit = suit;
            this.IsRedDora = isRedDora;

        }
        public TileSuit Suit { get; private set; }
        public int Number { get; private set; } // 数字（1～9）: 字牌の場合はnull
        public bool IsRedDora { get; private set; }

        public Tile(TileSuit suit, int number = 0, bool isRedDora = false)
        {
            Suit = suit;
            Number = number;
            IsRedDora = isRedDora;
        }

        public override string ToString()
        {
            return IsRedDora ? $"{Suit} {Number}(赤)" : $"{Suit} {Number}";
        }
    }

    public enum TileSuit
    {
        Manzu,
        Pinzu,
        Souzu,
        Fonpai,
        Sangenpai
    }

    public enum Fonpai
    {
        Ton = 1,
        Nan = 2,
        Sha = 3,
        Pei = 4
    }
    public enum Sangenpai
    {
        Haku = 1,
        Hatsu = 2,
        Chun = 3
    }
}
