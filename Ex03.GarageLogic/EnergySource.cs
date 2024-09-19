using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public abstract class EnergySource
    {
        public float MaxCapacity { get; protected set; }
        public float CurrentAmount { get; protected set; }

        public EnergySource(float i_maxCapacity) 
        { 
            MaxCapacity = i_maxCapacity;
        }


        // $G$ CSS-999 (-3) Missing blank line before return statement.
        public virtual Dictionary<string, Type> GetProperties()
        {
            Dictionary<string, Type> properties = new Dictionary<string, Type>();
            return properties;
        }

        public abstract void GetPropertiesAndValues(ref Dictionary<string, string> io_PropertiesList);

        public abstract void SetProperty(string i_Property, string i_Value);

        public void AddEnergy(float i_Amount)
        {
            if (CurrentAmount + i_Amount > MaxCapacity)
            {
                throw new ValueOutOfRangeException(0, MaxCapacity - CurrentAmount, "Energy amount exceeds max capacity");
            }

            CurrentAmount += i_Amount;
        }

        public override string ToString()
        {
            return string.Format("Current Amount: {0}{1}Max Capacity: {2}",
                CurrentAmount,
                Environment.NewLine,
                MaxCapacity);
        }
    }
}

