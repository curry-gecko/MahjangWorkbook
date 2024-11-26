using System;
using UnityEngine;

namespace CGC.App
{
    [System.Serializable]
    public class Tile
    {
        public Tile() { }
        public Tile(TileSuit suit, int number = 0, bool isRedDora = false)
        {
            Suit = suit;
            Number = number;
            IsRedDora = isRedDora;
        }

        public Tile(TileSuit suit, bool isRedDora)
        {
            this.Suit = suit;
            this.IsRedDora = isRedDora;

        }
        public TileSuit Suit { get; private set; }
        public int Number { get; private set; } // 数字（1～9）: 字牌の場合はnull
        public bool IsRedDora { get; private set; }

        public string TileToString()
        {
            string str = "";

            // 文字
            switch (this.Suit)
            {
                case TileSuit.Manzu:
                    str = Number + "萬";
                    break;
                case TileSuit.Pinzu:
                    str = Number + "筒";
                    break;
                case TileSuit.Souzu:
                    str = Number + "索";
                    break;
                case TileSuit.Fonpai:
                    str = ((Fonpai)Number).ToLocalizedString();
                    break;
                case TileSuit.Sangenpai:
                    str = ((Sangenpai)Number).ToLocalizedString();
                    break;
                default:
                    str = "this is not Tile";
                    break;
            }

            return str;
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
    public static class EnumExtensions
    {
        public static string ToLocalizedString(this Fonpai value)
        {
            return value switch
            {
                Fonpai.Ton => "東",
                Fonpai.Nan => "南",
                Fonpai.Sha => "西",
                Fonpai.Pei => "北",
                _ => value.ToString()
            };
        }

        public static string ToLocalizedString(this Sangenpai value)
        {
            return value switch
            {
                Sangenpai.Haku => "白",
                Sangenpai.Hatsu => "發",
                Sangenpai.Chun => "中",
                _ => value.ToString()
            };
        }
    }
}
