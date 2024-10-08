using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Unity;
using TMPro;
using UnityEngine.UI;
using VTOLAPI;
using System.Reflection;
using ModLoader.Framework;
using ModLoader.Framework.Attributes;

namespace CustomAircraftTemplateJAS39
{
    [ItemId("NotPolar.JAS39")]
    public class Main : VtolMod
    {
        public static string ModFolder { get; set; }

        public static Main instance;

        //Stores a prefab of the aircraft in order to spawn it in whenever you want
        public static GameObject aircraftPrefab;
        
        public static PlayerVehicle customAircraftPV;
        public static BuiltInCampaigns customBICampaigns;
        public static GameObject aircraftLoadoutConfiguratorPrefab;
        public static GameObject DebugTools;
        public static GameObject aircraftCustom;
        public static GameObject BOQuad;
        public static Single currentGAlpha;
        public static GameObject customAircraftPVobject;
        public static bool checkPVListFull = false;
        //public static MultiplayerSpawn.Vehicles aircraftMSVId;
        public static int unitListCount = 1;
        public static GameObject vehicleObject;
        public static float flapssetting =1;
        

        public static int i=0;

        public static GameObject playerGameObject;
       
        public static string pathToBundle;
        private string pathToAddonsBundle;
        public static bool logging = true;
        public static List<Campaign> campaignslist;
        public static String unitList;
        
        public static bool aircraftLoaded = false;
        public static TextMeshPro radarcontactlist;
        public static GameObject miniicp;
       
        public static AssetBundle bundleLoad;
        public static Campaign customCampaign;
        public static Campaign steamCampaign;
        public static WheelsController wc;
        public static GearAnimator animator;


        // This method is run once, when the Mod Loader is done initialising this game object
        private void Awake()
        {
            ModFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            //Debug.unityLogger.logEnabled = Main.logging;
            instance = this;

            //Debug.Log("JAS39 ML3" + AircraftInfo.AircraftAssetbundleName);
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            pathToBundle = Path.Combine(directory, AircraftInfo.AircraftAssetbundleName);
            

            //Debug.Log("JAS39 ML3" + pathToBundle);
          
            
            //Debug.Log("JAS39 ML6");
            VTResources.OnLoadingPlayerVehicles += AircraftAPI.VehicleAdd;

            AircraftAPI.VehicleAdd();
        }

        public override void UnLoad()
        {
            Debug.Log("Bye bye");
        }

        //This method is called every frame by Unity. Here you'll probably put most of your code
        void Update()
        {
            

        }


        //This method is like update but it's framerate independent. This means it gets called at a set time interval instead of every frame. This is useful for physics calculations
        void FixedUpdate()
        {
            

           
        }

        //This function is called every time a scene is loaded. this behaviour is defined in Awake().
        private void SceneLoaded(VTScenes scene)
        {
            //If you want something to happen in only one (or more) scenes, this is where you define it.
            
            //For example, lets say you're making a mod which only does something in the ready room and the loading scene. This is how your code could look:
            switch (scene)
            {
                
                case VTScenes.VehicleConfiguration:
                    //Debug.Log("JAS39 Reload the configurator");
                    StartCoroutine(InitWaiter());
                    

                    break;
                default:
                    //Debug.Log("JAS39 In scene: " + scene);

                    break;

            }


        }

        private IEnumerator InitWaiter()
        {
        //Debug.unityLogger.logEnabled = Main.logging;
        //Debug.Log("JAS39 InitWaiter Started");
            yield return new WaitForSeconds(1f);
          
            yield break;
        }

       
    }
}
