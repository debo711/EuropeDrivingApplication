using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedClasses
{
    public class ShortestPath
    {
        private int N;
        private bool[] Included;
        private int[] Distance;
        private int[] Path;
        private Map MyMap;
        private int loopCounter = 0;

        public ShortestPath(Map cityMap)
        {
            MyMap = cityMap;
            N = cityMap.N;
        }

        public int FindShortestPath(int startCityNUM, int endCityNUM)
        {
            InitializeArrays(startCityNUM);
            Search(endCityNUM);
            return ReportAnswer(endCityNUM);
        }

        private void InitializeArrays(int startCity)
        {
            Included = new bool[N];
            Distance = new int[N];
            Path = new int[N];

            for (int i = 0; i < Included.Length; i++)
            {
                if (i == startCity)
                    Included[i] = true;
                else
                    Included[i] = false;

                Distance[i] = MyMap.GetRoadDistance(startCity, i);

                if (Distance[i] != 0 && Distance[i] != int.MaxValue)
                {
                    Path[i] = startCity;
                }
                else
                    Path[i] = -1;
            }
        }

        private void Search(int endCity)
        {
            while (Included[endCity] != true)
            {
                int target = endCity;
                for (int i = 0; i < Included.Length; i++)
                {
                    if (Included[i] != true)
                    {
                        if (Distance[i] < Distance[target])
                            target = i;
                    }
                }

                Included[target] = true;
                for (int i = 0; i < N; i++)
                {
                    if (Included[i] == false)
                    {
                        if (MyMap.GetRoadDistance(target, i) != 0 && MyMap.GetRoadDistance(target, i) != int.MaxValue)
                        {
                            if ((Distance[target] + MyMap.GetRoadDistance(target, i)) < Distance[i])
                            {
                                Distance[i] = (Distance[target] + MyMap.GetRoadDistance(target, i));
                                Path[i] = target;
                            }
                        }
                    }
                }
                loopCounter++;
            }
        }

        private int ReportAnswer(int desination)
        {
            return Distance[desination];
        }

        private void ReportTraceOfTargets()
        {

        }
    }
}
