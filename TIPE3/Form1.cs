using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIPE3
{
    //Script principal pour la construction des noeuds
    public partial class Form1 : Form
    {
        //grille de dessin
        Bitmap b = new Bitmap(WindowParameters.height, WindowParameters.width);
        bool bezierMode = true;

        //Construction d'un noeud par clics
        Color[] colors = new Color[50];
        Noeud noeud;
        List<Brin> brinsConstruits = new List<Brin>();
        List<Vector2> brinEnConstruction = new List<Vector2>();

        Vector2 lastP1 = new Vector2(0, 0); //Dernier point de controle pour la courbe de Bezier
        List<CrossingPoint> crossingPoints = new List<CrossingPoint>();
        int currentColor = 0;
        bool crossingPointMode = false;
        bool canDraw = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //initialisation des couleurs
            Random rand = new Random();
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.FromArgb(rand.Next(50, 255), rand.Next(50, 255), rand.Next(50, 255));
            }
            InitializeBitmap();

            pictureBox1.Width = WindowParameters.width;
            pictureBox1.Height = WindowParameters.height;
         
            RefreshImage();
        }

        //Rafraîchit l'image
        void RefreshImage()
        {
            //Affichage des points de croisements
            foreach (CrossingPoint c in crossingPoints)
            {
                DrawPoint(new ColorizedPoint(c.position, Color.Red), b, 2);
                //Affichage des brins engagés dans le croisement
                for (int i = 0; i<3; i++)
                {
                    if(!(c.brins[i] == -1))
                    {
                        Color color = new Color();
                        if(c.brins[i] >= brinsConstruits.Count)
                        {
                            color = colors[currentColor];
                        }
                        else
                        {
                            color = brinsConstruits[c.brins[i]].color;
                        }
                        DrawPoint(new ColorizedPoint(c.position + new Vector2(-5 + 5 * i, -5), color),b,2);
                    }
                }
            }
            pictureBox1.Image = b;
        }

        //Colorie la grille en noir
        void InitializeBitmap()
        {
            for (int y = 0; y < b.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    b.SetPixel(x, y, Color.Black);
                }
            }
        }
        
        //Affiche un point colorié de la taille souhaitée
        public static void DrawPoint(ColorizedPoint cP, Bitmap b, int pointSize)
        {
            int x = (int)cP.point.x;
            int y = (int)cP.point.y;
            for (int dy = -pointSize; dy < pointSize ; dy++)
            {
                for (int dx = -pointSize; dx < pointSize; dx++)
                {
                    if (y + dy < WindowParameters.height && y + dy > 0 && x + dx < WindowParameters.width && x + dx > 0)
                    {
                        b.SetPixel(x + dx, y + dy, cP.color);
                    }
                }
            }
        }

        //Affiche une liste de points
        public static void DrawListOfPoint(List<ColorizedPoint> list, Bitmap b)
        {
            foreach (ColorizedPoint cP in list)
            {
                DrawPoint(cP, b, WindowParameters.pointSize);

            }
        }

       
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;

            //Position du point où on a cliqué
            Vector2 clickedPoint = new Vector2(mouseEventArgs.X, mouseEventArgs.Y);

            if (canDraw) //Vérifie si le noeud n'est pas terminé
            {
                if (!crossingPointMode) //Vérifie si on ne souhaite pas placer un point de croisement
                {
                    //On va ajouter un segment à un brin
                    if (brinEnConstruction.Count == 0)
                    {
                        //S'il s'agit du premier point du brin en construction

                        if (brinsConstruits.Count > 0)
                        {
                            //Si il y a déjà des brins de construits, on place le premier point du nouveau brin en aval du dernier point de croisement
                            brinEnConstruction.Add(crossingPoints[crossingPoints.Count - 1].position + WindowParameters.crossingSpace * Vector2.Direction(crossingPoints[crossingPoints.Count - 1].position, clickedPoint));
                        }
                        else
                        {
                            
                            //Le premier point du premier brin est automatiquement précédé d'un point de croisement initial

                            CrossingPoint initCrossPoint = new CrossingPoint();
                            initCrossPoint.position = clickedPoint + WindowParameters.crossingSpace * new Vector2(1, 0);
                                
                            //Par convention, le premier brin est le brin sortant de ce point de croisement
                            initCrossPoint.brins[2] = 0;
                                
                            
                            crossingPoints.Add(initCrossPoint);
                            
                            //On place le premier point du brin là où on a clqiué
                            brinEnConstruction.Add(clickedPoint);
                        }
                    }
                    else
                    {
                        //S'il ne s'agit pas du premier point du brin en construction

                        //On vérifie si on n'a pas cliqué sur un point de croisement
                        int crossPointIndex = -1;
                        for (int i = 0; i < crossingPoints.Count; i++)
                        {
                            if (Vector2.Distance(crossingPoints[i].position, clickedPoint) < WindowParameters.crossingSpace)
                            {
                                crossPointIndex = i;
                            }
                        }

                        if (crossPointIndex == -1)
                        {
                            //Si on n'a pas cliqué sur un point de croisement, le point cliqué est ajouté tel quel
                            brinEnConstruction.Add(clickedPoint);
                        }
                        else
                        {
                            //Sinon, le point cliqué est identifié au point de croisement, et le brin en construction est le brin passant au dessus de ce point de croisement
                            crossingPoints[crossPointIndex].brins[1] = brinsConstruits.Count;
                            brinEnConstruction.Add(crossingPoints[crossPointIndex].position);
                            
                        }
                    }
                    //On dessine le dernier point construit du brin dans tous les cas
                    DrawPoint(new ColorizedPoint(brinEnConstruction[brinEnConstruction.Count - 1], colors[currentColor]), b, 1);
                    if (brinEnConstruction.Count > 1)
                    {
                        //Si le brin en construction possède déjà un point, on trace une courbe de Bezier (ou le segment) reliant le dernier et l'avant dernier point
                        
                        
                        List<ColorizedPoint> curve = new List<ColorizedPoint>();
                        if (bezierMode)
                        {
                            Vector2 p0 = brinEnConstruction[brinEnConstruction.Count - 2];
                            Vector2 p2 = brinEnConstruction[brinEnConstruction.Count - 1];
                            Vector2 p1 = (p0 + p2) / 2;
                            if (brinEnConstruction.Count > 2)
                            {

                                p1 = p0 + Vector2.Distance(p0, p2) * Vector2.Direction(lastP1, p0);

                            }
                            lastP1 = p1;
                            curve = Utilities.PlotBezier(p0, p1, p2, colors[currentColor]);
                        }
                        else
                        {
                            curve = Utilities.PlotLine(brinEnConstruction[brinEnConstruction.Count - 2], brinEnConstruction[brinEnConstruction.Count - 1], colors[currentColor]);
                        }

                        List<Vector2> curve2 = curve.Select(x => x.point).ToList();
                        //Pour que les points restent dans le bon ordre, on retire le dernier point
                        brinEnConstruction.Remove(brinEnConstruction[brinEnConstruction.Count - 1]);

                        brinEnConstruction.AddRange(curve2);
                        DrawListOfPoint(curve, b);
                    }

                    RefreshImage();
                }
                else
                {
                    //Si on souhaite placer un point de croisement
                    if (brinsConstruits.Count > 0)
                    {

                        Brin dernierBrinConstruit = brinsConstruits[brinsConstruits.Count - 1];

                        CrossingPoint newCrossingPoint = new CrossingPoint();
                        newCrossingPoint.position = dernierBrinConstruit[dernierBrinConstruit.Length - 1] + WindowParameters.crossingSpace * Vector2.Direction(dernierBrinConstruit[dernierBrinConstruit.Length - 1], clickedPoint);
                        
                        //On vérifie si un brin déjà construit est proche du point
                        Vector2 closestPoint = brinsConstruits[0][0];
                        int closestBrin = 0;

                        for (int i = 0; i < brinsConstruits.Count; i++)
                        {
                            Brin b = brinsConstruits[i];
                            Vector2 closestPointInB = Utilities.ClosestPointOfCurve(newCrossingPoint.position, b.brin.ToList());

                            if (Vector2.Distance(newCrossingPoint.position, closestPointInB) < Vector2.Distance(newCrossingPoint.position, closestPoint))
                            {
                                if (i != brinsConstruits.Count - 1 || Vector2.Distance(dernierBrinConstruit[dernierBrinConstruit.brin.Length - 1], closestPointInB) > WindowParameters.crossingSpace) {
                                    closestPoint = closestPointInB;

                                    closestBrin = i;
                                }
                            }
                        }
                            
                        //Si le brin est assez près alors le point de croisement est identifié au point du brin le plus proche
                        if (Vector2.Distance(newCrossingPoint.position, closestPoint) < WindowParameters.crossingSpace)
                        {

                            newCrossingPoint.position = closestPoint;

                            //Le brin passant au dessus du point de croisement est le brin le plus proche
                            newCrossingPoint.brins[1] = closestBrin;
                            
                        }
                        //Le brin entrant est le dernier brin construit
                        newCrossingPoint.brins[0] = brinsConstruits.Count - 1;

                        //Le brin sortant est le brin qu'on va construire
                        newCrossingPoint.brins[2] = brinsConstruits.Count;

                        crossingPoints.Add(newCrossingPoint);
                        

                    }
                    crossingPointMode = false;

                    //Les points de croisement sont dessinés lors du rafraîchissement pour rester au premier plan
                }
            }
        }

        //Signale qu'on souhaite placer un point de croisement et termine le brin en construction pour passer au suivant
        private void button1_Click(object sender, EventArgs e)
        {
           
            if (canDraw)
            {
                if (brinEnConstruction.Count > 1)
                {

                    brinsConstruits.Add(new Brin(brinEnConstruction.ToArray(), colors[currentColor]));
                    brinEnConstruction = new List<Vector2>();
                    currentColor = (currentColor + 1) % colors.Length;
                    
                    crossingPointMode = true;
                }
            }
        }

        //Termine le noeud en faisant entrer le dernier brin dans le point de croisement initial
        private void button2_Click(object sender, EventArgs e)
        {
            if (canDraw)
            {
                List<ColorizedPoint> line = Utilities.PlotLine(brinEnConstruction[brinEnConstruction.Count - 1], crossingPoints[0].position - Vector2.Direction(brinEnConstruction[brinEnConstruction.Count - 1], crossingPoints[0].position) * WindowParameters.crossingSpace, colors[currentColor]);
                brinEnConstruction.AddRange(line.Select(x => x.point).ToList());
                DrawListOfPoint(line, b);
                crossingPoints[0].brins[0] = brinsConstruits.Count;
                brinsConstruits.Add(new Brin(brinEnConstruction.ToArray(), colors[currentColor]));
                noeud = new Noeud(brinsConstruits.ToArray(), crossingPoints.ToArray());
                canDraw = false;
                RefreshImage();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            KnotGroupPresentation KGP = noeud.CalculateKnotGroupPresentation();
            UnknotRecognitionAlgorithm.CheckKnot(KGP);
        }
    }
}
