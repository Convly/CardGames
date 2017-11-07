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

namespace Client.Objs.Windows
{
    /// <summary>
    /// Logique d'interaction pour Lobby.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        public Lobby()
        {
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
    }
}
