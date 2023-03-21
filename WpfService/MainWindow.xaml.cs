using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UdpLibrary;

namespace WpfService
{
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<string> messages = new();
        private Dictionary<IPEndPoint, long> counters = new();
        private readonly object lockObject = new();
        public MainWindow()
        {
            InitializeComponent();
            lbMessages.ItemsSource = messages;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            messages.Add("Service started ...");

            _ = Task.Run(async () =>
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 34567);
                UdpClient client = new(ep);

                while (true)
                {
                    UdpReceiveResult result = await client.ReceiveAsync();
                    _ = Task.Factory.StartNew(async (object r) =>
                    {
                        var innerResult = (UdpReceiveResult)r;

                        lock (lockObject)
                        {
                            if (counters.TryGetValue(innerResult.RemoteEndPoint, out long current))
                                current++;
                            else
                                current = 1;
                            counters[innerResult.RemoteEndPoint] = current;

                        }


                        byte[] bytes = innerResult.Buffer;
                        string message = Encoding.UTF8.GetString(bytes);

                        Dispatcher.Invoke(() => messages.Add(message));

                        Person p = new Person("Clemens", "Kerer");

                        using (MemoryStream stream = new())
                        {
                            using (BinaryWriter writer = new(stream, Encoding.UTF8))
                            {
                                writer.Write(p.Firstname);
                                writer.Write(p.Lastname);
                                writer.Close();
                            }

                            byte[] bytes2 = stream.ToArray();
                            await client.SendAsync(bytes2, bytes2.Length, innerResult.RemoteEndPoint);
                        }
                    }, result);
                }
            });
            
            // TODO: start the UDP service here
            // TODO: process each request on a separate thread
            // TODO: display the received message in the list box
            // TODO: use the Encoding class and BinaryReader/Writer for serialization (2 options)
        }

        // TODO: make sure the service is stopped when the window is closed
    }
}
