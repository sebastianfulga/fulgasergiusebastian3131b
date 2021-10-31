using OpenTK;
using System;
using System.Drawing;

namespace Lab_4
{
    public class Randomizer
    {
        private Random r;

        private const int MIN_VAL = -25;
        private const int MAX_VAL = 25;

        /// <summary>
        ///  constructor implicit
        /// </summary>
        public Randomizer()
        {
            r = new Random();
        }

        /// <summary>
        /// laborator 4 - punctul 3 - aceasta metoda returneaza o culoare random
        /// </summary>
        /// <returns>Culoarea generata random</returns>
        public Color RandomColor()
        {
            int genR = r.Next(0, 255);
            int genG = r.Next(0, 255);
            int genB = r.Next(0, 255);

            Color culoare = Color.FromArgb(genR, genG, genB);

            return culoare;
        }

        public int GetRandomOffsetPositive(int maxval)
        {
            int genInteger = r.Next(0, maxval);

            return genInteger;
        }

        public int GetRandomOffsetRanged(int maxval)
        {
            int genInteger = r.Next(-1 * maxval, maxval);

            return genInteger;
        }

        public Vector3 Generate3DPoint()
        {
            int a = r.Next(MIN_VAL, MAX_VAL);
            int b = r.Next(MIN_VAL, MAX_VAL);
            int c = r.Next(MIN_VAL, MAX_VAL);

            Vector3 vector = new Vector3(a, b, c);

            return vector;
        }

        public Vector3 Generate3DPoint(int spec)
        {
            int a = r.Next(-1 * spec, spec);
            int b = r.Next(-1 * spec, spec);
            int c = r.Next(-1 * spec, spec);

            Vector3 vector = new Vector3(a, b, c);

            return vector;
        }

        public int GeneratePositiveInt(int limit)
        {
            return r.Next(0, limit);
        }
    }
}
