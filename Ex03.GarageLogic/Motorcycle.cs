using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex03.GarageLogic.Car;

namespace Ex03.GarageLogic
{
    public class Motorcycle : Vehicle
    {
        // $G$ DSN-009 (-0) You should separate different classes/enums into different files.
        public enum eLicenseType
        {
            A,
            A1,
            AA,
            B1
        }

        public const uint k_NumOfTires = 2;
        public const float k_MaxTiresAirPressure = 33;
        public eLicenseType LicenseType { get; set; }
        public int EngineDisplacement { get; set; }

        public Motorcycle(string i_LicenseNumber, EnergySource i_EnergySource) : base(i_LicenseNumber, i_EnergySource, k_NumOfTires, k_MaxTiresAirPressure) { }

        public override Dictionary<string, Type> GetProperties()
        {
            Dictionary<string, Type> properties = base.GetProperties();
            properties.Add("driving license type", LicenseType.GetType());
            properties.Add("engine displacement", EngineDisplacement.GetType());
            return properties;
        }

        public override Dictionary<string, string> GetPropertiesAndValues()
        {
            Dictionary<string, string> properties = base.GetPropertiesAndValues();

            properties.Add("driving license type", LicenseType.ToString());
            properties.Add("engine displacement", EngineDisplacement.ToString());
            EnergySource.GetPropertiesAndValues(ref properties);
            properties.Add("number of tires", k_NumOfTires.ToString());

            for (int i = 0; i < k_NumOfTires; i++)
            {
                Tires[i].GetPropertiesAndValues(ref properties, i + 1);
            }

            return properties;
        }

        public override void SetProperty(string i_Property, string i_Value)
        {
            const bool v_IgnoreCaseSensitivity = true;
            if (i_Property == "model name")
            {
                ModelName = i_Value;
            }
            else if (i_Property == "driving license type")
            {    
                if (Enum.TryParse(i_Value, v_IgnoreCaseSensitivity, out eLicenseType value)
                    && Enum.GetNames(typeof(eLicenseType)).Contains(i_Value))
                { 
                    LicenseType = value;
                }
                else
                {
                    throw new ArgumentException("Invalid value for driver license type.");
                }  
            }
            else if (i_Property == "engine displacement")
            {
                if (int.TryParse(i_Value, out int value))
                {
                    if (value <= 0)
                    {
                        throw new ValueOutOfRangeException(0, 0, "Engine displacement can't be negative.");
                    }
                    EngineDisplacement = value;
                }
                else
                {
                    throw new ArgumentException("Invalid value for engine displacement.");
                }
            }
            else
            {
                throw new ArgumentException("Property not found in vehicle \"Motorcycle\".");
            }
        }

        public override string ToString()
        {
            return string.Format("{0}License Type: {1}{2}Engine Capacity: {3} cc",
                base.ToString(),
                LicenseType,
                Environment.NewLine,
                EngineDisplacement);
        }
    }
}

