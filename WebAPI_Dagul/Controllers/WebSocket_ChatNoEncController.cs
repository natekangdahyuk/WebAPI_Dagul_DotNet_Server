using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Web.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Web.WebSockets;

using ChatServerLib.Chatting;
using ChatServerLib.Network;
using ChatServerLib.Network.WebSocket;
using Network.Packet;
using Network.Base;
using Packet;
using ServerAFC;
using WebAPI_Dagul.Network;
using WebAPI_Dagul.PacketHandler;

namespace WebAPI_Dagul.Controllers
{    
    [RoutePrefix("WebSocketChatNoEnc")]
    public class WebSocket_ChatNoEncController : ApiController
    {
        private const int RECEIVE_BUFFER_MAX_SIZE = 1024 * 50; // Receive Buffer 최대 크기(단위: byte)

        private ThreadLocal<cPacketHandlerManager> PacketHandlerManager = new ThreadLocal<cPacketHandlerManager>(() =>
        {
            cPacketHandlerManager packetHandlerManager = new cPacketHandlerManager();
            packetHandlerManager.Add(ePacketId.cPKTChatLogin, new cCSChatLoginHandler());
            packetHandlerManager.Add(ePacketId.cPKTChatRoomChange, new cCSChatRoomChangeHandler());
            packetHandlerManager.Add(ePacketId.cPKTSay, new cCSSayHandler());            
            return packetHandlerManager;
        });

        private ThreadLocal<cPacketManager> PacketManager = new ThreadLocal<cPacketManager>(() =>
        {
            cPacketManager packetManager = new cPacketManager();
            packetManager.Add(new cPKTChatLogin());
            packetManager.Add(new cPKTChatLoginResult());
            packetManager.Add(new cPKTChatRoomChange());
            packetManager.Add(new cPKTChatRoomChangeResult());
            packetManager.Add(new cPKTSay());
            packetManager.Add(new cPKTSayResult());
            packetManager.Add(new cPKTSayNotify());
            packetManager.Add(new cPKTGameLogin());
            packetManager.Add(new cPKTGameLoginResult());
            packetManager.Add(new cPKTIdentify());
            packetManager.Add(new cPKTIdentifyResult());
            packetManager.Add(new cPKTAccountLoad());
            packetManager.Add(new cPKTAccountLoadResult());
            packetManager.Add(new cPKTInit());
            packetManager.Add(new cPKTInitResult());
            return packetManager;
        });



        [Route("PacketReceiver")] //game 관련 하나의 소켓을 만들어 놓고 대기한다.
        [HttpGet]
        public HttpResponseMessage Game()
        {
            if (true == System.Web.HttpContext.Current.IsWebSocketRequest)
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest(Receiver);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        private async Task Receiver(AspNetWebSocketContext context)
        {
            cPeerBase peer = cChatPlayerManager.Create(context);

            try
            {
                WebSocket socket = context.WebSocket;

                while (true)
                {
                    ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[RECEIVE_BUFFER_MAX_SIZE]);
                    WebSocketReceiveResult receiveResult = await socket.ReceiveAsync(receiveBuffer, CancellationToken.None);

                    if (socket.State == WebSocketState.Open)
                    {
                        string data = System.Text.Encoding.UTF8.GetString(receiveBuffer.Array, 0, receiveResult.Count);
                        data = data.Replace("\0", "");
                        data = data.Replace("\\", "");

                        int packetIdIndex = data.IndexOf(',');

                        string packetIdData = data.Substring(0, packetIdIndex);

                        string packetData = "";
                        packetData = data.Substring(packetIdIndex + 1, data.Length - packetIdData.Length - 1); // ','가 있어서 -1을 하였다.
                                                
                        peer.Receive(PacketManager.Value, PacketHandlerManager.Value, receiveBuffer, receiveResult.Count);

                    }

                }
            }
            catch (Exception e)
            {  
                Disconnect(peer.PeerId);
                cLogger.Error("Exception: {0}\r\n", e.Message);
            }
        }
        //

        private void Disconnect(string peerId)
        {
            cChatRoomManager.Leave(peerId);
            cPeerRegistry.Instance.Remove(peerId);
        }


    }//class
}
