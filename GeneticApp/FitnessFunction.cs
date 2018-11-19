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

        public IList<Edge> Edges { get; private set; }

        public FitnessFunction(List<Edge> EdgesArg)
        {
            Edges = EdgesArg;
        }

        public double Evaluate(IChromosome chromosome)
        {
            var genes = chromosome.GetGenes();
            var distanceSum = 0.0;
            var lastEdgeIndex = Convert.ToInt32(genes[0].Value, CultureInfo.InvariantCulture);

            var edgesIndexes = new List<int>();
            edgesIndexes.Add(lastEdgeIndex);

            // Calculates the total route distance.
            foreach (var g in genes)
            {
                var currentEdgeIndex = Convert.ToInt32(g.Value, CultureInfo.InvariantCulture);
                distanceSum += Edges[currentEdgeIndex].Cost;
                lastEdgeIndex = currentEdgeIndex;

                edgesIndexes.Add(lastEdgeIndex);
            }

            var fitness = 1.0 - (distanceSum / (Edges.Count * 1000.0));

            ((Chromosome)chromosome).Distance = distanceSum;

            // There is repeated cities on the indexes?
            var diff = Edges.Count - edgesIndexes.Distinct().Count();

            if (diff > 0)
            {
                fitness /= diff;
            }

            if (fitness < 0)
            {
                fitness = 0;
            }

            return fitness;
        }
    }
}
