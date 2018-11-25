using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GeneticApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Podaj nazwę plkiu z danymi(bez rozszerzenia)");
            string fileName = Console.ReadLine();
            string[] lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\" + fileName + ".txt");
            int edgesNumber = lines.Count();
            List<Edge> edges = new List<Edge>();
            int edgeIndex = 0;
            string stringSeparator = " ";
            foreach (string l in lines)
            {
                int[] values = l.Split(stringSeparator.ToCharArray(), StringSplitOptions.None).Select(s => int.Parse(s)).ToArray();

                // Krawędź może być przechodzona w obu kierunkach
                edges.Add(new Edge(values[0], values[1], values[2], edgeIndex));

                // Ddodawana jest także w odwróconej wersji
                edges.Add(new Edge(values[1], values[0], values[2], edgeIndex));

                //Krawędź i jej odwrócona wersja mają ten sam indeks(dla łatwiejszego odnajdowania)
                edgeIndex++;
            }

            EliteSelection selection = new EliteSelection();
            ThreeParentCrossover crossover = new ThreeParentCrossover();
            TworsMutation mutation = new TworsMutation();
            FitnessFunction fitness = new FitnessFunction(edges);
            Chromosome chromosome = new Chromosome(4 * edgesNumber, edges);
            Population population = new Population(200, 400, chromosome);

            GeneticAlgorithm ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation)
            {
                Termination = new GenerationNumberTermination(400)
            };

            Stopwatch timer = new Stopwatch();
            timer.Start();
            Console.WriteLine("START!");
            ga.Start();
            timer.Stop();

            Chromosome bestChromosome = ga.BestChromosome as Chromosome;
            int currentEdgeIndex = int.Parse(bestChromosome.GetGene(0).Value.ToString());
            Edge currentEdge = edges[currentEdgeIndex];
            int startVertex = currentEdge.VertexA;
            int totalCost = currentEdge.Cost;
            string verticesSequence = currentEdge.VertexA + "-" + currentEdge.VertexB;

            Console.WriteLine("Funkcja dopasowania najlepszego rozwiązania wynosi: {0}", bestChromosome.Fitness);
            for (int i = 1; i < bestChromosome.Length; i++)
            {
                currentEdgeIndex = int.Parse(bestChromosome.GetGene(i).Value.ToString());
                currentEdge = edges[currentEdgeIndex];
                currentEdge.Visited = true;
                edges.SingleOrDefault(e => e.VertexA == currentEdge.VertexB && e.VertexB == currentEdge.VertexA).Visited = true;
                totalCost += currentEdge.Cost;
                verticesSequence += "-" + currentEdge.VertexB;

                if (FitnessFunction.AllEdgesVisited(edges))
                {
                    if (currentEdge.VertexB == startVertex)
                    {
                        break;
                    }

                    Edge possibleEdge = edges.SingleOrDefault(e => e.VertexA == currentEdge.VertexB && e.VertexB == startVertex);
                    if (possibleEdge != null)
                    {
                        totalCost += possibleEdge.Cost;
                        verticesSequence += "-" + possibleEdge.VertexB;
                        break;
                    }
                }
            }

            Console.WriteLine("Ścieżka: {0}", verticesSequence);
            Console.WriteLine("Koszt najlepszego rozwiązania: {0}", totalCost);
            Console.WriteLine("Czas wykonania: {0}", timer.Elapsed.ToString(@"hh\:mm\:ss\:ff"));
            Console.ReadKey();
        }
    }
}