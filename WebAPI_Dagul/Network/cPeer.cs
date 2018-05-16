using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;

using Network.Packet;
using Network.Utility;
using Network.Base;
using Packet;
using Packet.Base;
using ServerAFC;

using ChatServerLib.Network.WebSocket;

namespace WebAPI_Dagul.Network
{
    /// <summary>
    /// 접속한 클라이언트 전송 처리
    /// </summary>
    public class cPeer : cPeerBase
    {
        public AspNetWebSocketContext Context { get; private set; }

        public cPeer(AspNetWebSocketContext context, IMarshaller marshaller)
        {
            Context = context;
            Marshaller = marshaller;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        // 비동기 처리가 필요하면 아래를 이용한다.
        //////////////////////////////////////////////////////////////////////////////////////////////
        //public async Task Send(ArraySegment<byte> message)
        //{
        //    await Context.WebSocket.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
        //    Context.WebSocket.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
        //}

        //public async Task Send(IPacket packet)
        //{
        //    string message;

        //    Marshaller.Marshal(packet, out message);
        //    ArraySegment<byte> binaryData = cConvertUtil.ConvertStringToBinary(message);
        //    await Context.WebSocket.SendAsync(binaryData, WebSocketMessageType.Text, true, CancellationToken.None);
        //}
        //////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 데이터를 전송한다.
        /// </summary>
        /// <param name="sendBuffer">패킷이 들어있는 버퍼</param>
        public override void Send(ArraySegment<byte> message)
        {
            Context.WebSocket.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// 패킷을 전송한다.
        /// </summary>
        /// <param name="packet">전송할 패킷</param>
        public override void Send(IPacket packet)
        {
            string message = Marshaller.Marshal(packet);
            cLogger.Information("Send Message: {0}\r\n", message);
            ArraySegment<byte> binaryData = cConvertUtil.ConvertStringToBinary(message);
            Context.WebSocket.SendAsync(binaryData, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// 패킷을 전송한다.
        /// </summary>
        /// <param name="packet">전송할 패킷</param>
        public override void SendDataNoEnc(IPacket packet)
        {
            string message = Marshaller.MarshalDataNoEnc(packet);
            cLogger.Information("Send Message: {0}\r\n", message);
            ArraySegment<byte> binaryData = cConvertUtil.ConvertStringToBinary(message);
            Context.WebSocket.SendAsync(binaryData, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// 받은 데이터를 처리한다.(주로 서버에서 사용)
        /// </summary>
        /// <param name="packetManager">패킷 매니저</param>
        /// <param name="packetHandlerManager">패킷 핸들러 매니저</param>
        /// <param name="receiveBuffer">데이터가 저장되어 있는 버퍼</param>
        /// <param name="receiveBufferLength">데이터가 저장되어 있는 버퍼의 길이(데이터의 길이)</param>
        /// <returns>성공 유무</returns>
        public override bool Receive(
            IPacketManager packetManager,
            cPacketHandlerManager packetHandlerManager,
            ArraySegment<Byte> receiveBuffer,
            int receiveBufferSize)
        {
            cLogger.Information("encode a received data\r\n");
            string data = Encoding.UTF8.GetString(receiveBuffer.Array, 0, receiveBufferSize);
            data = data.Replace("\0", "");
            data = data.Replace("\\", "");

            cLogger.Information("unmarshal a received data\r\n");
            IPacket packet = Marshaller.Unmarshal(data, packetManager); //AccountID 세팅을 해야 한다 하고라?       
            if (null == packet)
            {
                cLogger.Error(
                    "packet is not allocated (account_id: {0}, peer_id: {1}, data: {2}\r\n",
                    PeerId,
                    data);

                return false;
            }

            cLogger.Information("execute a packet handler\r\n");
            packetHandlerManager.Execute(this, packet);
            cLogger.Information("completed that a received data is executed\r\n");
            return true;
        }

        /// <summary>
        /// 버퍼에 저장되어 있는 내용을 처리한다.(주로 클라이언트에서 사용)
        /// </summary>
        /// <param name="packetHandlerManager">패킷 핸들러 매니저</param>
        /// <returns>성공 유무</returns>
        public override void Receive(IPacketManager packetManager, cPacketHandlerManager packetHandlerManager)
        {
        }
    }
}