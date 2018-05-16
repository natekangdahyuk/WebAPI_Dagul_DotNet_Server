using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Network.Base;
using ServerAFC;

using cChatRoomDict = System.Collections.Concurrent.ConcurrentDictionary<int, ChatServerLib.Chatting.cChatRoom>; // 키: 채팅방 인덱스
using cChatRoomPair = System.Collections.Generic.KeyValuePair<int, ChatServerLib.Chatting.cChatRoom>;

using cChatRoomIndexByAccountIdDict = System.Collections.Concurrent.ConcurrentDictionary<string, int>; // 키: 계정, 값: 채팅방 인덱스
using cChatRoomIndexByAccountIdPair = System.Collections.Generic.KeyValuePair<string, string>;

namespace ChatServerLib.Chatting
{
    /// <summary>
    /// 채팅방 전체를 관리한다.
    /// </summary>
    public static class cChatRoomManager
    {
        private const int CHAT_ROOM_MEMBER_MAX_COUNT = 2;           // 채팅방 최대 인원수
        private const int CHAT_ROOM_DEFAULT_COUNT = 100;              // 최초 채팅방 개수
        private static cChatRoomDict ChatRoomMap = new cChatRoomDict(); // 키: 채팅방 인덱스
        private static cChatRoomIndexByAccountIdDict ChatRoomIndexByAccountIdMap = new cChatRoomIndexByAccountIdDict(); // 계정을 이용하여 채팅방 인덱스 저장

        private static int NextChatRoomMaxIndex = 0; // 다음 채팅방 최대 인덱스

        public static int ChatRoomCount { get { return ChatRoomMap.Count; } }

        public static void OnTimer(object source, ElapsedEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            foreach (cChatRoomPair pair in ChatRoomMap)
            {
                cChatRoom chatRoom = pair.Value;
                message.AppendFormat("| ChatRoomIndex: {0}, MemberCount: {1} |",
                    chatRoom.ChatRoomIndex, chatRoom.GetPeerCount());
            }

            cLogger.Information("ChatRoomInfo -----> {0}\r\n", message);
        }

        /// <summary>
        /// 들어갈 수 있는 채팅방을 반환한다.
        /// </summary>
        /// <returns>채팅방</returns>
        private static cChatRoom GetSuitableChatRoom()
        {
            for (int loop1 = 0; NextChatRoomMaxIndex > loop1; ++loop1)
            {
                cChatRoom chatRoom = null;
                if (false == ChatRoomMap.TryGetValue(loop1, out chatRoom))
                {
                    continue;
                }

                if (CHAT_ROOM_MEMBER_MAX_COUNT > chatRoom.GetPeerCount())
                {
                    return chatRoom;
                }
            }

            return null;
        }

        /// <summary>
        /// 채팅방에 들어간다.
        /// </summary>
        /// <param name="accountId">계정</param>
        /// <param name="peer">피어</param>
        public static int Join(string accountId, IPeer peer)
        {
            try
            {
                cChatRoom chatRoom = GetSuitableChatRoom();

                // 새로운 채팅방을 생성한다.
                if (null == chatRoom)
                {
                    chatRoom = new cChatRoom(NextChatRoomMaxIndex);
                    if (false == ChatRoomMap.TryAdd(chatRoom.ChatRoomIndex, chatRoom))
                    {
                        throw new Exception(string.Format("cannot add chat room (chat_room_index: {0}", chatRoom.ChatRoomIndex));
                    }

                    ++NextChatRoomMaxIndex;
                }

                chatRoom.Join(accountId, peer);
                if (false == ChatRoomIndexByAccountIdMap.TryAdd(accountId, chatRoom.ChatRoomIndex))
                {
                    chatRoom.Leave(accountId);
                    throw new Exception(string.Format(
                        "cannot add chat room index by peer id dictionary (chat_room_index: {0}, account_id: {1}",
                        chatRoom.ChatRoomIndex, accountId));
                }

                cLogger.Information("join chatroom (chatroom_index: {0}, account_id: {1})\r\n",
                    chatRoom.ChatRoomIndex,
                    accountId);
                return chatRoom.ChatRoomIndex;
            }
            catch (Exception e)
            {
                cLogger.Error("Exception: {0}\r\n", e.Message);
                return 0;
            }
        }

