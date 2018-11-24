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

        private int verticesCount;
        Random randomizationProvider;
        public Chromosome(int verticesNumber) : base(verticesNumber)
        {
            verticesCount = verticesNumber;
            int selectedVertexIndex;  //losowy indeks jednego z sąsiadów
            int[] verticesIndexes = new int[verticesNumber]; 
            randomizationProvider = new Random();
            verticesIndexes[0] = randomizationProvider.Next(verticesNumber);
            for (int i = 1; i < verticesNumber; i++)
            {
                selectedVertexIndex = randomizationProvider.Next(verticesNumber);
                verticesIndexes[i] = selectedVertexIndex;
            }
            for (int i = 0; i < verticesNumber; i++)
            {
                ReplaceGene(i, new Gene(verticesIndexes[i]));
            }
        }

        public double Distance { get; internal set; }

        public override Gene GenerateGene(int geneIndex)
        {
            // return new Gene(RandomizationProvider.Current.GetInt(0, verticesCount));
            return new Gene(randomizationProvider.Next(verticesCount));
        }

        public override IChromosome CreateNew()
        {
            return new Chromosome(verticesCount);
        }

        public override IChromosome Clone()
        {
            var clone = base.Clone() as Chromosome;
            clone.Distance = Distance;

            return clone;
        }
    }
}
