using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.WebSockets;
using System.Web.WebSockets;

using ChatServerLib.Network.WebSocket;
using Network.Packet;

namespace WebAPI_Dagul.Network
{
    public static class cChatPlayerManager
    {
        public static cPeerBase Create(AspNetWebSocketContext context)
        {
            return new cChatPlayer(context, new cJSONMarshaller());
        }
    }
}