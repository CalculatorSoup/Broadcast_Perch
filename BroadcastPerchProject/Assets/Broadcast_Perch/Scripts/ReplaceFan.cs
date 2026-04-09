using BroadcastPerch.Content;
using R2API;
using RoR2;
using RoR2.ContentManagement;
using RoR2.Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using System;
using System.Text;
using LOP;
using RoR2BepInExPack.GameAssetPaths;

namespace BroadcastPerch
{
    public class ReplaceFan : MonoBehaviour
    {
        private GameObject fanInstance = null;
        private GameObject fan;
        private GameObject oldFan;
        private void Awake()
        {

            if (NetworkServer.active)
            {
                fan = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPathsBetter.RoR2_Base_frozenwall.FW_HumanFan_prefab).WaitForCompletion();
                oldFan = gameObject.transform.GetChild(0).gameObject;
                fan.transform.GetChild(0).GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMaterial = BroadcastPerchContent.treetopBlueMetal;

                JumpVolume fanJV = gameObject.GetComponent<RoR2.JumpVolume>();
                JumpVolume newFanJV = fan.transform.GetChild(0).GetChild(0).GetComponent<RoR2.JumpVolume>();

                if (newFanJV)
                {
                    Log.Debug("new fan's jump volume found");
                }

                newFanJV.jumpSoundString = fanJV.jumpSoundString;
                newFanJV.jumpVelocity = fanJV.jumpVelocity;
                newFanJV.time = fanJV.time;
                newFanJV.targetElevationTransform = fanJV.targetElevationTransform;

                InstantiateGeyserPrefab fanGSS = gameObject.GetComponent<InstantiateGeyserPrefab>();
                GateStateSetter newFanGSS = fan.transform.GetChild(0).GetChild(0).gameObject.AddComponent<GateStateSetter>();

                newFanGSS.gateToDisableWhenEnabled = fanGSS.gateToDisableWhenPurchased;
                newFanGSS.gateToEnableWhenEnabled = fanGSS.gateToEnableWhenPurchased;
                fanInstance = UnityEngine.Networking.NetworkManager.Instantiate(fan, oldFan.transform.position, oldFan.transform.rotation, gameObject.transform);
                //R2API.PrefabAPI.RegisterNetworkPrefab(fanInstance);
                NetworkServer.Spawn(fanInstance);
                GameObject.Destroy(oldFan);
                UnityEngine.Networking.NetworkManager.Destroy(oldFan);


            }
        }


    }
}
