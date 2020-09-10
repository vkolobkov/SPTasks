﻿using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringProcessors.BM
{
    public class BinaryStringProcessorBMs
    {
        private BinaryStringProcessor binaryStrProcessor = new BinaryStringProcessor();
        private const string binaryString_ShortAndGood = "1010101110010010110111001011001000";
        private const string binaryString_ShortAndBad = "10101011100100101101110010110010001";
        private const string binaryString_HugeAndGood = "1010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000";
        private const string binaryString_HugeAndBad = "1010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "10101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000"
            + "1010101110010010110111001011001000101010111001001011011100101100100010101011100100101101110010110010001010101110010010110111001011001000101010111001001011011100101100000";
        public BinaryStringProcessorBMs()
        { }

        [Benchmark]
        public bool ShortAndGood() => binaryStrProcessor.IsGood(binaryString_ShortAndGood);
        [Benchmark]
        public bool ShortAndBad() => binaryStrProcessor.IsGood(binaryString_ShortAndBad);
        [Benchmark]
        public bool HugeAndGood() => binaryStrProcessor.IsGood(binaryString_HugeAndGood);
        [Benchmark]
        public bool HugeAndBad() => binaryStrProcessor.IsGood(binaryString_HugeAndBad);


        //[Benchmark]
        //public bool ShortAndGood_ForeachWithIf() => binaryStrProcessor.IsGood_ForeachWithIf(binaryString_ShortAndGood);
        //[Benchmark]
        //public bool ShortAndBad_ForeachWithIf() => binaryStrProcessor.IsGood_ForeachWithIf(binaryString_ShortAndBad);
        //[Benchmark]
        //public bool HugeAndGood_ForeachWithIf() => binaryStrProcessor.IsGood_ForeachWithIf(binaryString_HugeAndGood);
        //[Benchmark]
        //public bool HugeAndBad_ForeachWithIf() => binaryStrProcessor.IsGood_ForeachWithIf(binaryString_HugeAndBad);


        //[Benchmark]
        //public bool ShortAndGood_ForeachWithCase() => binaryStrProcessor.IsGood_ForeachWithCase(binaryString_ShortAndGood);
        //[Benchmark]
        //public bool ShortAndBad_ForeachWithCase() => binaryStrProcessor.IsGood_ForeachWithCase(binaryString_ShortAndBad);
        //[Benchmark]
        //public bool HugeAndGood_ForeachWithCase() => binaryStrProcessor.IsGood_ForeachWithCase(binaryString_HugeAndGood);
        //[Benchmark]
        //public bool HugeAndBad_ForeachWithCase() => binaryStrProcessor.IsGood_ForeachWithCase(binaryString_HugeAndBad);


        //[Benchmark]
        //public bool ShortAndGood_ForeachWithCaseAndCode() => binaryStrProcessor.IsGood_ForeachWithCaseAndCode(binaryString_ShortAndGood);
        //[Benchmark]
        //public bool ShortAndBad_ForeachWithCaseAndCode() => binaryStrProcessor.IsGood_ForeachWithCaseAndCode(binaryString_ShortAndBad);
        //[Benchmark]
        //public bool HugeAndGood_ForeachWithCaseAndCode() => binaryStrProcessor.IsGood_ForeachWithCaseAndCode(binaryString_HugeAndGood);
        //[Benchmark]
        //public bool HugeAndBad_ForeachWithCaseAndCode() => binaryStrProcessor.IsGood_ForeachWithCaseAndCode(binaryString_HugeAndBad);


        //[Benchmark]
        //public bool ShortAndGood_ForWithIf() => binaryStrProcessor.IsGood_ForWithIf(binaryString_ShortAndGood);
        //[Benchmark]
        //public bool ShortAndBad_ForWithIf() => binaryStrProcessor.IsGood_ForWithIf(binaryString_ShortAndBad);
        //[Benchmark]
        //public bool HugeAndGood_ForWithIf() => binaryStrProcessor.IsGood_ForWithIf(binaryString_HugeAndGood);
        //[Benchmark]
        //public bool HugeAndBad_ForWithIf() => binaryStrProcessor.IsGood_ForWithIf(binaryString_HugeAndBad);


        //[Benchmark]
        //public bool ShortAndGood_ForWithCase() => binaryStrProcessor.IsGood_ForWithCase(binaryString_ShortAndGood);
        //[Benchmark]
        //public bool ShortAndBad_ForWithCase() => binaryStrProcessor.IsGood_ForWithCase(binaryString_ShortAndBad);
        //[Benchmark]
        //public bool HugeAndGood_ForWithCase() => binaryStrProcessor.IsGood_ForWithCase(binaryString_HugeAndGood);
        //[Benchmark]
        //public bool HugeAndBad_ForWithCase() => binaryStrProcessor.IsGood_ForWithCase(binaryString_HugeAndBad);


        //[Benchmark]
        //public bool ShortAndGood_ForWithCaseAndCode() => binaryStrProcessor.IsGood_ForWithCaseAndCode(binaryString_ShortAndGood);
        //[Benchmark]
        //public bool ShortAndBad_ForWithCaseAndCode() => binaryStrProcessor.IsGood_ForWithCaseAndCode(binaryString_ShortAndBad);
        //[Benchmark]
        //public bool HugeAndGood_ForWithCaseAndCode() => binaryStrProcessor.IsGood_ForWithCaseAndCode(binaryString_HugeAndGood);
        //[Benchmark]
        //public bool HugeAndBad_ForWithCaseAndCode() => binaryStrProcessor.IsGood_ForWithCaseAndCode(binaryString_HugeAndBad);


    }
}