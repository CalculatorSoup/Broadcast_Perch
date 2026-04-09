using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using EnemiesReturns.Enemies.Judgement.Arraign;
using EntityStates.NemCaptain.Weapon;
using FRCSharp;
using HG.Reflection;
using Microsoft.CodeAnalysis;
using R2API;
using R2API.Utils;
using RoR2;
using BroadcastPerch;
using RoR2.Navigation;
using RoR2BepInExPack.GameAssetPaths.Version_1_35_0;
using RoR2BepInExPack.GameAssetPathsBetter;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static R2API.DirectorAPI;

namespace BroadcastPerch
{
    public class ForgottenRelicsCompat
    {
        public static void AddEnemies()
        {
            // Brass Monolith

            if (BroadcastPerch.toggleBrassMonolith.Value && !FRCSharp.VF2ConfigManager.disableBrassMonolith.Value)
            {
                DirectorCardHolder MonolithHolder = new DirectorCardHolder
                {
                    Card = new DirectorCard
                    {
                        spawnCard = (SpawnCard)(object)VF2ContentPackProvider.cscBrassMonolith,
                        selectionWeight = 1,
                        minimumStageCompletions = 5
                    },
                    MonsterCategory = MonsterCategory.Champions
                };
                DirectorAPI.Helpers.AddNewMonsterToStage(MonolithHolder, false, DirectorAPI.Stage.Custom, BroadcastPerch.mapName);
                DirectorAPI.Helpers.AddNewMonsterToStage(MonolithHolder, false, DirectorAPI.Stage.Custom, BroadcastPerch.simuName);
                Log.Debug("Brass Monolith added to Broadcast Perch spawn pool.");
            };
        }
    }
}