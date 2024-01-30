using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets; //ここにTcpClientクラスがある
using System.Text;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class Item{
    public float x;
    public float y;
    public float z;
    public float visibility;
}
[System.Serializable]
public class ItemList{
    public Item[] items;
}

public class TCPServerScript : MonoBehaviour
{
    private List<TcpClient> clients = new List<TcpClient>();
    private TcpListener listener;
    private const int port = 12345;
    private bool running = false;
    private GameObject obj;
    private ItemList itemList;
    


    // Start is called before the first frame update
    void Start()
    {
        StartServer();
    }
    private void StartServer(){
        try{
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            running = true;
            Debug.Log("Server has started on port " + port.ToString());

            Thread listenerThread = new Thread(ListenForClients);
            listenerThread.Start();
        }
        catch(Exception e){
            Debug.Log("Socket error: " + e.Message);
        }
    }

    private void ListenForClients(){
        while(running){
            TcpClient client = listener.AcceptTcpClient();
            clients.Add(client);
            Debug.Log("Client connected");
            Thread clientThread = new Thread(HandleClientComm);
            clientThread.Start(client);
        }
    }

    private void HandleClientComm(object client){
        TcpClient tcpClient = (TcpClient)client; //ここではクライアントとの通信を行うためのTcpClientクラスのインスタンスを取得している
        // ここでの(TcpClient)clientという記法はキャストと呼ばれるもので、clientというobject型の変数をTcpClient型に変換している
        NetworkStream clientStream = tcpClient.GetStream(); //ここではクライアントとの通信を行うためのストリームを取得している

        byte[] message = new byte[4096];
        int bytesRead;

        while(true){
            bytesRead = 0;

            try{
                bytesRead = clientStream.Read(message, 0, 4096); // ここではクライアントからのメッセージを受け取っている
            }
            catch{
                break;
            }

            if(bytesRead == 0){
                break;
            }
            string receivedMessage = Encoding.UTF8.GetString(message, 0, bytesRead);
            ASCIIEncoding encoder = new ASCIIEncoding();
            System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead));
            // Debug.Log(receivedMessage);

            //リスポンス処理を行う
            string responseMessage = ProcessData(receivedMessage); //この中にはjsonのstring型が入っている
            byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
            clientStream.Write(responseBytes, 0, responseBytes.Length);
        }

        tcpClient.Close();
    }

    private string ProcessData(string data){

        itemList = JsonUtility.FromJson<ItemList>(data); // ここを実行するとitemlistがnullになる
        
        if (itemList == null || itemList.items == null) {
            Debug.LogError("Failed to parse JSON data.");
            return data;
        }
        //ここでは指定された座標に球を配置できるようにしたい
        // Debug.Log(itemList.items[0].x);
        // foreach(Item item in itemList.items){
        //     // CubeプレハブをGameObject型で取得
        //     GameObject obj = (GameObject)Resources.Load ("hand_joint");
        //     // Cubeプレハブを元に、インスタンスを生成、
        //     Instantiate (obj, new Vector3(item.x,item.y,item.z), Quaternion.identity);
        // }
        
        return data;
    }
    public ItemList getHnadPose(){
        return itemList;
    }
    private void OnDestroy()
    {
        running = false;
        listener.Stop();
    }
}
