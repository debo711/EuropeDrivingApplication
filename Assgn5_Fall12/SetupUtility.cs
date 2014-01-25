using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SharedClasses;

namespace SetupUtility
{
    class SetupUtility
    {
        static void Main(string[] args)
        {

            string filePrefix = ChooseFile();
            if (filePrefix == "exit")
                return;

            UserInterface UI = new UserInterface();

            StreamReader SR = File.OpenText(".\\..\\..\\..\\" + filePrefix + "RawData.txt");
            //FileStream TxtFile = new FileStream(".\\..\\..\\..\\" + filePrefix + "CityNames.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter TxtWriter = new StreamWriter(".\\..\\..\\..\\" + filePrefix + "CityNames.txt");
            FileStream BinaryFile = new FileStream(".\\..\\..\\..\\" + filePrefix + "MapData.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryWriter BinWriter = new BinaryWriter(BinaryFile);
            BinaryReader BinReader = new BinaryReader(BinaryFile);

            int[,] AdjMatrix;
            string[] ArrayOfNames;

            string lineread = SR.ReadLine();
            string[] linesplit = lineread.Split(' ');

            int N = int.Parse(linesplit[0]);
            BinWriter.Write(N);                   //write header
            string GraphDirection = linesplit[1];

            AdjMatrix = new int[N, N];
            ArrayOfNames = new string[N];

            for (int row = 0; row < AdjMatrix.GetLength(0); row++)
            {
                for (int col = 0; col < AdjMatrix.GetLength(1); col++)
                {
                    if (row == col)
                        AdjMatrix[row, col] = 0;              //put 0 in all diagonal
                    else
                        AdjMatrix[row, col] = int.MaxValue;   //put MaxValue in all other spots
                }
            }

            int locationLength = 0;
            for (int i = 0; i < N; i++)
            {
                ArrayOfNames[i] = SR.ReadLine();      //Read country data file
                if(ArrayOfNames[i].Length > locationLength)
                    locationLength = ArrayOfNames[i].Length;   //find country with biggest name
            }

            lineread = SR.ReadLine();
            while (lineread != null)
            {
                linesplit = lineread.Split(' ');

                int NodeA = int.Parse(linesplit[0]);
                int NodeB = int.Parse(linesplit[1]);
                int EdgeWeight = int.Parse(linesplit[2]);

                AdjMatrix[NodeA, NodeB] = EdgeWeight;
                AdjMatrix[NodeB, NodeA] = EdgeWeight;

                lineread = SR.ReadLine();
            }

            DumpMatrix(AdjMatrix, BinWriter);
            DumpCountryNames(locationLength, ArrayOfNames, TxtWriter);
            TxtWriter.Close();
            WriteHeader(filePrefix, N, UI);
            PrettyPrintMapData(UI, BinReader);
            StreamReader TxtReader = new StreamReader(".\\..\\..\\..\\" + filePrefix + "CityNames.txt"); //was forced to do it this way
            PrettyPrintCityNames(locationLength, UI, TxtReader);

            BinaryFile.Close();
            TxtReader.Close();
            SR.Close();
            UI.FinishWithUI();
        }

        static string ChooseFile()
        {
            string choice = "";

            while (choice != "1" && choice != "2")
            {
                Console.WriteLine();
                Console.WriteLine("Choose RawData input file:");
                Console.WriteLine("------------------------------");
                Console.WriteLine("1. EuropeRawData.txt");
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

        static void DumpMatrix(int[,] AdjacencyMatrix, BinaryWriter BW)
        {
            int sizeOfHeader = 4;
            BW.Seek(sizeOfHeader, SeekOrigin.Begin);
            for (int row = 0; row < AdjacencyMatrix.GetLength(0); row++)
            {
                for (int col = 0; col < AdjacencyMatrix.GetLength(1); col++)
                {
                    BW.Write(AdjacencyMatrix[row, col]);
                }
            }
        }

        static void DumpCountryNames(int locLength, string[] NamesArray, StreamWriter SW)
        {
            for (int i = 0; i < NamesArray.Length; i++)
            {
                if (NamesArray[i].Length < locLength)
                {
                    NamesArray[i] = NamesArray[i].PadRight(locLength);
                }
                SW.Write(NamesArray[i]);
            }
        }

        static void WriteHeader(string prefix, int Num, UserInterface UI)
        {
            UI.WriteThisLine("Map Data: " + prefix + "  Number of Cities: " + Num);
            UI.WriteThisLine("");
            UI.WriteThis("  ");

            for (int i = 0; i < Num; i++)
                UI.WriteThis(string.Format(" {0,11}", i));

            UI.WriteThisLine("");
            UI.WriteThis("  ");

            for (int i = 0; i < Num; i++)
                UI.WriteThis("------------");
            UI.WriteThisLine("");
        }

        static void PrettyPrintCityNames(int loclength, UserInterface UI, StreamReader SR)
        {
            char[] buffer = new char[loclength];
            while (!SR.EndOfStream)
            {
                SR.Read(buffer, 0, loclength);
                UI.WriteThisLine(string.Format(new string(buffer)));
            }
            UI.WriteThisLine("");
        }

        static void PrettyPrintMapData(UserInterface UI, BinaryReader BR)
        {
            BR.BaseStream.Seek(0, SeekOrigin.Begin);
            int N = BR.ReadInt32();
            for (int i = 0; i < N; i++)
            {
                UI.WriteThis(string.Format("{0,2}|",i));
                for (int j = 0; j < N; j++)
                {
                     UI.WriteThis(string.Format("{0,11} ", BR.ReadInt32()));
                }
                UI.WriteThisLine("");
            }
            UI.WriteThisLine("");
        }
    }
}
