using EnemiesReturns;
using EnemiesReturns.Configuration;
using EnemiesReturns.Enemies.Colossus;
using EnemiesReturns.Enemies.MechanicalSpider.Enemy;
using EnemiesReturns.Enemies.Spitter;
using EnemiesReturns.Enemies.Swift;
using R2API;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BroadcastPerch
{
    public class EnemiesReturnsCompat
    {
        public static void AddEnemies()
        {
            // Spitter
            if (BroadcastPerch.toggleSpitter.Value && General.EnableSpitter.Value)
            {
                var card = new RoR2.DirectorCard()
                {
                    spawnCard = (RoR2.SpawnCard)(object)SpitterBody.SpawnCards.cscSpitterDefault,
                    spawnDistance = RoR2.DirectorCore.MonsterSpawnDistance.Standard,
                    selectionWeight = Spitter.SelectionWeight.Value,
                    minimumStageCompletions = Spitter.MinimumStageCompletion.Value
                };

                var holder = new DirectorAPI.DirectorCardHolder
                {
                    Card = card,
                    MonsterCategory = DirectorAPI.MonsterCategory.Minibosses
                };

                if (!Spitter.DefaultStageList.Value.Contains(BroadcastPerch.mapName)) //Checking whether default stage list has this enemy to avoid adding a duplicate spawn card
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(holder, false, DirectorAPI.Stage.Custom, BroadcastPerch.mapName);
                    Log.Info("Spitter added to Broadcast Perch's spawn pool.");
                }
                if (!Spitter.DefaultStageList.Value.Contains(BroadcastPerch.simuName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(holder, false, DirectorAPI.Stage.Custom, BroadcastPerch.simuName);
                    Log.Info("Spitter added to Broadcast Perch's simulacrum spawn pool.");
                }

            }

            // Spider
            if (BroadcastPerch.toggleSpider.Value && General.EnableMechanicalSpider.Value)
            {
                var card = new RoR2.DirectorCard()
                {
                    spawnCard = (RoR2.SpawnCard)(object)MechanicalSpiderEnemyBody.SpawnCards.cscMechanicalSpiderGrassy,
                    spawnDistance = RoR2.DirectorCore.MonsterSpawnDistance.Standard,
                    selectionWeight = MechanicalSpider.SelectionWeight.Value,
                    minimumStageCompletions = MechanicalSpider.MinimumStageCompletion.Value
                };

                var holder = new DirectorAPI.DirectorCardHolder
                {
                    Card = card,
                    MonsterCategory = DirectorAPI.MonsterCategory.BasicMonsters
                };

                if (!MechanicalSpider.DefaultStageList.Value.Contains(BroadcastPerch.mapName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(holder, false, DirectorAPI.Stage.Custom, BroadcastPerch.mapName);
                    Log.Info("Mechanical Spider added to Broadcast Perch's spawn pool.");
                }
                if (!MechanicalSpider.DefaultStageList.Value.Contains(BroadcastPerch.simuName))
                {
                    DirectorAPI.Helpers.AddNewMonsterToStage(holder, false, DirectorAPI.Stage.Custom, BroadcastPerch.simuName);
                    Log.Info("Mechanical Spider added to Broadcast Perch's simulacrum spawn pool.");
                }

            }

        }
    }
}