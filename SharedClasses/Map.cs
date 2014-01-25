using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharedClasses
{
    public class Map
    {
        private static StreamReader SR;
        private static FileStream BFile;
        private static BinaryReader BR;

        public int N { get; set; }
        private string[] ArrayOfNames;
        private int sizeOfLocation = 10;
        private int sizeOfHeader = 4;
        private int sizeOfInt = 4;

        public Map(string filePrefix)
        {

            SR = new StreamReader(".\\..\\..\\..\\" + filePrefix + "CityNames.txt");
            BFile = new FileStream(".\\..\\..\\..\\" + filePrefix + "MapData.bin", FileMode.Open, FileAccess.Read);
            BR = new BinaryReader(BFile);

            N = BR.ReadInt32();
            ArrayOfNames = new string[N];
            LoadCountryNames();
        }

        public string GetCityName(int cityNumber)
        {
            for (int i = 0; i < ArrayOfNames.Length; i++)
            {
                if (i == cityNumber)
                    return ArrayOfNames[cityNumber];
            }
            return "ERROR";
        }

        public int GetCityNumber(string cityName)
        {
            int cityNum = -1;
            for (int i = 0; i < ArrayOfNames.Length; i++)
            {
                if(ArrayOfNames[i].Trim() == cityName)
                    cityNum = i;
            }
            return cityNum;
        }

        public int GetRoadDistance(int cityNumber1, int cityNumber2)
        {
            int byteOffset = sizeOfHeader + (cityNumber1 * (N * sizeOfInt)) + (cityNumber2 * sizeOfInt);
            BR.BaseStream.Seek(byteOffset, SeekOrigin.Begin);

            int weight = BR.ReadInt32();
            return weight;
        }

        private void LoadCountryNames()
        {
            char[] buffer = new char[sizeOfLocation];
            
            for(int i = 0; i < N ; i++)
            {
                SR.Read(buffer, 0, sizeOfLocation);
                ArrayOfNames[i] = new string(buffer);
            }
        }

        public void FinishWithMap()
        {
            SR.Close();
            BFile.Close();
        }
    }
}
