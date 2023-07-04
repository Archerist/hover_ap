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
            if (ModMain.session.Socket.Connected)
            {
                inst.label.text += " / " + ModMain.session.Players.GetPlayerName(ModMain.session.ConnectionInfo.Slot) + " Connected to AP";
            }
            else
            {
                inst.label.text += " / Not Connected";
            }

            ___instance = inst;
        }
    }
}
