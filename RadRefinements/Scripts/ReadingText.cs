using System;
using UnityEngine;
using static RadRefinements.Configs;
using static RadRefinements.RR_Plugin;

namespace RadRefinements
{
    internal class ReadingText : MonoBehaviour
    {
        internal TextMesh textMesh;
        internal ShipItem shipItem;
        internal Transform indicator1;
        internal Transform indicator2;
        private bool _isInSunlight;
        private float _sunlightTimer;
        internal bool isCompass;
        internal bool isChipLog;
        internal bool isClock;
        internal bool isQuadrant;
        internal bool isSunCompass;
        internal bool isInclinometer;
        internal bool isBarometer;
        internal bool isThermometer;
        internal bool isHygrometer;
        internal bool isWindCompass;
        internal bool isAnemometer;
        internal bool isWeathervane;

        internal bool isCompassFlipped = false;

        const float CHIPLOG_ANGLE_TO_KNOTS = 1f / 15f;
        const float ANEMOMETER_ANGLE_TO_KNOTS = 1f / 7.2f;

        private static readonly Vector3 NOT_HELD_SIZE = new Vector3(0.0114f, 0.0127f, 0.0127f);
        private static readonly Vector3 HELD_SIZE = new Vector3(0.0044f, 0.0057f, 0.0057f);

        private void LateUpdate()
        {
            if (!GameState.playing || GameState.currentlyLoading || GameState.loadingBoatLocalItems)
                return;

            if (textMesh == null)
                return;

            if (BoatCamera.on)
            {
                textMesh.gameObject.SetActive(false);
                return;
            }

            if (textMesh.color != readingTextColor.Value)
                textMesh.color = readingTextColor.Value;

            var isHeld = shipItem.held != null;
            var observerPos = Refs.observerMirror.transform.position;
            var itemPos = transform.position;
            var distToItem = Vector3.Distance(observerPos, itemPos);
            var notSoldInInvOrNotOnBoat =
                !shipItem.sold
                || gameObject.layer == 5
                || shipItem.currentActualBoat == null;
            var canNotShowReading = 
                isHeld
                || notSoldInInvOrNotOnBoat
                || Vector3.Angle(-transform.forward, observerPos - itemPos) > 85f
                || SpyglassPatches.HeldAndUp;

            if (isCompass)
            {
                if (indicator1 == null)
                    return;

                var canNotShow =
                    (!enableCompassDegreesText.Value && !enableCompassCardinalText.Value)
                    || (isHeld && !enableCompassReadingHeld.Value)
                    || notSoldInInvOrNotOnBoat
                    || distToItem > compassViewDist.Value
                    || SpyglassPatches.HeldAndUp;

                if (canNotShow)
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                var angleToPlayer = isHeld ? 0f : Vector3.SignedAngle(-transform.forward, observerPos - itemPos, Vector3.up);
                textMesh.transform.localEulerAngles = new Vector3(0, angleToPlayer, 0);
                textMesh.transform.localScale = isHeld ? HELD_SIZE : NOT_HELD_SIZE;

                var offset = isCompassFlipped ? 180f : 360f;
                var reading = (180f - indicator1.transform.localEulerAngles.y + offset) % 360f;
                textMesh.text = GetCompassReading(reading, compassCardinalPrecision.Value, compassDecimalPlaces.Value);
                textMesh.gameObject.SetActive(true);
            }

            else if (isChipLog)
            {
                if ( indicator1 == null)
                    return;

                var canNotShow =
                    !enableChipLogText.Value
                    || canNotShowReading
                    || distToItem > chipLogViewDist.Value
                    || !shipItem.GetPrivateField<bool>("thrown");

                if (canNotShow)
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                var reading = ((360f - indicator1.localEulerAngles.z) % 360f) * CHIPLOG_ANGLE_TO_KNOTS;
                textMesh.text = $"{reading.ToString("F" + chipLogDecimalPlaces.Value)} kts";
                textMesh.gameObject.SetActive(true);
            }

            else if (isClock)
            {
                var canNotShow =
                    (!enableClockGlobalText.Value && !enableClockLocalText.Value)
                    || canNotShowReading
                    || distToItem > clockViewDist.Value;

                if (canNotShow )
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                textMesh.text = GetClockReading();
                textMesh.gameObject.SetActive(true);
            }

            else if (isQuadrant)
            {
                var canNotShow =
                    !enableQuadrantText.Value ||
                    !shipItem.GetPrivateField<bool>("inspecting")
                    || shipItem.GetPrivateField<bool>("rotating");

                if (canNotShow)
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }
                var dial = ((ShipItemQuadrant)shipItem).GetPrivateField<Transform>("dial");
                var reading = dial.localEulerAngles.x;
                textMesh.text = $"{reading.ToString("F" + quadrantDecimalPlaces.Value)}°";
                textMesh.gameObject.SetActive(true);
            }

