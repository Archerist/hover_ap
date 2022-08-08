using System;
using HarmonyLib;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Packets;

namespace Hover_AP
{
    [HarmonyPatch(typeof(GuiVersionDateQuality), nameof(GuiVersionDateQuality.UpdateValue), new Type[] { })]
    class InfoPatch
    {
        static void Postfix(ref GuiVersionDateQuality ___instance)
        {
            var inst = ___instance;
            ModMain.session.Socket.ErrorReceived += (Exception e, string message) =>
            {
                GuiVersionDateQuality.UpdateValue();
                inst.label.text += " / Error Connecting to AP";
            };

            ModMain.session.Socket.SocketClosed += (WebSocketSharp.CloseEventArgs e) =>
            {

                GuiVersionDateQuality.UpdateValue();
                inst.label.text += " / AP Connection Closed";

            };

            ModMain.session.Socket.PacketReceived += (ArchipelagoPacketBase packet) => {

                if(packet is ConnectedPacket connectedPacket)
                {
                    GuiVersionDateQuality.UpdateValue();
                    inst.label.text += " / " + ModMain.session.Players.GetPlayerName(connectedPacket.Slot) +" Connected to AP";

                }

            };

            ___instance = inst;

           

        }
    }
}
