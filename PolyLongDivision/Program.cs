using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Numerics;

namespace PolyLongDivision
{
    static class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Please enter dividend.");
            BigRational[] dividend = ParseBigRationals(Console.ReadLine());

            Console.WriteLine("Divident parsed as");
            Console.WriteLine(BigRationalToStringBuilder(dividend));

            Console.WriteLine("Please enter divisor.");
            BigRational[] divisor = ParseBigRationals(Console.ReadLine());

            Console.WriteLine("Divisor parsed as");
            Console.WriteLine(BigRationalToStringBuilder(divisor));

            LongDiv.DivideResult result = LongDiv.PolyLongDivision(dividend, divisor);

            StringBuilder quotent = BigRationalToStringBuilder(result.Quotient);

            Console.WriteLine("Result: " + quotent);

            if (result.Remainder.Length > 0)
            {
                StringBuilder remainder = BigRationalToStringBuilder(result.Remainder);

                Console.WriteLine("Remainder: " + remainder);

            }
            else
            {
                Console.WriteLine("No remainder.");
            }

            Console.WriteLine("Press any key to exit. . .");
            Console.ReadKey();
        }


        public static StringBuilder BigRationalToStringBuilder(BigRational[] bigRationals)
        {
            StringBuilder resultSb = new StringBuilder();

            for (int i = 0; i < bigRationals.Length; i++)
            {

                if (bigRationals[i].Equals(0)) { continue; }

                resultSb.Insert(0, bigRationals[i]);

                switch (i)
                {
                    case 0:
                        break;
                    case 1:
                        resultSb.Insert(bigRationals[i].ToString().Length, "x ");
                        break;
                    default:
                        resultSb.Insert(bigRationals[i].ToString().Length, "x^" + i + " ");
                        break;
                }

                if (bigRationals[i].Sign.Equals(1))
                {
                    resultSb.Insert(0, "+");
                }

            }

            if (resultSb[0].Equals('+'))
            {
                resultSb.Remove(0, 1);
            }

            return resultSb;

        }

        public static BigRational[] ParseBigRationals(String input)
        {

            input = input.Replace(" ", string.Empty);

            MatchCollection matches = Regex.Matches(input, @"([+\-]?[0-9\/]*)(x\^[0-9\/]*|x)?");

            Dictionary<int, BigRational> values = new Dictionary<int, BigRational>();

            foreach (var match in matches)
            {

                String bufferInput = match.ToString();

                if (String.IsNullOrEmpty(bufferInput)) { continue; }

                int xPos = bufferInput.IndexOf('x');

                if (xPos == -1)
                {
                    if (bufferInput.Contains('/'))
                    {
                        String[] bufferFraction = bufferInput.Split('/');
                        values.Add(0,
                            new BigRational(BigInteger.Parse(bufferFraction[0]), BigInteger.Parse(bufferFraction[1])));
                    }
                    else
                    {
                        values.Add(0, new BigRational(BigInteger.Parse(bufferInput), 1));
                    }
                }
                else
                {
                    String coeff = bufferInput.Substring(0, xPos);

                    if (coeff.Equals("") || coeff.Equals("+")) { coeff = "1"; }
                    else if (coeff.Equals("-")) { coeff = "-1"; }

                    int xPower;
                    if (bufferInput.Last().Equals('x'))
                    {
                        xPower = 1;
                    }
                    else
                    {
                        xPower = Convert.ToInt32(bufferInput.Substring(xPos + 2));
                    }

                    if (coeff.Contains('/'))
                    {
                        String[] bufferFraction = coeff.Split('/');
                        values.Add(xPower,
                            new BigRational(BigInteger.Parse(bufferFraction[0]), BigInteger.Parse(bufferFraction[1])));
                    }
                    else
                    {
                        values.Add(xPower, new BigRational(BigInteger.Parse(coeff), 1));
                    }

                }

            }


            BigRational[] result = new BigRational[values.Keys.Max() + 1].Select(h => new BigRational(0, 1)).ToArray();


            foreach (KeyValuePair<int, BigRational> keyValuePair in values)
            {
                result[keyValuePair.Key] = keyValuePair.Value;
            }

            return result;
        }


    }

}
