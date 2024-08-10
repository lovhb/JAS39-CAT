using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using VTOLAPI;

namespace CustomAircraftTemplateJAS39 
{
    public class PIDFixer : MonoBehaviour
    {

        public static void IncreaseFlightAssistYawP()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


            //Debug.Log("IFARP 0.0");


            FlightAssist FA = go.GetComponent<FlightAssist>();
            FA.yawPID.kp = FA.yawPID.kp + 0.005f;
            FlightLogger.Log("fa yaw kp = " + FA.yawPID.kp);
        }
        public static void DecreaseFlightAssistYawP()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


            //Debug.Log("DFARP 0.0");


            FlightAssist FA = go.GetComponent<FlightAssist>();
            FA.yawPID.kp = FA.yawPID.kp - 0.005f;
            FlightLogger.Log("fa yawPID kp = " + FA.yawPID.kp);
        }
        public static void IncreaseFlightAssistRollP()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


            //Debug.Log("IFARP 0.0");


            FlightAssist FA = go.GetComponent<FlightAssist>();
            FA.rollPID.kp = FA.rollPID.kp + 0.05f;
            FlightLogger.Log("fa roll kp = " + FA.rollPID.kp);
        }
        public static void DecreaseFlightAssistRollP()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


            //Debug.Log("DFARP 0.0");


            FlightAssist FA = go.GetComponent<FlightAssist>();
            FA.rollPID.kp = FA.rollPID.kp - 0.05f;
            FlightLogger.Log("fa roll kp = " + FA.rollPID.kp);
        }

        public static void IncreaseFlightAssistpitchP()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


            //Debug.Log("IFAPP 0.0");


            FlightAssist FA = go.GetComponent<FlightAssist>();
            FA.pitchPID.kp = FA.pitchPID.kp + 0.05f;
            FlightLogger.Log("fa pitch kp = " + FA.pitchPID.kp);
        }
        public static void DecreaseFlightAssistpitchP()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


            //Debug.Log("DFAPP 0.0");


            FlightAssist FA = go.GetComponent<FlightAssist>();
            FA.pitchPID.kp = FA.pitchPID.kp - 0.05f;
            FlightLogger.Log("fa pitch kp = " + FA.pitchPID.kp);
        }


        public static void IncreaseAPAltP()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


            //Debug.Log("IAltP 0.0");


            VTOLAutoPilot AP = go.GetComponent<VTOLAutoPilot>();
            AP.altitudePitchPID.kp = AP.altitudePitchPID.kp + 0.05f;
            FlightLogger.Log("ap alt pitch kp = " + AP.altitudePitchPID.kp);
        }
        public static void DecreaseAPAltP()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


            //Debug.Log("DAltP 0.0");


            VTOLAutoPilot AP = go.GetComponent<VTOLAutoPilot>();
            AP.altitudePitchPID.kp = AP.altitudePitchPID.kp - 0.05f;
            FlightLogger.Log("ap alt pitch kp = " + AP.altitudePitchPID.kp);
        }

        public static void IncreaseAPAltC()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


            //Debug.Log("IAltP 0.0");


            VTOLAutoPilot AP = go.GetComponent<VTOLAutoPilot>();
            AP.altitudeClimbPID.kp = AP.altitudeClimbPID.kp + 0.05f;
            FlightLogger.Log("ap alt climb kp = " + AP.altitudeClimbPID.kp);
        }
        public static void DecreaseAPAltC()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


            //Debug.Log("DAltP 0.0");


            VTOLAutoPilot AP = go.GetComponent<VTOLAutoPilot>();
            AP.altitudeClimbPID.kp = AP.altitudeClimbPID.kp - 0.05f;
            FlightLogger.Log("ap alt climb kp = " + AP.altitudeClimbPID.kp);
        }

        public static void IncreaseAPHEadingR()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


            //Debug.Log("IAltP 0.0");



            VTOLAutoPilot AP = go.GetComponent<VTOLAutoPilot>();
            AP.headingRollPID.kp = AP.headingRollPID.kp + 0.05f;
            FlightLogger.Log("ap heading r kp = " + AP.headingRollPID.kp);
        }
        public static void DecreaseAPHEadingR()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


//Debug.Log("DAltP 0.0");


            VTOLAutoPilot AP = go.GetComponent<VTOLAutoPilot>();
            AP.headingRollPID.kp = AP.headingRollPID.kp - 0.05f;
            FlightLogger.Log("ap heading r kp = " + AP.headingRollPID.kp);
        }

        public static void IncreaseAPHeadingT()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


            //Debug.Log("IAltP 0.0");


            VTOLAutoPilot AP = go.GetComponent<VTOLAutoPilot>();
            AP.headingTurnPID.kp = AP.headingTurnPID.kp + 0.05f;
            FlightLogger.Log("ap hdg turn kp = " + AP.headingTurnPID.kp);
        }
        public static void DecreaseAPHeadingT()
        {
            GameObject go = VTAPI.GetPlayersVehicleGameObject();


            //Debug.Log("DAltP 0.0");


            VTOLAutoPilot AP = go.GetComponent<VTOLAutoPilot>();
            AP.headingTurnPID.kp = AP.headingTurnPID.kp - 0.05f;
            FlightLogger.Log("ap hdg turn kp = " + AP.headingTurnPID.kp);
        }
    }
    class AircraftAPI
    {
        public static GameObject SEAT_ADJUST_POSE_BOUNDS;
        private static Texture2D MenuTexture;
        public static PlayerVehicle pvAircraft;
        private static GameObject vPrefab;
        private static MissileLauncher ML;
        private static GameObject mPrefab;

        public static void VehicleAdd()
        {
            // Debug.Log("VA1.0");

            // Call private static method FinallyLoadExtVehicle
            Traverse.Create("VTResources")
                    .Method("FinallyLoadExtVehicle", Main.pathToBundle, AircraftInfo.AircraftPrefabName)
                    .GetValue();

            // Get player vehicle
            pvAircraft = VTResources.GetPlayerVehicle(AircraftInfo.vehicleName);

            // Load standalone LOD infos
            VTResources.LoadStandaloneLODInfos(false, false);

            // Call private static method GenerateStandaloneCustomScenarios
            var lodCampaign = Traverse.Create("VTResources")
                                      .Method("GenerateStandaloneCustomScenarios", new object[] { AircraftInfo.vehicleName, true })
                                      .GetValue<LODCampaignInfo>();

            // Assign standalone custom scenarios
            pvAircraft.standaloneCustomScenarios = lodCampaign;
        }



        public static GameObject GetChildWithName(GameObject obj, string name, bool check)
        {

            //Debug.unityLogger.logEnabled = Main.logging;
            Transform[] children = obj.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (check) {
                 //Debug.Log("Looking for:" + name + ", Found:" + child.name); 
                }
                if (child.name == name || child.name == (name + "(Clone)"))
                {
                    return child.gameObject;
                }
            }


            return null;

        }

       
        
       



       
    }




}
