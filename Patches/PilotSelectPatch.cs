using HarmonyLib;
using ModLoader.Classes;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using VTOLVR.Multiplayer;
using static Rewired.UI.ControlMapper.ControlMapper;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;
using Object = UnityEngine.Object;

namespace CustomAircraftTemplateJAS39
{
    //You should not need to edit any of this code!

    public class PilotSelectPatch : MonoBehaviour
    {


        public static void SetNVGShader()
        {
            GameObject helmetCamera = AircraftAPI.GetChildWithName(Main.aircraftCustom, "Camera (eye) Helmet", false);
            ScreenMaskedColorRamp smcr = helmetCamera.GetComponent<ScreenMaskedColorRamp>();
            Shader NVGShader = Shader.Find("Hidden/Grayscale Effect NVG");
            smcr.shader = NVGShader;


        }




    }


    [HarmonyPatch(typeof(BlackoutEffect), "LateUpdate")]
    public class BlackoutPatchPost
    {
        private static bool Prefix(BlackoutEffect __instance)
        {




            Traverse trav1 = Traverse.Create(__instance);
            float num = Mathf.Abs((float)trav1.Field("gAccum").GetValue()) * __instance.aFactor;
            trav1.Field("alpha").SetValue(Mathf.Lerp((float)trav1.Field("gAccum").GetValue(), num, 20f * Time.deltaTime));
            //Color32 alphaCol = __instance.quadRenderer.GetComponent<MeshRenderer>().material.GetColor("_Cutoff");
            //Debug.Log("BPP 1.0:" + alphaCol);
            float newAlpha = (float)trav1.Field("alpha").GetValue();
            //Debug.Log("BPP 1.0:" + newAlpha);
            if (newAlpha < 0.001f)
            {
                __instance.quadRenderer.enabled = false;

                return false;
            }

            __instance.quadRenderer.enabled = true;
            if (newAlpha > 1.0f) { newAlpha = 1.0f; }
            if (newAlpha < -0.000001f) { newAlpha = 0.0f; }


            //Debug.Log("BPP 1.1:" + newAlpha);
            Color colorNew = new Color(0f, 0f, 0f, newAlpha);
            __instance.quadRenderer.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color", colorNew);
            if (!__instance.accelDied && __instance.useFlightInfo && __instance.flightInfo && __instance.flightInfo.maxInstantaneousG > __instance.instantaneousGDeath)
            {
                FlightLogger.Log("Died by instantaneous G-force (" + __instance.flightInfo.maxInstantaneousG.ToString("0.0") + ")");
                __instance.AccelDie();
            }
            return false;
        }

    }

    //    [HarmonyPatch(typeof(BlackoutEffect), nameof(BlackoutEffect.Start))]
    //public class BlackoutPatch
    //{
    //    private static bool Prefix(BlackoutEffect __instance)
    //    {
    //        Debug.Log("BProfile 1.0");
    //        Traverse trav1 = Traverse.Create(__instance);
    //        Debug.Log("BProfile 1.2");
    //        trav1.Field("images").SetValue(__instance.GetComponentsInChildren<Image>());
    //        Debug.Log("BProfile 1.1");
    //        if (__instance.quadRenderer)
    //        {
    //            Debug.Log("BProfile 1.3");
    //            Image[] array = (Image[])trav1.Field("images").GetValue();
    //            for (int i = 0; i < array.Length; i++)
    //            {
    //                Debug.Log("BProfile 1.4");
    //                array[i].gameObject.SetActive(false);
    //            }
    //           MaterialPropertyBlock mat = new MaterialPropertyBlock();
    //            Debug.Log("BProfile 1.5");
    //            trav1.Field("quadProps").SetValue(mat);
    //            Debug.Log("BProfile 1.6");
    //            trav1.Field("colorID").SetValue(Shader.PropertyToID("_TintColor"));
    //        }
    //        trav1.Field("nvg").SetValue(__instance.GetComponentInParent<NightVisionGoggles>());
    //        if (!__instance.flightInfo)
    //        {
    //            __instance.flightInfo = __instance.GetComponentInParent<FlightInfo>();
    //        }
    //        VehicleMaster componentInParent = __instance.GetComponentInParent<VehicleMaster>();
    //        if (componentInParent)
    //        {
    //            componentInParent.OnPilotDied += __instance.AccelDie;
    //        }

    //        return false;
    //    }
    //}


    [HarmonyPatch(typeof(VTResources), nameof(VTResources.LoadExternalVehicle))]
    public class OkuPatch
    {
        private static bool Prefix()
        {
            var t = Traverse.Create(typeof(VTResources));
            t.Field<bool>("canLoadExternalVehicles").Value = true;

            return true;
        }
    }









