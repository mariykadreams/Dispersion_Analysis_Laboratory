using MathNet.Numerics.Distributions;
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Крок 1: Обчислення середніх значень у кожній групі та  загального середнього значення");
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

        Console.WriteLine($"Крок 2.Обчислення суми квадратів відхилень");

        double totalSumOfSquares = CalculateTotalSumOfSquares(occupancyData, totalAverage);
        Console.WriteLine($"Загальна сума квадратів відхилень: {totalSumOfSquares:F2}");

        double factorSumOfSquares = CalculateFactorSumOfSquares(new double[] { avg1, avg2, avg3 }, totalAverage, 6);
        Console.WriteLine($"Факторна сума квадратів: {factorSumOfSquares:F2}");

        double residualSumOfSquares = totalSumOfSquares - factorSumOfSquares;
        Console.WriteLine($"Залишкова сума квадратів: {residualSumOfSquares:F2}");

        Console.WriteLine($"Крок 3.Обчислення дисперсії");
        int df1 = 3 - 1; 
        int df2 = occupancyData.Length * occupancyData[0].Length - 3; 

        double factorVariance = factorSumOfSquares / df1; 
        double residualVariance = residualSumOfSquares / df2; 

        Console.WriteLine("Крок 4: Обчислення F-статистики");
        double F_observed = factorVariance / residualVariance;
        Console.WriteLine($"Спостережуване значення F: {F_observed:F2}");

        double F_critical_table = GetFCriticalValueFromTable(fTable, df1, df2);
        Console.WriteLine($"Критичне значення F (з таблиці): {F_critical_table:F2}");


        double F_critical_library = GetFCriticalValueLibrary(0.05, df1, df2);
        Console.WriteLine($"Критичне значення F (бібліотека): {F_critical_library:F2}");

        if (F_observed >= F_critical_table)
        {
            Console.WriteLine("Відхиляємо нульову гіпотезу: відстань впливає на заповнюваність готелів.");
        }
        else
        {
            Console.WriteLine("Не відхиляємо нульову гіпотезу: відстань не має суттєвого впливу на заповнюваність.");
        }
    }
    static readonly double[,] fTable = {
        { 161.4, 199.5, 215.7, 224.6, 230.2, 234.0, 236.8, 238.9, 240.5, 241.9 },
        { 18.51, 19.0, 19.16, 19.25, 19.3, 19.33, 19.35, 19.37, 19.38, 19.4 },
        { 10.13, 9.55, 9.28, 9.12, 9.01, 8.94, 8.89, 8.85, 8.81, 8.79 },
        { 7.71, 6.94, 6.59, 6.39, 6.26, 6.16, 6.09, 6.04, 6.00, 5.96 },
        { 6.61, 5.79, 5.41, 5.19, 5.05, 4.95, 4.88, 4.82, 4.77, 4.74 },
        { 5.99, 5.14, 4.76, 4.53, 4.39, 4.28, 4.21, 4.15, 4.10, 4.06 },
        { 5.59, 4.74, 4.35, 4.12, 3.97, 3.87, 3.79, 3.73, 3.68, 3.64 },
        { 5.32, 4.46, 4.07, 3.84, 3.69, 3.58, 3.50, 3.44, 3.39, 3.35 },
        { 5.12, 4.26, 3.86, 3.63, 3.48, 3.37, 3.29, 3.23, 3.18, 3.14 },
        { 4.96, 4.10, 3.71, 3.48, 3.33, 3.22, 3.14, 3.07, 3.02, 2.98 },
        { 4.84, 3.98, 3.59, 3.36, 3.20, 3.09, 3.01, 2.95, 2.90, 2.85 },
        { 4.75, 3.89, 3.49, 3.26, 3.11, 3.00, 2.91, 2.85, 2.80, 2.75 },
        { 4.67, 3.81, 3.41, 3.18, 3.03, 2.92, 2.83, 2.77, 2.71, 2.64 },
        { 4.60, 3.74, 3.34, 3.11, 2.96, 2.82, 2.76, 2.70, 2.65, 2.60 },
        { 4.54, 3.68, 3.29, 3.06, 2.90, 2.79, 2.71, 2.64, 2.59, 2.54 },
        { 4.49, 3.63, 3.24, 3.01, 2.85, 2.74, 2.66, 2.59, 2.54, 2.49 }
    };

    static double GetFCriticalValueFromTable(double[,] fTable, int df1, int df2)
    {
        return fTable[df2 - 1, df1 - 1];
    }
    static double GetFCriticalValueLibrary(double alpha, int numeratorDf, int denominatorDf)
    {
        return FisherSnedecor.InvCDF(numeratorDf, denominatorDf, 1 - alpha);
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

    static double CalculateFactorSumOfSquares(double[] groupAverages, double overallAverage, int groupSize)
    {
        double factorSumOfSquares = 0;

        for (int i = 0; i < groupAverages.Length; i++)
        {
            double deviation = groupAverages[i] - overallAverage;
            factorSumOfSquares += groupSize * (deviation * deviation);
        }

        return factorSumOfSquares;
    }

   
}
 