using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class ElectricEnergySource : EnergySource
    {
        public ElectricEnergySource(float i_MaxTimeBeforeCharge) : base(i_MaxTimeBeforeCharge) { }

        public override Dictionary<string, Type> GetProperties()
        {
            Dictionary<string, Type> properties = base.GetProperties();
            properties.Add("current time until battery runs out", CurrentAmount.GetType());
            return properties;
        }

        public override void GetPropertiesAndValues(ref Dictionary<string, string> io_PropertiesList)
        {
            io_PropertiesList.Add("energy source", "Electric");
            io_PropertiesList.Add("max time until battery runs out", MaxCapacity.ToString());
            io_PropertiesList.Add("current time until battery runs out", CurrentAmount.ToString());
        }

        public override void SetProperty(string i_Property, string i_Value)
        {
            if (i_Property == "current time until battery runs out")
            {
                if (!float.TryParse(i_Value, out float value))
                {
                    throw new FormatException("Invalid value for current time until battery runs out.");
                }

                if (value < 0)
                {
                    throw new ValueOutOfRangeException(0, MaxCapacity, "Time can't be less than 0.");
                }
                if (value > MaxCapacity)
                {
                    throw new ValueOutOfRangeException(0, MaxCapacity, $"Current time until empty can't surpass max time set for battery ({MaxCapacity}).");
                }
                CurrentAmount = value;
            }
            else
            {
                throw new ArgumentException("Property not found in \"Electric\" energy source.");
            }
        }

        public void ChargeBattery(float i_HoursToAdd)
        {
            AddEnergy(i_HoursToAdd);
        }

        public override string ToString()
        {
            return string.Format("{0}", base.ToString());
        }
    }
}
