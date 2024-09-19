using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class FuelEnergySource : EnergySource
    {

        // $G$ DSN-009 (-0) You should separate different classes/enums into different files.
        public enum eFuelType
        {
            Octan95,
            Octan96,
            Octan98,
            Soler
        }

        public eFuelType FuelType { get; }

        public FuelEnergySource(eFuelType i_FuelType, float i_FuelTankCapacity) : base(i_FuelTankCapacity)
        {
            FuelType = i_FuelType;
        }

        public override Dictionary<string, Type> GetProperties()
        {
            Dictionary<string, Type> properties = base.GetProperties();
            properties.Add("current fuel amount", typeof(float));
            return properties;
        }

        public override void GetPropertiesAndValues(ref Dictionary<string, string> io_PropertiesList)
        {
            io_PropertiesList.Add("energy source", "Fuel");
            io_PropertiesList.Add("fuel type", FuelType.ToString());
            io_PropertiesList.Add("max fuel amount", MaxCapacity.ToString());
            io_PropertiesList.Add("current fuel amount", CurrentAmount.ToString());
        }

        public override void SetProperty(string i_Property, string i_Value)
        {
            if (i_Property == "current fuel amount")
            {
                if (!float.TryParse(i_Value, out float value))
                {
                    throw new FormatException("Invalid value for current fuel amount.");
                }

                if (value < 0)
                {
                    throw new ValueOutOfRangeException(0, MaxCapacity, "Fuel amount can't be less than 0.");
                }
                if (value > MaxCapacity)
                {
                    throw new ValueOutOfRangeException(0, MaxCapacity, $"Fuel amount can't surpass max fuel amount ({MaxCapacity}).");
                }
                CurrentAmount = value;
            }
            else
            {
                throw new ArgumentException("Property not found in \"Fuel\" energy source.");
            }
        }

        public void Refuel(float i_Amount, eFuelType i_FuelType)
        {
            if (i_FuelType != FuelType)
            {
                throw new ArgumentException("Incorrect fuel type");
            }

            AddEnergy(i_Amount);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}Fuel Type: {2}",
                base.ToString(),
                Environment.NewLine,
                FuelType);
        }
    }
}

