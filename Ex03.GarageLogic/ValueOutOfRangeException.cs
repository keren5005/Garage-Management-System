using System;

namespace Ex03.GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        public ValueOutOfRangeException(float i_MinValue, float i_MaxValue, string i_Message)
            : base(i_Message)
        {
            MinValue = i_MinValue;
            MaxValue = i_MaxValue;
        }
    }
}
