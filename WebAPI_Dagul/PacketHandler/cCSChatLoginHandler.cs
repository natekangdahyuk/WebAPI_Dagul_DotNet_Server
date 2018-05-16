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
    public class cCSChatLoginHandler : IPacketHandler
    {
        public cCSChatLoginHandler()
        {
        }

        public void Execute(IPeer peer, IPacket packet)
        {
            cPKTChatLogin recvPacket = (cPKTChatLogin)packet;
            cPKTChatLoginResult resultPacket = new cPKTChatLoginResult();

            do
            {
                if (0 == recvPacket.AccountId.Length)
                {
                    resultPacket.ResultCode = cPKTChatLoginResult.eResultCode.TOO_SHORT_ACCOUNT_ID;
                    break;
                }

                cChatPlayer chatPlayer = (cChatPlayer)peer;
                chatPlayer.AccountId = recvPacket.AccountId;

                cLogger.Information("a peer is added (account_id: {0})\r\n", chatPlayer.AccountId);
                peer.PeerId = chatPlayer.AccountId;
                cPeerRegistry.Instance.Add(peer);

                cLogger.Information("join to chatroom (account_id: {0})\r\n", chatPlayer.AccountId);
                int retChatRoomNumber = cChatRoomManager.Join(recvPacket.AccountId, peer);

                resultPacket.ResultCode = cPKTChatLoginResult.eResultCode.SUCCEED;
                resultPacket.ChatRoomNumber = retChatRoomNumber;

            } while (false);

            peer.Send(resultPacket);
        }

    }
}