using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HG;
using BroadcastPerch.Content;
using R2API;
using R2API.AddressReferencedAssets;
using R2API.Utils;
using RoR2;
using RoR2.ContentManagement;
using RoR2BepInExPack.GameAssetPaths;
using RoR2BepInExPack.GameAssetPathsBetter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Diagnostics;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using RoR2.Navigation;
//Copied from a private Unity project I use for testing maps copied from Ancient Observatory copied from Wetland Downpour copied from Fogbound Lagoon copied from Nuketown


#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace BroadcastPerch
{
    [BepInPlugin(GUID, Name, Version)]
    public class BroadcastPerch : BaseUnityPlugin
    {
        public const string Author = "wormsworms";

        public const string Name = "Broadcast_Perch";

        public const string Version = "1.2.0";

        public const string GUID = Author + "." + Name;

        public static BroadcastPerch instance;

        public static ConfigEntry<bool> enableRegular;
        public static ConfigEntry<bool> enableSimulacrum;
        public static ConfigEntry<bool> stage1Simulacrum;
        public static ConfigEntry<preferredOST> mapOST;

        public static ConfigEntry<bool> toggleSpider;
        public static ConfigEntry<bool> toggleSpitter;

        public static ConfigEntry<bool> toggleWayfarer;
        public static ConfigEntry<bool> toggleMimic;

        public static ConfigEntry<bool> toggleBrassMonolith;

        public const string mapName = "broadcastperch_wormsworms";
        public const string simuName = "itbroadcastperch_wormsworms";

        public enum preferredOST
        {
            The_Treehouse_that_Time_Forgot,
            The_Raindrop_that_Fell_to_the_Sky
        }

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);

            ConfigSetup();

            ContentManager.collectContentPackProviders += GiveToRoR2OurContentPackProviders;

            RoR2.Language.collectLanguageRootFolders += CollectLanguageRootFolders;

            On.RoR2.MusicController.StartIntroMusic += MusicController_StartIntroMusic;

            SceneManager.sceneLoaded += SceneSetup;

            RoR2.RoR2Application.onLoadFinished += AddModdedEnemies;

        }

        private void MusicController_StartIntroMusic(On.RoR2.MusicController.orig_StartIntroMusic orig, RoR2.MusicController self)
        {
            orig(self);
            AkSoundEngine.PostEvent("WORM_Perch_Play_Music_System", self.gameObject);
        }

        public static void AddModdedEnemies()
        {
            if (IsEnemiesReturns.enabled)
            {
                EnemiesReturnsCompat.AddEnemies(); //Mechanical Spider, Spitter
            }
            if (IsStarstorm2.enabled)
            {
                Starstorm2Compat.AddEnemies(); //Wayfarer, Mimic
            }
            if (IsForgottenRelics.enabled)
            {
                ForgottenRelicsCompat.AddEnemies(); //Brass Monolith
            }
        }

        private void Destroy()
        {
            RoR2.Language.collectLanguageRootFolders -= CollectLanguageRootFolders;
        }

        private static void GiveToRoR2OurContentPackProviders(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
        {
            addContentPackProvider(new ContentProvider());
        }

        public void CollectLanguageRootFolders(List<string> folders)
        {
            folders.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(base.Info.Location), "Language"));
            folders.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(base.Info.Location), "Plugins/Language"));
        }

        private void SceneSetup(Scene newScene, LoadSceneMode loadSceneMode)
        {
            if (newScene.name == mapName)
            {
                // Disable collision on OOB ring prefabs
                Transform ringHolder = GameObject.Find("HOLDER: Skybox/Rings").transform;
                for (int i = 0; i < ringHolder.childCount; i++)
                {
                    GameObject ring = ringHolder.GetChild(i).GetChild(0).gameObject;
                    ring.GetComponent<MeshCollider>().enabled = false;
                    ring.layer = 0;
                }

                GameObject[] miscObjects = { GameObject.Find("RANDOM: Short Center Log/Props/Generator"),
                                             GameObject.Find("RANDOM: Short Center Log/Props/Generator (2)"),
                                             GameObject.Find("RANDOM: Short Center Log/Props/Generator (4)")
                                            };
                foreach (GameObject miscObject in miscObjects)
                {
                    if (miscObject.transform.GetChild(0) != null)
                    {
                        miscObject.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial = BroadcastPerchContent.treetopBlueMetal;
                    }
                }

            }

            if (newScene.name == mapName || newScene.name == simuName)
            {
                AmbienceSetup();
                BroadcastMusicString();
                // Swapping out metal materials for various objects. surely there must be a more efficient way to do this. oh well
                Transform generatorHolder = GameObject.Find("Human Props/Generators").transform;
                for (int i = 0; i < generatorHolder.childCount; i++)
                {
                    GameObject generator = generatorHolder.GetChild(i).GetChild(0).gameObject;
                    generator.GetComponent<MeshRenderer>().sharedMaterial = BroadcastPerchContent.treetopBlueMetal;
                }
                Transform towerHolder = GameObject.Find("Human Props/Cell Towers").transform;
                for (int i = 0; i < towerHolder.childCount; i++)
                {
                    GameObject tower = towerHolder.GetChild(i).GetChild(0).GetChild(0).gameObject;
                    tower.GetComponent<MeshRenderer>().sharedMaterial = BroadcastPerchContent.treetopMetal;
                }
                Transform containerHolder = GameObject.Find("Human Props/Shipping Containers").transform;
                for (int i = 0; i < containerHolder.childCount; i++)
                {
                    GameObject container = containerHolder.GetChild(i).GetChild(0).gameObject;
                    container.GetComponent<MeshRenderer>().sharedMaterial = BroadcastPerchContent.treetopBlueMetal;
                }
                GameObject[] miscObjects = { GameObject.Find("RANDOM: Tall Center Log/Props/Generator"),
                                             GameObject.Find("RANDOM: Tall Center Log/Props/Generator (2)"),
                                             GameObject.Find("RANDOM: Tall Center Log/Props/Generator (4)")};
                foreach (GameObject miscObject in miscObjects)
                {
                    if (miscObject.transform.GetChild(0) != null)
                    {
                        miscObject.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial = BroadcastPerchContent.treetopBlueMetal;
                    }
                }
                //Destroy unnecessary light attached to prefab
                GameObject.Destroy(GameObject.Find("Dish Light/SM_Light3(Clone)/Light/Point Light"));
            }
        
        }

        private void AmbienceSetup()
        {
            GameObject ambience = GameObject.Find("Ambience");
            if (ambience)
            {
                AkBank bank = ambience.GetComponent<AkBank>();
                AkAmbient[] ambientList = ambience.GetComponents<AkAmbient>();
                AkAmbient ambient1 = ambientList[0];
                AkAmbient ambient2 = ambientList[1];
                if (bank)
                {
                    WwiseBankReference lakeSound = Addressables.LoadAssetAsync<WwiseBankReference>("Wwise/B3099A00-993A-4AD4-86FD-EBD151F09FB5.asset").WaitForCompletion();
                    WwiseEventReference startLakeSound = Addressables.LoadAssetAsync<WwiseEventReference>("Wwise/6C9A5B06-3C87-4DD2-835F-B0F2385B7700.asset").WaitForCompletion();
                    WwiseEventReference stopSound = Addressables.LoadAssetAsync<WwiseEventReference>("Wwise/6F2ADD1C-BD55-431F-A62F-80CCD5F9631D.asset").WaitForCompletion();
                    bank.data.WwiseObjectReference = lakeSound;
                    ambient1.data.WwiseObjectReference = startLakeSound;
                    ambient2.data.WwiseObjectReference = stopSound;
                }
            }
            else
            {
                Log.Error("no ambience :(");
            }
        }

        private void BroadcastMusicString()
        {
            if (!NetworkServer.active) return;

            string bgSongToken = "WORM_CHAT_BP_SONGPLAYING";

            if (BroadcastPerchContent.treetopSceneDef.mainTrack.cachedName == "BroadcastPerchMainMusic")
            {
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = bgSongToken });
            }
        }

        private void ConfigSetup()
        {
            enableRegular =
                base.Config.Bind<bool>("00 - Stages",
                                       "Enable Broadcast Perch",
                                       true,
                                       "If true, Broadcast Perch can appear in regular runs.");
            enableSimulacrum =
                base.Config.Bind<bool>("00 - Stages",
                                       "Enable Simulacrum Variant",
                                       true,
                                       "If true, Broadcast Perch can appear in the Simulacrum.");
            stage1Simulacrum =
                base.Config.Bind<bool>("00 - Stages",
                                       "Enable Simulacrum Variant on Stage 1",
                                       false,
                                       "If false, Broadcast Perch will only appear after clearing at least one stage in the Simulacrum, like Commencement.");
            mapOST =
                base.Config.Bind<preferredOST>("00 - Stages",
                                        "Soundtrack - Stage Music",
                                        preferredOST.The_Treehouse_that_Time_Forgot,
                                        "Set the stage's soundtrack. 'The Raindrop that Fell to the Sky' was the original track used prior to version 1.2.0.");
            toggleSpider =
                base.Config.Bind<bool>("01 - Monsters: EnemiesReturns",
                                       "Enable Mechanical Spider",
                                       true,
                                       "If true, Mechanical Spiders will appear in Broadcast Perch.");
            toggleSpitter =
                base.Config.Bind<bool>("01 - Monsters: EnemiesReturns",
                                       "Enable Spitter",
                                       true,
                                       "If true, Spitters will appear in Broadcast Perch.");
            toggleBrassMonolith =
                base.Config.Bind<bool>("02 - Monsters: Forgotten Relics",
                                       "Enable Brass Monolith",
                                       true,
                                       "If true, Brass Monoliths will appear in Broadcast Perch (after clearing 5 stages).");
            toggleWayfarer =
                base.Config.Bind<bool>("03 - Monsters: Starstorm 2",
                                       "Enable Wayfarer",
                                       true,
                                       "If true, Wayfarers will appear in Broadcast Perch.");
            toggleMimic =
                base.Config.Bind<bool>("03 - Monsters: Starstorm 2",
                                       "Enable Security Chest",
                                       true,
                                       "If true, Security Chests (Mimics) will appear in Broadcast Perch.");
        }
    }
}