    [HarmonyPatch(typeof(PlayerSpawn), nameof(PlayerSpawn.OnSpawnUnit))]
    public class PlayerSpawnSelectedVehicleinScenario
    {
        private static bool Prefix()
        {
            if (PilotSaveManager.currentVehicle.vehicleName != AircraftInfo.vehicleName) { return true; }
            VTScenario.current.vehicle = PilotSaveManager.currentVehicle;
            Main.aircraftCustom = FlightSceneManager.instance.playerActor.gameObject;
            return true;

        }

    }
    [HarmonyPatch(typeof(MultiplayerSpawn), nameof(MultiplayerSpawn.OnSpawnUnit))]
    public class MPlayerSpawnSelectedVehicleinScenario
    {
        private static bool Prefix()
        {
            if (PilotSaveManager.currentVehicle.vehicleName != AircraftInfo.vehicleName) { return true; }
            // VTScenario.current.vehicle = PilotSaveManager.currentVehicle;
            Main.aircraftCustom = FlightSceneManager.instance.playerActor.gameObject;
            return true;

        }

    }

    [HarmonyPatch(typeof(PilotSelectUI), nameof(PilotSelectUI.StartSelectedPilotButton))]
    public class StartPilot
    {
        private static bool Prefix(PilotSelectUI __instance)
        {
            //Debug.Log("JAS39 StartPilot Patch 1.0");
            var traverse = Traverse.Create(__instance);
            traverse.Field("vehicles").SetValue(PilotSaveManager.GetVehicleList());
            return true;
        }
    }

    [HarmonyPatch(typeof(PlayerSpawn), nameof(PlayerSpawn.OnPreSpawnUnit))]
    public class OnPreSpawnSelectedVehicleinScenario
    {
        private static bool Prefix()
        {
            //if (PilotSaveManager.currentVehicle.vehicleName != AircraftInfo.vehicleName) { return true; }
            VTScenario.current.vehicle = PilotSaveManager.currentVehicle;
            return true;
        }
    }
    [HarmonyPatch(typeof(LoadoutConfigurator), nameof(LoadoutConfigurator.Initialize))]
    public class LoadInAircraftsWeapons
    {


        public static void Prefix(LoadoutConfigurator __instance)
        {
            if (PilotSaveManager.currentVehicle.vehicleName != AircraftInfo.vehicleName) { return; }
            __instance.availableEquipStrings.Clear();
            //Debug.Log("JAS39 LIAWPF  1.0");
            //Debug.Log("JAS39 LIAWPF  1.2");
            foreach (string item2 in PilotSaveManager.currentVehicle.GetEquipNamesList())
            {
                //Debug.Log("JAS39 LIAWPF  1.3" + item2);
                //if (!(VTScenario.currentScenarioInfo.gameVersion > new GameVersion(1, 3, 0, 30, GameVersion.ReleaseTypes.Testing)) || VTScenario.currentScenarioInfo.allowedEquips.Contains(item2))
                {
                    //Debug.Log("JAS39 LIAWPF  1.4" + item2);
                    __instance.availableEquipStrings.Add(item2);

                }
            }
            //Debug.Log("JAS39 LIAWPF  1.5");

        }

    }



    [HarmonyPatch(typeof(VTOLQuickStart), "FireStartFlyingEvents")]

    public class JAS39_FireStartFlyingEvents_post
    {
        public static void Postfix(VTOLQuickStart __instance)
        {

            //Debug.Log("JAS39  1.27");

            __instance.StartCoroutine(VehicleDelayedFunctions.SetupVehicleWithDelays());

            //Debug.Log("JAS39  1.28");

        }
    }
    public static class VehicleDelayedFunctions
    {
        public static IEnumerator SetupVehicleWithDelays()
        {
            //Debug.Log("JAS39  1.27.1");
            yield return new WaitForSeconds(3f);
            //Debug.Log("JAS39  1.27.2");
            CustomElements.SetUpGauges();
            // Debug.Log("JAS39  1.27.3");
            PilotSelectPatch.SetNVGShader();
            // Debug.Log("JAS39  1.27.4");
            yield break;
        }
    }


    [HarmonyPatch(typeof(VehicleConfigSceneSetup))]
    public class VehicleConfigSceneSetupSettingforAllWeapons
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(VehicleConfigSceneSetup), "Start", new Type[] { });
        }

        private static bool Prefix()
        {
            if (PilotSaveManager.currentVehicle.vehicleName != AircraftInfo.vehicleName)
            {
                return true;
            }
            PilotSaveManager.currentCampaign.isStandaloneScenarios = false;
            return true;
        }
    }
}