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

        public List<Edge> Edges { get; private set; }

        public FitnessFunction(List<Edge> EdgesArg)
        {
            Edges = EdgesArg;
        }

        public double Evaluate(IChromosome chromosome)
        {
            var genes = chromosome.GetGenes();
            var distanceSum = 0.0;
            var lastEdgeIndex = Convert.ToInt32(genes[0].Value, CultureInfo.InvariantCulture);

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
        public  bool AllEdgesVisited(List<Edge> edges)
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