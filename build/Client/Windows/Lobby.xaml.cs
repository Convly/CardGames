using System.Windows;
using System.Windows.Input;

namespace Client.Windows
{
    /// <summary>
    /// Interaction logic for Lobby.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        private static Lobby instance = null;

        /// <summary>
        /// Getter and Setter for the singleton instance of <see cref="Lobby"/>
        /// </summary>
        public static Lobby Instance { get => instance; set => instance = value; }

        /// <summary>
        /// Default constructor of the <see cref="Lobby"/>
        /// </summary>
        public Lobby()
        {
            instance = this;
            InitializeComponent();
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
