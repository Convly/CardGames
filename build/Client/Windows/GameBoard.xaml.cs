using CardGameResources.Game;
using CardGameResources.Net;
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
            player2.Content = gameClient.UsersList.ElementAt((gameClient.UsersList.FindIndex(gameClient.Name.StartsWith) + 1) % 4);
            player3.Content = gameClient.UsersList.ElementAt((gameClient.UsersList.FindIndex(gameClient.Name.StartsWith) + 2) % 4);
            player4.Content = gameClient.UsersList.ElementAt((gameClient.UsersList.FindIndex(gameClient.Name.StartsWith) + 3) % 4);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Network.Client.Instance.SendDataToServer(new Packet(GameClient.Instance.Name, PacketType.GAME, new Gamecall(GameAction.C_TAKE_TRUMP_AS, GameBoard.Instance.trumColor_combobox.Text)));
            GameBoard.Instance.trumpAs_pnel.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Network.Client.Instance.SendDataToServer(new Packet(GameClient.Instance.Name, PacketType.GAME, new Gamecall(GameAction.C_TAKE_TRUMP_AS, null)));
            GameBoard.Instance.trumpAs_pnel.Visibility = Visibility.Hidden;
        }

        private void boardCard1_Drop(object sender, DragEventArgs e)
        {
            Image imageControl = (Image)sender;
            if ((e.Data.GetData(typeof(ImageSource)) != null))
            {
                ImageSource image = e.Data.GetData(typeof(ImageSource)) as ImageSource;
                imageControl = new Image() { Width = 100, Height = 100, Source = image };
                string[] splitedSrc = ((ImageSource)e.Data.GetData(typeof(ImageSource)) as BitmapImage).UriSource.OriginalString.Split('/');
                string color = splitedSrc[splitedSrc.Count() - 2];
                string value = splitedSrc[splitedSrc.Count() - 1].Split('.')[0];
                Network.Client.Instance.SendDataToServer(new Packet(GameClient.Instance.Name, PacketType.GAME, new Gamecall(GameAction.C_PLAY_CARD, new Card(value[0], color))));
            }
        }

        private void boardCard2_Drop(object sender, DragEventArgs e)
        {
            Image imageControl = (Image)sender;
            if ((e.Data.GetData(typeof(ImageSource)) != null))
            {
                ImageSource image = e.Data.GetData(typeof(ImageSource)) as ImageSource;
                imageControl = new Image() { Width = 100, Height = 100, Source = image };
                string[] splitedSrc = ((ImageSource)e.Data.GetData(typeof(ImageSource)) as BitmapImage).UriSource.OriginalString.Split('/');
                string color = splitedSrc[splitedSrc.Count() - 2];
                string value = splitedSrc[splitedSrc.Count() - 1].Split('.')[0];
                Network.Client.Instance.SendDataToServer(new Packet(GameClient.Instance.Name, PacketType.GAME, new Gamecall(GameAction.C_PLAY_CARD, new Card(value[0], color))));
            }
        }

        private void boardCard3_Drop(object sender, DragEventArgs e)
        {
            Image imageControl = (Image)sender;
            if ((e.Data.GetData(typeof(ImageSource)) != null))
            {
                ImageSource image = e.Data.GetData(typeof(ImageSource)) as ImageSource;
                imageControl = new Image() { Width = 100, Height = 100, Source = image };
                string[] splitedSrc = ((ImageSource)e.Data.GetData(typeof(ImageSource)) as BitmapImage).UriSource.OriginalString.Split('/');
                string color = splitedSrc[splitedSrc.Count() - 2];
                string value = splitedSrc[splitedSrc.Count() - 1].Split('.')[0];
                Network.Client.Instance.SendDataToServer(new Packet(GameClient.Instance.Name, PacketType.GAME, new Gamecall(GameAction.C_PLAY_CARD, new Card(value[0], color))));
            }
        }

        private void boardCard4_Drop(object sender, DragEventArgs e)
        {
            Image imageControl = (Image)sender;
            if ((e.Data.GetData(typeof(ImageSource)) != null))
            {
                ImageSource image = e.Data.GetData(typeof(ImageSource)) as ImageSource;
                imageControl = new Image() { Width = 100, Height = 100, Source = image };
                string[] splitedSrc = ((ImageSource)e.Data.GetData(typeof(ImageSource)) as BitmapImage).UriSource.OriginalString.Split('/');
                string color = splitedSrc[splitedSrc.Count() - 2];
                string value = splitedSrc[splitedSrc.Count() - 1].Split('.')[0];
                Network.Client.Instance.SendDataToServer(new Packet(GameClient.Instance.Name, PacketType.GAME, new Gamecall(GameAction.C_PLAY_CARD, new Card(value[0], color))));
            }
        }

        private void userCard1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        private void userCard2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        private void userCard3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        private void userCard4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        private void userCard5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        private void userCard6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        private void userCard7_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        private void userCard8_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }
    }
}
