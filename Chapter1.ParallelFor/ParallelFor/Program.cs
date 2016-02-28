using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelFor
{
    class Program
    {
        private const int MAX_NUMBER_COUNT = 3000;
        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        private static IReadOnlyList<int> GetData()
        {
            var r = new Random((int)DateTime.Now.Ticks);
            var listOfInts = new List<int>();
            
            for (var i = 0; i < MAX_NUMBER_COUNT; i++)
            {
                listOfInts.Add(r.Next());
            }
            return listOfInts;

        }
        static void Main(string[] args)
        {
            var integers = GetData();

            //Foreach sencillo
            for (int index = 0; index < integers.Count; ++index)
                
            {
             //   Console.WriteLine("{0} = {1};", index, integers[index]);
            }

            //ParallelFor
            Console.WriteLine("Usando ParallelFor");
            if (integers is int[])
            {
                Console.WriteLine("Es un arreglo de enteros!!!");
            }

            if (integers is IList<int>)
            {
                Console.WriteLine("Es una lista de enteros!!!");
            }

            ParallelLoopResult result = Parallel.For(0, integers.Count,
                         index =>
                         {
                             //Console.WriteLine("{0} = {1};", index, integers[index]);
                         });

            if (result.IsCompleted)
            {
                Console.WriteLine("result esta completado");  
            }

            var indexWhereBreakWasCalled = result.LowestBreakIteration;
            if (indexWhereBreakWasCalled.HasValue)
            {
                Console.WriteLine("{0} = En este indice se llamo break", indexWhereBreakWasCalled);    
            }
            else
            {
                Console.WriteLine("No se usó break en este parallel for");
            }
            
            

            Console.ReadKey();

        }
    }
}
