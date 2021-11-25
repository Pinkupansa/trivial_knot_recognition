using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPE3
{
    //Définition d'un nombre complexe et des opérations usuelles sur les nombres complexes (certaines opérations sont superflues car le code vient d'un ancien projet)
    public class Complex
    {
        private double re;
        private double im;
        public static Complex j = new Complex(0, 1);
        public static Complex ForbiddenValue = null;
        public double Re
        {
            get
            {
                return re;
            }
            set
            {
                re = value;
            }
        }
        public double Im
        {
            get
            {
                return im;
            }
            set
            {
                im = value;
            }
        }


        public double Module { get { return Math.Sqrt(re * re + im * im); } }
        public double Arg
        {
            get
            {

                if (re > 0)
                {

                    return Math.Atan(im / re);
                }
                else
                {
                    if (im > 0)
                    {

                        return Math.Atan(im / re) + Math.PI;
                    }
                    else
                    {

                        return Math.Atan(im / re) - Math.PI;
                    }
                }
            }
        }
        public Complex(double _re, double _im)
        {
            re = _re;
            im = _im;
        }

        public static Complex operator -(Complex c) => (c == ForbiddenValue) ? ForbiddenValue : new Complex(-c.Re, -c.Im);

        public static Complex operator +(Complex c1, Complex c2) => Add(c1, c2);
        public static Complex operator +(double real, Complex c) => new Complex(real, 0) + c;
        public static Complex operator +(Complex c, double real) => new Complex(real, 0) + c;

        public static Complex operator -(Complex c1, Complex c2) => Add(c1, -c2);
        public static Complex operator -(double real, Complex c) => new Complex(real, 0) - c;
        public static Complex operator -(Complex c, double real) => c - new Complex(real, 0);

        public static Complex operator *(Complex c1, Complex c2) => Multiply(c1, c2);
        public static Complex operator *(double real, Complex c) => new Complex(real, 0) * c;
        public static Complex operator *(Complex c, double real) => c * new Complex(real, 0);

        public static Complex operator /(Complex c1, Complex c2) => Divide(c1, c2);
        public static Complex operator /(double real, Complex c) => new Complex(real, 0) / c;
        public static Complex operator /(Complex c, double real) => c / new Complex(real, 0);

        static Complex Add(Complex c1, Complex c2)
        {
            if (c1 == ForbiddenValue || c2 == ForbiddenValue)
            {
                return ForbiddenValue;
            }
            return new Complex(c1.Re + c2.re, c1.Im + c2.im);
        }
        static Complex Multiply(Complex c1, Complex c2)
        {
            if (c1 == ForbiddenValue || c2 == ForbiddenValue)
            {
                return ForbiddenValue;
            }
            return new Complex(c1.Re * c2.Re - c1.Im * c2.Im, c1.Im * c2.Re + c1.Re * c2.Im);
        }
        static Complex Invert(Complex c)
        {
            if (c == ForbiddenValue)
            {
                return 0 + 0 * j;
            }
            double squaredModule = c.Module * c.Module;

            return new Complex(c.Re / squaredModule, -c.Im / squaredModule);
        }
        static Complex Divide(Complex c1, Complex c2)
        {
            if (c1 == ForbiddenValue || c2 == ForbiddenValue)
            {
                return 0 + 0 * j;
            }
            if (c2.Module == 0)
            {
                return ForbiddenValue;
            }
            return Multiply(c1, Invert(c2));
        }
        public static Complex Conjugate(Complex c)
        {
            if (c == ForbiddenValue)
            {
                return ForbiddenValue;
            }
            return new Complex(c.Re, -c.Im);
        }
        public static Complex Exp(Complex c)
        {
            if (c == ForbiddenValue)
            {
                return ForbiddenValue;
            }
            return Math.Exp(c.Re) * (Math.Cos(c.Im) + j * Math.Sin(c.Im));
        }
        public static Complex Cos(Complex z)
        {
            if (z == ForbiddenValue)
            {
                return ForbiddenValue;
            }
            return (Exp(j * z) + Exp(-j * z)) / 2;
        }
        public static Complex Sin(Complex z)
        {
            if (z == ForbiddenValue)
            {
                return ForbiddenValue;
            }
            return (Exp(j * z) - Exp(-j * z)) / (2 * j);
        }
        public static Complex Pow(Complex c1, Complex c2)
        {
            if (c1 == ForbiddenValue || c2 == ForbiddenValue)
            {
                return ForbiddenValue;
            }
            double rho1 = c1.Module;
            double rho2 = c2.Module;
            double theta1 = c1.Arg;
            double theta2 = c2.Arg;
            double rho = Math.Pow(rho1, rho2 * Math.Cos(theta2)) * Math.Exp(-theta1 * rho2 * Math.Sin(theta2));
            double theta = (Math.Cos(theta2) * theta1 + Math.Log(rho1) * Math.Sin(theta2)) * rho2;
            return new Complex(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
        public static double Distance(Complex c1, Complex c2)
        {
            if (c1 == ForbiddenValue || c2 == ForbiddenValue)
            {
                return 1f / 0f;
            }
            return (c1 - c2).Module;
        }
        public string Name
        {
            get
            {
                if (re == 0 && im == 0)
                {
                    return "0";
                }
                else if (re == 0)
                {
                    return im + "i";
                }
                else if (im == 0)
                {
                    return re.ToString();
                }
                return re + " + " + im + "i";
            }
        }
        public static void Print(Complex c)
        {
            Console.WriteLine(c.Name);
        }
    }
}
