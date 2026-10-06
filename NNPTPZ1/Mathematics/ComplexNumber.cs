using System;

namespace NNPTPZ1
{

    namespace Mathematics
    {
        public class ComplexNumber
        {
            public double Real { get; set; }
            public double Imaginary { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is ComplexNumber)
                {
                    ComplexNumber other = obj as ComplexNumber;
                    return other.Real == Real && other.Imaginary == Imaginary;
                }
                return base.Equals(obj);
            }

            public readonly static ComplexNumber Zero = new ComplexNumber()
            {
                Real = 0,
                Imaginary = 0
            };

            public ComplexNumber Multiply(ComplexNumber complexNumber)
            {
                ComplexNumber @this = this;
                return new ComplexNumber()
                {
                    Real = @this.Real * complexNumber.Real - @this.Imaginary * complexNumber.Imaginary,
                    Imaginary = (float)(@this.Real * complexNumber.Imaginary + @this.Imaginary * complexNumber.Real)
                };
            }

            public double GetAbsolute()
            {
                return Math.Sqrt(Real * Real + Imaginary * Imaginary);
            }

            public ComplexNumber Add(ComplexNumber complexNumber)
            {
                ComplexNumber a = this;
                return new ComplexNumber()
                {
                    Real = a.Real + complexNumber.Real,
                    Imaginary = a.Imaginary + complexNumber.Imaginary
                };
            }
            public double GetAngleInRadians()
            {
                return Math.Atan(Imaginary / Real);
            }

            public ComplexNumber Subtract(ComplexNumber complexNumber)
            {
                ComplexNumber a = this;
                return new ComplexNumber()
                {
                    Real = a.Real - complexNumber.Real,
                    Imaginary = a.Imaginary - complexNumber.Imaginary
                };
            }

            public override string ToString()
            {
                return $"({Real} + {Imaginary}i)";
            }

            public ComplexNumber Divide(ComplexNumber complexNumber)
            {
                var numerator = this.Multiply(new ComplexNumber() { Real = complexNumber.Real, Imaginary = -complexNumber.Imaginary });
                var denominator = complexNumber.Real * complexNumber.Real + complexNumber.Imaginary * complexNumber.Imaginary;

                return new ComplexNumber()
                {
                    Real = numerator.Real / denominator,
                    Imaginary = (float)(numerator.Imaginary / denominator)
                };
            }
        }
    }
}
