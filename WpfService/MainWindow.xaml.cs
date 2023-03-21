using System.Windows;

namespace WpfService
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO: start the UDP service here
            // TODO: process each request on a separate thread
            // TODO: display the received message in the list box
            // TODO: use the Encoding class and BinaryReader/Writer for serialization (2 options)
        }

        // TODO: make sure the service is stopped when the window is closed
    }
}
