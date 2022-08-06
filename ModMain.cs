using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using Archipelago.MultiClient.Net;
namespace TestMod
{
    public class ModMain : MelonMod
    {

        private static Dictionary<string, string> apDict = new Dictionary<string, string>()
            {
                {"url","archipelago.gg" },
                {"port","38281" },
                {"slot","" },
                {"password","" }

            };

        private ArchipelagoSession session;
        private MelonPreferences_Category apMelon;
        public override void OnApplicationStart()
        {
            apMelon = MelonPreferences.CreateCategory("Archipelago");
            apMelon.SetFilePath("UserData/HoverAP.cfg", true);

            var copyDict = new Dictionary<string,string>(apDict);
            
            foreach (var kvp in apDict)
            {
                var entry = apMelon.GetEntry<string>(kvp.Key);
                copyDict[kvp.Key] = entry is null ? apMelon.CreateEntry<string>(kvp.Key, kvp.Value).Value : entry.Value;
            }

            foreach(var kvp in copyDict)
            {
                apDict[kvp.Key] = kvp.Value;
            }


            session = ArchipelagoSessionFactory.CreateSession(apDict["url"], Convert.ToInt32(apDict["port"]));

            LoggerInstance.Msg(apDict.ToString());

            Login();



        }

        public override void OnApplicationQuit()
        {
            apMelon.SaveToFile();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {

                Login();
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {

                var playerList = new List<string>();

                foreach (var player in session.Players.AllPlayers)
                {
                    playerList.Add(player.Alias);
                }

                LoggerInstance.Msg(String.Join(", ", playerList.ToArray() ));


            }


        }

        private bool Login()
        {
            bool info = false;
            try
            {
                foreach (var kvp in apDict)
                {
                    LoggerInstance.Msg($"{kvp.Key}: {kvp.Value}");
                }
                string[] tags = { "IgnoreGame" };
                info = session.TryConnectAndLogin(
                    "Minecraft",
                    apDict["slot"],
                    new Version(0, 3, 4),
                    Archipelago.MultiClient.Net.Enums.ItemsHandlingFlags.RemoteItems,
                    tags
                    ).Successful;
                LoggerInstance.Msg("login success {0}", info);

            }
            catch (Exception e)
            {
                LoggerInstance.Error(e.ToString());
            }

            return info;
        }
    }
}
