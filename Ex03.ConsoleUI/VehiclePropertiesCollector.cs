using System;
using System.Collections.Generic;
using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    public class VehiclePropertiesCollector
    {
        private Dictionary<string, Type> m_VehicleProperties;
        private Dictionary<string, Type> m_EnergySourceProperties;
        private List<Dictionary<string, Type>> m_TiresProperties;

        public Dictionary<string, Type> VehicleProperties
        {
            get { return m_VehicleProperties; }
            set { m_VehicleProperties = value; }
        }

        public Dictionary<string, Type> EnergySourceProperties
        {
            get { return m_EnergySourceProperties; }
            set { m_EnergySourceProperties = value; }
        }

        public List<Dictionary<string, Type>> TiresProperties
        {
            get { return m_TiresProperties; }
            set { m_TiresProperties = value; }
        }
 
        public VehiclePropertiesCollector(Vehicle i_Vehicle)
        {
            TiresProperties = new List<Dictionary<string, Type>>();

            VehicleProperties = i_Vehicle.GetProperties();
            EnergySourceProperties = i_Vehicle.EnergySource.GetProperties();

            foreach (var tire in i_Vehicle.Tires)
            {
                TiresProperties.Add(tire.GetProperties());
            }
        }
    }
}
