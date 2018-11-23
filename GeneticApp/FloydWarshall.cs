using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticApp
{
    class FloydWarshall
    {

        public const int cst = 999999;

        public static List<int>[,] CalculatePaths(int[,] graph)
        {
            var verticesCount=graph.GetLongLength(0);
            int[,] distance = graph;
            List<int>[,] shortestPaths = new List<int>[verticesCount,verticesCount];
            for(int i = 0; i < verticesCount; i++)
            {
                for(int j = 0; j < verticesCount; j++)
                {
                    shortestPaths[i, j] = new List<int>();
                }
            }
           

            for (int k = 0; k < verticesCount; ++k)
            {
                for (int i = 0; i < verticesCount; ++i)
                {
                    for (int j = 0; j < verticesCount; ++j)
                    {
                        if (distance[i, k] + distance[k, j] < distance[i, j])
                        {
                            distance[i, j] = distance[i, k] + distance[k, j];
                            shortestPaths[i, j].Clear();
                            if (shortestPaths[i, k].Count != 0)
                            {
                                shortestPaths[i, j].AddRange(shortestPaths[i,k]);
                            }
                           
                            shortestPaths[i, j].Add(k);
                            
                            if (shortestPaths[k, j].Count != 0)
                            {
                                shortestPaths[i, j].AddRange(shortestPaths[k, j]);
                            }
                            
                        }
                            
                    }
                }
            }
            return shortestPaths;
        }
    }
}
