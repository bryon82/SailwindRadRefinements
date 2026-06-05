using BepInEx;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadRefinements
{
    internal class Crate
    {
        public static string GetCrateInventory(ShipItemCrate crate)
        {
            var crateInventory = crate.GetPrivateField<CrateInventory>("crateInventory");
            if (crateInventory == null)
                return string.Empty;
            var containedItems = crateInventory.containedItems;
            if (containedItems == null || containedItems.Count == 0)
                return string.Empty;

            var invDict = new Dictionary<string, int>();
            foreach (var item in containedItems)
            {
                var key = NormalizeKey(item);
                if (!invDict.ContainsKey(key))
                {
                    invDict[key] = 1;
                }
                else
                {
                    invDict[key]++;
                }
            }
            var sb = new StringBuilder();
            sb.AppendLine($"crate");
            foreach (var pair in invDict.OrderBy(p => p.Key))
            {
                sb.AppendLine($"{pair.Key}: {pair.Value}");
            }
            return sb.ToString();
        }

        private static string NormalizeKey(ShipItem item)
        {
            var name = item.name;
            var key = item.description;
            var tagIndex = key.IndexOf('<');
            if (tagIndex >= 0)
                key = key.Substring(0, tagIndex);

            if (key.IndexOf('%') >= 0)
                return name;

            if (name.Equals("fishing hook") || name.Equals("knife") || name.Equals("hammer"))
                return name;

            return key.IsNullOrWhiteSpace() ? name : key;
        }
    }
}
