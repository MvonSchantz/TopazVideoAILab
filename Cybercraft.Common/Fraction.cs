using System;
using System.Collections.Generic;
using System.Linq;

namespace Cybercraft.Common
{
    public class Fraction
    {
        public long Numerator { get; private set; }
        public long Denominator { get; private set; }

        public double Value => ((double)Numerator) / Denominator;

        public Fraction()
        {
            Numerator = 0;
            Denominator = 1;
        }

        public Fraction(long numerator, long denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public Fraction(long value)
        {
            Numerator = value;
            Denominator = 1;
        }

        public Fraction(Fraction other)
        {
            Numerator = other.Numerator;
            Denominator = other.Denominator;
        }

        public static Fraction FromLong(long value) => new Fraction(value, 1);

        public override string ToString() => $"{Numerator}/{Denominator}";

        public void DivideBy(Fraction other)
        {
            if (other.Numerator == 0)
            {
                throw new DivideByZeroException();
            }
            Numerator = other.Denominator * Numerator;
            Denominator = other.Numerator * Denominator;
            Minimize();
        }

        public void MultiplyBy(Fraction other)
        {
            Numerator = other.Numerator * Numerator;
            Denominator = other.Denominator * Denominator;
            Minimize();
        }

        public void Add(Fraction other)
        {
            long numerator = Numerator * other.Denominator;
            long denominator = Denominator * other.Denominator;

            long otherNumerator = other.Numerator * Denominator;
            //long otherDenominator = other.Denominator * Denominator;

            Numerator = numerator + otherNumerator;
            Denominator = denominator;
            Minimize();
        }

        public static Fraction operator +(Fraction a) => a;
        public static Fraction operator -(Fraction a) => new Fraction(-a.Numerator, a.Denominator);

        public static Fraction operator +(Fraction a, Fraction b)
        {
            var result = new Fraction(a);
            result.Add(b);
            return result;
        }

        public static Fraction operator -(Fraction a, Fraction b) => a + (-b);

        public static Fraction operator *(Fraction a, Fraction b)
        {
            var result = new Fraction(a);
            result.MultiplyBy(b);
            return result;
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            var result = new Fraction(a);
            result.DivideBy(b);
            return result;
        }

        public static implicit operator Fraction(int i) => FromLong(i);

        public static long[] GetFactors(long num)
        {
            if (num == 0)
                return new[] { 0L };
            if (num == 1)
                return new[] { 1L };
            if (num == 2)
                return new[] { 2L };
            if (num == 3)
                return new[] { 3L };

            var result = new List<long>();

            // Take out the 2s.
            while (num % 2 == 0)
            {
                result.Add(2);
                num /= 2;
            }

            // Take out other primes.
            long factor = 3;
            while (factor * factor <= num)
            {
                if (num % factor == 0)
                {
                    // This is a factor.
                    result.Add(factor);
                    num /= factor;
                }
                else
                {
                    // Go to the next odd number.
                    factor += 2;
                }
            }

            // If num is not 1, then whatever is left is prime.
            if (num > 1) result.Add(num);

            return result.ToArray();
        }

        public void Minimize()
        {
            var numerator = GetFactors(Numerator).ToList();
            var denominator = GetFactors(Denominator).ToList();

            for (int i = numerator.Count - 1; i >= 0; i--)
            {
                if (denominator.Contains(numerator[i]))
                {
                    denominator.Remove(numerator[i]);
                    numerator.RemoveAt(i);
                }
            }

            for (int i = denominator.Count - 1; i >= 0; i--)
            {
                if (numerator.Contains(denominator[i]))
                {
                    numerator.Remove(denominator[i]);
                    denominator.RemoveAt(i);
                }
            }

            Numerator = 1;
            Denominator = 1;
            foreach (var w in numerator)
            {
                Numerator *= w;
            }

            foreach (var h in denominator)
            {
                Denominator *= h;
            }
        }
    }
}
