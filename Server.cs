using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class Server : MonoBehaviour
{
    Socket server;
    byte[] buffer = new byte[1024];

    // Start is called before the first frame update
    void Start()
    {
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        server.Bind(new IPEndPoint(IPAddress.Any, 10000));
        server.Listen(10);

        server.BeginAccept(AcceptCallback, server);

        print("서버 실행");
    }

    void AcceptCallback(IAsyncResult result)
    {
        Socket client = server.EndAccept(result);
        IPEndPoint addr = ((IPEndPoint)client.RemoteEndPoint);

        print(string.Format("{0}, {1}", addr.ToString(), addr.Port.ToString()));


        client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecvCallback, client); 


        print("pending");



        /*
        byte[] buffer = new byte[100];

        int len = client.Receive(buffer);

        print("pending");
        string recv = System.Text.ASCIIEncoding.ASCII.GetString(buffer);

        print(recv);
        */
    }


    void RecvCallback(IAsyncResult result)
    {
        Socket client = (Socket)result.AsyncState;
        int len = client.EndReceive(result);

        if (len > 0)
        {
            string recv = System.Text.ASCIIEncoding.ASCII.GetString(this.buffer);

            print(recv);

            client.BeginSend(buffer, 0, len + 1,
               SocketFlags.None, SendCallback, client);
    }

        client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecvCallback, client);  
    }


    void SendCallback(IAsyncResult result)
    {
        Socket client = (Socket)result.AsyncState;
        int len = Client.EndSend(result);

        print("보낸결과 : " + len);
    }



// Update is called once per frame
void Update()
    {
        
    }
}
