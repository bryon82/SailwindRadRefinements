using System;
using UnityEngine;
using static RadRefinements.Configs;
using static RadRefinements.RR_Plugin;

namespace RadRefinements
{
    internal class ReadingText : MonoBehaviour
    {
        internal TextMesh readingTextMesh;
        internal ShipItem shipItem;
        internal Transform pointer;
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
        //internal bool isAnemometer;

        internal bool isCompassFlipped = false;

        const float ANGLE_TO_KNOTS = 1f / 15f;
        private static readonly Vector3 NOT_HELD_SIZE = new Vector3(0.0114f, 0.0127f, 0.0127f);
        private static readonly Vector3 HELD_SIZE = new Vector3(0.0044f, 0.0057f, 0.0057f);

        private void LateUpdate()
        {
            if (!GameState.playing || GameState.currentlyLoading || GameState.loadingBoatLocalItems)
                return;

            if (readingTextMesh == null)
                return;

            if (BoatCamera.on)
            {
                readingTextMesh.gameObject.SetActive(false);
                return;
            }

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
                if (pointer == null)
                    return;

                var canNotShow =
                    (!enableCompassDegreesText.Value && !enableCompassCardinalText.Value)
                    || (isHeld && !enableCompassReadingHeld.Value)
                    || notSoldInInvOrNotOnBoat
                    || distToItem > compassViewDist.Value
                    || SpyglassPatches.HeldAndUp;

                if (canNotShow)
                {
                    readingTextMesh.gameObject.SetActive(false);
                    return;
                }

                var angleToPlayer = isHeld ? 0f : Vector3.SignedAngle(-transform.forward, observerPos - itemPos, Vector3.up);
                readingTextMesh.transform.localEulerAngles = new Vector3(0, angleToPlayer, 0);
                readingTextMesh.transform.localScale = isHeld ? HELD_SIZE : NOT_HELD_SIZE;

                var offset = isCompassFlipped ? 180f : 360f;
                var reading = (180f - pointer.transform.localEulerAngles.y + offset) % 360f;
                readingTextMesh.text = GetCompassReading(reading, compassCardinalPrecision.Value);
                readingTextMesh.gameObject.SetActive(true);
            }

            else if (isChipLog)
            {
                if ( pointer == null)
                    return;

                var canNotShow =
                    !enableChipLogText.Value
                    || canNotShowReading
                    || distToItem > chipLogViewDist.Value
                    || !shipItem.GetPrivateField<bool>("thrown");

                if (canNotShow)
                {
                    readingTextMesh.gameObject.SetActive(false);
                    return;
                }

                readingTextMesh.text = $"{(360f - pointer.localEulerAngles.z) * ANGLE_TO_KNOTS:F1} kts";
                readingTextMesh.gameObject.SetActive(true);
            }

            else if (isClock)
            {
                var canNotShow =
                    (!enableClockGlobalText.Value && !enableClockLocalText.Value)
                    || canNotShowReading
                    || distToItem > clockViewDist.Value;

                if (canNotShow )
                {
                    readingTextMesh.gameObject.SetActive(false);
                    return;
                }

                readingTextMesh.text = GetClockReading();
                readingTextMesh.gameObject.SetActive(true);
            }

            else if (isQuadrant)
            {
                var canNotShow =
                    !enableQuadrantText.Value ||
                    !shipItem.GetPrivateField<bool>("inspecting")
                    || shipItem.GetPrivateField<bool>("rotating");

                if (canNotShow)
                {
                    readingTextMesh.gameObject.SetActive(false);
                    return;
                }
                var dial = ((ShipItemQuadrant)shipItem).GetPrivateField<Transform>("dial");
                var reading = Math.Round(dial.localEulerAngles.x, 2);
                readingTextMesh.text = $"{reading}°";
                readingTextMesh.gameObject.SetActive(true);
            }

            else if (isSunCompass)
            {
                if (!enableSunCompassText.Value || shipItem.held == null || gameObject.layer == 5)
                {
                    readingTextMesh.gameObject.SetActive(false);
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
                    readingTextMesh.gameObject.SetActive(false);
                    return;
                }

                var lat = FloatingOriginManager.instance.GetGlobeCoords(transform).z;
                readingTextMesh.text = $"{lat:F1}°";
                readingTextMesh.gameObject.SetActive(true);
            }

