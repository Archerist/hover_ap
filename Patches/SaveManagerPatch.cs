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
            var rand = new Random();
            MelonLogger.Msg("{0}, {1}", itemName, count);
            var MissingList = ModMain.session.Locations.AllMissingLocations;
            var itemIndex = rand.Next(0, MissingList.Count);
            var itemId = MissingList[itemIndex];
            MelonLogger.Msg("Checked {0}", itemId);

            ModMain.session.Locations.CompleteLocationChecks(itemId);

        }
    }
}
