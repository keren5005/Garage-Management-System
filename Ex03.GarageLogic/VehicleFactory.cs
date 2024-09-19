using System;

namespace Ex03.GarageLogic
{

    // $G$ DSN-009 (-2) You should separate different classes/enums into different files.
    public static class VehicleFactory
    {
        public enum eVehicleType
        {
            FuelMotorcycle,
            ElectricMotorcycle,
            FuelCar,
            ElectricCar,
            Truck
        }

        // $G$ CSS-999 (-3) Const fields should start with k_.
        // $G$ DSN-001 (-2) These members do not belong in this class.

        public const FuelEnergySource.eFuelType MotorcycleFuel = FuelEnergySource.eFuelType.Octan98;
        public const FuelEnergySource.eFuelType CarFuel = FuelEnergySource.eFuelType.Octan95;
        public const FuelEnergySource.eFuelType TruckFuel = FuelEnergySource.eFuelType.Soler;
        public const float MotorcycleFuelTankCapacity = (float)5.5;
        public const float CarFuelTankCapacity = 45;
        public const float TruckTankCapacity = 120;
        public const float MotorcycleBatteryCapacity = (float)2.5;
        public const float CarBatteryCapacity = (float)3.5;

        public static Vehicle CreateVehicle(int i_requestedVehicleType, string i_LicenseNumber)
        {
            if (!Enum.IsDefined(typeof(eVehicleType), i_requestedVehicleType))
            {
                throw new ArgumentOutOfRangeException("Invalid vehicle type chosen.");
            }

            eVehicleType newVehicleType = (eVehicleType)i_requestedVehicleType;
            switch (newVehicleType)
            {
                case eVehicleType.FuelMotorcycle:
                    return new Motorcycle(i_LicenseNumber, new FuelEnergySource(MotorcycleFuel, MotorcycleFuelTankCapacity));
                case eVehicleType.ElectricMotorcycle:
                    return new Motorcycle(i_LicenseNumber, new ElectricEnergySource(MotorcycleBatteryCapacity));
                case eVehicleType.FuelCar:
                    return new Car(i_LicenseNumber, new FuelEnergySource(CarFuel, CarFuelTankCapacity));
                case eVehicleType.ElectricCar:
                    return new Car(i_LicenseNumber, new ElectricEnergySource(CarBatteryCapacity));
                case eVehicleType.Truck:
                    return new Truck(i_LicenseNumber, new FuelEnergySource(TruckFuel, TruckTankCapacity));
                default:
                    throw new ArgumentException("Unknown vehicle type.");
            }
        }
    }
}
