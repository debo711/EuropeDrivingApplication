using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharedClasses
{
    public class UserInterface
    {
        StreamWriter LW;
        StreamReader SR;

        public UserInterface()
        {
            LW = new StreamWriter(".\\..\\..\\..\\LogSession.txt");
        }

        public UserInterface(string prefix)
        {
            SR = File.OpenText(".\\..\\..\\..\\" + prefix + "CityPairs.txt");
            LW = new StreamWriter(".\\..\\..\\..\\LogSession.txt", true);
        }

        public bool NoMorePairs()
        {
            bool EndOrNot = SR.EndOfStream;
            return EndOrNot;
        }

        public void GetCityPair(out string startCity, out string endCity)
        {
            string line = SR.ReadLine();
            string[] splitline = line.Split(' ');

            startCity = splitline[0];
            endCity = splitline[1];
        }

        public void WriteThis(string aline)
        {
            LW.Write(aline);
        }

        public void WriteThisLine(string line)
        {
            LW.WriteLine(line);
        }

        public void FinishWithUI()
        {
            LW.Close();
        }
    }
}
