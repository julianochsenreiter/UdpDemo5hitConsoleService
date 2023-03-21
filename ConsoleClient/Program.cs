using System.Net;
using System.Net.Sockets;
using System.Text;
using UdpLibrary;

Console.WriteLine("Press a key to start ...");
Console.ReadKey();

string message = "Hello students!";
byte[] bytes = Encoding.UTF8.GetBytes(message);

UdpClient client = new();
IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Loopback, 34567);
await client.SendAsync(bytes, bytes.Length, remoteEndpoint);

UdpReceiveResult response = await client.ReceiveAsync();
byte[] bytes2 = response.Buffer;
using(MemoryStream stream  = new(bytes2))
{
    using (BinaryReader reader = new(stream, Encoding.UTF8))
    {
        string firstname = reader.ReadString();
        string lastname = reader.ReadString();
        Person p = new(firstname, lastname);
        Console.WriteLine(p);
    }
}

// TODO: send a text message using the Encoding class
// TODO: send a person object using a binary reader/writer
// TODO: use async calls where possible
