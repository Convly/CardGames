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
        private static GameBoard instance;
        public static GameBoard Instance { get => instance; set => instance = value; }
        public GameBoard()
        {
            instance = this;
            InitializeComponent();
            GameClient gameClient = GameClient.Instance;
            player2.Content = gameClient.UsersList.ElementAt((gameClient.UsersList.FindIndex(gameClient.Name.StartsWith) + 3) % 4);
            player3.Content = gameClient.UsersList.ElementAt((gameClient.UsersList.FindIndex(gameClient.Name.StartsWith) + 1) % 4);
            player4.Content = gameClient.UsersList.ElementAt((gameClient.UsersList.FindIndex(gameClient.Name.StartsWith) + 2) % 4);
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
