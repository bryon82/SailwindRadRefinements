using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
            if (containedItems == null || containedItems.Count() == 0)
                return string.Empty;

            var invDict = new Dictionary<string, int>();
            foreach (var item in containedItems)
            {
                var key = item.description;
                key = key.Contains('<') ? key.Substring(0, key.IndexOf('<')) : key;                
                key = key.Contains('%') ? item.name : key;
                key = item.name.Equals("fishing hook") ? item.name : key;
                key = item.name.Equals("knife") ? item.name : key;
                key = string.IsNullOrEmpty(key) ? item.name : key;
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
    }
}
