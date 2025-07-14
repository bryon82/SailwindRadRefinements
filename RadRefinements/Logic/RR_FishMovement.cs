using Crest;
using System.Collections.Generic;
using UnityEngine;
using static RadRefinements.RR_Plugin;

namespace RadRefinements
{
    internal class RR_FishMovement : MonoBehaviour
    {
        public readonly struct FishProperties
        {
            public float Force { get; }
            public float Tension { get; }

            public FishProperties(float force, float tension)
            {
                Force = force;
                Tension = tension;
            }
        }

        private static readonly Dictionary<string, FishProperties> _fishData = new Dictionary<string, FishProperties>
        {
            { "31 templefish (A)", new FishProperties(10f, 0.95f) },
            { "32 sunspot fish (A)", new FishProperties(13f, 0.85f) },
            { "46 tuna (A)", new FishProperties(20f, 0.78f) },
            { "35 shimmertail (E)", new FishProperties(18f, 0.83f) },
            { "33 salmon (E)", new FishProperties(26f, 0.77f) },
            { "34 eel (E)", new FishProperties(30f, 0.72f) },
            { "38 blackfin hunter (M)", new FishProperties(20f, 0.85f) },
            { "36 trout (M)", new FishProperties(25f, 0.74f) },
            { "37 north fish (M)", new FishProperties(22f, 0.79f) },
            { "141 swamp fish 1 (snapper", new FishProperties(21f, 0.83f) },
            { "142 swamp fish 2 (bubbler)", new FishProperties(15f, 0.9f) },
            { "148 swamp fish 3", new FishProperties(28f, 0.76f) },
            { "140 gold albacore", new FishProperties(32f, 0.7f) },
        };

        private float _timer;
        private float _fishForce;
        private FishingRodFish _fish;

        public FishingRodFish Fish
        {
            get => _fish;
            set => _fish = value;
        }

        private void Awake()
        {
            _timer = 0f;
            _fishForce = 0f;
        }

        private void Update()
        {
            if (GameState.wasInSettingsMenu)
                return;

            var fishDead = _fish.GetPrivateField<bool>("fishDead");
            var lastLineLength = _fish.GetPrivateField<float>("lastLineLength");
            var fishEnergy = _fish.GetPrivateField<float>("fishEnergy");
            var floater = _fish.GetPrivateField<SimpleFloatingObject>("floater");
            var bobber = _fish.GetPrivateField<Rigidbody>("bobber");

            fishDead = fishDead && lastLineLength <= 15f;
            _fish.SetPrivateField("fishDead", fishDead);

            if (fishDead || _fish.currentFish is null || fishEnergy <= 0f || !floater.InWater)
            {
                _fishForce = 0f;
                floater.SetPrivateField("_buoyancyCoeff", 3f);
                return;
            }

            if (_fishForce == 0f)
            {
                if (_fishData.ContainsKey(_fish.currentFish.name))
                {
                    var fishData = _fishData[_fish.currentFish.name];
                    _fishForce = fishData.Force;
                }
                else
                {
                    _fishForce = 10f;
                    LogWarning($"{_fish.currentFish.name} not found");
                }
            }

            if (_timer <= 0f)
            {
                _fishForce = -_fishForce;
                _timer = 10f + Random.Range(0, 0.3f * _fishForce);
            }
            floater.SetPrivateField("_buoyancyCoeff", 1f);
            bobber.AddRelativeForce(Vector3.right * _fishForce);
            bobber.AddRelativeForce(Vector3.forward * Random.Range(0f, 5f));
            bobber.AddRelativeForce(Vector3.up * Random.Range(-3f, 0));

            _timer -= 2f * Time.deltaTime;
        }

        public static float FishTension(string fishName)
        {
            if (_fishData.ContainsKey(fishName))
            {
                return _fishData[fishName].Tension;
            }
            else
            {
                LogWarning($"{fishName} not found in fish data.");
                return 0.95f;
            }
        }
    }
}
