using System;
using NuimoHelpers;
using NuimoHelpers.LedMatrices;
using NuimoSDK;

namespace TimerApp
{
    internal static class TimeDisplayExtensions
    {
        private static readonly NuimoLedMatrix OneHour = new NuimoLedMatrix(
            "**       " +
            "**       ");

        private static readonly NuimoLedMatrix TwoHour = new NuimoLedMatrix(
            "** **    " +
            "** **    ");

        private static readonly NuimoLedMatrix ThreeHour = new NuimoLedMatrix(
            "** ** ** " +
            "** ** ** ");


        public static NuimoLedMatrix AddHours(this NuimoLedMatrix matrix, int hours)
        {
            if (hours == 0)
                return matrix;
            if (hours > 3)
                throw new ArgumentException("hours > 3 not supported");

            NuimoLedMatrix matrixToAdd;
            switch (hours)
            {
                case 1:
                    matrixToAdd = OneHour;
                    break;
                case 2:
                    matrixToAdd = TwoHour;
                    break;
                case 3:
                    matrixToAdd = ThreeHour;
                    break;
                default:
                    return null;
            }

            return matrix.AddLedMatrix(matrixToAdd, 0, 7);
        }


        public static NuimoLedMatrix AddMinutes(this NuimoLedMatrix matrix, int minutes)
        {
            var textToAdd = minutes + ":";
            var matrixForText = textToAdd.ToSmallLedMatrix();
            matrix = matrix.AddLedMatrix(matrixForText);
            return matrix;
        }

        public static NuimoLedMatrix AddSeconds(this NuimoLedMatrix matrix, int seconds)
        {
            var textToAdd = ":" + seconds;
            var matrixForText = textToAdd.ToSmallLedMatrix();
            matrix = matrix.AddLedMatrix(matrixForText);
            return matrix;
        }
    }
}