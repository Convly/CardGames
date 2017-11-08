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
        private static MainWindow instance;
        public static MainWindow Instance { get => instance; set => instance = value; }
        public MainWindow()
        {
            instance = this;
            InitializeComponent();
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Network.Client.Instance.Start(GameClient.Instance.EntryPoint, ip_txtbox.Text, int.Parse(port_txtbox.Text));
                GameClient.Instance.Name = name_txtbox.Text;
                Network.Client.Instance.SendDataToServer(new Packet(name_txtbox.Text, PacketType.SYS, new Syscall(SysCommand.C_REGISTER, new List<string> { name_txtbox.Text })));
            }
            catch (Exception exc)
            {
                MessageBox.Show("Cannot connect to the specified host!");
                Console.Error.WriteLine(exc.Message);
                return;
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
