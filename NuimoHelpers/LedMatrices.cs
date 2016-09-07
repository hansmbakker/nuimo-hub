using System;
using System.Collections.Generic;
using System.Linq;
using NuimoSDK;

namespace NuimoHelpers.LedMatrices
{
    public static class Icons
    {
        public static readonly NuimoLedMatrix MusicNote = new NuimoLedMatrix(
            "         " +
            "  .....  " +
            "  .....  " +
            "  .   .  " +
            "  .   .  " +
            "  .   .  " +
            " ..  ..  " +
            "... ...  " +
            " .   .   ");

        public static readonly NuimoLedMatrix LightBulb = new NuimoLedMatrix(
            "         " +
            "   ...   " +
            "  .   .  " +
            "  .   .  " +
            "  .   .  " +
            "   ...   " +
            "   ...   " +
            "   ...   " +
            "    .    ");


        public static readonly NuimoLedMatrix PowerOn = new NuimoLedMatrix(
            "         " +
            "         " +
            "   ...   " +
            "  .....  " +
            "  .....  " +
            "  .....  " +
            "   ...   " +
            "         " +
            "         ");

        public static readonly NuimoLedMatrix PowerOff = new NuimoLedMatrix(
            "         " +
            "         " +
            "   ...   " +
            "  .   .  " +
            "  .   .  " +
            "  .   .  " +
            "   ...   " +
            "         " +
            "         ");

        public static readonly NuimoLedMatrix Shuffle = new NuimoLedMatrix(
            "         " +
            "         " +
            " ..   .. " +
            "   . .   " +
            "    .    " +
            "   . .   " +
            " ..   .. " +
            "         " +
            "         ");

        public static readonly NuimoLedMatrix Play = new NuimoLedMatrix(
            "         " +
            "   .     " +
            "   ..    " +
            "   ...   " +
            "   ....  " +
            "   ...   " +
            "   ..    " +
            "   .     " +
            "         ");

        public static readonly NuimoLedMatrix Pause = new NuimoLedMatrix(
            "         " +
            "  .. ..  " +
            "  .. ..  " +
            "  .. ..  " +
            "  .. ..  " +
            "  .. ..  " +
            "  .. ..  " +
            "  .. ..  " +
            "         ");

        public static readonly NuimoLedMatrix Next = new NuimoLedMatrix(
            "         " +
            "         " +
            "   .  .  " +
            "   .. .  " +
            "   ....  " +
            "   .. .  " +
            "   .  .  " +
            "         " +
            "         ");

        public static readonly NuimoLedMatrix Previous = new NuimoLedMatrix(
            "         " +
            "         " +
            "  .  .   " +
            "  . ..   " +
            "  ....   " +
            "  . ..   " +
            "  .  .   " +
            "         " +
            "         ");

        public static readonly NuimoLedMatrix QuestionMark = new NuimoLedMatrix(
            "   ...   " +
            "  .   .  " +
            " .     . " +
            "      .  " +
            "     .   " +
            "    .    " +
            "    .    " +
            "         " +
            "    .    ");

        public static readonly NuimoLedMatrix Bluetooth = new NuimoLedMatrix(
            "    *    " +
            "    **   " +
            "  * * *  " +
            "   ***   " +
            "    *    " +
            "   ***   " +
            "  * * *  " +
            "    **   " +
            "    *    ");

        public static readonly NuimoLedMatrix Home = new NuimoLedMatrix(
            "    *    " +
            "   * **  " +
            "  *   *  " +
            " *     * " +
            "** *   **" +
            " *     * " +
            " *  ** * " +
            " *  ** * " +
            " ******* "
        );

        public static readonly NuimoLedMatrix Bed = new NuimoLedMatrix(
            "         " +
            "         " +
            "         " +
            "         " +
            "        *" +
            "      ***" +
            "*********" +
            "*********" +
            " *     * ");

        public static readonly NuimoLedMatrix Couch = new NuimoLedMatrix(
            "         " +
            "         " +
            "         " +
            "         " +
            " xxxxxxx " +
            "x       x" +
            "xxxxxxxxx" +
            "xxxxxxxxx" +
            " x     x ");

        public static readonly NuimoLedMatrix Door = new NuimoLedMatrix(
            "         " +
            "  xxxxx  " +
            "  x   x  " +
            "  x   x  " +
            "  x  xx  " +
            "  x   x  " +
            "  x   x  " +
            "  x   x  " +
            "  xxxxx  ");

        public static readonly NuimoLedMatrix Cast = new NuimoLedMatrix(
            "         " +
            "*********" +
            "*       *" +
            "***     *" +
            "   *    *" +
            "**  *   *" +
            "  * *   *" +
            "* * *****" +
            "         ");

        public static readonly NuimoLedMatrix Timer = new NuimoLedMatrix(
            "    *    " +
            "   ***   " +
            "  *   *  " +
            " *   * * " +
            " *  *  * " +
            " *     * " +
            "  *   *  " +
            "   ***   " +
            "         ");
    }

    public static class ProgressBars
    {
        /// <summary>
        /// </summary>
        /// <param name="progress">Value between 0.0 and 1.0</param>
        /// <returns></returns>
        public static NuimoLedMatrix VerticalBar(double progress)
        {
            var ledStringParts = Enumerable.Range(0, 9)
                .Reverse()
                .Select(i =>
                    progress > i / 9.0
                        ? "    .    "
                        : "         ");

            return new NuimoLedMatrix(string.Join("", ledStringParts));
        }

