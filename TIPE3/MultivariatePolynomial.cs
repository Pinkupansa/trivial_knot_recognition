using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPE3
{
    public class MultivariatePolynomial
    {
        //Ce dictionnaire représente les coefficients du polynôme. Si à la clé [1,2,2] on trouve la valeur 3, alors le polynôme contient le monôme 3*x*y^2*z^2. Si à la clé [2,3,6,4] on trouve la valeur i, le polynôme contient le monôme i*x^2*y^3*z^6*w^4.
        Dictionary<int[], Complex> coefficients;
        public MultivariatePolynomial(Dictionary<int[], Complex> coefficients)
        {
            this.coefficients = coefficients;
        }

        public static MultivariatePolynomial operator +(MultivariatePolynomial p1, MultivariatePolynomial p2) => Add(p1, p2);
        public static MultivariatePolynomial operator -(MultivariatePolynomial p1, MultivariatePolynomial p2) => Add(p1, -p2);
        public static MultivariatePolynomial operator *(MultivariatePolynomial p1, MultivariatePolynomial p2) => Multiply(p1, p2);
        public static MultivariatePolynomial operator *(Complex c, MultivariatePolynomial p) => Multiply(c, p);
        public static MultivariatePolynomial operator -(MultivariatePolynomial p) => (-1+0*Complex.j)*p;

        //Addition de deux polynomes
        static MultivariatePolynomial Add(MultivariatePolynomial p1, MultivariatePolynomial p2)
        {
            Dictionary<int[], Complex> sum = new Dictionary<int[], Complex>();
            foreach (KeyValuePair<int[], Complex> kvp in p1.coefficients)
            {
                sum.Add(kvp.Key, kvp.Value);
            }
            foreach (KeyValuePair<int[], Complex> kvp in p2.coefficients)
            {
                if (sum.ContainsKey(kvp.Key))
                {
                    sum[kvp.Key] = sum[kvp.Key] + kvp.Value;
                }
                else
                {
                    sum.Add(kvp.Key, kvp.Value);
                }
            }
            return new MultivariatePolynomial(sum);
        }
        static MultivariatePolynomial Multiply(Complex c, MultivariatePolynomial p)
        {
            Dictionary<int[], Complex> product = new Dictionary<int[], Complex>();
            foreach (KeyValuePair<int[], Complex> kvp in p.coefficients)
            {
                product.Add(kvp.Key, c*kvp.Value);
            }
            return new MultivariatePolynomial(product);
        }
        //Multiplication de deux polynômes
        static MultivariatePolynomial Multiply(MultivariatePolynomial p1, MultivariatePolynomial p2)
        {
            Dictionary<int[], Complex> product = new Dictionary<int[], Complex>();

            foreach (KeyValuePair<int[], Complex> kvp1 in p1.coefficients)
            {
                foreach (KeyValuePair<int[], Complex> kvp2 in p2.coefficients)
                {
                    int[] monomial1 = kvp1.Key;
                    int[] monomial2 = kvp2.Key;

                    int[] newMonomial = new int[Math.Max(monomial1.Length, monomial2.Length)];

                    for (int i = 0; i < newMonomial.Length; i++)
                    {
                        int sum = 0;
                        if (i < monomial1.Length)
                        {
                            sum += monomial1[i];
                        }
                        if (i < monomial2.Length)
                        {
                            sum += monomial2[i];
                        }
                        newMonomial[i] = sum;
                    }
                    if (product.ContainsKey(newMonomial))
                    {
                        product[newMonomial] += kvp1.Value * kvp2.Value;
                    }
                    else
                    {
                        product.Add(newMonomial, kvp1.Value * kvp2.Value);
                    }
                }
            }
            return new MultivariatePolynomial(product);

        }
        public static MultivariatePolynomial RealPart(MultivariatePolynomial p)
        {
            Dictionary<int[], Complex> re = new Dictionary<int[], Complex>();
            foreach (KeyValuePair<int[],Complex> kvp in p.coefficients)
            {
                re.Add(kvp.Key, kvp.Value.Re + 0 * Complex.j);
            }
            return new MultivariatePolynomial(re);
        }
        public static MultivariatePolynomial ImaginaryPart(MultivariatePolynomial p)
        {
            Dictionary<int[], Complex> im = new Dictionary<int[], Complex>();
            foreach (KeyValuePair<int[], Complex> kvp in p.coefficients)
            {
                im.Add(kvp.Key, kvp.Value.Im + 0 * Complex.j);
            }
            return new MultivariatePolynomial(im);
        }
        public string Name
        {
            get
            {
                string s = "";
                foreach (KeyValuePair<int[], Complex> kvp in coefficients)
                {
                    if (kvp.Value.Re != 0 || kvp.Value.Im != 0)
                    {
                        if (kvp.Value.Re != 0 && kvp.Value.Im != 0)
                        {
                            s += "(";
                        }

                        s += kvp.Value.Name;
                        if (kvp.Value.Re != 0 && kvp.Value.Im != 0)
                        {
                            s += ")";
                        }

                        for (int i = 0; i < kvp.Key.Length; i++)
                        {
                            if (kvp.Key[i] != 0)
                            {
                                if (kvp.Key[i] != 1)
                                {
                                    s += "x_" + i + "^" + kvp.Key[i];
                                }
                                else
                                {
                                    s += "x_" + i;
                                }
                            }
                        }
                        s += " + ";
                    }
                }

                return s;
            }
        }
        public static void Print(MultivariatePolynomial mvp)
        {
            Console.WriteLine(mvp.Name);
        }
    }
}
