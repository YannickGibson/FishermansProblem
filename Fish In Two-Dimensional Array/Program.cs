using System;
using System.Threading;
using System.Diagnostics;
namespace Fish_In_Two_Dimensional_Array
{
    class Program
    {
        static Random rnd = new Random();
        static int[,] GenerateSquaredWater(int side, int fishToSpawn)
        {
            int[,] squaredWater = new int[side, side];
            
            int randomAmount;
            int x, y;
            while (fishToSpawn != 0)
            {
                x = rnd.Next(0, squaredWater.GetLength(0));
                y = rnd.Next(0, squaredWater.GetLength(1));

                if (squaredWater[x, y] == 0)
                {
                    if (0 <= fishToSpawn - 9)
                        randomAmount = rnd.Next(0, 9);
                    else
                        randomAmount = fishToSpawn;

                    squaredWater[x, y] = randomAmount;
                    fishToSpawn -= randomAmount;
                }
            }
            return squaredWater;
        }

        static public void SolveFishermansProblem(int fishToSpawn, int sideOfArea, int sideOfNetArea, bool printFinalMap)
        {
            
            Console.Write($"Fish Spawned: {fishToSpawn}");
            Console.Write($", Area: {sideOfArea} x {sideOfArea} m");
            Console.WriteLine($", Net Area: {sideOfNetArea} x {sideOfNetArea} m");

            int[,] oneKilometerSquaredOfWater = GenerateSquaredWater(sideOfArea, fishToSpawn);

            Catch bestCatch = GetBestCatchPosition(oneKilometerSquaredOfWater, sideOfNetArea);
            Console.WriteLine($"Best Position To Catch Most Fish: {bestCatch.X + 1}, {bestCatch.Y + 1}"); // + 1 cuz index
            Console.WriteLine($"Amount Of Fish Caught: {bestCatch.NumberOfFish}");

            if (printFinalMap == true)
            {
                PrintFishCatchArea(oneKilometerSquaredOfWater, bestCatch, sideOfNetArea);
            }

        }

        static void Main(string[] args)
        {// Doesnt throw exceptions to not loop infinitelly while creating fish
            Stopwatch sw = new Stopwatch();

            sw.Start();
            // 10 x 10 m
            SolveFishermansProblem(
                fishToSpawn: 55,
                sideOfArea: 10, // 10 x 10 m
                sideOfNetArea: 3, // 3 x 3m
                printFinalMap: true
                );
           
            Console.WriteLine($">>Time Elapse {sw.ElapsedMilliseconds}ms\n");
            sw.Restart();

            // 1 x 1 km
            SolveFishermansProblem(
                fishToSpawn: 100_000,
                sideOfArea: 1000, // 1 x 1 km
                sideOfNetArea: 30, // 30 x 30 m
                printFinalMap: false
                );
            Console.WriteLine($">>Time Elapse {sw.ElapsedMilliseconds}ms\n");
            sw.Stop();
            Console.ReadLine();
        }

        private static void PrintFishCatchArea(int[,] fishArray, Catch bestCatch, int sideOfNetArea)
        {
            for (int y = 0; y < fishArray.GetLength(1); y++)
            {
                Console.Write("-");
                for (int x = 0; x < fishArray.GetLength(0); x++)
                {
                    if (x >= bestCatch.X && x < bestCatch.X + sideOfNetArea
                        && y >= bestCatch.Y && y < bestCatch.Y + sideOfNetArea)
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    else
                        Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"{fishArray[x, y]}-");
                }
                Console.WriteLine();
            }
        }

        class Catch
        {
            public Catch(int x, int y, int numberOfFish = 0)
            {
                X = x;
                Y = y;
                NumberOfFish = numberOfFish;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int NumberOfFish {get; set;}
        }
        private static Catch GetBestCatchPosition(int[,] oneKilometerSquaredOfWater, int sideOfArea)
        {// 4 nested for's, YIKES

            Catch bestPosition = null;
            int sumOfFish = 0;
            for (int y = 0; y < oneKilometerSquaredOfWater.GetLength(1) - sideOfArea; y++)
            {
                for (int x = 0; x < oneKilometerSquaredOfWater.GetLength(0) - sideOfArea; x++)
                {

                    //Count fish
                    int currSumOfFish = 0;
                    for (int yy = y; yy < y + sideOfArea; yy++)
                    {
                        for (int xx = x; xx < x + sideOfArea; xx++)
                        {
                            currSumOfFish += oneKilometerSquaredOfWater[xx, yy];
                        }
                    }
                    if (currSumOfFish > sumOfFish)
                    {
                        sumOfFish = currSumOfFish;
                        bestPosition = new Catch(x, y, sumOfFish);
                    }

                }
            }


            return bestPosition;
        }
    }
}
