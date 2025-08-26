using System.Collections.Generic;

namespace RadRefinements
{
    public class Knife
    {
        private static readonly Dictionary<string, int> _woodPiecesPerContainer =
            new Dictionary<string, int>
                {
                    { "small crate", 4 },
                    { "standard crate", 6 },
                    { "large crate", 8 },
                    { "very large crate", 10 },
                    { "standard barrel", 6 },
                    { "firewood", 4 },
                    { "fishing hooks", 4 },
                    { "lantern candles", 2 },
                    { "green tobacco", 2 },
                    { "blue tobacco", 2 },
                    { "black tobacco", 2 },
                    { "brown tobacco", 2 },
                    { "white tobacco", 2 },
                    { "empty crate", 6 },
                    { "sealing nails", 2 },
                    { "spoon lures", 2 },
                    { "swimbait lures", 2 },
                    { "topwater lures", 2 },
                };

        public static Dictionary<string, int> WoodPiecesPerContainer => _woodPiecesPerContainer;
    }
}
