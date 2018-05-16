using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChatServerLib.Chatting;
using ChatServerLib.Network;
using Network.Base;
using Packet;
using Packet.Base;
using ServerAFC;
using WebAPI_Dagul.Network;

namespace WebAPI_Dagul.PacketHandler
{
    public class cCSSayHandler : IPacketHandler
    {
        public cCSSayHandler()
        {
        }

        public void Execute(IPeer peer, IPacket packet)
        {
            cPKTSay recvPacket = (cPKTSay)packet;
            cPKTSayResult sendPacket = new cPKTSayResult();
            sendPacket.Message = recvPacket.Message;

            if (true == cChatRoomManager.Broadcast(peer.PeerId, recvPacket.Message))
            {
                sendPacket.ResultCode = cPKTSayResult.eResultCode.SUCCEED;
            }
            else
            {
                sendPacket.ResultCode = cPKTSayResult.eResultCode.FAILURE;
            }

            peer.Send(sendPacket);
        }


    }
}