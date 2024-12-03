using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.App.TileExtensions
{
    public static class TileExtensions
    {
        public static void SortTiles(this List<Tile> tiles)
        {
            if (tiles == null || tiles.Count == 0) return;

            tiles.Sort((a, b) =>
            {
                // Suitで比較
                int suitComparison = a.Suit.CompareTo(b.Suit);
                if (suitComparison != 0) return suitComparison;

                // Numberで比較
                int numberComparison = a.Number.CompareTo(b.Number);
                if (numberComparison != 0) return numberComparison;

                // 赤ドラ優先
                return b.IsRedDora.CompareTo(a.IsRedDora);
            });
        }


        public static void SortTileObjects(this List<TileObject> tileObjects)
        {
            if (tileObjects == null || tileObjects.Count == 0) return;

            tileObjects.Sort((a, b) =>
            {
                // Suitで比較
                int suitComparison = a.tile.Suit.CompareTo(b.tile.Suit);
                if (suitComparison != 0) return suitComparison;

                // Numberで比較
                int numberComparison = a.tile.Number.CompareTo(b.tile.Number);
                if (numberComparison != 0) return numberComparison;

                // 赤ドラ優先
                return b.tile.IsRedDora.CompareTo(a.tile.IsRedDora);
            });
        }
        // 雀頭の候補を取得する
        public static IEnumerable<Tile> GetPairs(this List<Tile> tiles)
        {
            return tiles
                .GroupBy(tile => new { tile.Suit, tile.Number })
                .Where(group => group.Count() >= 2)
                .Select(group => group.First());
        }
    }
}