            else if (isBarometer)
            {
                if (!enableBarometerText.Value || canNotShowReading || distToItem > barometerViewDist.Value)
                {
                    readingTextMesh.gameObject.SetActive(false);
                    return;
                }

                var reading = shipItem.GetPrivateField<float>("_pressure");
                readingTextMesh.text = $"{Mathf.Lerp(26f, 31.9f, reading):F1}inHg";
                readingTextMesh.gameObject.SetActive(true);
            }

            else if (isThermometer)
            {
                if (!enableThermometerText.Value || canNotShowReading || distToItem > thermometerViewDist.Value)
                {
                    readingTextMesh.gameObject.SetActive(false);
                    return;
                }

                var reading = shipItem.GetPrivateField<float>("_temperature");
                readingTextMesh.text = $"{Mathf.Lerp(10f, 115f, reading):F1}°F";
                readingTextMesh.gameObject.SetActive(true);
            }

            else if (isHygrometer)
            {
                if (!enableHygrometerText.Value || canNotShowReading || distToItem > hygrometerViewDist.Value)
                {
                    readingTextMesh.gameObject.SetActive(false);
                    return;
                }

                var reading = shipItem.GetPrivateField<float>("_humidity");
                readingTextMesh.text = $"{reading * 100:F1}%";
                readingTextMesh.gameObject.SetActive(true);
            }

            else if (isInclinometer)
            {
                if (pointer == null)
                    return;

                if (!enableInclinometerText.Value || canNotShowReading || distToItem > inclinometerViewDist.Value)
                {
                    readingTextMesh.gameObject.SetActive(false);
                    return;
                }

                var reading = pointer.localEulerAngles.z;
                if (reading > 180f)
                    reading = negativeInclinometerAngles.Value ? reading - 360f : 360f - reading;
                readingTextMesh.text = $"{reading:F1}°";
                readingTextMesh.gameObject.SetActive(true);
            }

            else if (isWindCompass)
            {
                if (pointer == null)
                    return;

                var canNotShow =
                    (!enableWindCompassDegreesText.Value && !enableWindCompassCardinalText.Value)
                    || isHeld 
                    || notSoldInInvOrNotOnBoat
                    || distToItem > compassViewDist.Value
                    || SpyglassPatches.HeldAndUp;

                if (canNotShow)
                {
                    readingTextMesh.gameObject.SetActive(false);
                    return;
                }

                var angleToPlayer = isHeld ? 0f : Vector3.SignedAngle(-transform.forward, observerPos - itemPos, Vector3.up);
                readingTextMesh.transform.localEulerAngles = new Vector3(0, angleToPlayer, 0);
                readingTextMesh.transform.localScale = isHeld ? HELD_SIZE : NOT_HELD_SIZE;

                var reading = (pointer.transform.localEulerAngles.y + 180f + transform.eulerAngles.y) % 360f;
                readingTextMesh.text = GetCompassReading(reading, windCompassCardinalPrecision.Value);
                readingTextMesh.gameObject.SetActive(true);
            }

            //else if (isAnemometer)
            //{
            //    if (!enableChipLogText.Value || pointer == null)
            //        return;

            //    if (canNotShowReading || distToItem > chipLogViewDist.Value)
            //    {
            //        readingTextMesh.gameObject.SetActive(false);
            //        return;
            //    }

            //    readingTextMesh.text = $"{(360f - pointer.localEulerAngles.z) * ANGLE_TO_KNOTS:F1} kts";
            //    readingTextMesh.gameObject.SetActive(true);
            //}
        }

        private static string GetCompassReading(float reading, int cardinalPrecision)
        {
            if (!enableCompassCardinalText.Value)
                return $"{reading:F1}°";

            if (!enableCompassDegreesText.Value)
                return CompassRose.GetAbbreviatedDir(reading, cardinalPrecision);

            return $"{CompassRose.GetAbbreviatedDir(reading, cardinalPrecision)}\n{reading:F1}°";
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