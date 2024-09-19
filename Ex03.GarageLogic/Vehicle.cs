using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        // $G$ DSN-999 (-3) This kind of field should be readonly.
        private string m_ModelName;
        private string m_LicenseNumber;
        private EnergySource m_EnergySource;
        private List<Tire> m_tires;

        public string ModelName 
        {
            get { return m_ModelName; }
            set { m_ModelName = value; }
        }
        public string LicenseNumber
        {
            get { return m_LicenseNumber; }
            set { m_LicenseNumber = value; }
        }
        public EnergySource EnergySource
        {
            get { return m_EnergySource; }
            set { m_EnergySource = value; }
        }
        public List<Tire> Tires
        {
            get { return m_tires; }
            set { m_tires = value; }
        }

        public Vehicle(string i_LicenseNumber, EnergySource i_EnergySource, uint i_NumOfTires, float i_MaxTiresAirPressure)
        {
            LicenseNumber = i_LicenseNumber;
            EnergySource = i_EnergySource;
            Tires = new List<Tire>();

            for (int i = 0; i < i_NumOfTires; i++)
            {
                Tires.Add(new Tire(i_MaxTiresAirPressure));
            }
        }

        public virtual Dictionary<string, Type> GetProperties()
        {
            Dictionary<string, Type> properties = new Dictionary<string, Type>();

            properties.Add("model name", typeof(string));
 
            return properties;
        }

        // $G$ CSS-999 (-3) Missing blank line before return statement.
        public virtual Dictionary<string, string> GetPropertiesAndValues()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("vehicle type", this.GetType().Name);
            properties.Add("license number", LicenseNumber);
            properties.Add("model name", ModelName);
            return properties;
        }

        public virtual void SetProperty(string i_Property, string i_Value)
        {
            if (i_Property == "model name")
            {
                ModelName = i_Value;
            }
            else
            {
                throw new ArgumentException("Property not found in vehicle.");
            }
        }
        public void InflateWheelsToMax()
        {
            foreach (Tire tire in Tires)
            {
                tire.InflateToMax();
            }
        }

        public override string ToString()
        {
            string tiresDetails = string.Join(Environment.NewLine, Tires);
            return string.Format(
                "Model Name: {0}{1}License Number: {2}{1}Energy Source: {3}{1}Tires:{1}{4}",
                ModelName,
                Environment.NewLine,
                LicenseNumber,
                EnergySource,
                tiresDetails);
        }
    }

    // $G$ DSN-009 (-0) You should separate different classes/enums into different files.
    public class Tire
    {
        // $G$ DSN-999 (-3) This kind of field should be readonly.
        public string ManufacturerName { get; set; }
        public float CurrentAirPressure { get; set; }
        // $G$ DSN-999 (-3) This kind of field should be readonly.
        public float MaxAirPressure { get; }

        public Tire(float i_MaxAitPressure)
        {
            MaxAirPressure = i_MaxAitPressure;
        }

        public Dictionary<string, Type> GetProperties()
        {
            Dictionary<string, Type> properties = new Dictionary<string, Type>();

            properties.Add("manufacturer name", typeof(string));
            properties.Add("current air pressure", CurrentAirPressure.GetType());

            return properties;
        }
        public void GetPropertiesAndValues(ref Dictionary<string, string> io_PropertiesList, int i_TireID)
        {
            io_PropertiesList.Add($"tire {i_TireID} manufacturer name", ManufacturerName);
            io_PropertiesList.Add($"tire {i_TireID} max air pressure", MaxAirPressure.ToString());
            io_PropertiesList.Add($"tire {i_TireID} current air pressure", CurrentAirPressure.ToString());
        }

        public virtual void SetProperty(string i_Property, string i_Value)
        {
            if (i_Property == "manufacturer name")
            {
                ManufacturerName = i_Value;
            }
            else if (i_Property == "current air pressure")
            {
                if (!(float.TryParse(i_Value, out float result)))
                {
                    throw new FormatException("Invalid value for current air pressure.");
                }

                if (result < 0 || result > MaxAirPressure)
                {
                    throw new ValueOutOfRangeException(0, MaxAirPressure, $"Value for current air pressure must be between {0} and {MaxAirPressure}.");
                }

                CurrentAirPressure = result;
            }
            else
            {
                throw new ArgumentException("Property not found in object \"tire\".");
            }
        }

        public void InflateToMax()
        {
            CurrentAirPressure = MaxAirPressure;
        }

        public void AddAir(float i_AirToAdd)
        {
            if (CurrentAirPressure + i_AirToAdd > MaxAirPressure)
            {
                throw new ValueOutOfRangeException(0, MaxAirPressure - CurrentAirPressure, "Air pressure exceeds max limit.");
            }
            CurrentAirPressure += i_AirToAdd;
        }

        public override string ToString()
        {
            return string.Format(
                "Manufacturer Name: {0}, Current Air Pressure: {1}, Max Air Pressure: {2}",
                ManufacturerName,
                CurrentAirPressure,
                MaxAirPressure);
        }
    }
}
