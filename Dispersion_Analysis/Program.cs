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

        double factorVariance = factorSumOfSquares / (3 - 1); // k - 1, де k - кількість груп
        double residualVariance = residualSumOfSquares / (occupancyData.Length * occupancyData[0].Length - 3); // N - k

        Console.WriteLine($"Крок 4: Обчислення F-статистики");
        double F_observed = factorVariance / residualVariance;
        Console.WriteLine($"Спостережуване значення F: {F_observed:F2}");

        double F_critical = GetFCriticalValue(0.05, 2, 15); // Знайти критичне значення
        Console.WriteLine($"Критичне значення F: {F_critical:F2}");

        if (F_observed >= F_critical)
        {
            Console.WriteLine("Відхиляємо нульову гіпотезу: відстань впливає на заповнюваність готелів.");
        }
        else
        {
            Console.WriteLine("Не відхиляємо нульову гіпотезу: відстань не має суттєвого впливу на заповнюваність.");
        }
    }
    static double GetFCriticalValue(double alpha, int numeratorDf, int denominatorDf)
    {
        // Тут ви можете використовувати статистичну бібліотеку або вручну
        // Повертає критичне значення F на основі alpha та ступенів свободи
        // Для прикладу повертаємо фіксоване значення
        return 3.20; // Це лише приклад, вам потрібно знайти правильне значення
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
