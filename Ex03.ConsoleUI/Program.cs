using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex03.GarageLogic;
// $G$ RUL-004 (-20) Wrong zip name format + folder name format

namespace Ex03.ConsoleUI
{
    // $G$ CSS-999 (-3) The class must have an access modifier.
    class Program
    {
        // $G$ CSS-999 (-3) The method must have an access modifier.
        static void Main(string[] args)
        {
            // $G$ DSN-999 (-2) Correct design: UI: Has 'Garage' as a member. Controls the flow of the program.
            Garage garage = new Garage();
            UI ui = new UI(garage);
            
            // $G$ CSS-999 (-3) Bad method name --> methods should describe an action.
            ui.MainMenu();
        }
    }
}
