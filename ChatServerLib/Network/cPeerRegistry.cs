using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Network.Base;
using ServerAFC;

namespace ChatServerLib.Network
{
    public class cPeerRegistry : ServerAFC.cSingleton<cPeerRegistry>
    {
        private ConcurrentDictionary<string, IPeer> PeerMapByAccountIdMap = new ConcurrentDictionary<string, IPeer>(); //< Peer를 관리하는 맵(키: AccountId)

        public bool Add(IPeer peer)
        {
            if (null == PeerMapByAccountIdMap.GetOrAdd(peer.PeerId, peer))
            {
                cLogger.Warning("cannot add peer to peer registry (peer_id: {0})\r\n", peer.PeerId);
                return false;
            }

            cLogger.Information("add account id to peer registry(peer_id: {0})\r\n", peer.PeerId);
            return true;
        }

        public bool Remove(string peerId)
        {
            IPeer peer = null;
            if (false == PeerMapByAccountIdMap.TryRemove(peerId, out peer))
            {
                cLogger.Warning("cannot find account id to peer registry (peer_id: {0})\r\n", peerId);
                return false;
            }

            cLogger.Information("remove account id to peer registry (peer_id: {0})\r\n", peerId);
            return true;
        }
    }
}
