using System.Reflection;
using HarmonyLib;

namespace CustomAircraftTemplateJAS39.Patches.TranspilerStuff
{
    public class TranspilerUtils
    {
        public static FieldInfo PlayerVehicleVehicleName => AccessTools.Field(typeof(PlayerVehicle), nameof(PlayerVehicle.vehicleName));
        public static FieldInfo LODCampaignHideFromMenu => AccessTools.Field(typeof(LODCampaignInfo), nameof(LODCampaignInfo.hideFromMenu));

        // When enabled, will let a campaign be added to the list ONLY if the player owns the dlc.
        private const bool AllowDLC = true;

        public static bool ReturnTrueIfCustomVehicle(string vehicle)
        {
            if (PilotSaveManager.currentVehicle == null || PilotSaveManager.currentVehicle.vehicleName != AircraftInfo.vehicleName) return false;
        
            var pv = VTResources.GetPlayerVehicle(vehicle);
        
            if (pv.dlc)
            {
                if (!AllowDLC)
                    return false;
            
                // DLC check, removing this means you will feel the wrath of Bahamuto's D.
                return pv.IsDLCOwned();
            }
            
            return true;
        }
    }
}