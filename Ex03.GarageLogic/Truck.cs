using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex03.GarageLogic.Motorcycle;

namespace Ex03.GarageLogic
{
    public class Truck : Vehicle
    {
        public const uint NumOfTires = 12;
        public const float MaxTiresAirPressure = 28;
        public bool IsCarryingHazardousMaterials { get; set; }
        public float CargoVolume { get; set; }
        public FuelEnergySource FuelSource { get; set; }

        public Truck(string i_LicenseNumber, FuelEnergySource i_EnergySource) : base(i_LicenseNumber, i_EnergySource, NumOfTires, MaxTiresAirPressure) { }

        public override Dictionary<string, Type> GetProperties()
        {
            Dictionary<string, Type> properties = base.GetProperties();
            properties.Add("is carrying hazardous materials", IsCarryingHazardousMaterials.GetType());
            properties.Add("cargo volume", CargoVolume.GetType());
            return properties;
        }

        public override Dictionary<string, string> GetPropertiesAndValues()
        {
            Dictionary<string, string> properties = base.GetPropertiesAndValues();

            properties.Add("is carrying hazardous materials", IsCarryingHazardousMaterials.ToString());
            properties.Add("cargo volume", CargoVolume.ToString());
            EnergySource.GetPropertiesAndValues(ref properties);
            properties.Add("number of tires", NumOfTires.ToString());

            for (int i = 0; i < NumOfTires; i++)
            {
                Tires[i].GetPropertiesAndValues(ref properties, i + 1);
            }

            return properties;
        }
        public override void SetProperty(string i_Property, string i_Value)
        {
            if (i_Property == "model name")
            {
                ModelName = i_Value;
            }
            else if (i_Property == "is carrying hazardous materials")
            {
                if (bool.TryParse(i_Value, out bool result))
                {
                    IsCarryingHazardousMaterials = result;
                }
                else
                {
                    throw new FormatException("Input is not a valid boolean value.");
                }
            }
            else if (i_Property == "cargo volume")
            {
                if (!float.TryParse(i_Value, out float value))
                {
                    throw new FormatException("Invalid value for cargo volume.");
                }

                if (value < 0)
                {
                    throw new ValueOutOfRangeException(0, 0, "Cargo volume can't be less than 0.");
                }
                    
                CargoVolume = value;
            }
            else
            {
                throw new ArgumentException("Property not found in vehicle \"Truck\".");
            }
        }

        public void Refuel(float i_AmountToAdd, FuelEnergySource.eFuelType i_FuelType)
        {
            FuelSource.Refuel(i_AmountToAdd, i_FuelType);
        }

        public override string ToString()
        {
            return string.Format("{0}Carrying Hazardous Materials: {1}{2}Cargo Volume: {3} cubic meters{2}{4}",
                base.ToString(),
                IsCarryingHazardousMaterials,
                Environment.NewLine,
                CargoVolume,
                FuelSource);
        }
    }
}

