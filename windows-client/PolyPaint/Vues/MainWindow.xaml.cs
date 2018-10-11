﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace PolyPaint.Vues
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Gallery gallery;
        private FenetreDessin FenetreDessin;
        private string AvatarLocation;

        public MainWindow()
        {
            this.FenetreDessin = new FenetreDessin();
            this.Server_Connect();
            InitializeComponent();
            this.gallery = new Gallery();

        }
        private void Server_Connect()
        {
            LoginDialogBox dlg = new LoginDialogBox();
            if (dlg.ShowDialog() == false)
            {
                System.Environment.Exit(0);
            }  
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((System.Windows.Controls.Button)e.Source).Uid);

            GridCursor.Margin = new Thickness(10 + (150 * index), 0, 0, 0);

            switch (index)
            {
                case 0:
                    GridMain.Content = "Gallery";
                    break;
                case 1:
                    GridMain.Content = "Users";
                    break;
                case 2:
                    GridMain.Content = "Chat";
                    break;
                case 3:
                    GridMain.Content = this.FenetreDessin;
                    break;
            }
        }
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
                this.AvatarLocation = fileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(this.AvatarLocation);
                bitmap.EndInit();
                AvatarImage.Source = bitmap;
            }
        }
    }
}