        /// <summary>
        /// 채팅방에서 나간다.
        /// </summary>
        /// <param name="accountId">계정</param>
        /// <returns>성공 유무</returns>
        public static bool Leave(string accountId)
        {
            int chatRoomIndex;
            if (false == ChatRoomIndexByAccountIdMap.TryGetValue(accountId, out chatRoomIndex))
            {
                cLogger.Warning("cannot find account id (account_id: {0})", accountId);
                return false;
            }
            ChatRoomIndexByAccountIdMap.TryRemove(accountId, out chatRoomIndex);//Ro

            cChatRoom chatRoom = null;
            if (false == ChatRoomMap.TryGetValue(chatRoomIndex, out chatRoom))
            {            
                cLogger.Error("cannot find chat room (chat_room_index: {0}, account_id: {1})",
                    chatRoom.ChatRoomIndex,
                    accountId);

                return false;
            }

            chatRoom.Leave(accountId);
            cLogger.Information("leave chatroom (chatroom_index: {0}, account_id: {1})\r\n", chatRoomIndex, accountId);
            return true;
        }

        /// <summary>
        /// 전체 채팅방에 메세지를 브로드캐스팅 한다.
        /// </summary>
        /// <param name="accountId">계정</param>
        /// <param name="message">메세지</param>
        public static void BroadcastAll(string accountId, string message)
        {
            Parallel.ForEach(ChatRoomMap, pair =>
            {
                cChatRoom chatRoom = pair.Value;
                if (null != chatRoom)
                {
                    chatRoom.Broadcast(accountId, message);
                }
            });
        }

        /// <summary>
        /// 채팅방에 메세지를 브로드캐스팅 한다.
        /// </summary>
        /// <param name="chatRoomIndex">채팅방 인덱스</param>
        /// <param name="accountId">계정</param>
        /// <param name="message">메세지</param>
        /// <returns>성공 유무</returns>
        public static bool Broadcast(int chatRoomIndex, string accountId, string message)
        {
            cChatRoom chatRoom = null;
            if (false == ChatRoomMap.TryGetValue(chatRoomIndex, out chatRoom))
            {
                return false;
            }

            chatRoom.Broadcast(accountId, message);
            return true;
        }

        /// <summary>
        /// 채팅방에 메세지를 브로드캐스팅 한다.
        /// </summary>
        /// <param name="accountId">계정</param>
        /// <param name="message">메세지</param>
        /// <returns>성공 유무</returns>
        public static bool Broadcast(string accountId, string message)
        {
            int chatRoomIndex = 0;
            if (false == ChatRoomIndexByAccountIdMap.TryGetValue(accountId, out chatRoomIndex))
            {
                cLogger.Warning("cannot find chatroom index by account id (account_id: {0})", accountId);
                return false;
            }

            cLogger.Information("broadcast message (account_id: {0}, message: {1})\r\n", accountId, message);
            return Broadcast(chatRoomIndex, accountId, message);
        }

        /// <summary>
        /// 전체 플레이어수를 반환한다.
        /// </summary>
        /// <returns></returns>
        public static int GetPeerCount()
        {
            int peerCount = 0;
            foreach (KeyValuePair<int, cChatRoom> pair in ChatRoomMap)
            {
                cChatRoom chatRoom = pair.Value;
                peerCount += chatRoom.GetPeerCount();
            }

            return peerCount;
        }

        /// <summary>
        /// 채팅방을 변경한다.
        /// </summary>
        /// <param name="accountId">계정</param>
        /// <param name="newChatRoomIndex">새로운 채팅방 인덱스</param>
        /// <returns>성공 유무</returns>
        public static bool ChangeChatRoom(string accountId, int newChatRoomIndex)
        {
            int oldChatRoomIndex;
            if (false == ChatRoomIndexByAccountIdMap.TryGetValue(accountId, out oldChatRoomIndex))
            {
                return false;
            }

            cChatRoom oldChatRoom = null;
            if (false == ChatRoomMap.TryGetValue(oldChatRoomIndex, out oldChatRoom))
            {
                return false;
            }

            if (null == oldChatRoom)
            {
                return false;
            }
                                    
            ChatRoomIndexByAccountIdMap.TryUpdate(accountId, newChatRoomIndex, oldChatRoomIndex);
            

            IPeer peer = oldChatRoom.Leave(accountId);

            cChatRoom newChatRoom = null;
            if (false == ChatRoomMap.TryGetValue(newChatRoomIndex, out newChatRoom))
            {
                oldChatRoom.Join(accountId, peer);
                return false;
            }

            //여기가 살짝 이상함
            newChatRoom.Join(accountId, peer);
            return true;
        }
    }
}
