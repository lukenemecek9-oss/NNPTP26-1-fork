using System;
using System.Collections.Generic;
using System.Drawing;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1
{
    /// <summary>
    /// This program should produce Newton fractals.
    /// See more at: https://en.wikipedia.org/wiki/Newton_fractal
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ProgramArguments programArguments;

            if (!ProgramArguments.TryParseArguments(args, out programArguments))
            {
                ParseFailure();
                return;
            }

            using (FractalEnvironment environment = new FractalEnvironment(programArguments))
            {
                FractalRenderer renderer = new FractalRenderer();

                renderer.Render(environment, programArguments);

                environment.Bitmap.Save(programArguments.Output);
            }
        }

        private static void ParseFailure()
        {
            Console.WriteLine("Převod se nezdařil.");
        }  
    }
}
