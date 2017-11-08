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
    /// Logique d'interaction pour Lobby.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        private static Lobby instance;
        public static Lobby Instance { get => instance; set => instance = value; }
        public Lobby()
        {
            instance = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            progress.Value += 25;
            if (progress.Value == 100)
            {
                GameBoard board = new GameBoard();
                App.Current.MainWindow = board;
                this.Close();
                board.Show();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
