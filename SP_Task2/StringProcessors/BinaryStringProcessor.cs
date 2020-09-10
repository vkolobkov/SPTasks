using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringProcessors
{
    public class BinaryStringProcessor
    {
        public bool IsGood(string binaryString)
        {
            if (binaryString == null)
                throw new ArgumentNullException("binaryString", "Binary string should be specified.");

            int goodness = 0;
            foreach(char c in binaryString)
            {
                if (c == '1')
                {
                    goodness++;
                }
                else if (c == '0')
                {
                    goodness--;
                }
                else
                {
                    //Console.WriteLine($"Expecting 0 and 1 characters only in string {binaryString}"); // commented out due to correct benchmark results
                    return false;
                }

                if(goodness < 0)
                {
                    //Console.WriteLine($"Bad prefix for string {binaryString}"); // commented out due to correct benchmark results
                    return false;
                }
            }
            if (goodness == 0)
                return true;
            else
                return false;
        }

        #region Benchmark methods

        // * Summary *

        //BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1016 (1909/November2018Update/19H2)
        //Intel Core i7-4770 CPU 3.40GHz(Haswell), 1 CPU, 8 logical and 4 physical cores
        // [Host]     : .NET Framework 4.8 (4.8.4200.0), X86 LegacyJIT
        //  DefaultJob : .NET Framework 4.8 (4.8.4200.0), X86 LegacyJIT


        //|                              Method |        Mean |     Error |    StdDev |
        //|------------------------------------ |------------:|----------:|----------:|
        //|                        ShortAndGood |    70.73 ns |  0.827 ns |  0.733 ns |
        //|                         ShortAndBad |    67.48 ns |  0.451 ns |  0.352 ns |
        //|                         HugeAndGood | 7,013.28 ns | 85.155 ns | 79.654 ns |
        //|                          HugeAndBad | 6,951.34 ns | 47.087 ns | 39.320 ns |
        //|          ShortAndGood_ForeachWithIf |    70.72 ns |  0.293 ns |  0.260 ns |
        //|           ShortAndBad_ForeachWithIf |    67.80 ns |  0.473 ns |  0.419 ns |
        //|           HugeAndGood_ForeachWithIf | 7,015.02 ns | 86.907 ns | 81.293 ns |
        //|            HugeAndBad_ForeachWithIf | 6,945.08 ns | 23.928 ns | 18.681 ns |
        //|        ShortAndGood_ForeachWithCase |    70.42 ns |  0.590 ns |  0.461 ns |
        //|         ShortAndBad_ForeachWithCase |    67.53 ns |  0.341 ns |  0.266 ns |
        //|         HugeAndGood_ForeachWithCase | 7,056.15 ns | 70.962 ns | 66.378 ns |
        //|          HugeAndBad_ForeachWithCase | 6,944.51 ns | 50.103 ns | 39.117 ns |
        //| ShortAndGood_ForeachWithCaseAndCode |    70.50 ns |  1.120 ns |  0.935 ns |
        //|  ShortAndBad_ForeachWithCaseAndCode |    68.52 ns |  0.911 ns |  0.852 ns |
        //|  HugeAndGood_ForeachWithCaseAndCode | 7,013.25 ns | 66.084 ns | 61.815 ns |
        //|   HugeAndBad_ForeachWithCaseAndCode | 6,942.72 ns | 55.088 ns | 46.001 ns |
        //|              ShortAndGood_ForWithIf |    70.49 ns |  0.538 ns |  0.449 ns |
        //|               ShortAndBad_ForWithIf |    67.99 ns |  0.722 ns |  0.640 ns |
        //|               HugeAndGood_ForWithIf | 7,020.92 ns | 82.056 ns | 76.755 ns |
        //|                HugeAndBad_ForWithIf | 6,988.59 ns | 50.852 ns | 47.567 ns |
        //|            ShortAndGood_ForWithCase |    70.58 ns |  0.613 ns |  0.543 ns |
        //|             ShortAndBad_ForWithCase |    67.36 ns |  0.434 ns |  0.339 ns |
        //|             HugeAndGood_ForWithCase | 6,956.30 ns | 71.393 ns | 59.616 ns |
        //|              HugeAndBad_ForWithCase | 6,955.03 ns | 47.985 ns | 40.070 ns |
        //|     ShortAndGood_ForWithCaseAndCode |    70.54 ns |  0.361 ns |  0.320 ns |
        //|      ShortAndBad_ForWithCaseAndCode |    67.41 ns |  0.531 ns |  0.471 ns |
        //|      HugeAndGood_ForWithCaseAndCode | 7,000.77 ns | 48.682 ns | 43.155 ns |
        //|       HugeAndBad_ForWithCaseAndCode | 6,930.29 ns | 35.334 ns | 27.587 ns |

        /*
        public bool IsGood_ForeachWithIf(string binaryString)
        {
            int goodness = 0;
            foreach (char c in binaryString)
            {
                if (c == '1')
                {
                    goodness++;
                }
                else if (c == '0')
                {
                    goodness--;
                }
                else
                {
                    //Console.WriteLine($"Expecting 0 and 1 characters only in string {binaryString}");
                    return false;
                }

                if (goodness < 0)
                {
                    //Console.WriteLine($"Bad prefix for string {binaryString}");
                    return false;
                }
            }
            if (goodness == 0)
                return true;
            else
                return false;
        }

        public bool IsGood_ForeachWithCase(string binaryString)
        {
            int goodness = 0;
            foreach (char c in binaryString)
            {
                switch (c)
                {
                    case '1':
                        goodness++;
                        break;
                    case '0':
                        goodness--;
                        break;
                    default:
                        //Console.WriteLine($"Expecting 0 and 1 characters only in string {binaryString}");
                        return false;
                        break;
                }

                if (goodness < 0)
                {
                    //Console.WriteLine($"Bad prefix for string {binaryString}");
                    return false;
                }
            }
            if (goodness == 0)
                return true;
            else
                return false;
        }

        public bool IsGood_ForeachWithCaseAndCode(string binaryString)
        {
            int goodness = 0;
            foreach (char c in binaryString)
            {
                switch ((int)c)
                {
                    case 49:
                        goodness++;
                        break;
                    case 48:
                        goodness--;
                        break;
                    default:
                        //Console.WriteLine($"Expecting 0 and 1 characters only in string {binaryString}");
                        return false;
                        break;
                }

                if (goodness < 0)
                {
                    //Console.WriteLine($"Bad prefix for string {binaryString}");
                    return false;
                }
            }
            if (goodness == 0)
                return true;
            else
                return false;
        }

        public bool IsGood_ForWithIf(string binaryString)
        {
            int goodness = 0;
            int stringLength = binaryString.Length;
            for (int charCounter = 0; charCounter < stringLength; charCounter++)
            {
                if (binaryString[charCounter] == '1')
                {
                    goodness++;
                }
                else if (binaryString[charCounter] == '0')
                {
                    goodness--;
                }
                else
                {
                    //Console.WriteLine($"Expecting 0 and 1 characters only in string {binaryString}");
                    return false;
                }

                if (goodness < 0)
                {
                    //Console.WriteLine($"Bad prefix for string {binaryString}");
                    return false;
                }
            }
            if (goodness == 0)
                return true;
            else
                return false;
        }

        public bool IsGood_ForWithCase(string binaryString)
        {
            int goodness = 0;
            int stringLength = binaryString.Length;
            for (int charCounter = 0; charCounter < stringLength; charCounter++)
            {
                switch (binaryString[charCounter])
                {
                    case '1':
                        goodness++;
                        break;
                    case '0':
                        goodness--;
                        break;
                    default:
                        //Console.WriteLine($"Expecting 0 and 1 characters only in string {binaryString}");
                        return false;
                        break;
                }

                if (goodness < 0)
                {
                    //Console.WriteLine($"Bad prefix for string {binaryString}");
                    return false;
                }
            }
            if (goodness == 0)
                return true;
            else
                return false;
        }

        public bool IsGood_ForWithCaseAndCode(string binaryString)
        {
            int goodness = 0;
            int stringLength = binaryString.Length;
            for (int charCounter = 0; charCounter < stringLength; charCounter++)
            {
                switch ((int)binaryString[charCounter])
                {
                    case 49:
                        goodness++;
                        break;
                    case 48:
                        goodness--;
                        break;
                    default:
                        //Console.WriteLine($"Expecting 0 and 1 characters only in string {binaryString}");
                        return false;
                        break;
                }

                if (goodness < 0)
                {
                    //Console.WriteLine($"Bad prefix for string {binaryString}");
                    return false;
                }
            }
            if (goodness == 0)
                return true;
            else
                return false;
        }
        */

        #endregion

    }
}
