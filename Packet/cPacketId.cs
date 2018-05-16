using System;
using System.Collections.Generic;

using System.Text;


namespace Packet
{
    public enum ePacketId
    {        
        cPKTChatLogin =0,
        cPKTChatLoginResult=1,
        cPKTChatRoomChange=2,
        cPKTChatRoomChangeResult=3,
        cPKTSay=4,
        cPKTSayResult=5,
        cPKTSayNotify=6,
        cPKTGameLogin=7,

        cPKTGameLoginResult =8,
        cPKTIdentify=9,
        cPKTIdentifyResult=10,
        cPKTAccountLoad=11,
        cPKTAccountLoadResult=12,

        cPKTInit = 13,
        cPKTInitResult = 14,

        Max
    }        
}
