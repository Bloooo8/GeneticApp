using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticApp
{
    public class FitnessFunction : IFitness
    {

        public int[,] WeightArray { get; private set; }

        public FitnessFunction(int[,] weightArray)
        {
            WeightArray = weightArray;
        }

        public double Evaluate(IChromosome chromosome)
        {
            var genes = chromosome.GetGenes();
            var distanceSum = 0.0;

            #region Implementacja z krawędziami(zła)
            /* var lastEdgeIndex = Convert.ToInt32(genes[0].Value, CultureInfo.InvariantCulture);

             int startingPoint = Edges[Convert.ToInt32(genes[0].Value, CultureInfo.InvariantCulture)].VertexA; // węzeł początkowy
             for (int i = 0; i < genes.Length; i++)
             {
                 int edgeIndex = Convert.ToInt32(genes[i].Value, CultureInfo.InvariantCulture);
                 Edge edge = Edges[edgeIndex];
                 Edge reverseEdge = Edges.Find(e => e.VertexA==edge.VertexB && e.VertexB==edge.VertexA); // znalezienie odwrotej krawędzi
                 Edges[edgeIndex].Visited = true; // krawędź została odwiedzona
                 reverseEdge.Visited = true; // odwrotność krawędzi została odwiedzona

                 if (i != 0 && Edges[edgeIndex].VertexA != Edges[Convert.ToInt32(genes[i - 1].Value, CultureInfo.InvariantCulture)].VertexB)
                 {
                     distanceSum += Edges[edgeIndex].Cost * 1000; //jeśli ścieżka nie jest poprawna koszt jest znacząco zwiększany
                     lastEdgeIndex = edgeIndex;
                 }
                 else
                 {
                     distanceSum += Edges[edgeIndex].Cost;
                     lastEdgeIndex = edgeIndex;
                 }
                 if (AllEdgesVisited(Edges)) // sprawdzenie czy ścieżka jest zamknięta i czy wszystkie krawędzie zostały odwiedzone
                 {
                     if (Edges[edgeIndex].VertexB == Edges[Convert.ToInt32(genes[0].Value, CultureInfo.InvariantCulture)].VertexA)
                     {
                         break;
                     }

                 }
             }
             if (!AllEdgesVisited(Edges))
             {
                 distanceSum *= 1000;
             }
             foreach (Edge e in Edges)
             {
                 e.Visited = false;
             }

             var fitness = 1.0 / distanceSum;


             return fitness;

         }
         */
            #endregion

            var lastVertexIndex = Convert.ToInt32(genes[0].Value, CultureInfo.InvariantCulture);
            List<Edge> edges = Program.edges;
            List<int>[,] shortestPaths=Program.shortestPaths;
            List<int> vertices = new List<int>();
            Edge edge;

           // int startingPoint = Convert.ToInt32(genes[0].Value, CultureInfo.InvariantCulture); // węzeł początkowy
            for (int i = 0; i < genes.Length-1; i++)
            {
                distanceSum += WeightArray[i, i + 1];
                edge=edges.Find(e => (e.VertexA == i && e.VertexB == i + 1));
                if (edge != null)
                    edge.Visited = true;
                else
                {
                    vertices = shortestPaths[i, i + 1];
                   
                    if (vertices.Count > 1)
                    {
                        for(int k = 1; k < vertices.Count - 1; k++)
                        {
                            edge = edges.Find(e => (e.VertexA == vertices[k] && e.VertexB == vertices[k-1]));
                            if (edge != null)
                                edge.Visited = true;

                        }
                    }
                    else
                    {
                        edge = edges.Find(e => (e.VertexA == i && e.VertexB == vertices[0]));
                        if (edge != null)
                            edge.Visited = true;
                    }
                    edge = edges.Find(e => (e.VertexA == vertices.Last() && e.VertexB == i+1));
                    if (edge != null)
                        edge.Visited = true;

                }
            }

            edge=edges.Find(e => (e.VertexA == genes.Length - 1 && e.VertexB == 0));
            if (edge != null)
                edge.Visited = true;
            distanceSum += WeightArray[genes.Length - 1, 0];
            if (!AllEdgesVisited(edges))
                distanceSum *= 1000;

                var fitness = 1.0 / distanceSum;


            return fitness;

        }

        public bool AllEdgesVisited(List<Edge> edges)
        {
            foreach (var e in edges)
            {
                if (e.Visited == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}