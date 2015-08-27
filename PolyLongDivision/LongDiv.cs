using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Numerics;

namespace PolyLongDivision
{
    internal static class LongDiv
    {

        public class DivideResult
        {
            public BigRational[] Quotient;
            public BigRational[] Remainder;
        }

        public static DivideResult PolyLongDivision(BigRational[] dividend, BigRational[] divisor)
        {

            DivideResult divideResult = new DivideResult();

            BigRational[] bufferDividend = dividend;

            int divisorMaxPower = divisor.Length - 1;

            BigRational divisorMaxCoeff = divisor.Last();

            BigRational[] result = new BigRational[dividend.Length];

            while (true)
            {
                long dividentMaxPower = bufferDividend.Length - 1;

                long resultXPower = dividentMaxPower - divisorMaxPower;

                if (resultXPower < 0)
                {
                    divideResult.Quotient = RemoveUselessZeros(result);
                    divideResult.Remainder = RemoveUselessZeros(bufferDividend);
                    return divideResult;
                }

                BigRational dividentCoeff = bufferDividend.Last();

                BigRational resultCoeff = dividentCoeff/divisorMaxCoeff;


                result[resultXPower] = resultCoeff;

                BigRational[] buffSubtract = new BigRational[bufferDividend.Length].Select(h => new BigRational(0,1)).ToArray();

                Array.Copy(divisor, 0, buffSubtract, resultXPower, divisor.Length);

                Parallel.For(0, buffSubtract.Length, i => buffSubtract[i] *= resultCoeff);

                bufferDividend = ArraySubtrInts(bufferDividend, buffSubtract);

                bufferDividend = RemoveUselessZeros(bufferDividend);

            }
        }

        public static BigRational[] ArraySubtrInts(BigRational[] from, BigRational[] subAmount)
        {
            BigRational[] result = new BigRational[from.Length];

            for (int i = 0; i < from.Length - 1; i++)
            {
                result[i] = from[i] - subAmount[i];
            }

            return result;
        }

        public static BigRational[] RemoveUselessZeros(BigRational[] source)
        {
            int numOfLeadingZero = 0;


            Array.Reverse(source);


            foreach (var i in source)
            {
                if (i == 0 )
                {
                    numOfLeadingZero++;
                }
                else
                {
                    break;
                }
            }

            if (numOfLeadingZero == 0)
            {
                Array.Reverse(source);
                return source;
            }

            BigRational[] arrayBuffer = new BigRational[source.Length - numOfLeadingZero];

            Array.Copy(source, numOfLeadingZero, arrayBuffer, 0, arrayBuffer.Length);
            Array.Reverse(arrayBuffer);
            return arrayBuffer;
        }


    }
}
