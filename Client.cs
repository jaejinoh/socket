using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;


public class Client : MonoBehaviour
{

    Socket client;
    string fromNetThread = "";
    byte[] buffer = new byte[1024];

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (fromNetThread.Length > 0)
        {
            GameObject.Find("UI Text").GetComponent<UnityEngine.UI.Text>().text += "\n" + fromNetThread;

            fromNetThread = "";
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("서버접속중");
            client = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            client.BeginConnect("127.0.0.1", 10000, null, client);
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            string msg = GameObject.Find("InputField").GetComponent<UnityEngine.UI.InputField>().text;

            buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(msg);


            client.BeginSend(buffer, 0, buffer.Length,
                SocketFlags.None, SendCallback,  null);
        }
    }


    void SendCallback(IAsyncResult result)
    {
        int len = client.EndSend(result);

        print("보낸결과 : " + len);

        client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecvCallback, null);
	}



    void RecvCallback(IAsyncResult result)
    {
        int len = client.EndReceive(result);

        if (len > 0)
        {
            string recv = System.Text.ASCIIEncoding.ASCII.GetString(this.buffer);

            fromNetThread = recv;

            print(recv);
        }

    }

	internal static int EndSend(IAsyncResult result)
	{
		throw new NotImplementedException();
	}
}
