using CardGameResources.Net;
using Client.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool processConnect = false;
        private static MainWindow instance;
        public static MainWindow Instance { get => instance; set => instance = value; }
        public bool ProcessConnect { get => processConnect; set => processConnect = value; }

        public MainWindow()
        {
            instance = this;
            InitializeComponent();
        }

        /// <summary>
        /// Connect the client to the server when he click on this button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ProcessConnect == true)
            {
                return;
            }

            this.ProcessConnect = true;

            try
            {
                Network.Client.Instance.Start(GameClient.EntryPoint, GameClient.ChatEntryPoint, ip_txtbox.Text, int.Parse(port_txtbox.Text));
                GameClient.Instance.Name = name_txtbox.Text;
                Network.Client.Instance.SendDataToServer(new Packet(name_txtbox.Text, PacketType.SYS, new Syscall(SysCommand.C_REGISTER, new List<string> { name_txtbox.Text }), true));
                this.ProcessConnect = false;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Cannot connect to the specified host!");
                Console.Error.WriteLine(exc.Message);
                this.ProcessConnect = false;
            }
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
    }
}
