using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelFor
{
    class Program
    {
        public static string TempFolder
        {
            get { return ConfigurationManager.AppSettings["tempFolder"]; }
        }

        private const int MAX_NUMBER_COUNT = 300;
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
            //Method1();
            StopAndBreakWithFiles();

            Console.ReadKey();
        }

        private static void StopAndBreakWithFiles()
        {
            if (!Directory.Exists(TempFolder))
            {
                Directory.CreateDirectory(TempFolder);
            }

            var integers = GetData();
            var result = Parallel.For(0, integers.Count,
               (index, state) =>
               {

                   //if (state.IsStopped)
                   //{
                   //    return;
                   //}

                   Console.WriteLine($"Inciando hilo = {Thread.CurrentThread.ManagedThreadId} | para el indice = {index}");


                   //if (index == 50)
                   //{
                   //    Console.WriteLine($"Hilo = {Thread.CurrentThread.ManagedThreadId} | Llamando a stop en {index} ");
                   //    state.Stop();
                   //}

                   if (index == 50)
                   {
                       //Console.WriteLine($"Hilo = {Thread.CurrentThread.ManagedThreadId} | Llamando a break en {index} ");
                      // state.Break();
                   }


                   if (!state.IsStopped)
                   {
                       //1. Crear archivo
                       var fileName = $@"{TempFolder}\Hilo-{index}.txt";
                       using (var stream = File.CreateText(fileName))
                       {
                           //2. Escribir al archivo
                           //stream.WriteLine($"Estamos en damier = {index}");
                           Console.WriteLine($"Hilo = {Thread.CurrentThread.ManagedThreadId} | para el indice = {index} | ejecutandose");
                           //2.1   Flush archivo
                           stream.Flush();
                       }

                       //3. Eliminar archivo
                       File.Delete(fileName);
                   }
                   else
                   {
                       Console.WriteLine($"Hilo = {Thread.CurrentThread.ManagedThreadId} | para el indice = {index} | se volvio loco");
                   }

                   Console.WriteLine($"Finalizando correctamente hilo = {Thread.CurrentThread.ManagedThreadId} | para el indice = {index}");

               });



        }

        private static void Method1()
        {
            var integers = GetData();

            //Foreach sencillo
            for (int index = 0; index < integers.Count; ++index)

            {
                Console.WriteLine("{0} = {1};", index, integers[index]);
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
                (index, state) =>
                {
                    Console.WriteLine("{0} = {1};", index, integers[index]);
                    state.Break(); //pare pero cuando pueda
                    //state.Stop();// pare YA!!
                });

            if (result.IsCompleted)
            //depende de break y stop.
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
        }
    }
}
