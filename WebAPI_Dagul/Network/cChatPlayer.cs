using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebSockets;

using Network.Base;

namespace WebAPI_Dagul.Network
{
    public class cChatPlayer : cPeer
    {
        public string AccountId { get; set; }

        public cChatPlayer(AspNetWebSocketContext context, IMarshaller marshaller)
            : base(context, marshaller)
        {
        }
    }
}