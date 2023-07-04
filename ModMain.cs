using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using Archipelago.MultiClient.Net;
using System.Linq;

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
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            MelonLogger.Msg("OnSceneWasLoaded: " + buildIndex.ToString() + " | " + sceneName);
            switch (buildIndex)
            {
                case 0:
                    break;
                case 10:
                    Login();
                    break;
                default:
                    break;

            }
            
        }


        public static ArchipelagoSession session;
        private MelonPreferences_Category apMelon;
        public override void OnInitializeMelon()
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

            }
            catch (Exception e)
            {
                LoggerInstance.Error(e.ToString());
                LoggerInstance.Error("SessionFailed");
            }

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
                var playerList = session.Players.AllPlayers.Select(player => player.Name).ToArray();

                LoggerInstance.Msg(String.Join(", ", playerList ));
            }


        }

        private void Login()
        {
            try
            {
                foreach (var kvp in apServerInfo)
                {
                    LoggerInstance.Msg($"{kvp.Key}: {kvp.Value}");
                }
                string[] tags = { "IgnoreGame" };
                var info = session.TryConnectAndLogin(
                    "Timespinner",
                    apServerInfo["slot"],
                    Archipelago.MultiClient.Net.Enums.ItemsHandlingFlags.RemoteItems
                    );


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

            }
            catch (Exception e)
            {
                LoggerInstance.Error(e.ToString());
            }
        }
    }
}
