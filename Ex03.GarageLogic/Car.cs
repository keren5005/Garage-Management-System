using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex03.GarageLogic.Motorcycle;

namespace Ex03.GarageLogic
{

    // $G$ DSN-009 (-0) You should separate different classes/enums into different files.
    public class Car : Vehicle
    {
        public enum eColor
        {
            Red,
            White,
            Black,
            Yellow
        }

        public enum eNumOfDoors
        {
            Two = 2,
            Three,
            Four,
            Five
        }

        public const uint NumOfTires = 5;
        public const float MaxTiresAirPressure = 31;

        private eColor m_Color;
        private eNumOfDoors m_NumOfDoors;

        public Car(string i_LicenseNumber, EnergySource i_EnergySource) : base(i_LicenseNumber, i_EnergySource, NumOfTires, MaxTiresAirPressure) { }

        public override Dictionary<string, Type> GetProperties()
        {
            Dictionary<string, Type> properties = base.GetProperties();
            properties.Add("car color", m_Color.GetType());
            properties.Add("number of doors", m_NumOfDoors.GetType());
            return properties;
        }

        public override Dictionary<string, string> GetPropertiesAndValues()
        {
            Dictionary<string, string> properties = base.GetPropertiesAndValues();

            properties.Add("car color", m_Color.ToString());
            properties.Add("number of doors", m_NumOfDoors.ToString());
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
            const bool v_IgnoreCaseSensitivity = true;
            if (i_Property == "model name")
            {
                ModelName = i_Value;
            }
            else if (i_Property == "car color")
            {
                if (Enum.TryParse(i_Value, v_IgnoreCaseSensitivity, out eColor value) 
                    && Enum.GetNames(typeof(eColor)).Contains(i_Value))
                {
                    m_Color = value;
                }
                else
                {
                    StringBuilder colorsError = new StringBuilder();
                    colorsError.Append("Invalid value for car color. Available colors are:");
                    foreach (eColor color in Enum.GetValues(typeof(eColor)))
                    {
                        colorsError.Append($" {color.ToString()}");
                    }
                    colorsError.Append(".");
                    throw new ArgumentException(colorsError.ToString());
                }
            }
            else if (i_Property == "number of doors")
            {
                int minValue = Enum.GetValues(typeof(eNumOfDoors)).Cast<int>().Min();
                int maxValue = Enum.GetValues(typeof(eNumOfDoors)).Cast<int>().Max();

                if (Enum.TryParse(i_Value, v_IgnoreCaseSensitivity, out eNumOfDoors value))
                {
                    if ((int)value < minValue || (int)value > maxValue)
                    {
                        throw new ValueOutOfRangeException(minValue, maxValue, $"Value for number of doors must be between {minValue} and {maxValue}.");
                    }

                    m_NumOfDoors = value;
                }
                else
                {
                    throw new ArgumentException("Invalid value for number of doors.");
                }
            }
            else
            {
                throw new ArgumentException("Property not found in vehicle \"Car\".");
            }
        }
        public override string ToString()
        {
            return string.Format("{0}Color: {1}{2}Number of Doors: {3}{2}",
                base.ToString(),
                m_Color,
                Environment.NewLine,
                m_NumOfDoors);
        }
    }
}

