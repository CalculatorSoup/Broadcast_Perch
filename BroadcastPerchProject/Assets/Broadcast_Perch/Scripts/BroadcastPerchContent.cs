using HG;
using BroadcastPerch.Content;
using R2API;
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using RoR2.Networking;
using RoR2BepInExPack.GameAssetPaths;
using ShaderSwapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using static RoR2.Console;
using static UnityEngine.UI.Image;

namespace BroadcastPerch.Content
{
    public static class BroadcastPerchContent
    {

        internal const string ScenesAssetBundleFileName = "BroadcastPerchScenes";
        internal const string AssetsAssetBundleFileName = "BroadcastPerchAssets";

        private static AssetBundle _scenesAssetBundle;
        private static AssetBundle _assetsAssetBundle;

        internal static UnlockableDef[] UnlockableDefs;
        internal static SceneDef[] SceneDefs;

        //Broadcast Perch
        internal static SceneDef treetopSceneDef;
        internal static Sprite treetopSceneDefPreviewSprite;
        internal static Material treetopBazaarSeer;
        internal static Material treetopMetal;
        internal static Material treetopBlueMetal;

        //Simulacrum
        internal static SceneDef simuSceneDef;
        internal static Sprite simuSceneDefPreviewSprite;
        internal static Material simuBazaarSeer;


        public static List<Material> SwappedMaterials = new List<Material>();

        internal static IEnumerator LoadAssetBundlesAsync(AssetBundle scenesAssetBundle, AssetBundle assetsAssetBundle, IProgress<float> progress, ContentPack contentPack)
        {
            _scenesAssetBundle = scenesAssetBundle;
            _assetsAssetBundle = assetsAssetBundle;
            
            var upgradeStubbedShaders = _assetsAssetBundle.UpgradeStubbedShadersAsync();
            while (upgradeStubbedShaders.MoveNext())
            {
                yield return upgradeStubbedShaders.Current;
            }

            yield return LoadAllAssetsAsync(assetsAssetBundle, progress, (Action<UnlockableDef[]>)((assets) =>
            {
                contentPack.unlockableDefs.Add(assets);
            }));

            
            yield return LoadAllAssetsAsync(_assetsAssetBundle, progress, (Action<Sprite[]>)((assets) =>
            {
                treetopSceneDefPreviewSprite = assets.First(a => a.name == "texBPScenePreview");
                simuSceneDefPreviewSprite = assets.First(a => a.name == "texBPScenePreview");
            }));
            

            yield return LoadAllAssetsAsync(_assetsAssetBundle, progress, (Action<Material[]>)((assets) =>
            {
                treetopMetal = assets.First(a => a.name == "matPTMetal");
                treetopBlueMetal = assets.First(a => a.name == "matPTBlueMetal");
            }));

            yield return LoadAllAssetsAsync(_assetsAssetBundle, progress, (Action<SceneDef[]>)((assets) =>
            {
                SceneDefs = assets;
                treetopSceneDef = SceneDefs.First(sd => sd.baseSceneNameOverride == BroadcastPerch.mapName);
                simuSceneDef = SceneDefs.First(sd => sd.baseSceneNameOverride == BroadcastPerch.simuName);
                Log.Debug(treetopSceneDef.nameToken);
                contentPack.sceneDefs.Add(assets);
            }));

            treetopSceneDef.portalMaterial = R2API.StageRegistration.MakeBazaarSeerMaterial((Texture2D)treetopSceneDef.previewTexture);

            var mainTrackDefRequest = Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/MusicTrackDefs/muSong13.asset");
            while (!mainTrackDefRequest.IsDone)
            {
                yield return null;
            }
            var bossTrackDefRequest = Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/MusicTrackDefs/muSong05.asset");
            while (!bossTrackDefRequest.IsDone)
            {
                yield return null;
            }

            treetopSceneDef.mainTrack = mainTrackDefRequest.Result;
            treetopSceneDef.bossTrack = bossTrackDefRequest.Result;

            simuSceneDef.mainTrack = treetopSceneDef.mainTrack;
            simuSceneDef.bossTrack = treetopSceneDef.bossTrack;

            if (BroadcastPerch.enableRegular.Value)
            {
                R2API.StageRegistration.RegisterSceneDefToNormalProgression(treetopSceneDef);
            }

            if (BroadcastPerch.enableSimulacrum.Value && BroadcastPerch.stage1Simulacrum.Value)
            {
                Simulacrum.RegisterSceneToSimulacrum(simuSceneDef, true);
            } else if (BroadcastPerch.enableSimulacrum.Value && !BroadcastPerch.stage1Simulacrum.Value)
            {
                Simulacrum.RegisterSceneToSimulacrum(simuSceneDef, false);
            }

        }

        internal static void Unload()
        {
            _assetsAssetBundle.Unload(true);
            _scenesAssetBundle.Unload(true);
        }

        private static IEnumerator LoadAllAssetsAsync<T>(AssetBundle assetBundle, IProgress<float> progress, Action<T[]> onAssetsLoaded) where T : UnityEngine.Object
        {
            var sceneDefsRequest = assetBundle.LoadAllAssetsAsync<T>();
            while (!sceneDefsRequest.isDone)
            {
                progress.Report(sceneDefsRequest.progress);
                yield return null;
            }

            onAssetsLoaded(sceneDefsRequest.allAssets.Cast<T>().ToArray());

            yield break;
        }
    }
}
