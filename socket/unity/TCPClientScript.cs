using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class TCPClientScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try{
            TcpClient client = new TcpClient("127.0.0.1",12345);

            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.WriteLine("Hello World!");
            writer.Flush();

            StreamReader reader = new StreamReader(client.GetStream());
            string text = reader.ReadLine();
            Debug.Log(text);

            client.Close();
        }
        catch(Exception e){
            Debug.Log(e);
        }
    }

}
