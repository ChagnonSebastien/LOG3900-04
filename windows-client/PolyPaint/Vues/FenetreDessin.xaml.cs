using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using PolyPaint.VueModeles;
using System.IO;
using System.Windows.Controls;
using PolyPaint.Modeles.Outils;
using System.Windows.Input;
using System.Windows.Forms;

namespace PolyPaint
{
    /// <summary>
    /// Logique d'interaction pour FenetreDessin.xaml
    /// </summary>
    public partial class FenetreDessin : Page
    {

        public FenetreDessin()
        {
            InitializeComponent();
            DataContext = new VueModele();
        }
        
        // Pour gérer les points de contrôles.
        private void GlisserCommence(object sender, DragStartedEventArgs e) => (sender as Thumb).Background = Brushes.Black;
        private void GlisserTermine(object sender, DragCompletedEventArgs e) => (sender as Thumb).Background = Brushes.White;
        private void GlisserMouvementRecu(object sender, DragDeltaEventArgs e)
        {
            String nom = (sender as Thumb).Name;
            if (nom == "horizontal" || nom == "diagonal") colonne.Width = new GridLength(Math.Max(32, colonne.Width.Value + e.HorizontalChange));
            if (nom == "vertical" || nom == "diagonal") ligne.Height = new GridLength(Math.Max(32, ligne.Height.Value + e.VerticalChange));
        }

        // Pour la gestion de l'affichage de position du pointeur.
        private void Canvas_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) => textBlockPosition.Text = "";
        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point p = e.GetPosition(Canvas);
            textBlockPosition.Text = Math.Round(p.X) + ", " + Math.Round(p.Y) + "px";
        }

        private void DupliquerSelection(object sender, RoutedEventArgs e)
        {
            Canvas.CopySelection();
            Canvas.Paste();
        }

        private void SupprimerSelection(object sender, RoutedEventArgs e) => Canvas.CutSelection();

        
        private void Menu_Change_Avatar_Click(object sender, System.EventArgs e)
        {
            string fileName = null;

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog().ToString().Equals("OK"))
                {
                    fileName = openFileDialog1.FileName;
                }
            }

            if (fileName != null)
            {
                string text = File.ReadAllText(fileName);
            }
        }

        private void ChoisirOutil(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext == null || this.ToolSelection.SelectedIndex == -1)
            {
                return;
            }
            
            ((VueModele)this.DataContext).ChoisirOutil.Execute((Tool)this.ToolSelection.SelectedItem);
        }

        private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ((VueModele)this.DataContext).MouseUp.Execute(e.GetPosition((IInputElement)sender));
        }

        private void Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ((VueModele)this.DataContext).MouseDown.Execute(e.GetPosition((IInputElement)sender));
        }

        private void Canvas_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ((VueModele)this.DataContext).MouseMove.Execute(e.GetPosition((IInputElement)sender));
        }

        private void Canvas_SelectionChanged(object sender, EventArgs e)
        {
            ((VueModele)this.DataContext).SelectStrokes.Execute(Canvas.GetSelectedStrokes().Clone());
            Canvas.Select(new System.Windows.Ink.StrokeCollection());
        }
    }
}
