using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Network.Packet;
using Network.Base;
using Packet.Base;
using ServerAFC;

namespace ChatServerLib.Network.WebSocket
{
    /// <summary>
    /// 접속한 클라이언트 전송 처리
    /// </summary>
    public abstract class cPeerBase : IPeer
    {
        public string PeerId { get; set; }
        public IMarshaller Marshaller { get; set; }

        /// <summary>
        /// 생성자
        /// </summary>
        public cPeerBase()
        {
            PeerId = Guid.NewGuid().ToString();
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
        public abstract void Send(ArraySegment<byte> message);

        /// <summary>
        /// 패킷을 전송한다.
        /// </summary>
        /// <param name="packet">전송할 패킷</param>
        public abstract void Send(IPacket packet);

        /// <summary>
        /// 패킷을 전송한다.
        /// </summary>
        /// <param name="packet">전송할 패킷</param>
        public abstract void SendDataNoEnc(IPacket packet);

        /// <summary>
        /// 받은 데이터를 처리한다.(주로 서버에서 사용)
        /// </summary>
        /// <param name="packetManager">패킷 매니저</param>
        /// <param name="packetHandlerManager">패킷 핸들러 매니저</param>
        /// <param name="receiveBuffer">데이터가 저장되어 있는 버퍼</param>
        /// <param name="receiveBufferLength">데이터가 저장되어 있는 버퍼의 길이(데이터의 길이)</param>
        /// <returns>성공 유무</returns>
        public abstract bool Receive(
            IPacketManager packetManager,
            cPacketHandlerManager packetHandlerManager,
            ArraySegment<Byte> receiveBuffer,
            int receiveBufferSize);

        /// <summary>
        /// 버퍼에 저장되어 있는 내용을 처리한다.(주로 클라이언트에서 사용)
        /// </summary>
        /// <param name="packetHandlerManager">패킷 핸들러 매니저</param>
        /// <returns>성공 유무</returns>
        public abstract void Receive(IPacketManager packetManager, cPacketHandlerManager packetHandlerManager);

        /// <summary>
        /// 마샬러를 반환한다.
        /// </summary>
        /// <returns>반환할 마샬러</returns>
        public IMarshaller GetMarshaller()
        {
            return Marshaller;
        }
    }
}
