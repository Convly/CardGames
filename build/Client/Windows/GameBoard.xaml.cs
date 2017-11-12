using CardGameResources.Game;
using CardGameResources.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Client.Windows
{
    /// <summary>
    /// Logique d'interaction pour GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Window
    {
        private static GameBoard instance;
        /// <summary>
        /// Getter and Setter for the singleton instance of the <see cref="GameBoard"/>
        /// </summary>
        public static GameBoard Instance { get => instance; set => instance = value; }

        /// <summary>
        /// Default constructor for the <see cref="GameBoard"/>
        /// </summary>
        public GameBoard()
        {
            instance = this;
            InitializeComponent();
            GameClient gameClient = GameClient.Instance;
            player2.Content = gameClient.UsersList.ElementAt((gameClient.UsersList.IndexOf(gameClient.Name) + 1) % 4);
            player3.Content = gameClient.UsersList.ElementAt((gameClient.UsersList.IndexOf(gameClient.Name) + 2) % 4);
            player4.Content = gameClient.UsersList.ElementAt((gameClient.UsersList.IndexOf(gameClient.Name) + 3) % 4);
        }

        /// <summary>
        /// Quit the program when you press Escape
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        /// <summary>
        /// Action to send request of <see cref="GameAction.C_TAKE_TRUMP_AS"/> as a color
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Network.Client.Instance.SendDataToServer(new Packet(GameClient.Instance.Name, PacketType.GAME, new Gamecall(GameAction.C_TAKE_TRUMP_AS, GameBoard.Instance.trumColor_combobox.Text)));
            GameBoard.Instance.trumpAs_pnel.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Action to send request of <see cref="GameAction.C_TAKE_TRUMP_AS"/> as null
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Network.Client.Instance.SendDataToServer(new Packet(GameClient.Instance.Name, PacketType.GAME, new Gamecall(GameAction.C_TAKE_TRUMP_AS, null)));
            GameBoard.Instance.trumpAs_pnel.Visibility = Visibility.Hidden;
        }
        /*****************************************************************/

        /// <summary>
        /// Set the card dropped previously to the boardCard
        /// </summary>
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

        /// <summary>
        /// Set the card dropped previously to the boardCard
        /// </summary>
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

        /// <summary>
        /// Set the card dropped previously to the boardCard
        /// </summary>
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

        /// <summary>
        /// Set the card dropped previously to the boardCard
        /// </summary>
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
        /*********************************************************/

        /// <summary>
        /// Get the card for the drop with the mouse
        /// </summary>
        private void userCard1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        /// <summary>
        /// Get the card for the drop with the mouse
        /// </summary>
        private void userCard2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        /// <summary>
        /// Get the card for the drop with the mouse
        /// </summary>
        private void userCard3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        /// <summary>
        /// Get the card for the drop with the mouse
        /// </summary>
        private void userCard4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }
        
        /// <summary>
        /// Get the card for the drop with the mouse
        /// </summary>
        private void userCard5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        /// <summary>
        /// Get the card for the drop with the mouse
        /// </summary>
        private void userCard6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        /// <summary>
        /// Get the card for the drop with the mouse
        /// </summary>
        private void userCard7_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }

        /// <summary>
        /// Get the card for the drop with the mouse
        /// </summary>
        private void userCard8_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.All);
        }
        /*******************************************************/


        /// <summary>
        /// Send a msg from the chat to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chat_boxe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(chat_boxe.Text))
            {
                Network.Client.Instance.SendMsgChat("[" + GameClient.Instance.Name + "] " +  chat_boxe.Text + "\n");
                chat_boxe.Clear();
            }
        }
    }
}
