using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Ex03.GarageLogic;
using static Ex03.GarageLogic.Car;
using static Ex03.GarageLogic.FuelEnergySource;
using static Ex03.GarageLogic.Garage;
using static Ex03.GarageLogic.Motorcycle;

namespace Ex03.ConsoleUI
{
    public class UI
    {
        private Garage m_Garage;

        public UI(Garage i_Garage)
        {
            m_Garage = i_Garage;
        }

        // $G$ CSS-999 (-0) Bad method name --> methods should describe an action.
        public void MainMenu()
        {
            printMainMenu();
            while (true)
            {
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        addOrEditVehicle();
                        break;
                    case "2":
                        displayVehiclesMenu();
                        break;
                    case "3":
                        ChangeVehicleStatus();
                        break;
                    case "4":
                        inflateTiresToMax();
                        break;
                    case "5":
                        RefuelVehicle();
                        break;
                    case "6":
                        ChargeVehicle();
                        break;   
                    case "7":
                        DisplayVehicleDetails();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        continue;
                }

                printMainMenu();
            }
        }


        // $G$ NTT-999 (-10) You should have used Environment.NewLine instead of "\n".
        // $G$ NTT-001 (-10) You should have used verbatim string here.
        private void printMainMenu()
        {
            Console.WriteLine("\n=======================================");
            Console.WriteLine("      Garage Management System");
            Console.WriteLine("=======================================");
            Console.WriteLine("1. Add a new vehicle");
            Console.WriteLine("2. Display vehicles by filter");
            Console.WriteLine("3. Change vehicle status");
            Console.WriteLine("4. Inflate tires to max");
            Console.WriteLine("5. Refuel vehicle");
            Console.WriteLine("6. Charge vehicle battery");
            Console.WriteLine("7. Display vehicle details");
            Console.WriteLine("=======================================");
            Console.Write("Please enter your choice: ");
        }

        private void addOrEditVehicle()
        {
            Console.WriteLine("Enter the vehicle license number:");
            string userInput = Console.ReadLine();

            bool shouldAddVehicle = m_Garage.CheckWhetherToAddVehicle(userInput);
            if (!shouldAddVehicle)
            {
                Console.WriteLine($"Vehicle with the same license number already exists in the garage- vehicle status updated to \"{eVehicleStatus.InRepair}\".");
            }
            else
            {
                addNewVehicle(userInput);
            }
        }

        // $G$ NTT-001 (-0) You should have used verbatim string here.
        private void addNewVehicle(string i_NewVehicleLicenseNumber)
        {
            Console.WriteLine("=======================================");
            Console.WriteLine("           Add a New Vehicle");
            Console.WriteLine("=======================================");

            Vehicle newVehicle = getNewVehicle(i_NewVehicleLicenseNumber);
            VehiclePropertiesCollector properties = new VehiclePropertiesCollector(newVehicle);

            getPropertiesFromUser(properties, newVehicle);
            addVehicleToGarage(newVehicle);
        }

        private void addVehicleToGarage(Vehicle i_NewVehicle)
        {
            Console.WriteLine("Enter owner name: ");
            string ownerName = Console.ReadLine();
            Console.WriteLine("Enter owner phone number: ");
            do
            {
                string ownerPhone = Console.ReadLine();
                try
                {
                    m_Garage.AddVehicle(i_NewVehicle, ownerName, ownerPhone);
                    Console.WriteLine("Vehicle successfully added to garage!");
                    break;
                }
                catch (Exception exception) when (exception is ArgumentException || exception is FormatException)
                {
                    printExceptionErrorMessage(exception);
                }
            } while (true);
        }

