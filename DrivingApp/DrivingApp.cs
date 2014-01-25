using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedClasses;

namespace DrivingApp
{
    class DrivingApp
    {
        static void Main(string[] args)
        {
            string fileNamePrefix = ChooseFile();
            if (fileNamePrefix == "exit")
                return;

            UserInterface UI = new UserInterface(fileNamePrefix);
            Map CityMap = new Map(fileNamePrefix);
            ShortestPath SP = new ShortestPath(CityMap);

            while (!UI.NoMorePairs())
            {
                string startCityName, endCityName;
                int startCityNum, endCityNum;
                int Distance;
                UI.GetCityPair(out startCityName, out endCityName);
                startCityNum = CityMap.GetCityNumber(startCityName);
                endCityNum = CityMap.GetCityNumber(endCityName);
                UI.WriteThisLine("#   #   #   #   #   #   #   #   #   #   #   #");
                UI.WriteThisLine(string.Format("{0} ({1}) TO {2} ({3})", startCityName,startCityNum,endCityName,endCityNum));
                if (startCityNum == -1 || endCityNum == -1)
                    UI.WriteThisLine("ERROR - one of the cities not on this map");
                else
                {
                    Distance = SP.FindShortestPath(startCityNum, endCityNum);
                    UI.WriteThisLine("DISTANCE: " + Distance);
                }
            }

            UI.FinishWithUI();
            CityMap.FinishWithMap();
        }

        static string ChooseFile()
        {
            string choice = "";

            while (choice != "1" && choice != "2")
            {
                Console.WriteLine();
                Console.WriteLine("Choose City Pairs input file:");
                Console.WriteLine("------------------------------");
                Console.WriteLine("1. EuropeCityPairs.txt");
                Console.WriteLine("2. Exit");
                Console.WriteLine("------------------------------");
                Console.Write("Enter choice: ");

                choice = Console.ReadLine();
            }

            switch (choice) // process selected menu item
            {
                case "2":
                    return "exit"; // exit program
                case "1":
                    return "Europe";
                default:
                    return "Europe";
            }
        }
    }
}
