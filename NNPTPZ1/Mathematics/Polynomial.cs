using System.Collections.Generic;

namespace NNPTPZ1
{

    namespace Mathematics
    {
        public class Polynomial
        {
            /// <summary>
            /// Coeficient
            /// </summary>
            public List<ComplexNumber> Coeficient { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public Polynomial() => Coeficient = new List<ComplexNumber>();

            public void Add(ComplexNumber coeficient) =>
                Coeficient.Add(coeficient);

            /// <summary>
            /// Derives this polynomial and creates new one
            /// </summary>
            /// <returns>Derivated polynomial</returns>
            public Polynomial Derive()
            {
                Polynomial polynomial = new Polynomial();
                for (int index = 1; index < Coeficient.Count; index++)
                {
                    polynomial.Coeficient.Add(Coeficient[index].Multiply(new ComplexNumber() { Real = index }));
                }

                return polynomial;
            }

            /// <summary>
            /// Evaluates polynomial at given point
            /// </summary>
            /// <param name="pointOfEvaluation">point of evaluation</param>
            /// <returns>result</returns>
            public ComplexNumber Eval(double pointOfEvaluation)
            {
                var result = Evaluation(new ComplexNumber() { Real = pointOfEvaluation, Imaginary = 0 });
                return result;
            }

            /// <summary>
            /// Evaluates polynomial at given point
            /// </summary>
            /// <param name="pointOfEvaluation">point of evaluation</param>
            /// <returns>result</returns>
            public ComplexNumber Evaluation(ComplexNumber pointOfEvaluation)
            {
                ComplexNumber sum = ComplexNumber.Zero;
                for (int i = 0; i < Coeficient.Count; i++)
                {
                    ComplexNumber coeficient = Coeficient[i];
                    ComplexNumber pointPower = pointOfEvaluation;
                    int power = i;

                    if (i > 0)
                    {
                        for (int j = 0; j < power - 1; j++)
                            pointPower = pointPower.Multiply(pointOfEvaluation);

                        coeficient = coeficient.Multiply(pointPower);
                    }

                    sum = sum.Add(coeficient);
                }

                return sum;
            }

            /// <summary>
            /// ToString
            /// </summary>
            /// <returns>String repr of polynomial</returns>
            public override string ToString()
            {
                string str = "";
                int i = 0;
                for (; i < Coeficient.Count; i++)
                {
                    str += Coeficient[i];
                    if (i > 0)
                    {
                        int j = 0;
                        for (; j < i; j++)
                        {
                            str += "x";
                        }
                    }
                    if (i + 1 < Coeficient.Count)
                        str += " + ";
                }
                return str;
            }
        }
    }
}