            else if (isSunCompass)
            {
                if (!enableSunCompassText.Value || shipItem.held == null || gameObject.layer == 5)
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                _sunlightTimer += Time.deltaTime;
                if (_sunlightTimer >= 5f)
                {
                    _sunlightTimer = 0f;
                    _isInSunlight = IsInSunlight();
                }

                if (!_isInSunlight)
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                var lat = FloatingOriginManager.instance.GetGlobeCoords(transform).z;
                textMesh.text = $"{lat.ToString("F" + sunCompassDecimalPlaces.Value)}°";
                textMesh.gameObject.SetActive(true);
            }

            else if (isBarometer)
            {
                if (!enableBarometerText.Value || canNotShowReading || distToItem > barometerViewDist.Value)
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                var normalizedReading = shipItem.GetPrivateField<float>("_pressure");

                var reading = Mathf.Lerp(26f, 31.9f, normalizedReading);
                if (barometerUnits.Value == "hPa" || barometerUnits.Value == "mbar")
                    reading *= 33.86389f;
                else if (barometerUnits.Value == "atm")
                    reading *= 0.03342f;

                textMesh.text = $"{reading.ToString("F" + barometerDecimalPlaces.Value)}{barometerUnits.Value}";
                textMesh.gameObject.SetActive(true);
            }

            else if (isThermometer)
            {
                if (!enableThermometerText.Value || canNotShowReading || distToItem > thermometerViewDist.Value)
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                var normalizedReading = shipItem.GetPrivateField<float>("_temperature");

                var reading = Mathf.Lerp(10f, 115f, normalizedReading);
                if (thermometerUnits.Value == "°C")
                    reading = (reading - 32) * 5 / 9;
                else if (thermometerUnits.Value == "K")
                    reading = (reading - 32) * 5 / 9 + 273.15f;

                textMesh.text = $"{reading.ToString("F" + thermometerDecimalPlaces.Value)}{thermometerUnits.Value}";
                textMesh.gameObject.SetActive(true);
            }

            else if (isHygrometer)
            {
                if (!enableHygrometerText.Value || canNotShowReading || distToItem > hygrometerViewDist.Value)
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                var reading = shipItem.GetPrivateField<float>("_humidity");
                textMesh.text = $"{(reading * 100).ToString("F" + hygrometerDecimalPlaces.Value)}%";
                textMesh.gameObject.SetActive(true);
            }

            else if (isInclinometer)
            {
                if (indicator1 == null)
                    return;

                if (!enableInclinometerText.Value || canNotShowReading || distToItem > inclinometerViewDist.Value)
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                var reading = indicator1.localEulerAngles.z;
                if (reading > 180f)
                    reading = negativeInclinometerAngles.Value ? reading - 360f : 360f - reading;
                textMesh.text = $"{reading.ToString("F" + inclinometerDecimalPlaces.Value)}°";
                textMesh.gameObject.SetActive(true);
            }

            else if (isWindCompass)
            {
                if (indicator1 == null)
                    return;

                var canNotShow =
                    (!enableWindCompassDegreesText.Value && !enableWindCompassCardinalText.Value)
                    || isHeld 
                    || notSoldInInvOrNotOnBoat
                    || distToItem > compassViewDist.Value
                    || SpyglassPatches.HeldAndUp;

                if (canNotShow)
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                var angleToPlayer = isHeld ? 0f : Vector3.SignedAngle(-transform.forward, observerPos - itemPos, Vector3.up);
                textMesh.transform.localEulerAngles = new Vector3(0, angleToPlayer, 0);
                textMesh.transform.localScale = isHeld ? HELD_SIZE : NOT_HELD_SIZE;

                var reading = (indicator1.transform.localEulerAngles.y + 180f + transform.eulerAngles.y) % 360f;
                textMesh.text = GetCompassReading(reading, windCompassCardinalPrecision.Value, windCompassDecimalPlaces.Value);
                textMesh.gameObject.SetActive(true);
            }

