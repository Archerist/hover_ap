using System;
using HarmonyLib;
using MelonLoader;
namespace TestMod
{

    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.AddItem), new Type[] { typeof(Item), typeof(byte) })]
    class SaveManagerPatch
    {

        static void Prefix(Item item, byte count)
        {
            MelonLogger.Msg("Prefix works");
            MelonLogger.Msg("{0}, {1}", item.itemType, count);

        }
    }
}