        private void getPropertiesFromUser(VehiclePropertiesCollector properties, Vehicle newVehicle)
        {
            List<Dictionary<string, Type>> DictionariesOfProperties = new List<Dictionary<string, Type>>();

            DictionariesOfProperties.Add(properties.VehicleProperties);
            DictionariesOfProperties.Add(properties.EnergySourceProperties);
            foreach (var tire in properties.TiresProperties)
            {
                DictionariesOfProperties.Add(tire);
            }

            int currentTire = 0;

            // $G$ DSN-999 (-0) Why var??
            foreach (var dictionary in DictionariesOfProperties)
            {
                foreach (var property in dictionary)
                {
                    do
                    {
                        try
                        {
                            if (dictionary == properties.VehicleProperties)
                            {
                            handleProperty(property, newVehicle);
                            }
                            else if (dictionary == properties.EnergySourceProperties)
                            {
                                handleProperty(property, newVehicle.EnergySource);
                            }
                            else
                            {
                                Console.Write($"Tire #{currentTire + 1} - ");
                                handleProperty(property, newVehicle.Tires[currentTire]);
                            }
                            break;
                        }
                        catch (Exception exception)
                        {
                            printExceptionErrorMessage(exception);
                        }
                    } while (true);
                }
                if (properties.TiresProperties.Contains(dictionary))
                {
                    currentTire++;
                }
            }
        }

