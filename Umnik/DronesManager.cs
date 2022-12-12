using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umnik
{
    static class DronesManager
    {
        private static List<Drone> _drones = new List<Drone>();
        public static List<Drone> Drones
        {
            get => _drones;
        }
        private static List<DroneColour> _availableColours = GetListOfAvailableColours();

        public delegate void OnRemoveDroneEventHandler(Drone drone);
        public static event OnRemoveDroneEventHandler? OnRemoveDroneEvent;

        public delegate void OnAddDroneEventHandler(Drone drone);
        public static event OnAddDroneEventHandler? OnAddDroneEvent;

        public static void RemoveDrone(Drone drone)
        {
            OnRemoveDroneEvent?.Invoke(drone);
            _drones.Remove(drone);
        }

        public static void AddDrone()
        {
            DroneColour colourForBitmap = _availableColours.FirstOrDefault();
            double addLat = 0.1;
            double addLong = 0.1;
            Drone drone = new Drone(colourForBitmap, new PointLatLng(43.9151144529437 + (int)colourForBitmap * addLat, 42.7288770675659 + (int)colourForBitmap * addLong));
            _availableColours.Remove(drone.DroneColor);
            OnAddDroneEvent?.Invoke(drone);
            _drones.Add(drone);
        }

        static List<DroneColour> GetListOfAvailableColours()
        {
            List <DroneColour> availableColours = new List <DroneColour>();
            for (DroneColour i = 0; i < DroneColour.MaxDroneColour; i++)
            {
                availableColours.Add(i);
            }
            return availableColours;
        }
    }
}
