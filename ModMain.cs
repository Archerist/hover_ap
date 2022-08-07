using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using Archipelago.MultiClient.Net;
namespace Hover_AP
{
    public class ModMain : MelonMod
    {

        private static Dictionary<string, string> apServerInfo = new Dictionary<string, string>()
            {
                {"url","archipelago.gg" },
                {"port","38281" },
                {"slot","" },
                {"password","" }

            };



        public static ArchipelagoSession session;
        private MelonPreferences_Category apMelon;
        public override void OnApplicationStart()
        {

            InitAP();

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

        private void InitAP()
        {
            apMelon = MelonPreferences.CreateCategory("Archipelago");
            apMelon.SetFilePath("UserData/HoverAP.cfg", true);

            var copyDict = new Dictionary<string, string>(apServerInfo);

            foreach (var kvp in apServerInfo)
            {
                var entry = apMelon.GetEntry<string>(kvp.Key);
                copyDict[kvp.Key] = entry is null ? apMelon.CreateEntry<string>(kvp.Key, kvp.Value).Value : entry.Value;
            }

            apServerInfo = copyDict;
            try
            {
                session = ArchipelagoSessionFactory.CreateSession(apServerInfo["url"], Convert.ToInt32(apServerInfo["port"]));
            } catch (Exception e)
            {
                LoggerInstance.Error(e.ToString());
            }

            Login();

        }

        private bool Login()
        {
            bool success = false;
            try
            {
                foreach (var kvp in apServerInfo)
                {
                    LoggerInstance.Msg($"{kvp.Key}: {kvp.Value}");
                }
                string[] tags = { "IgnoreGame" };
                var info = session.TryConnectAndLogin(
                    "Minecraft",
                    apServerInfo["slot"],
                    new Version(0, 3, 4),
                    Archipelago.MultiClient.Net.Enums.ItemsHandlingFlags.RemoteItems,
                    tags
                    );

                success = info.Successful;

                if(info is LoginFailure fail)
                {
                    foreach(var err in fail.Errors)
                    {
                        LoggerInstance.Error(err);
                    }
                }else if(info is LoginSuccessful loggedin)
                {
                    LoggerInstance.Msg("Slot: {0}", loggedin.Slot);
                    LoggerInstance.Msg("Team: {0}", loggedin.Team);
                }

                LoggerInstance.Msg("login success {0}", success);

            }
            catch (Exception e)
            {
                LoggerInstance.Error(e.ToString());
            }

            return success;
        }
    }
}
