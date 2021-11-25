using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIPE3
{

    class Noeud
    {
        Brin[] brins;
        CrossingPoint[] crossingPoints;

        public Noeud(Brin[] brins, CrossingPoint[] crossingPoints)
        {
            if (crossingPoints.Length == brins.Length)
            {
                this.brins = brins;
                this.crossingPoints = crossingPoints;
            }
        }
        //Détermination de la présentation de Wirtinger du groupe de noeud
        public KnotGroupPresentation CalculateKnotGroupPresentation()
        {
            int[] generators = new int[brins.Length];
            CrossingRelation[] relations = new CrossingRelation[brins.Length];
            for (int i = 0; i < generators.Length; i++)
            {
                generators[i] = i;
            }
            foreach (CrossingPoint crossing in crossingPoints)
            {
                
                Brin brinEntrant = brins[crossing.brins[0]];
                Console.WriteLine(crossing.brins[0]);
                Console.WriteLine(crossing.brins[1]);
                Brin brinPassant = brins[crossing.brins[1]];
          
                Vector2 pointEntrant = brinEntrant[brinEntrant.Length - 1];

                Vector2 pointPassantSuivantCroisement = brinPassant[Array.IndexOf(brinPassant.brin, crossing.position) + 1];

                Vector2 vDir = Vector2.Direction(crossing.position, pointPassantSuivantCroisement);
                Vector2 v = Vector2.Direction(crossing.position, pointEntrant);

                double cp = Vector2.CrossProduct(vDir, v);

                CrossingRelation rel = new CrossingRelation();
                rel.generators = new int[4]{ crossing.brins[1], crossing.brins[0], crossing.brins[1],crossing.brins[2] };
                if (cp > 0)
                {
                    rel.invert = new bool[4] { false, false, true, true };
                }
                else
                {
                    rel.invert = new bool[4] { true, false, false, true };
                }
                relations[crossing.brins[0]] = rel;
            
            }
            return new KnotGroupPresentation(generators, relations);
        }


    }
    public class CrossingPoint
    {
        public Vector2 position;
       
        //Le point de croisement met en jeu trois brins : le brin entrant, le brin passant au dessus, et le brin sortant (dans cet ordre)
        public int[] brins = new int[3] { -1, -1, -1 };


    }
    //Presentation d'un groupe de noeud
    public struct KnotGroupPresentation
    {
        int[] generators;
        CrossingRelation[] relations;
        public int[] Generators
        {
            get
            {
                return generators;
            }
        }
        public CrossingRelation[] Relations
        {
            get
            {
                return relations;
            }
        }
        public KnotGroupPresentation(int[] generators, CrossingRelation[] relations)
        {
            this.generators = generators;
            this.relations = relations;
        }
    }
    public struct CrossingRelation
    {
        public int[] generators; //générateurs impliqués dans la relation
        public bool[] invert; //la kème case vaut false si c'est g_k qui apparaît dans la relation, true si c'est son inverse
    }

}