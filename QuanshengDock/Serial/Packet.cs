﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanshengDock.Serial
{
    public static class Packet
    {
        public const ushort Hello = 0x514;
        public const ushort GetRssi = 0x527;
        public const ushort KeyPress = 0x601;
        public const ushort GetScreen = 0x603;
        public const ushort Scan = 0x608;
        public const ushort ScanAdjust = 0x609;
        public const ushort ScanReply = 0x708;
        public const ushort ImHere = 0x515;
        public const ushort RssiInfo = 0x528;
        public const ushort WriteEeprom = 0x51D;
        public const ushort WriteEepromReply = 0x51E;
        public const ushort ReadEeprom = 0x51B;
        public const ushort ReadEepromReply = 0x51C;
    }
}
