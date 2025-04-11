using Crest;
using System.Collections.Generic;
using UnityEngine;

namespace RadRefinements
{
    internal class FishMovement : MonoBehaviour
    {
        static readonly Dictionary<string, float> fishForces = new Dictionary<string, float>
        {
            { "31 templefish (A)", 10f },
            { "32 sunspot fish (A)", 13f },
            { "46 tuna (A)", 20f },
            { "35 shimmertail (E)", 18f },
            { "33 salmon (E)", 26f },
            { "34 eel (E)", 30f },
            { "38 blackfin hunter (M)", 20f },
            { "36 trout (M)", 25f },
            { "37 north fish (M)", 22f },
            { "141 swamp fish 1 (snapper", 21f }, // its named with the missing closing paren
            { "142 swamp fish 2 (bubbler)", 15f },
            { "148 swamp fish 3", 28f },
        };
        
        private float t;
        private float force;
        internal FishingRodFish fish;

        private void Awake()
        {
            t = 0f;
            force = 0f;
        }

        private void Update()
        {
            if (GameState.wasInSettingsMenu)
                return;

            var fishDead = fish.GetPrivateField<bool>("fishDead");
            var lastLineLength = fish.GetPrivateField<float>("lastLineLength");
            var fishEnergy = fish.GetPrivateField<float>("fishEnergy");
            var floater = fish.GetPrivateField<SimpleFloatingObject>("floater");
            var bobber = fish.GetPrivateField<Rigidbody>("bobber");

            fishDead = fishDead && lastLineLength <= 15f;
            fish.SetPrivateField("fishDead", fishDead);                                

            if (fishDead || fish.currentFish is null || fishEnergy <= 0f || !floater.InWater)
            {
                force = 0f;
                floater.SetPrivateField("_buoyancyCoeff", 3f);
                return;
            }

            if (force == 0f)
            {
                if (fishForces.ContainsKey(fish.currentFish.name))
                {
                    force = fishForces[fish.currentFish.name];
                }
                else
                {
                    force = 10f;
                    Plugin.logger.LogDebug($"{fish.currentFish.name} not found");
                }
            }

            if (t <= 0f)
            {
                force = -force;
                t = 10f + Random.Range(0, 0.3f * force);
            }
            floater.SetPrivateField("_buoyancyCoeff", 1f);
            bobber.AddRelativeForce(Vector3.right * force);
            bobber.AddRelativeForce(Vector3.forward * Random.Range(0f, 5f));
            bobber.AddRelativeForce(Vector3.up * Random.Range(-3f, 0));

            t -= 2f * Time.deltaTime;
        }
    }    
    
}
