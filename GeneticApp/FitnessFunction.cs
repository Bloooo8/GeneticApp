using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using System.Collections.Generic;
using System.Linq;

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
            Gene[] genes = chromosome.GetGenes();
            double distanceSum = 0.0;
            int lastEdgeIndex = int.Parse(genes[0].Value.ToString());

            // Węzeł początkowy
            int startingPoint = Edges[int.Parse(genes[0].Value.ToString())].VertexA;
            for (int i = 0; i < genes.Length; i++)
            {
                int edgeIndex = int.Parse(genes[i].Value.ToString());
                Edge edge = Edges[edgeIndex];

                // Znalezienie odwrotej krawędzi
                Edge reverseEdge = Edges.SingleOrDefault(e => e.VertexA == edge.VertexB && e.VertexB == edge.VertexA);

                // Oznaczenie krawędzi i jej odwrotności jako oznaczona
                edge.Visited = true;
                reverseEdge.Visited = true;

                if (i != 0 && edge.VertexA != Edges[int.Parse(genes[i - 1].Value.ToString())].VertexB)
                {
                    // Jeśli ścieżka nie jest poprawna koszt jest znacząco zwiększany
                    distanceSum += edge.Cost * 1000;
                    lastEdgeIndex = edgeIndex;
                }
                else
                {
                    distanceSum += edge.Cost;
                    lastEdgeIndex = edgeIndex;
                }

                // Sprawdzenie czy ścieżka jest zamknięta i czy wszystkie krawędzie zostały odwiedzone
                if (AllEdgesVisited(Edges))
                {
                    if (edge.VertexB == startingPoint)
                    {
                        break;
                    }

                    Edge possibleEdge = Edges.SingleOrDefault(e => e.VertexA == edge.VertexB && e.VertexB == startingPoint);
                    if (possibleEdge != null)
                    {
                        distanceSum += possibleEdge.Cost;
                        break;
                    }
                }
            }

            if (!AllEdgesVisited(Edges))
            {
                distanceSum *= 1000;
            }

            Edges.ForEach(e => e.Visited = false);

            return 1.0 / distanceSum;
        }

        public static bool AllEdgesVisited(List<Edge> edges)
        {
            return edges.All(e => e.Visited);
        }
    }
}