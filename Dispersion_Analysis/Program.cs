using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Крок 1: Обчислення середніх значень у кожній групі");
        double[][] occupancyData = {
            new double[] { 92, 98, 89, 97, 90, 94 }, // до 3 км
            new double[] { 90, 86, 84, 91, 83, 82 }, // від 3 до 5 км
            new double[] { 97, 79, 74, 85, 73, 77 }  // більше 5 км
        };

        double avg1 = CalculateAverage(occupancyData[0]);
        double avg2 = CalculateAverage(occupancyData[1]);
        double avg3 = CalculateAverage(occupancyData[2]);

        double totalAverage = CalculateOverallAverage(occupancyData);

        Console.WriteLine("Середнє значення заповнюваності готелів:");
        Console.WriteLine($"До 3 км: {avg1:F2}");
        Console.WriteLine($"Від 3 до 5 км: {avg2:F2}");
        Console.WriteLine($"Більше 5 км: {avg3:F2}");
        Console.WriteLine($"Загальне середнє: {totalAverage:F2}");


        double totalSumOfSquares = CalculateTotalSumOfSquares(occupancyData, totalAverage);
        Console.WriteLine($"Загальна сума квадратів відхилень: {totalSumOfSquares:F2}");
    }

    static double CalculateAverage(double[] data)
    {
        double sum = 0;
        for (int i = 0; i < data.Length; i++)
        {
            sum += data[i];
        }
        return sum / data.Length;
    }

    static double CalculateOverallAverage(double[][] data)
    {
        double totalSum = 0;
        int totalCount = 0;

        for (int i = 0; i < data.Length; i++)
        {
            for (int j = 0; j < data[i].Length; j++)
            {
                totalSum += data[i][j];
                totalCount++;
            }
        }

        return totalSum / totalCount;
    }

    static double CalculateTotalSumOfSquares(double[][] data, double overallAverage)
    {
        double sumOfSquares = 0;

        for (int i = 0; i < data.Length; i++)
        {
            for (int j = 0; j < data[i].Length; j++)
            {
                double deviation = data[i][j] - overallAverage;
                sumOfSquares += deviation * deviation;
            }
        }

        return sumOfSquares;
    }

}