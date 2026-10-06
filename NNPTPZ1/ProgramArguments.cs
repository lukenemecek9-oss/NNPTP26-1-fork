using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPTPZ1
{
    public class ProgramArguments
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }
        public string Output { get; set; }
        public double[] Coefficients { get; set; }

        private static bool TryParseArgsToInteger(string[] args, out int width, out int height)
        {
            width = 0; height = 0;

            if (!int.TryParse(args[0], out width))
            {
                return false;
            }

            if (!int.TryParse(args[1], out height))
            {
                return false;
            }

            return true;
        }

        private static bool TryParseArgsToDouble(string[] args, out double xmin, out double xmax, out double ymin, out double ymax)
        {
            xmin = 0; xmax = 0; ymin = 0; ymax = 0;

            if (!double.TryParse(args[2], out xmin))
            {
                return false;
            }

            if (!double.TryParse(args[3], out xmax))
            {
                return false;
            }

            if (!double.TryParse(args[4], out ymin))
            {
                return false;
            }

            if (!double.TryParse(args[5], out ymax))
            {
                return false;
            }

            return true;
        }

        private static bool TryParsePolynomialCoefficients(string[] args, out double[] coefficients)
        {
            coefficients = new double[args.Length - 7];

            for (int coefficientIndex = 0; coefficientIndex < coefficients.Length; coefficientIndex++)
            {
                if (!double.TryParse(
                        args[coefficientIndex + 7],
                        out coefficients[coefficientIndex]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool TryParseArguments(string[] args, out ProgramArguments programArguments)
        {
            programArguments = new ProgramArguments();

            if (args.Length < 8)
            {
                return false;
            }


            if (!TryParseArgsToInteger(args, out int width, out int height))
            {
                return false;
            }

            programArguments.Width = width;
            programArguments.Height = height;


            if (!TryParseArgsToDouble(args, out double xmin, out double xmax, out double ymin, out double ymax))
            {
                return false;
            }

            programArguments.XMin = xmin;
            programArguments.XMax = xmax;
            programArguments.YMin = ymin;
            programArguments.YMax = ymax;

            programArguments.Output = args[6];

            double[] coefficients;

            if (!TryParsePolynomialCoefficients(args, out coefficients))
            {
                return false;
            }

            programArguments.Coefficients = coefficients;

            return true;
        }

    }
}
