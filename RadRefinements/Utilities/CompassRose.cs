using System;
using System.Collections.Generic;

namespace RadRefinements
{
    public static class CompassRose
    {
        private static readonly string[] Directions4 = { "North", "East", "South", "West", "North" };
        private static readonly string[] Directions8 = {
            "North", "Northeast", "East", "Southeast",
            "South", "Southwest", "West", "Northwest", "North"
        };
        private static readonly string[] Directions16 = {
            "North", "North-northeast", "Northeast", "East-northeast",
            "East", "East-southeast", "Southeast", "South-southeast",
            "South", "South-southwest", "Southwest", "West-southwest",
            "West", "West-northwest", "Northwest", "North-northwest", "North"
        };
        private static readonly string[] Directions32 = {
            "North", "North by east", "North-northeast", "Northeast by north",
            "Northeast", "Northeast by east", "East-northeast", "East by north",
            "East", "East by south", "East-southeast", "Southeast by east",
            "Southeast", "Southeast by south", "South-southeast", "South by east",
            "South", "South by west", "South-southwest", "Southwest by south",
            "Southwest", "Southwest by west", "West-southwest", "West by south",
            "West", "West by north", "West-northwest", "Northwest by west",
            "Northwest", "Northwest by north", "North-northwest", "North by west", "North"
        };
        private static readonly Dictionary<string, string> Abbreviations = BuildAbbreviations();

        private static Dictionary<string, string> BuildAbbreviations()
        {
            var abbreviations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            AddAbbreviations(abbreviations, Directions4);
            AddAbbreviations(abbreviations, Directions8);
            AddAbbreviations(abbreviations, Directions16);
            AddAbbreviations(abbreviations, Directions32);
            return abbreviations;
        }

        private static void AddAbbreviations(Dictionary<string, string> abbreviations, string[] directions)
        {
            foreach (var direction in directions)
            {
                if (abbreviations.ContainsKey(direction))
                    continue;

                abbreviations[direction] = Abbreviate(direction);
            }
        }

        private static string Abbreviate(string direction)
        {
            return direction
                .ToLower()
                .Replace("north", "N")
                .Replace("east", "E")
                .Replace("south", "S")
                .Replace("west", "W")
                .Replace("-", "")
                .Replace(" by ", "b");
        }

        public static string GetCardinalDirection(float degrees, int precision)
        {
            if (precision == 4)
            {
                var index = (int)Math.Round(degrees / 90.0) % 4;
                return Directions4[index];
            }
            else if (precision == 8)
            {
                var index = (int)Math.Round(degrees / 45.0) % 8;
                return Directions8[index];
            }
            else if (precision == 16)
            {
                var index = (int)Math.Round(degrees / 22.5) % 16;
                return Directions16[index];
            }
            else // precision == 32
            {
                var index = (int)Math.Round(degrees / 11.25) % 32;
                return Directions32[index];
            }
        }

        public static string GetAbbreviatedDirection(float degrees, int precision)
        {
            string direction = GetCardinalDirection(degrees, precision);

            return Abbreviations.TryGetValue(direction, out var abbreviated)
                ? abbreviated
                : Abbreviate(direction);
        }
    }
}