            else if (isAnemometer)
            {
                if (!enableAnemometerText.Value || indicator1 == null || indicator2 == null)
                    return;

                if (canNotShowReading || distToItem > anemometerViewDist.Value)
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                var reading = ((360f - indicator1.localEulerAngles.z) % 360f) * ANEMOMETER_ANGLE_TO_KNOTS;
                var reading2 = indicator2.localEulerAngles.z / 120f;
                textMesh.text = $"{(reading + reading2 * 50).ToString("F" + anemometerDecimalPlaces.Value)} kts";
                textMesh.gameObject.SetActive(true);
            }

            else if (isWeathervane)
            {
                if ((!enableWeathervaneDegreesText.Value && !enableWeathervanePointOfSailText.Value) || indicator1 == null)
                    return;

                var canNotShow =
                    notSoldInInvOrNotOnBoat
                    || distToItem > weathervaneViewDist.Value
                    || SpyglassPatches.HeldAndUp;

                if (canNotShow)
                {
                    textMesh.gameObject.SetActive(false);
                    return;
                }

                var tmParent = textMesh.transform.parent;
                var angleToPlayer = Vector3.SignedAngle(-tmParent.forward, observerPos - itemPos, Vector3.up);
                textMesh.transform.localEulerAngles = new Vector3(0, angleToPlayer, 0);

                var weatherVaneAngle = (indicator1.eulerAngles.y + 180f) % 360f;
                var boatHeading = shipItem.currentActualBoat.parent.eulerAngles.y;
                var reading = (weatherVaneAngle - boatHeading + 540f) % 360f - 180f;

                textMesh.text = GetWeatherVaneReading(reading);
                textMesh.gameObject.SetActive(true);
            }
        }

        private static string GetWeatherVaneReading(float reading)
        {
            var text = string.Empty;

            if (enableWeathervanePointOfSailText.Value)
            {
                if (reading > 0f)
                    text = "starboard ";
                else
                    text = "port ";

                var absReading = Mathf.Abs(reading);
                if (absReading <= 30f)
                    text = "in irons";
                else if (absReading <= 50f)
                    text += "close-hauled";
                else if (absReading <= 80f)
                    text += "close reach";
                else if (absReading <= 120f)
                    text += "beam reach";
                else if (absReading <= 160f)
                    text += "broad reach";
                else
                    text = "running downwind";
            }

            if (enableWeathervaneDegreesText.Value) 
                text += $"\n{reading.ToString("F" + weathervaneDecimalPlaces.Value)}°";

            return text;
        }

        private static string GetCompassReading(float reading, int cardinalPrecision, int decimalPlaces)
        {
            if (!enableCompassCardinalText.Value)
                return $"{reading.ToString("F" + decimalPlaces)}°";

            if (!enableCompassDegreesText.Value)
                return CompassRose.GetAbbreviatedDir(reading, cardinalPrecision);

            return $"{CompassRose.GetAbbreviatedDir(reading, cardinalPrecision)}\n{reading.ToString("F" + decimalPlaces)}°";
        }

        private static string GetClockReading()
        {
            var globalTime = Sun.sun.globalTime;
            var localTime = Sun.sun.localTime;

            if (!enableClockLocalText.Value)
                return GetTime(globalTime);
            if (!enableClockGlobalText.Value)
                return GetTime(localTime);

            return $"{GetTime(globalTime)}\n\n\n\n{GetTime(localTime)}";
        }

        private static string GetTime(float time)
        {
            var hours = (int)time;
            var minutes = Math.Round((time % 1) * 60) % 60;
            return $"{hours:00}:{minutes:00}";
        }

        private bool IsInSunlight()
        {
            var time = Sun.sun.localTime;
            if (shipItem.held == null || time < 11 || time > 13)
                return false;

            Vector3 directionToSun = -Sun.sun.transform.forward;
            int layerMask = Physics.DefaultRaycastLayers | (1 << 24);
            Vector3 rayOrigin = transform.position + directionToSun * 0.01f;

            bool blocked = Physics.Raycast(
                rayOrigin,
                directionToSun,
                Mathf.Infinity,
                layerMask,
                QueryTriggerInteraction.Collide
            );

            return !blocked;
        }
    }
}

//var textRenderer = textMesh.GetComponent<Renderer>();
//var textPosition = textRenderer != null ? textRenderer.bounds.center : text.position;
//var directionToText = textPosition - observerPosition;
//var distanceToText = directionToText.magnitude;

//(distanceToText > 0.1f &&
//Physics.Raycast(observerPosition, directionToText.normalized, out var hit, distanceToText, ~0, QueryTriggerInteraction.Collide) &&
//!hit.transform.IsChildOf(__instance.transform)))