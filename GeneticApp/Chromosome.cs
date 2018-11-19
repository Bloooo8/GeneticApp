using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticApp
{
    public class Chromosome : ChromosomeBase
    {
        private readonly int edgesNumber;

        public Chromosome(int edgesQuantity) : base(edgesQuantity)
        {
            edgesNumber = edgesQuantity;
            var edgesIndexes = RandomizationProvider.Current.GetUniqueInts(edgesQuantity, 0, edgesQuantity);

            for (int i = 0; i < edgesQuantity; i++)
            {
                ReplaceGene(i, new Gene(edgesIndexes[i]));
            }
        }

        public double Distance { get; internal set; }

        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(RandomizationProvider.Current.GetInt(0, edgesNumber));
        }

        public override IChromosome CreateNew()
        {
            return new Chromosome(edgesNumber);
        }

        public override IChromosome Clone()
        {
            var clone = base.Clone() as Chromosome;
            clone.Distance = Distance;

            return clone;
        }
    }
}
