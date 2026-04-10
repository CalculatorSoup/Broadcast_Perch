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

        public const string Version = "1.0.0";

        public const string GUID = Author + "." + Name;

        public static BroadcastPerch instance;

        public static ConfigEntry<bool> enableRegular;
        public static ConfigEntry<bool> enableSimulacrum;
        public static ConfigEntry<bool> stage1Simulacrum;

        public static ConfigEntry<bool> toggleSpider;
        public static ConfigEntry<bool> toggleSpitter;

        public static ConfigEntry<bool> toggleWayfarer;

        public static ConfigEntry<bool> toggleBrassMonolith;

        public const string mapName = "broadcastperch_wormsworms";
        public const string simuName = "itbroadcastperch_wormsworms";
        private static GameObject fanPrefab;


        private void Awake()
        {
            instance = this;

            Log.Init(Logger);

            ConfigSetup();

            ContentManager.collectContentPackProviders += GiveToRoR2OurContentPackProviders;

            RoR2.Language.collectLanguageRootFolders += CollectLanguageRootFolders;

            SceneManager.sceneLoaded += SceneSetup;

            RoR2.RoR2Application.onLoadFinished += AddModdedEnemies;

        }

        public static void AddModdedEnemies()
        {
            if (IsEnemiesReturns.enabled)
            {
                EnemiesReturnsCompat.AddEnemies(); //Mechanical Spider, Spitter
            }
            if (IsStarstorm2.enabled)
            {
                Starstorm2Compat.AddEnemies(); //Wayfarer
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

            }

            if (newScene.name == mapName || newScene.name == simuName)
            {
                Log.Debug("looking for objects");

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
                Transform containerHolder = GameObject.Find("Human Props/Containers").transform;
                for (int i = 0; i < containerHolder.childCount; i++)
                {
                    GameObject container = containerHolder.GetChild(i).GetChild(0).gameObject;
                    container.GetComponent<MeshRenderer>().sharedMaterial = BroadcastPerchContent.treetopBlueMetal;
                }
                Transform shipPiece = GameObject.Find("RANDOM: Tall Center Log/Props/Ship Piece").transform;
                if (shipPiece.GetChild(0).GetChild(0).GetChild(0) != null)
                {
                    shipPiece.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>().sharedMaterial = BroadcastPerchContent.treetopMetal;
                    shipPiece.GetChild(0).GetChild(0).GetChild(0).gameObject.layer = 11;
                }
                Transform shipDebris = GameObject.Find("RANDOM: Short Center Log/Props/Debris (3)").transform;
                if (shipDebris.GetChild(0) != null)
                {
                    shipDebris.GetChild(0).gameObject.GetComponent<MeshRenderer>().sharedMaterial = BroadcastPerchContent.treetopMetal;
                }

                GameObject[] miscObjects = { GameObject.Find("RANDOM: Short Center Log/Props/Generator"),
                                             GameObject.Find("RANDOM: Short Center Log/Props/Generator (2)"),
                                             GameObject.Find("RANDOM: Short Center Log/Props/Generator (4)"),
                                             GameObject.Find("RANDOM: Tall Center Log/Props/Generator"),
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
                GameObject.Destroy(GameObject.Find("Dish Light/FW_Light2On(Clone)/LightPostLight"));

                //Unused code for editing / retexturing fans. I couldn't get them to work properly, so the map uses Aphelian Sanctuary jump pads instead. very sad and I cry
                
                /*
                Transform fanHolder = GameObject.Find("HOLDER: Jump Pads/Fans").transform;
                Transform tpRelay = null;
                if (newScene.name == mapName)
                {
                    tpRelay = GameObject.Find("SceneInfo/TP Relay").transform;
                }
                for (int i = 0; i < fanHolder.childCount; i++)
                {
                    //Log.Debug(fanHolder.GetChild(i).name);
                    //Log.Debug(fanHolder.GetChild(i).GetChild(0).name);
                    //GameObject fan = fanHolder.GetChild(i).GetChild(0).gameObject;


                    //fan.transform.GetChild(0).GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMaterial = BroadcastPerchContent.treetopBlueMetal;
                    //var fanBehavior = fan.transform.GetComponent<RoR2.ChestBehavior>();
                    //var fanPC = fan.transform.GetComponent<RoR2.PurchaseInteraction>();

                    //fanBehavior.Open();
                    //fanPC.SetAvailable(false);
                    

                    //NetworkServer.Spawn(fanHolder.GetChild(i).gameObject);
                    //NetworkServer.Spawn(fan);

                    if (newScene.name == mapName && tpRelay != null)
                    {
                        var tpRelayComponent = tpRelay.GetComponent<RoR2.EntityLogic.TeleporterEventRelay>();
                        tpRelayComponent.onTeleporterBeginCharging.AddListener(fanBehavior.Open);
                        tpRelayComponent.onTeleporterBeginCharging.AddListener(delegate { fanPC.SetAvailable(false); });

                    }
                }
                */
                
            }
        
        }
        

        public static void CreateFanPrefab(ContentPack contentPack)
        {
            var fan = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_frozenwall.FW_HumanFan_prefab).WaitForCompletion();
            fanPrefab = fan.InstantiateClone("TreetopFan", true);

            fanPrefab.TryGetComponent(out RoR2.PurchaseInteraction pi);
            pi.automaticallyScaleCostWithDifficulty = true;
            pi.cost = 5;

            fanPrefab.transform.GetChild(0).GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMaterial = BroadcastPerchContent.treetopBlueMetal;

            var particleRenderer1 = fanPrefab.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>().main;
            var particleRenderer2 = fanPrefab.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<ParticleSystem>().main;
            var particleRenderer3 = fanPrefab.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<ParticleSystem>().main;
            var particleRenderer4 = fanPrefab.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<ParticleSystem>().main;

            particleRenderer1.startColor = Color.gray;
            particleRenderer2.startColor = Color.gray;
            particleRenderer3.startColor = Color.gray;
            particleRenderer4.startColor = Color.gray;

            fanPrefab = fanPrefab.InstantiateClone("TreetopFan", true);


            contentPack.networkedObjectPrefabs.Add(new GameObject[] { fanPrefab });
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
        }
    }
}
