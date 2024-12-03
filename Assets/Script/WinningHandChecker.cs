using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGC.App.TileExtensions;

namespace CGC.App
{
    public class WinningHandChecker
    {
        // 和了判定
        public bool CheckWinningHand(List<Tile> tiles)
        {
            // 特殊形（七対子、国士無双）の判定
            if (CheckSevenPairs(tiles) || CheckKokushiMusou(tiles))
            {
                return true;
            }

            // 通常形の判定
            return CheckStandardHand(tiles);
        }

        // 七対子の判定
        private bool CheckSevenPairs(List<Tile> hand)
        {
            var pairs = hand.GroupBy(tile => new { tile.Suit, tile.Number })
                                  .Where(group => group.Count() == 2)
                                  .Count();

            return pairs == 7; // 7ペアであれば七対子
        }

        // 国士無双の判定
        private bool CheckKokushiMusou(List<Tile> hand)
        {
            var terminalsAndHonors = new HashSet<(TileSuit, int)>
        {
            (TileSuit.Manzu, 1), (TileSuit.Manzu, 9),
            (TileSuit.Pinzu, 1), (TileSuit.Pinzu, 9),
            (TileSuit.Souzu, 1), (TileSuit.Souzu, 9),
            (TileSuit.Fonpai, 1), (TileSuit.Fonpai, 2), (TileSuit.Fonpai, 3), (TileSuit.Fonpai, 4),
            (TileSuit.Sangenpai, 1), (TileSuit.Sangenpai, 2), (TileSuit.Sangenpai, 3),
        };

            var distinctTiles = hand.Distinct().Select(tile => (tile.Suit, tile.Number)).ToHashSet();
            return terminalsAndHonors.IsSubsetOf(distinctTiles);
        }

        // 通常形（雀頭＋面子×4）の判定
        private bool CheckStandardHand(List<Tile> hand)
        {
            // 雀頭を選択
            foreach (var pair in hand.GetPairs())
            {
                var remainingTiles = new List<Tile>(hand);
                remainingTiles.RemoveAll(tile => tile.Suit == pair.Suit && tile.Number == pair.Number);

                if (CheckMelds(remainingTiles))
                {
                    return true;
                }
            }

            return false;
        }

        // 順子・刻子・槓子を構成可能か判定
        private bool CheckMelds(List<Tile> tiles)
        {
            if (tiles.Count == 0) return true; // 全ての牌を面子にできれば和了

            for (int i = 0; i < tiles.Count; i++)
            {
                var tile = tiles[i];

                // 刻子判定
                if (tiles.Count(t => t.Suit == tile.Suit && t.Number == tile.Number) >= 3)
                {
                    var remaining = new List<Tile>(tiles);
                    remaining.RemoveAll(t => t.Suit == tile.Suit && t.Number == tile.Number);
                    if (CheckMelds(remaining)) return true;
                }

                // 順子判定
                if (tile.Number <= 7 &&
                    tiles.Any(t => t.Suit == tile.Suit && t.Number == tile.Number + 1) &&
                    tiles.Any(t => t.Suit == tile.Suit && t.Number == tile.Number + 2))
                {
                    var remaining = new List<Tile>(tiles);
                    remaining.Remove(tile);
                    remaining.RemoveAll(t => t.Suit == tile.Suit && t.Number == tile.Number + 1);
                    remaining.RemoveAll(t => t.Suit == tile.Suit && t.Number == tile.Number + 2);
                    if (CheckMelds(remaining)) return true;
                }
            }

            return false;
        }
    }
}