using System;
using System.Collections.Generic;
using System.Linq;
using NuimoSDK;

namespace NuimoHelpers
{
    public static class LedMatrixExtensions
    {
        public static NuimoLedMatrix AddLedMatrix(this NuimoLedMatrix matrix, NuimoLedMatrix matrixToAdd,
            int shiftRight = 0, int shiftDown = 0)
        {
            var ledsToAdd = matrixToAdd.Leds
                .ShiftRight(shiftRight)
                .ShiftDown(shiftDown);

            var leds = matrix.Leds.Zip(ledsToAdd, (led1, led2) => led1 || led2).ToArray();
            var matrixToReturn = leds.ToNuimoLedMatrix();

            return matrixToReturn;
        }

        private static bool[] ShiftRight(this bool[] leds, int amount)
        {
            if (amount < 1)
                return leds;

            var shiftedLeds = leds
                .Take(Math.Min(leds.Length, 81))
                .Partition(9)
                .Select(ledRow => ledRow.ToArray().ShiftRow(amount))
                .SelectMany(x => x);

            return shiftedLeds.ToArray();
        }

        private static bool[] ShiftRow(this bool[] ledRow, int amount)
        {
            if (amount < 1)
                return ledRow;

            Array.Copy(ledRow, 0, ledRow, amount, ledRow.Length - amount);
            for (var i = 0; i < amount; i++)
                ledRow[i] = false;

            return ledRow;
        }

        private static bool[] ShiftDown(this bool[] leds, int amount)
        {
            if (amount < 1)
                return leds;

            var ledsToInsert = Enumerable.Repeat(false, amount * 9).ToList();

            var shiftedLeds = ledsToInsert
                .Concat(leds)
                .Take(Math.Min(leds.Length, 81));

            return shiftedLeds.ToArray();
        }

        private static NuimoLedMatrix ToNuimoLedMatrix(this bool[] leds)
        {
            var ledCharArray = leds
                .Take(Math.Min(leds.Length, 81))
                .Select(ledOn => ledOn ? '*' : ' ')
                .ToArray();
            var ledString = new string(ledCharArray);
            var matrix = new NuimoLedMatrix(ledString);
            return matrix;
        }

        private static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> items, int partitionSize)
        {
            return items.Select((item, inx) => new {item, inx})
                .GroupBy(x => x.inx / partitionSize)
                .Select(g => g.Select(x => x.item));
        }
    }
}