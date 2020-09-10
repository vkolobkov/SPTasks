using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringProcessors.BM
{
    class Program
    {
        static void Main(string[] args)
        {
            Summary[] summary = BenchmarkRunner.Run(typeof(Program).Assembly);
            Console.ReadLine();
        }
    }
}