        /// <summary>
        /// </summary>
        /// <param name="progress">Value between 0.0 and 1.0</param>
        /// <returns></returns>
        public static NuimoLedMatrix VolumeBar(double progress)
        {
            var width = (int) Math.Ceiling(Math.Max(0.0, Math.Min(1.0, progress)) * 9);
            var ledStringParts = Enumerable.Range(0, 9)
                .Select(i =>
                    new string(' ', 9 - (i + 1)) + new string('.', i + 1))
                .Select(part => part.Substring(0, width));

            return new NuimoLedMatrix(string.Join("", ledStringParts));
        }
    }

    public static class Characters
    {
        public static readonly NuimoLedMatrix LetterB = new NuimoLedMatrix(
            "         " +
            "   ...   " +
            "   .  .  " +
            "   .  .  " +
            "   ...   " +
            "   .  .  " +
            "   .  .  " +
            "   ...   " +
            "         ");

        public static readonly NuimoLedMatrix LetterO = new NuimoLedMatrix(
            "         " +
            "   ...   " +
            "  .   .  " +
            "  .   .  " +
            "  .   .  " +
            "  .   .  " +
            "  .   .  " +
            "   ...   " +
            "         ");

        public static readonly NuimoLedMatrix LetterG = new NuimoLedMatrix(
            "         " +
            "   ...   " +
            "  .   .  " +
            "  .      " +
            "  . ...  " +
            "  .   .  " +
            "  .   .  " +
            "   ...   " +
            "         ");

        public static readonly NuimoLedMatrix LetterT = new NuimoLedMatrix(
            "         " +
            " ....... " +
            "    .    " +
            "    .    " +
            "    .    " +
            "    .    " +
            "    .    " +
            "    .    " +
            "         ");

        public static readonly NuimoLedMatrix LetterW = new NuimoLedMatrix(
            "         " +
            " .     . " +
            " .     . " +
            " .     . " +
            " .     . " +
            " .  .  . " +
            " .  .  . " +
            "  .. ..  " +
            "         ");

        public static readonly NuimoLedMatrix LetterY = new NuimoLedMatrix(
            "         " +
            "  .   .  " +
            "  .   .  " +
            "   . .   " +
            "    .    " +
            "    .    " +
            "    .    " +
            "    .    " +
            "         ");

        public static readonly NuimoLedMatrix SmallOne = new NuimoLedMatrix(
            " *       " +
            "**       " +
            " *       " +
            " *       " +
            "***      ");

        public static readonly NuimoLedMatrix SmallTwo = new NuimoLedMatrix(
            " *       " +
            "* *      " +
            "  *      " +
            " *       " +
            "***      ");

        public static readonly NuimoLedMatrix SmallThree = new NuimoLedMatrix(
            " *       " +
            "* *      " +
            " **      " +
            "* *      " +
            " *       ");

        public static readonly NuimoLedMatrix SmallFour = new NuimoLedMatrix(
            "  *      " +
            " **      " +
            "* *      " +
            "***      " +
            "  *      ");

        public static readonly NuimoLedMatrix SmallFive = new NuimoLedMatrix(
            "***      " +
            "*        " +
            "**       " +
            "  *      " +
            "**       ");

        public static readonly NuimoLedMatrix SmallSix = new NuimoLedMatrix(
            "***      " +
            "*        " +
            "***      " +
            "* *      " +
            "***      ");

        public static readonly NuimoLedMatrix SmallSeven = new NuimoLedMatrix(
            "***      " +
            "  *      " +
            "  *      " +
            " *       " +
            " *       ");

        public static readonly NuimoLedMatrix SmallEight = new NuimoLedMatrix(
            " *       " +
            "* *      " +
            " *       " +
            "* *      " +
            " *       ");

        public static readonly NuimoLedMatrix SmallNine = new NuimoLedMatrix(
            " *       " +
            "* *      " +
            " **      " +
            "  *      " +
            " *       ");

        public static readonly NuimoLedMatrix SmallZero = new NuimoLedMatrix(
            "***      " +
            "* *      " +
            "* *      " +
            "* *      " +
            "***      ");

        public static readonly NuimoLedMatrix SmallColon = new NuimoLedMatrix(
            "**       " +
            "**       " +
            "         " +
            "**       " +
            "**       ");

        private static Dictionary<char, NuimoLedMatrix> SmallMatricesForCharacters => new
            Dictionary<char, NuimoLedMatrix>
            {
                {'1', SmallOne},
                {'2', SmallTwo},
                {'3', SmallThree},
                {'4', SmallFour},
                {'5', SmallFive},
                {'6', SmallSix},
                {'7', SmallSeven},
                {'8', SmallEight},
                {'9', SmallNine},
                {'0', SmallZero},
                {':', SmallColon}
            };

        public static NuimoLedMatrix ToLedMatrix(this char character, bool small)
        {
            if (small)
                if (SmallMatricesForCharacters.ContainsKey(character))
                    return SmallMatricesForCharacters[character];
            throw new KeyNotFoundException("Ledmatrix not defined");
        }

        public static NuimoLedMatrix ToSmallLedMatrix(this string text)
        {
            var matrix = new NuimoLedMatrix("");

            var offset = 0;
            foreach (var character in text)
            {
                matrix = matrix.AddLedMatrix(character.ToLedMatrix(true), offset);
                offset += 3;
            }
            return matrix;
        }

        public static string ToLedString(this NuimoLedMatrix matrix)
        {
            return new string(matrix.Leds.Select(led => led ? '*' : ' ').ToArray());
        }
    }
}