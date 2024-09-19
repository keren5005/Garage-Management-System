using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        public enum eVehicleStatus
        {
            InRepair,
            Repaired,
            Paid
        }

        private Dictionary<string, VehicleData> m_Vehicles;

        public Garage()
        {
            m_Vehicles = new Dictionary<string, VehicleData>();
        }

        public List<string> GetAllVehicleStatuses()
        {
            return new List<string>(Enum.GetNames(typeof(eVehicleStatus)));
        }
        
        public int GetVehiclesAmount() 
        { 
            return m_Vehicles.Count; 
        }

        public bool CheckWhetherToAddVehicle(string i_LicenseNumber)
        {
            bool foundVehicleInGarage = m_Vehicles.ContainsKey(i_LicenseNumber);
            bool shouldAddVehicle = true;

            if (foundVehicleInGarage)
            {
                m_Vehicles[i_LicenseNumber].Status = eVehicleStatus.InRepair;
                shouldAddVehicle = false;
            }
            return shouldAddVehicle;
        }
        public void AddVehicle(Vehicle i_Vehicle, string i_OwnerName, string i_OwnerPhone)
        {
            bool shouldAddVehicle = CheckWhetherToAddVehicle(i_Vehicle.LicenseNumber);

            if (!shouldAddVehicle)
            {
                throw new ArgumentException($"Vehicle with the same license number already exists in the garage- vehicle data updated, status = \\\"{{eVehicleStatus.InRepair}}\\\".\"");
            }

            if (int.TryParse(i_OwnerPhone, out int ownerPhone))
            {
                m_Vehicles[i_Vehicle.LicenseNumber] = new VehicleData(i_Vehicle, i_OwnerName, i_OwnerPhone, eVehicleStatus.InRepair);
            }
            else
            {
                throw new ArgumentException("Invalid phone number.");
            }
        }
        public bool FindVehicleInGarage(string i_LicenseNumber)
        {
            bool vehicleFound = false;

            foreach(var vehicle in m_Vehicles)
            {
                if (vehicle.Key == i_LicenseNumber)
                {
                    vehicleFound = true;
                    break;
                }
            }

            return vehicleFound;
        }
        public void InflateTiresToMax(string i_LicenseNumber)
        {
            if (!m_Vehicles.ContainsKey(i_LicenseNumber))
            {
                throw new ArgumentException("Vehicle with the given license number does not exist in the garage.");
            }

            m_Vehicles[i_LicenseNumber].Vehicle.InflateWheelsToMax();
        }

        public void Refuel(string i_LicenseNumber, float i_Amount, FuelEnergySource.eFuelType i_FuelType)
        {
            if (!m_Vehicles.ContainsKey(i_LicenseNumber))
            {
                throw new ArgumentException("Vehicle with the given license number does not exist in the garage.");
            }

            Vehicle vehicle = m_Vehicles[i_LicenseNumber].Vehicle;
            if (vehicle.EnergySource is FuelEnergySource fuelSource)
            {
                fuelSource.Refuel(i_Amount, i_FuelType);
            }
            else
            {
                throw new ArgumentException("Vehicle with the given license number is not a fuel vehicle.");
            }
        }

        public void ChargeBattery(string i_LicenseNumber, float i_HoursToAdd)
        {
            if (!m_Vehicles.ContainsKey(i_LicenseNumber))
            {
                throw new ArgumentException("Vehicle with the given license number does not exist in the garage.");
            }

            Vehicle vehicle = m_Vehicles[i_LicenseNumber].Vehicle;
            if (vehicle.EnergySource is ElectricEnergySource electricSource)
            {
                electricSource.ChargeBattery(i_HoursToAdd);
            }
            else
            {
                throw new ArgumentException("Vehicle with the given license number is not an electric vehicle.");
            }
        }

        public void ChangeVehicleStatus(string i_LicenseNumber, eVehicleStatus i_NewStatus)
        {
            if (!m_Vehicles.ContainsKey(i_LicenseNumber))
            {
                throw new ArgumentException("Vehicle with the given license number does not exist in the garage.");
            }

            m_Vehicles[i_LicenseNumber].Status = i_NewStatus;
        }

        public Dictionary<string,string> GetVehicleDetails(string i_LicenseNumber)
        {
            if (!m_Vehicles.ContainsKey(i_LicenseNumber))
            {
                throw new ArgumentException("Vehicle with the given license number does not exist in the garage.");
            }

            VehicleData vehicleData = m_Vehicles[i_LicenseNumber];
            return vehicleData.GetVehicleProperties();
        }

        /*
        The GetLicenseNumbers method in the Garage class is responsible for retrieving a list of license numbers for all vehicles in the garage. 
        It also has the optional functionality to filter the license numbers based on the vehicle's status.*/
        public List<string> GetLicenseNumbers(eVehicleStatus? i_FilterStatus = null)
        {
            List<string> licenseNumbers = new List<string>();
            foreach (var vehicleData in m_Vehicles.Values)
            {
                if (!i_FilterStatus.HasValue || vehicleData.Status == i_FilterStatus.Value)
                {
                    licenseNumbers.Add(vehicleData.Vehicle.LicenseNumber);
                }
            }

            return licenseNumbers;
        }

        private class VehicleData
        {
            public Vehicle Vehicle { get; set; }
            public string OwnerName { get; set; }
            public string OwnerPhone { get; set; }
            public eVehicleStatus Status { get; set; }
            public VehicleData(Vehicle i_Vehicle, string i_OwnerName, string i_OwnerPhone, eVehicleStatus i_Status)
            {
                Vehicle = i_Vehicle;
                OwnerName = i_OwnerName;
                OwnerPhone = i_OwnerPhone;
                Status = i_Status;
            }

            public Dictionary<string, string> GetVehicleProperties()
            {
                Dictionary<string, string> garageProperties = new Dictionary<string, string>();

                garageProperties.Add("owner name", OwnerName);
                garageProperties.Add("status in garage", Status.ToString());

                Dictionary<string, string> vehicleProperties = Vehicle.GetPropertiesAndValues();

                Dictionary<string, string> mergedDict = garageProperties.Concat(vehicleProperties).ToDictionary(x => x.Key, x => x.Value);
                return mergedDict;
            }
        }
    }
}

