using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticApp
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Podaj nazwę plkiu z danymi(bez rozszerzenia)");
            string fileName = Console.ReadLine();
            string[] lines = System.IO.File.ReadAllLines(Directory.GetCurrentDirectory()+"\\"+fileName+".txt");
            int edgesNumber = lines.Count();
            List<Edge> edges = new List<Edge>();
            List<int> vertices = new List<int>();
            int edgeIndex = 0;
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

                edges.Add(new Edge(numericValues[0], numericValues[1], numericValues[2], edgeIndex));//krawędź może być przechodzona w obu kierunkach
                edges.Add(new Edge(numericValues[1], numericValues[0], numericValues[2], edgeIndex));//dlatego dodawana jest także w odwróconej wersji
                edgeIndex++;//krawędź i jej odwrócona wersja mają ten sam indeks(dla łatwiejszego odnajdowania)

            }
            int verticesNumber = vertices.Count;
                   
            var selection = new EliteSelection();
            var crossover = new ThreeParentCrossover();
            var mutation = new TworsMutation();
            var fitness = new FitnessFunction(edges);
            var chromosome = new Chromosome(4*edgesNumber,edges);
            var population = new Population(200, 400, chromosome);

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            ga.Termination = new GenerationNumberTermination(400);

            Stopwatch timer = new Stopwatch();
            timer.Start();
            Console.WriteLine("GA running...");
            ga.Start();
            timer.Stop();

            Chromosome bestChromosome = ga.BestChromosome as Chromosome;
            int totalCost = 0;
            int currentEdgeIndex = Convert.ToInt32(bestChromosome.GetGene(0).Value, CultureInfo.InvariantCulture);
            Edge currentEdge = edges[currentEdgeIndex];
            int startVertex = currentEdge.VertexA;
            totalCost += currentEdge.Cost;
            string verticesSequence = currentEdge.VertexA.ToString()+"-"+currentEdge.VertexB.ToString();

            Console.WriteLine("Funkcja dopasowania najlepszego rozwiązania wynosi: {0}", bestChromosome.Fitness);
            for(int i = 1; i < bestChromosome.Length; i++)
            {
                currentEdgeIndex = Convert.ToInt32(bestChromosome.GetGene(i).Value, CultureInfo.InvariantCulture);
                currentEdge = edges[currentEdgeIndex];
                currentEdge.Visited = true;
                edges.Find(e => e.VertexA == currentEdge.VertexB && e.VertexB == currentEdge.VertexA).Visited = true;
                totalCost += currentEdge.Cost;
                verticesSequence = verticesSequence + "-" + currentEdge.VertexB.ToString();
                if (FitnessFunction.AllEdgesVisited(edges)) 
                {
                    if (currentEdge.VertexB == startVertex)
                    {
                        break;
                    }
                        
                    Edge possibleEdge = edges.Find(e => e.VertexA == currentEdge.VertexB && e.VertexB == startVertex);
                    if (possibleEdge != null)
                    {
                        totalCost += possibleEdge.Cost;
                        verticesSequence = verticesSequence + "-" + possibleEdge.VertexB.ToString();
                        break;
                    }
                   
                }
            }
            TimeSpan executionTime = timer.Elapsed;
            Console.WriteLine("Ścieżka: {0}", verticesSequence);
            Console.WriteLine("Koszt najlepszego rozwiązania: {0}", totalCost);
            Console.WriteLine("Czas wykonania: {0}", executionTime);
            Console.ReadKey();
            
        }

        
    }
}

