﻿using System;
using System.Threading;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true) {
                Console.WriteLine("Hello World!");

                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }
    }
}