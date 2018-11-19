using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Piotrek\Desktop\Nowy\Sem 9\Inteligentne systemy komputerowe\Aplikacja\dane.txt");
            int edgesNumber = lines.Count();
            List<Edge> edges = new List<Edge>();
            List<int> vertices = new List<int>();
            string stringSeparator =  "\t";
            foreach(string l in lines)
            {
                string[] values = l.Split(stringSeparator.ToCharArray(),StringSplitOptions.None);
                int[] numericValues = values.Select(s=>int.Parse(s)).ToArray();

                if (!vertices.Contains(numericValues[0]))
                {
                    vertices.Add(numericValues[0]);
                }
                if (!vertices.Contains(numericValues[1]))
                {
                    vertices.Add(numericValues[1]);
                }

                edges.Add(new Edge(numericValues[0], numericValues[1], numericValues[2]));
            }
            int verticesNumber = vertices.Count;
            var selection = new RouletteWheelSelection();
            var crossover = new OrderedCrossover();
            var mutation = new ReverseSequenceMutation();
            var fitness = new FitnessFunction(edges);
            var chromosome = new Chromosome(edgesNumber);
            var population = new Population(50, 100, chromosome);

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            ga.Termination = new GenerationNumberTermination(100);

            Console.WriteLine("GA running...");
            ga.Start();

            Console.WriteLine("Best solution found has {0} fitness.", ga.BestChromosome.Fitness);
            foreach(Gene g in ga.BestChromosome.GetGenes())
            {
                Console.Write("{0},", g);
            }
            
        }
    }
}