        private void handleProperty(KeyValuePair<string, Type> i_Property, object obj)
        {
            Console.WriteLine($"Enter {i_Property.Key}:");
            string input = Console.ReadLine();
            MethodInfo setPropertyMethod = obj.GetType().GetMethod("SetProperty", new[] { typeof(string), typeof(string) });
            
            try
            {
                setPropertyMethod.Invoke(obj, new object[] { i_Property.Key, input });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        private Vehicle getNewVehicle(string i_NewVehicleLicenseNumber)
        {
            Console.WriteLine("Choose vehicle type:");
            foreach (var type in Enum.GetValues(typeof(VehicleFactory.eVehicleType)))
            {
                Console.WriteLine($"{(int)type + 1}. {type}");
            }

            Vehicle newVehicle;
            do
            {
                try
                {
                    int vehicleType = int.Parse(Console.ReadLine());
                    newVehicle = VehicleFactory.CreateVehicle(vehicleType - 1, i_NewVehicleLicenseNumber);
                    break;
                }
                catch (Exception exception) when (exception is ArgumentOutOfRangeException ||  exception is FormatException)
                {
                    printExceptionErrorMessage(exception);
                }
       
            } while (true);

            return newVehicle;
        }

        private VehicleFactory.eVehicleType getChosenVehicleType()
        {
            int input = int.Parse(Console.ReadLine());
            VehicleFactory.eVehicleType vehicleType = (VehicleFactory.eVehicleType)(input - 1);

            if (!Enum.IsDefined(typeof(VehicleFactory.eVehicleType), vehicleType))
            {
                throw new ArgumentOutOfRangeException("Invalid vehicle type chosen.");
            }

            return vehicleType;
        }

        private void displayVehiclesMenu()
        {
            if (m_Garage.GetVehiclesAmount() == 0)
            {
                Console.WriteLine("There are no vehicles in the garage.");
            }
            else
            {
                StringBuilder displayMenu = new StringBuilder();
                List<string> statuses = m_Garage.GetAllVehicleStatuses();
                displayMenu.AppendLine("Which vehicles would you like to see?");
                displayMenu.AppendLine("\t1. All vehicles");
                for (int i = 0; i < statuses.Count; ++i)
                {
                    displayMenu.AppendLine($"\t{i + 2}. {statuses[i]}");
                }
                displayMenu.AppendLine("\t0. Back to main menu");

                Console.WriteLine(displayMenu.ToString());
                string input = Console.ReadLine();

                do
                {
                    if (input != "0")
                    {
                        if (int.TryParse(input, out int result))
                        {
                            if (result >= 1 && result <= statuses.Count + 1)
                            {
                                printVehiclesInGarage(result - 2);
                                break;
                            }
                        }
                        if (input == "0")
                        {
                            break;
                        }
                        Console.WriteLine("Invalid output. Try again.");
                    }
                } while (true); 
            }
            
        }

        private void printVehiclesInGarage(int i_Filter)
        {
            StringBuilder displayTitle = new StringBuilder();
            string filterTitle;
            List<string> licenseNumbers; 

            if (i_Filter == -1) {
                licenseNumbers = m_Garage.GetLicenseNumbers(null);
            }
            else
            {
                licenseNumbers = m_Garage.GetLicenseNumbers((eVehicleStatus)i_Filter);
            }

            if (licenseNumbers.Count == 0)
            {
                Console.WriteLine("No vehicles matching the filter exist in the garage.");
            }
            else
            {
                displayTitle.AppendLine("=======================================");
                if (i_Filter == -1)
                {
                    filterTitle = "         All Vehicles";
                }
                else
                {
                    filterTitle = $"         {Enum.GetName(typeof(eVehicleStatus), i_Filter)}";
                }
                displayTitle.AppendLine(filterTitle);
                displayTitle.AppendLine("=======================================");
                Console.WriteLine(displayTitle.ToString());

                foreach (string licenseNumber in licenseNumbers)
                {
                    Console.WriteLine(licenseNumber);
                }
            }
            
        }

        // $G$ NTT-001 (-0) You should have used verbatim string here.
        // $G$ NTT-999 (-0) You should have used Environment.NewLine instead of "\n".
        // $G$ DSN-999 (-0) This method should be private (none of the other classes uses it).
        public void ChangeVehicleStatus()
        {
            if (m_Garage.GetVehiclesAmount() == 0)
            {
                Console.WriteLine("There are no vehicles in the garage.");
            }
            else
            {
                Console.WriteLine("\n=======================================");
                Console.WriteLine("          Change Vehicle Status");
                Console.WriteLine("=======================================");

                string licenseNumber = getLegalLicenseNumber();

                Console.WriteLine("Choose new status:");
                foreach (var status in Enum.GetValues(typeof(eVehicleStatus)))
                {
                    Console.WriteLine($"{(int)status}. {status}");
                }

                const bool v_IgnoreCaseSensitivity = true;

                do
                {
                    if (Enum.TryParse(Console.ReadLine(), v_IgnoreCaseSensitivity, out eVehicleStatus newStatus)
                        && Enum.IsDefined(typeof(eVehicleStatus), newStatus))
                    {
                        try
                        {
                            m_Garage.ChangeVehicleStatus(licenseNumber, newStatus);
                            Console.WriteLine("Vehicle status changed successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to change vehicle status: {ex.Message}");
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid new status. Try again!");
                    }
                } while (true);

            }

        }


        // $G$ NTT-001 (-0) You should have used verbatim string here.
        // $G$ NTT-999 (-0) You should have used Environment.NewLine instead of "\n".
        private void inflateTiresToMax()
        {
            if (m_Garage.GetVehiclesAmount() == 0)
            {
                Console.WriteLine("There are no vehicles in the garage.");
            }
            else
            {
                Console.WriteLine("\n=======================================");
                Console.WriteLine("         Inflate Tires to Max");
                Console.WriteLine("=======================================");

                string licenseNumber = getLegalLicenseNumber();

                try
                {
                    m_Garage.InflateTiresToMax(licenseNumber);
                    Console.WriteLine("Tires inflated to max successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to inflate tires: {ex.Message}");
                }
            }

        }



        // $G$ NTT-001 (-0) You should have used verbatim string here.
        // $G$ NTT-999 (-0) You should have used Environment.NewLine instead of "\n".
        // $G$ DSN-999 (-3) This method should be private (none of the other classes uses it).
        // $G$ DSN-007 (-3) This method is too long, should be divided into several methods.
        public void RefuelVehicle()
        {
            if (m_Garage.GetVehiclesAmount() == 0)
            {
                Console.WriteLine("There are no vehicles in the garage.");
            }
            else
            {
                Console.WriteLine("\n=======================================");
                Console.WriteLine("           Refuel Vehicle");
                Console.WriteLine("=======================================");

                string licenseNumber = getLegalLicenseNumber();
                float fuelAmount;

                Console.Write("Enter amount of fuel to add: ");
                do
                {
                    if (!float.TryParse(Console.ReadLine(), out fuelAmount) || fuelAmount <= 0)
                    {
                        Console.WriteLine("Invalid amount of fuel. Try again.");
                        continue;
                    }
                    break;
                } while (true);

                // $G$ DSN-999 (-3) Why var??
                Console.WriteLine("Choose fuel type:");
                foreach (var type in Enum.GetValues(typeof(eFuelType)))
                {
                    Console.WriteLine($"{(int)type}. {type}");
                }

                const bool v_IgnoreCaseSensitivity = true;

                do
                {
                    if (Enum.TryParse(Console.ReadLine(), v_IgnoreCaseSensitivity, out eFuelType parsedFuelType)
                        && Enum.IsDefined(typeof(eFuelType), parsedFuelType))
                    {
                        try
                        {
                            m_Garage.Refuel(licenseNumber, fuelAmount, parsedFuelType);
                            Console.WriteLine("Vehicle refueled successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to refuel vehicle: {ex.Message}");
                        }

                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid fuel type. Try again!");
                    }
                } while (true);
            }
        }

        // $G$ DSN-999 (-0) This method should be private (none of the other classes uses it).
        public void ChargeVehicle()
        {
            if (m_Garage.GetVehiclesAmount() == 0)
            {
                Console.WriteLine("There are no vehicles in the garage.");
            }
            else
            {
                Console.WriteLine("\n=======================================");
                Console.WriteLine("           Charge Vehicle");
                Console.WriteLine("=======================================");

                string licenseNumber = getLegalLicenseNumber();
                float hoursAmount;

                Console.Write("Enter hours to charge: ");
                do
                {
                    if (!float.TryParse(Console.ReadLine(), out hoursAmount) || hoursAmount <= 0)
                    {
                        Console.WriteLine("Invalid number of hours. Try again.");
                        continue;
                    }

                    break;
                } while (true);

                try
                {
                    m_Garage.ChargeBattery(licenseNumber, hoursAmount);
                    Console.WriteLine("Vehicle charged successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to charge vehicle: {ex.Message}");
                }
            }
        }

        // $G$ DSN-999 (-0) This method should be private (none of the other classes uses it).
        public void DisplayVehicleDetails()
        {
            if (m_Garage.GetVehiclesAmount() == 0)
            {
                Console.WriteLine("There are no vehicles in the garage.");
            }
            else
            {
                string licenseNumber = getLegalLicenseNumber();

                Console.WriteLine("\n=======================================");
                Console.WriteLine("          Vehicle Details");
                Console.WriteLine("=======================================");

                try
                {
                    Dictionary<string,string> details = m_Garage.GetVehicleDetails(licenseNumber);
                    
                    foreach(var detail in details)
                    {
                        Console.WriteLine($"{detail.Key}: {detail.Value}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to get vehicle details: {ex.Message}");
                }
            }
        }


        // $G$ CSS-999 (-3) Missing blank line before return statement.
        private string getLegalLicenseNumber()
        {
            Console.Write("Enter license number: ");
            string licenseNumber;

            do
            {
                licenseNumber = Console.ReadLine();
                if (!m_Garage.FindVehicleInGarage(licenseNumber))
                {
                    Console.WriteLine("Vehicle with this license number doesn't exist in the garage. Try again.");
                    continue;
                }
                break;
            } while (true);
            return licenseNumber;
        }

        // $G$ CSS-999 (-3) Missing blank line after local variable.
        private void printExceptionErrorMessage(Exception i_Exception)
        {
            string errorMessage;
            if (i_Exception is ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                errorMessage = argumentOutOfRangeException.ParamName;
            }
            else
            {
                errorMessage = i_Exception.Message;
            }

            Console.Write($"{i_Exception.GetType()}: {errorMessage} ");
            Console.WriteLine("Try again.");
        }
    }
}
