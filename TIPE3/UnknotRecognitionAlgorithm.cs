using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPE3
{
    public static class UnknotRecognitionAlgorithm
    {
        public static void CheckKnot(KnotGroupPresentation KGP)
        {


            //On se demande ici si une représentation du groupe de noeud dans SL2(C) non commutative, est possible, et alors on saura que le noeud est bien noué
            int n = KGP.Generators.Length;
            MultivariatePolynomialMatrix[] m = new MultivariatePolynomialMatrix[n]; //Contient les images des générateurs du groupe de noeud par la représentation du groupe de noeud dans SL2(C). 
            
            for (int k = 0; k < n; k++)
            {
                m[k] = DefineMk(k, n);//image du k_ieme générateur 
            }
            List<MultivariatePolynomial> P = new List<MultivariatePolynomial>();//Contient tous les polynômes qui doivent pouvoir être annulés pour que la représentation du groupe de noeud dans SL2(C) soit bien définie
            for (int k = 0; k < n; k++)
            {
                MultivariatePolynomialMatrix e = null;
                MultivariatePolynomialMatrix m_k = m[KGP.Relations[k].generators[1]]; //Correspond dans les faits à m[k] (image du générateur associé au brin entrant)
                MultivariatePolynomialMatrix m_k1 = m[KGP.Relations[k].generators[3]]; //Correpond en général à m[k+1] (sauf quand k = n-1) (image du générateur associé au brin sortant)
                MultivariatePolynomialMatrix m_j = m[KGP.Relations[k].generators[0]]; //Image du générateur associé au brin passant;
                if (KGP.Relations[k].invert[0] == false) //relation de type g_j^-1*g_k*g_j*g_(k+1)^-1
                {
                    e = m_k1 * m_j - m_j * m_k;
                }
                else //relation de type g_j^-1*g_k*g_j*g(k+1)^-1
                {
                    e = m_k * m_j - m_j * m_k1;
                }
                P.Add(MultivariatePolynomial.RealPart(e[0, 0]));
                P.Add(MultivariatePolynomial.ImaginaryPart(e[0, 0]));
                P.Add(MultivariatePolynomial.RealPart(e[1, 0]));
                P.Add(MultivariatePolynomial.ImaginaryPart(e[1, 0])); //On veut que e soit nulle (pour que la représentation du groupe de noeud dans SL2(C) soit bien définie, c'est à dire que les m_k vérifient les mêmes relations que les g_k), et au vu de la manière dont est définie e il suffit de vérifier que e11 et e12 sont nuls
                P.Add(DeterminantMinusOne(k, n)); //det(m_k) - 1 doit être nul pour que m_k soit dans SL2(C)
            }
            //On va maintenant constituer un système d'une équation et une inéquation polynomiale qui possède une solution si et seulement si le noeud est noué

            //Les polynômes de P sont simulatanément annulables si et seulement si la somme de leurs carrés est annulable
            MultivariatePolynomial SquareSum = P[0]*P[0];
            for (int i = 1; i < P.Count; i++)
            {
                SquareSum = SquareSum + P[i] * P[i];
            }

            //Puisque toutes les relations entre les générateurs sont des relations de conjugaison, pour que la représentation soit non commutative, il faut et il suffit qu'un des g_k aient une image différente de celle de g_0
            //L'une des expressions a_k - a_0,...,d_k-d_0 doit donc pouvoir ne pas être nulle. Ce qui est possible si est seulement si la somme des carrés de toutes les expressions de ce type peut ne pas être nulle.

            MultivariatePolynomial a_0 = A_kMonomial(0, n);
            MultivariatePolynomial b_0 = B_kMonomial(0, n);
            MultivariatePolynomial c_0 = C_kMonomial(0, n);
            MultivariatePolynomial d_0 = D_kMonomial(0, n);
            MultivariatePolynomial SquareSum2 = a_0 - a_0; //on part en fait du polynôme nul
            for (int k = 1; k < n; k++)
            {
                MultivariatePolynomial a_k = A_kMonomial(k, n);
                MultivariatePolynomial b_k = B_kMonomial(k, n);
                MultivariatePolynomial c_k = C_kMonomial(k, n);
                MultivariatePolynomial d_k = D_kMonomial(k, n);
                SquareSum2 = SquareSum2 + (a_k - a_0) * (a_k - a_0) + (b_k - b_0) * (b_k - b_0) + (c_k - c_0) * (c_k - c_0) + (d_k - d_0) * (d_k - d_0);
            }
            MultivariatePolynomial.Print(SquareSum);
            MultivariatePolynomial.Print(SquareSum2);
        }

       
        
        public static MultivariatePolynomial A_kMonomial(int k, int n)
        {
            int[] a_kMonomial = new int[4 * n];
            a_kMonomial[4 * k] = 1;
            Dictionary<int[], Complex> a_kDict = new Dictionary<int[], Complex>();
            a_kDict.Add(a_kMonomial, 1 + 0 * Complex.j);
            return new MultivariatePolynomial(a_kDict);
        }
        public static  MultivariatePolynomial B_kMonomial(int k, int n)
        {
            int[] b_kMonomial = new int[4 * n];
            b_kMonomial[4 * k+1] = 1;
            Dictionary<int[], Complex> b_kDict = new Dictionary<int[], Complex>();
            b_kDict.Add(b_kMonomial, 1 + 0 * Complex.j);
            return new MultivariatePolynomial(b_kDict);
        }
        public static MultivariatePolynomial C_kMonomial(int k, int n)
        {
            int[] c_kMonomial = new int[4 * n];
            c_kMonomial[4 * k+2] = 1;
            Dictionary<int[], Complex> c_kDict = new Dictionary<int[], Complex>();
            c_kDict.Add(c_kMonomial, 1 + 0 * Complex.j);
            return new MultivariatePolynomial(c_kDict);
        }
        public static MultivariatePolynomial D_kMonomial(int k, int n)
        {
            int[] d_kMonomial = new int[4 * n];
            d_kMonomial[4 * k+3] = 1;
            Dictionary<int[], Complex> d_kDict = new Dictionary<int[], Complex>();
            d_kDict.Add(d_kMonomial, 1 + 0 * Complex.j);
            return new MultivariatePolynomial(d_kDict);
        }
        public static MultivariatePolynomial DeterminantMinusOne(int k, int n)
        {
            //Forme a_k^2 + b_k^2 + c_k^2 + d_k^2 - 1 
            int[] a_kSquared = new int[4 * n];
            a_kSquared[4 * k] = 2;
            int[] b_kSquared = new int[4 * n];
            b_kSquared[4 * k + 1] = 2;
            int[] c_kSquared = new int[4 * n];
            c_kSquared[4 * k + 2] = 2;
            int[] d_kSquared = new int[4 * n];
            d_kSquared[4 * k + 3] = 2;
            int[] one = new int[4 * n];

            Dictionary<int[], Complex> detMinOneDict = new Dictionary<int[], Complex>();
            detMinOneDict.Add(a_kSquared, 1 + 0 * Complex.j);
            detMinOneDict.Add(b_kSquared, 1 + 0 * Complex.j);
            detMinOneDict.Add(c_kSquared, 1 + 0 * Complex.j);
            detMinOneDict.Add(d_kSquared, 1 + 0 * Complex.j);
            detMinOneDict.Add(one, -1 + 0 * Complex.j);
            return new MultivariatePolynomial(detMinOneDict);
        }
        //Définition de l'image de g_k
        public static MultivariatePolynomialMatrix DefineMk(int k,int n)
        {
            MultivariatePolynomial[,] m_k = new MultivariatePolynomial[2, 2];
            int[] a_kMonomial = new int[4 * n];
            a_kMonomial[4 * k] = 1;
            int[] b_kMonomial = new int[4 * n];
            b_kMonomial[4 * k+1] = 1;
            int[] c_kMonomial = new int[4 * n];
            c_kMonomial[4 * k+2] = 1;
            int[] d_kMonomial = new int[4 * n];
            d_kMonomial[4 * k+3] = 1;

            //Definition du coefficient 11 de la matrice
            Dictionary<int[], Complex> m11Dict = new Dictionary<int[], Complex>();
            m11Dict.Add(a_kMonomial, 1 + 0 * Complex.j);
            m11Dict.Add(b_kMonomial, Complex.j);
            //Definition du coefficient 12 de la matrice
            Dictionary<int[], Complex> m12Dict = new Dictionary<int[], Complex>();
            m12Dict.Add(c_kMonomial, 1 + 0 * Complex.j);
            m12Dict.Add(d_kMonomial, Complex.j);
            //Definition du coefficient 21 de la matrice
            Dictionary<int[], Complex> m21Dict = new Dictionary<int[], Complex>();
            m21Dict.Add(c_kMonomial, -1 + 0 * Complex.j);
            m21Dict.Add(d_kMonomial, Complex.j);
            //Definition du coefficient 22 de la matrice
            Dictionary<int[], Complex> m22Dict = new Dictionary<int[], Complex>();
            m22Dict.Add(a_kMonomial, 1 + 0 * Complex.j);
            m22Dict.Add(b_kMonomial, -Complex.j);

            m_k[0, 0] = new MultivariatePolynomial(m11Dict);
            m_k[1, 0] = new MultivariatePolynomial(m12Dict);
            m_k[0, 1] = new MultivariatePolynomial(m21Dict);
            m_k[1, 1] = new MultivariatePolynomial(m22Dict);
            return new MultivariatePolynomialMatrix(m_k);
        }
    }
}
