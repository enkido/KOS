﻿using UnityEngine;
using kOS.Craft;

namespace kOS.Values
{
    public class GeoCoordinates : SpecialValue
    {
        public double Lat;
        public double Lng;
        public Vessel Vessel;
        public CelestialBody Body;

        public GeoCoordinates(Vessel vessel)
        {
            Lat = vessel.GetVesselLattitude();
            Lng = vessel.GetVesselLongitude();
            Vessel = vessel;

            Body = vessel.mainBody;
        }

        public GeoCoordinates(Vessel vessel, float lat, float lng)
        {
            Lat = lat;
            Lng = lng;
            Vessel = vessel;

            Body = vessel.mainBody;
        }

        public GeoCoordinates(Vessel vessel, double lat, double lng)
        {
            Lat = lat;
            Lng = lng;
            Vessel = vessel;

            Body = vessel.mainBody;
        }

        public float GetBearing(Vessel vessel)
        {
            return VesselUtils.AngleDelta(vessel.GetHeading(), GetHeadingFromVessel(vessel));
        }
    
        public float GetHeadingFromVessel(Vessel vessel)
        {
            var up = vessel.upAxis;
            var north = vessel.GetNorthVector();

            var targetWorldCoords = vessel.mainBody.GetWorldSurfacePosition(Lat, Lng, vessel.altitude);
            
            var vector = Vector3d.Exclude(vessel.upAxis, targetWorldCoords - vessel.GetWorldPos3D()).normalized;
            var headingQ = Quaternion.Inverse(Quaternion.Euler(90, 0, 0) * Quaternion.Inverse(Quaternion.LookRotation(vector, up)) * Quaternion.LookRotation(north, up));

            return headingQ.eulerAngles.y;
        }

        public double DistanceFrom(Vessel vessel)
        {
            return Vector3d.Distance(vessel.GetWorldPos3D(), Body.GetWorldSurfacePosition(Lat, Lng, vessel.altitude));
        }

        public override object GetSuffix(string suffixName)
        {
            switch (suffixName)
            {
                case "LAT":
                    return Lat;
                case "LNG":
                    return Lng;
                case "DISTANCE":
                    return DistanceFrom(Vessel);
                case "HEADING":
                    return (double)GetHeadingFromVessel(Vessel);
                case "BEARING":
                    return (double)GetBearing(Vessel);
                default:
                    return null;
            }
        }

        public override string ToString()
        {
            return "LATLNG(" + Lat + ", " + Lng + ")";
        }
    }
}
