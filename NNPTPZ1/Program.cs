using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Linq.Expressions;
using System.Threading;
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

            int width, height;
            if (!TryParseArgsToInteger(args, out width, out height))
            {
                ParseFailure();
                return;
            }

            double xmin, xmax, ymin, ymax;
            if (!TryParseArgsToDouble(args, out xmin, out xmax, out ymin, out ymax))
            {
                ParseFailure();
                return;
            }

            string output = args[6];

            Bitmap bitmap = new Bitmap(width, height);

            double xstep = (xmax - xmin) / width;
            double ystep = (ymax - ymin) / height;

            List<Cplx> koreny = new List<Cplx>();

            double coefficient1, coefficient2, coefficient3, coefficient4;
            if (!TryParsePolynomialCoefficients(args, out coefficient1, out coefficient2, out coefficient3, out coefficient4))
            {
                ParseFailure();
                return;
            }

            Poly polynomial = new Poly();
            PolynomialCoeficientAddition(coefficient1, coefficient2, coefficient3, coefficient4, polynomial);
            Poly polynomialDerivative = polynomial.Derive();

            Console.WriteLine(polynomial);
            Console.WriteLine(polynomialDerivative);

            var colorsPalette = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };

            var maxid = 0;

            // TODO: cleanup!!!
            // for every pixel in image...
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    // find "world" coordinates of pixel
                    double y = ymin + i * ystep;
                    double x = xmin + j * xstep;

                    Cplx ox = new Cplx()
                    {
                        Real = x,
                        Imaginary = (float)(y)
                    };

                    if (ox.Real == 0)
                        ox.Real = 0.0001;
                    if (ox.Imaginary == 0)
                        ox.Imaginary = 0.0001f;

                    //Console.WriteLine(ox);

                    // find solution of equation using newton's iteration
                    float it = 0;
                    for (int q = 0; q < 30; q++)
                    {
                        var diff = polynomial.Eval(ox).Divide(polynomialDerivative.Eval(ox));
                        ox = ox.Subtract(diff);

                        //Console.WriteLine($"{q} {ox} -({diff})");
                        if (Math.Pow(diff.Real, 2) + Math.Pow(diff.Imaginary, 2) >= 0.5)
                        {
                            q--;
                        }
                        it++;
                    }

                    //float newtonsIterations = FindSolutionUsingNewtonsIteration(polynomial, polynomialDerivative, ref ox);

                    //Console.ReadKey();

                    // find solution root number
                    var known = false;
                    var id = 0;
                    for (int w = 0; w < koreny.Count; w++)
                    {
                        if (Math.Pow(ox.Real - koreny[w].Real, 2) + Math.Pow(ox.Imaginary - koreny[w].Imaginary, 2) <= 0.01)
                        {
                            known = true;
                            id = w;
                        }
                    }
                    if (!known)
                    {
                        koreny.Add(ox);
                        id = koreny.Count;
                        maxid = id + 1;
                    }

                    //int rootNumber = FindRootNumber(koreny, ref maxid, ox);

                    // colorize pixel according to root number
                    //int vv = rootNumber;
                    //int vv = rootNumber * 50 + (int)newtonsIterations*5;
                    var vv = colorsPalette[id % colorsPalette.Length];
                    vv = Color.FromArgb(vv.R, vv.G, vv.B);
                    vv = Color.FromArgb(Math.Min(Math.Max(0, vv.R - (int)it * 2), 255), Math.Min(Math.Max(0, vv.G - (int)it * 2), 255), Math.Min(Math.Max(0, vv.B - (int)it * 2), 255));
                    //vv = Math.Min(Math.Max(0, vv), 255);
                    bitmap.SetPixel(j, i, vv);
                    //bmp.SetPixel(j, i, Color.FromArgb(vv, vv, vv));
                }
            }

            // TODO: delete I suppose...
            //for (int i = 0; i < 300; i++)
            //{
            //    for (int j = 0; j < 300; j++)
            //    {
            //        Color c = bitmap.GetPixel(j, i);
            //        int nv = (int)Math.Floor(c.R * (255.0 / maxid));
            //        bitmap.SetPixel(j, i, Color.FromArgb(nv, nv, nv));
            //    }
            //}

            bitmap.Save(output ?? "../../../out.png");
            //Console.ReadKey();
        }

        private static void PolynomialCoeficientAddition(double coefficient1, double coefficient2, double coefficient3, double coefficient4, Poly polynomial)
        {
            polynomial.Add(new Cplx() { Real = coefficient1 });
            polynomial.Add(new Cplx() { Real = coefficient2 });
            polynomial.Add(new Cplx() { Real = coefficient3 });
            polynomial.Add(new Cplx() { Real = coefficient4 });
        }

        private static bool TryParsePolynomialCoefficients(string[] args, out double coefficient1, out double coefficient2, out double coefficient3, out double coefficient4)
        {
            coefficient1 = 0; coefficient2 = 0; coefficient3 = 0; coefficient4 = 0;

            if (!double.TryParse(args[7], out coefficient1))
                return false;

            if (!double.TryParse(args[8], out coefficient2))
                return false;

            if (!double.TryParse(args[9], out coefficient3))
                return false;

            if (!double.TryParse(args[10], out coefficient4))
                return false;

            return true;
        }

        private static void ParseFailure()
        {
            Console.WriteLine("Převod se nezdařil.");
        }

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
    }

    namespace Mathematics
    {
        public class Poly
        {
            /// <summary>
            /// Coeficient
            /// </summary>
            public List<Cplx> Coeficient { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public Poly() => Coeficient = new List<Cplx>();

            public void Add(Cplx coe) =>
                Coeficient.Add(coe);

            /// <summary>
            /// Derives this polynomial and creates new one
            /// </summary>
            /// <returns>Derivated polynomial</returns>
            public Poly Derive()
            {
                Poly p = new Poly();
                for (int q = 1; q < Coeficient.Count; q++)
                {
                    p.Coeficient.Add(Coeficient[q].Multiply(new Cplx() { Real = q }));
                }

                return p;
            }

            /// <summary>
            /// Evaluates polynomial at given point
            /// </summary>
            /// <param name="x">point of evaluation</param>
            /// <returns>y</returns>
            public Cplx Eval(double x)
            {
                var y = Eval(new Cplx() { Real = x, Imaginary = 0 });
                return y;
            }

            /// <summary>
            /// Evaluates polynomial at given point
            /// </summary>
            /// <param name="x">point of evaluation</param>
            /// <returns>y</returns>
            public Cplx Eval(Cplx x)
            {
                Cplx s = Cplx.Zero;
                for (int i = 0; i < Coeficient.Count; i++)
                {
                    Cplx coef = Coeficient[i];
                    Cplx bx = x;
                    int power = i;

                    if (i > 0)
                    {
                        for (int j = 0; j < power - 1; j++)
                            bx = bx.Multiply(x);

                        coef = coef.Multiply(bx);
                    }

                    s = s.Add(coef);
                }

                return s;
            }

            /// <summary>
            /// ToString
            /// </summary>
            /// <returns>String repr of polynomial</returns>
            public override string ToString()
            {
                string s = "";
                int i = 0;
                for (; i < Coeficient.Count; i++)
                {
                    s += Coeficient[i];
                    if (i > 0)
                    {
                        int j = 0;
                        for (; j < i; j++)
                        {
                            s += "x";
                        }
                    }
                    if (i + 1 < Coeficient.Count)
                        s += " + ";
                }
                return s;
            }
        }

        public class Cplx
        {
            public double Real { get; set; }
            public float Imaginary { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is Cplx)
                {
                    Cplx x = obj as Cplx;
                    return x.Real == Real && x.Imaginary == Imaginary;
                }
                return base.Equals(obj);
            }

            public readonly static Cplx Zero = new Cplx()
            {
                Real = 0,
                Imaginary = 0
            };

            public Cplx Multiply(Cplx b)
            {
                Cplx a = this;
                // aRe*bRe + aRe*bIm*i + aIm*bRe*i + aIm*bIm*i*i
                return new Cplx()
                {
                    Real = a.Real * b.Real - a.Imaginary * b.Imaginary,
                    Imaginary = (float)(a.Real * b.Imaginary + a.Imaginary * b.Real)
                };
            }
            public double GetAbS()
            {
                return Math.Sqrt(Real * Real + Imaginary * Imaginary);
            }

            public Cplx Add(Cplx b)
            {
                Cplx a = this;
                return new Cplx()
                {
                    Real = a.Real + b.Real,
                    Imaginary = a.Imaginary + b.Imaginary
                };
            }
            public double GetAngleInDegrees()
            {
                return Math.Atan(Imaginary / Real);
            }
            public Cplx Subtract(Cplx b)
            {
                Cplx a = this;
                return new Cplx()
                {
                    Real = a.Real - b.Real,
                    Imaginary = a.Imaginary - b.Imaginary
                };
            }

            public override string ToString()
            {
                return $"({Real} + {Imaginary}i)";
            }

            internal Cplx Divide(Cplx b)
            {
                // (aRe + aIm*i) / (bRe + bIm*i)
                // ((aRe + aIm*i) * (bRe - bIm*i)) / ((bRe + bIm*i) * (bRe - bIm*i))
                //  bRe*bRe - bIm*bIm*i*i
                var tmp = this.Multiply(new Cplx() { Real = b.Real, Imaginary = -b.Imaginary });
                var tmp2 = b.Real * b.Real + b.Imaginary * b.Imaginary;

                return new Cplx()
                {
                    Real = tmp.Real / tmp2,
                    Imaginary = (float)(tmp.Imaginary / tmp2)
                };
            }
        }
    }
}
