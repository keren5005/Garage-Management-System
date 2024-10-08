## Overview
The **Garage Management System** is a console-based application written in C#. It allows users to manage vehicles in a garage, perform various maintenance tasks, and track the status of vehicles. This system supports multiple vehicle types such as cars, motorcycles, and trucks, each with their own properties. Users can refuel, charge electric vehicles, inflate tires, and change the status of vehicles.

## Features
- **Add New Vehicles**: Supports cars, motorcycles, and trucks with unique attributes.
- **Refuel and Charge Vehicles**: Refuel vehicles or charge electric vehicles based on their energy source.
- **Tire Maintenance**: Inflate vehicle tires to the maximum allowable pressure.
- **Change Vehicle Status**: Update the status of vehicles (InRepair, Repaired, Paid).
- **View Vehicle Details**: Retrieve full details of any vehicle, including owner details and status.
- **Filter Vehicles**: Display vehicles by status or show all vehicles in the garage.

## How to Run the Project

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (latest version recommended).
- IDE supporting C# such as Visual Studio, Rider, or Visual Studio Code.

### Running the Application
1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/garage-management-system.git
    ```

2. Open the project in your C#-compatible IDE.
3. Build the solution.
4. Run the application in the terminal or the IDE's console.

### Sample Workflow


## Vehicle Types Supported
- **Fuel Motorcycles**
- **Electric Motorcycles**
- **Fuel Cars**
- **Electric Cars**
- **Trucks**

Each vehicle type has specific properties, such as fuel type, battery capacity, tire count, and cargo volume for trucks.

## Main Features

### 1. Add a New Vehicle
Allows users to add cars, motorcycles, or trucks by entering details such as license number, model, energy source, and tire information.

### 2. Display Vehicles by Filter
Users can view all vehicles or filter them by their status (InRepair, Repaired, Paid).

### 3. Change Vehicle Status
Update a vehicle's status in the garage (InRepair, Repaired, Paid).

### 4. Inflate Tires to Maximum Pressure
Automatically inflate all tires of a vehicle to their maximum allowable pressure.

### 5. Refuel or Charge a Vehicle
Refuel fuel vehicles or charge electric vehicles by entering the desired amount of fuel or charge.

### 6. Display Vehicle Details
View full details of any vehicle, including the owner's name, phone number, and vehicle-specific properties like model, energy source, and tire information.

## Project Structure

- **Program.cs**: Entry point for the application. It initializes the garage and launches the main menu.
- **UI.cs**: Handles user interaction and calls garage services based on the user's choices.
- **Garage.cs**: Core class that manages the collection of vehicles, their statuses, and performs maintenance operations (like refueling, charging, inflating tires).
- **Vehicle.cs**: Base class representing a vehicle with properties like license number, model name, energy source, and tires.
- **Car.cs**, **Motorcycle.cs**, **Truck.cs**: Derived classes for specific vehicle types, with properties like cargo volume for trucks and engine displacement for motorcycles.
- **FuelEnergySource.cs** and **ElectricEnergySource.cs**: Handle energy management for vehicles, either fuel-based or electric.

## Exception Handling
- **ValueOutOfRangeException**: Handles invalid values like exceeding fuel or tire pressure limits.
- **ArgumentException**: Thrown when invalid arguments are passed during vehicle creation or modification.

