using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class UDP : MonoBehaviour
{
    private string host = "127.0.0.1";
    private int Un_Pyport = 50007;
    private int Py_Unport = 50008;
    private UdpClient client1;
    private UdpClient client2;
    IPEndPoint remoteEP = null;

    void Start()
    {
        client1 = new UdpClient();
        client1.Connect(host, Un_Pyport);
        client2 = new UdpClient(Py_Unport);
        client2.Client.ReceiveTimeout = 100000;
        ListenForUDPMessage();
    }

    async void ListenForUDPMessage()
    {
        while (true)
        {
            UdpReceiveResult result = await client2.ReceiveAsync();
            byte[] data = result.Buffer;
            string text = Encoding.UTF8.GetString(data);
            Debug.Log(text);
        }

    }

    public void send()
    { 
        ///UnityÇ©ÇÁPythonÇ÷UDPëóêMÇ∑ÇÈ
        var message = Encoding.UTF8.GetBytes("Hello World!");
        client1.Send(message, message.Length);
        Debug.Log("OK");
    }

    private void emoemo(string txt)
    {
        Debug.Log("ä¥èÓîªíËÅF" + txt);
    }

    private void OnDestroy()
    {
        client1.Close();
        client2.Close();
    }
}
