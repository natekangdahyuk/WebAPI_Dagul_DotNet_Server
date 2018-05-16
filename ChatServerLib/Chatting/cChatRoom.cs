using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatServerLib.Network;
using Network.Base;
using Packet;
using ServerAFC;

namespace ChatServerLib.Chatting
{
    /// <summary>
    /// 채팅방 처리를 한다.
    /// </summary>
    public class cChatRoom
    {
        private ConcurrentDictionary<string, IPeer> PeerMap = null; // Key: AccountId

        public int ChatRoomIndex { get; set; } // 채팅방 인덱스

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="chatRoomIndex">채팅방 인덱스</param>
        public cChatRoom(int chatRoomIndex)
        {
            ChatRoomIndex = chatRoomIndex;
            PeerMap = new ConcurrentDictionary<string, IPeer>();
        }

        /// <summary>
        /// 채팅방에 들어간다.
        /// </summary>
        /// <param name="accountId">계정</param>
        /// <param name="peer">피어</param>
        public void Join(string accountId, IPeer peer)
        {
            cLogger.Information("join chatroom (account_id: {0})\r\n", accountId);

            try
            {
                PeerMap.TryAdd(accountId, peer);

            }
            catch (Exception e)
            {
                cLogger.Information("Exception: ChatRoom --------> Exception: {0}\r\n", e.Message);
            }
        }

        /// <summary>
        /// 채팅방에서 나간다.
        /// </summary>
        /// <param name="accountId">계정</param>
        /// <returns>성공 유무</returns>
        public IPeer Leave(string accountId)
        {
            IPeer peer = null;
            if (false == PeerMap.TryRemove(accountId, out peer))
            {
                return null;
            }
                        

            return peer;
        }

        /// <summary>
        /// 메세지를 브로드캐스팅 한다.
        /// </summary>
        /// <param name="accountId">계정</param>
        /// <param name="message">메세지</param>
        public void Broadcast(string accountId, string message)
        {
            cPKTSayNotify sendPacket = new cPKTSayNotify();
            sendPacket.AccountId = accountId;
            sendPacket.Message = message;

            cLogger.Information("Broadcast AccountId: {0}, Message: {1}\r\n", accountId, message);

            StringBuilder log = new StringBuilder();
            foreach (KeyValuePair<string, IPeer> pair in PeerMap)
            {
                log.AppendFormat("AccountId: {0}, ", pair.Value.PeerId);
            }

            cLogger.Information("ChatRoom Member =====> {0}\r\n", log.ToString());

            Parallel.ForEach(PeerMap, pair =>
            {
                IPeer peer = pair.Value;
                if (null != peer)
                {
                    peer.Send(sendPacket);
                }
            });
        }

        public int GetPeerCount()
        {
            return PeerMap.Count;
        }
    }
}
