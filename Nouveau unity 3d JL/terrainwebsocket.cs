using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;

public class terrainwebsocket : MonoBehaviour
{
    WebSocket websocket;
    // ici 2 veriables 
    public static int height;
    public static int width;
    public static bool isConnect;
    public static bool receivedmapinfo, needtoremove, needtomovebot, newbot;
    public static int bx1, by1, bx2, by2, cx1,cy1,ex1,ey1,m2,m3,rx1,ry1;
    public static byte[] mapInfos;
    
// Start is called before the first frame update
async void Start()
    {
        
        isConnect = false;
        receivedmapinfo = false;
        needtoremove = false;
        needtomovebot = false;
        newbot = false;
        websocket = new WebSocket("ws://127.0.0.1:4626/display");//url

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
            isConnect = true;
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
            isConnect = false;
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
            isConnect = false;
        };

        websocket.OnMessage += (bytes) =>
        {

            //Debug.Log("OnMessage!");
            //Debug.Log(bytes);

            
            // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            //Debug.Log("OnMessage! " + message);
            /* switch (bytes[0])
             {
                 case System.Text.Encoding.ASCII.GetBytes("M")[0]:

             }*/
            Debug.Log("OnMessage! " + message);
            if (bytes[0]== System.Text.Encoding.ASCII.GetBytes("M")[0])
            {
               
                receivedmapinfo = true;
                height = bytes[3];
                width = bytes[1];
                //Debug.Log(height);
                int surface = width * height;
                mapInfos = new byte[surface];
                //Debug.Log("surface" + surface);
                //Debug.Log("new bot");
                newbot = true;
                for (int i = 0; i < surface; i++)
                    {
                        
                        mapInfos[i] = bytes[i+5];
                    //Debug.Log("mapInfos" + mapInfos[i]);

                }
                for (int i = 0; i < width; i++)
                {

                    mapInfos[i] = 0;
                    
                }
                for (int i = surface-width; i < surface; i++)
                {

                    mapInfos[i] = 0;
                    

                }
                for (int i = width; i < surface; i += width)
                {
                    mapInfos[i] = 0;
                    mapInfos[i - 1] = 0;
                }



            }
            if (bytes[0]== System.Text.Encoding.ASCII.GetBytes("E")[0])
           {
                //Debug.Log("OnMessage! " + message);
                //Debug.Log("energie");
                ex1 = bytes[1];
                ey1 = bytes[2];
            }
           if(bytes[0] == System.Text.Encoding.ASCII.GetBytes("P")[0])
            {

                bx1 = bytes[1];
                by1 = bytes[2];
                bx2 = bytes[3];
                by2 = bytes[4];
                needtomovebot = true;
               // Debug.Log("bot info 1 " + bx1 + by1 +"new"+ bx2 + by2);
            }
            
            if (bytes[0] == System.Text.Encoding.ASCII.GetBytes("C")[0])
            {
                cx1 = bytes[1];
                cy1 = bytes[2];

                 
            }
            if (bytes[0] == System.Text.Encoding.ASCII.GetBytes("R")[0])
            {
                rx1 = bytes[1];
                ry1 = bytes[2];
                needtoremove = true;
                
            }
            else{
                needtoremove = false;
            }
            if (bytes[0] == System.Text.Encoding.ASCII.GetBytes("B")[0])
            {
                
            }

        };



        // waiting for messages
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending bytes
            await websocket.Send(new byte[] { 10, 20, 30 });

            // Sending plain text
            await websocket.SendText("plain text message");
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

}

