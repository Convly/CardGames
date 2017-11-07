using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.Windows
{
    /// <summary>
    /// Logique d'interaction pour GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Window
    {
        public GameBoard()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void userCard1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            Image imageControl = (Image)sender;
            if ((e.Data.GetData(typeof(ImageSource)) != null))
            {
                ImageSource image = e.Data.GetData(typeof(ImageSource)) as ImageSource;
                imageControl = new Image() { Width = 100, Height = 100, Source = image };
                boardCard1.Source = (ImageSource)e.Data.GetData(typeof(ImageSource));
            }
        }
    }
}
