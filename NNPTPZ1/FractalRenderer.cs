using System;
using System.Drawing;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1
{
    public class FractalRenderer
    {
        public void Render(FractalEnvironment environment, ProgramArguments arguments)
        {
            for (int pixelY = 0; pixelY < arguments.Height; pixelY++)
            {
                for (int pixelX = 0; pixelX < arguments.Width; pixelX++)
                {
                    ProcessPixel(environment, arguments, pixelX, pixelY);
                }
            }
        }

        private void ProcessPixel(FractalEnvironment environment, ProgramArguments arguments, int pixelX, int pixelY)
        {
            ComplexNumber complexNumber = CreateComplexNumber(environment, arguments, pixelX, pixelY);

            CheckForMinimalValue(complexNumber);

            float iterationCount = FindSolutionUsingNewtonsIteration(environment.Polynomial, environment.PolynomialDerivative, ref complexNumber);

            int rootNumber = FindSolutionUsingRootNumber(environment.Roots, complexNumber);

            ColorizePixelAccordingToRootNumber(environment.Bitmap, environment.ColorPalette, pixelX, pixelY, iterationCount, rootNumber);
        }

        private ComplexNumber CreateComplexNumber(FractalEnvironment environment, ProgramArguments arguments, int pixelX, int pixelY)
        {
            double y = arguments.YMin + pixelY * environment.YStep;
            double x = arguments.XMin + pixelX * environment.XStep;

            return new ComplexNumber
            {
                Real = x,
                Imaginary = (float)y
            };
        }

        private void CheckForMinimalValue( ComplexNumber complexNumber)
        {
            const float minimalValueFloat = 0.0001f;
            const double minimalValueDouble = 0.0001;

            if (complexNumber.Real == 0)
            {
                complexNumber.Real = minimalValueDouble;
            }

            if (complexNumber.Imaginary == 0)
            {
                complexNumber.Imaginary = minimalValueFloat;
            }
        }

        private int FindSolutionUsingRootNumber( System.Collections.Generic.List<ComplexNumber> roots, ComplexNumber currentPoint)
        {
            bool isKnown = false;
            int rootNumber = 0;

            for (int rootIndex = 0; rootIndex < roots.Count; rootIndex++)
            {
                if (Math.Pow(
                        currentPoint.Real - roots[rootIndex].Real, 2)
                    + Math.Pow(
                        currentPoint.Imaginary - roots[rootIndex].Imaginary, 2)
                    <= 0.01)
                {
                    isKnown = true;
                    rootNumber = rootIndex;
                }
            }

            if (!isKnown)
            {
                roots.Add(currentPoint);
                rootNumber = roots.Count;
            }

            return rootNumber;
        }

        private float FindSolutionUsingNewtonsIteration( Polynomial polynomial, Polynomial polynomialDerivative, ref ComplexNumber currentPoint)
        {
            float iterationCount = 0;

            for (int iteration = 0; iteration < 30; iteration++)
            {
                ComplexNumber newtonStep = polynomial.Evaluation(currentPoint).Divide(polynomialDerivative.Evaluation(currentPoint));

                currentPoint = currentPoint.Subtract(newtonStep);

                if (Math.Pow(newtonStep.Real, 2)
                    + Math.Pow(newtonStep.Imaginary, 2) >= 0.5)
                {
                    iteration--;
                }

                iterationCount++;
            }

            return iterationCount;
        }

        private void ColorizePixelAccordingToRootNumber(Bitmap bitmap, Color[] colorPalette, int pixelX, int pixelY, float iterationCount, int rootNumber)
        {
            Color color =
                colorPalette[rootNumber % colorPalette.Length];

            color = Color.FromArgb(
                Math.Min(Math.Max(0, color.R - (int)iterationCount * 2), 255),
                Math.Min(Math.Max(0, color.G - (int)iterationCount * 2), 255),
                Math.Min(Math.Max(0, color.B - (int)iterationCount * 2), 255));

            bitmap.SetPixel(pixelY, pixelX, color);
        }
    }
}