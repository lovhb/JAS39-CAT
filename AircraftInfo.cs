using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAircraftTemplateJAS39
{
    public class AircraftInfo
    {

        //READ ME, IMPORTANT!!!!!!!!
        //You must change HarmonyId in order for your custom aircraft mod to be compatible with other aircraft mods
        public const string HarmonyId = "Bovine.JAS39";

        
        
        //The name of the aircraft specified in the External Vehicle Info Component
       
        public const string vehicleName = "Saab JAS-39 Gripen";
       
        //Names of the prefab name you created and the name of the Assetbundle that you created
        public const string AircraftAssetbundleName = "jas39";
        public const string AircraftPrefabName = "JAS39";


    }
}
