using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChatServerLib.Chatting;
using Network.Base;
using Packet;
using Packet.Base;
using WebAPI_Dagul.Network;

namespace WebAPI_Dagul.PacketHandler
{
    public class cCSChatRoomChangeHandler : IPacketHandler
    {
        public cCSChatRoomChangeHandler()
        {
        }

        public void Execute(IPeer peer, IPacket packet)
        {
            cPKTChatRoomChange recvPacket = (cPKTChatRoomChange)packet;
            cPKTChatRoomChangeResult resultPacket = new cPKTChatRoomChangeResult();

            do
            {
                cChatPlayer chatPlayer = (cChatPlayer)peer;
                if (false == cChatRoomManager.ChangeChatRoom(chatPlayer.AccountId, recvPacket.NewChatRoomIndex))
                {
                    resultPacket.ResultCode = cPKTChatRoomChangeResult.eResultCode.CANNOT_CHANGE_CHAT_ROOM;
                    break;
                }

                resultPacket.ResultCode = cPKTChatRoomChangeResult.eResultCode.SUCCEED;

            } while (false);

            peer.Send(resultPacket);
        }


    }
}