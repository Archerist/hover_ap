using System;
using HarmonyLib;
using MelonLoader;

namespace Hover_AP
{

    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.AddItem), new Type[] { typeof(string), typeof(byte) })]
    class SaveManagerPatch
    {

        static void Prefix(string itemName, byte count)
        {
            MelonLogger.Msg("Prefix works");
            MelonLogger.Msg("{0}, {1}", itemName, count);

        }
    }
}
