using NNPTPZ1.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NNPTPZ1
{
    public class FractalEnvironment : IDisposable
    {
        public Bitmap Bitmap { get; set; }
        public Polynomial Polynomial { get; set; }
        public Polynomial PolynomialDerivative { get; set; }
        public List<ComplexNumber> Roots { get; set; }
        public Color[] ColorPalette { get; set; }
        public double XStep { get; set; }
        public double YStep { get; set; }

        public FractalEnvironment(ProgramArguments arguments)
        {
            Bitmap = new Bitmap(arguments.Width, arguments.Height);

            CalculateSteps(arguments.Width, arguments.Height, arguments.XMin, arguments.XMax, arguments.YMin, arguments.YMax, out double xStep, out double yStep);

            XStep = xStep;
            YStep = yStep;

            Roots = new List<ComplexNumber>();

            Polynomial = CreatePolynomial(arguments.Coefficients);
            PolynomialDerivative = Polynomial.Derive();

            ColorPalette = CreateColorPalette();
        }

        private static Polynomial CreatePolynomial(double[] coefficients)
        {
            Polynomial polynomial = new Polynomial();

            for (int index = 0; index < coefficients.Length; index++)
            {
                polynomial.Add(new ComplexNumber
                {
                    Real = coefficients[index]
                });
            }

            return polynomial;
        }

        private static Color[] CreateColorPalette()
        {
            return new Color[]
            {
                Color.Red,
                Color.Blue,
                Color.Green,
                Color.Yellow,
                Color.Orange,
                Color.Fuchsia,
                Color.Gold,
                Color.Cyan,
                Color.Magenta
            };
        }

        private static void CalculateSteps(int width, int height, double xmin, double xmax, double ymin, double ymax, out double xstep, out double ystep)
        {
            xstep = (xmax - xmin) / width;
            ystep = (ymax - ymin) / height;
        }

        public void Dispose()
        {
            Bitmap.Dispose();
        }
    }
}
