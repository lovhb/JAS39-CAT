using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using ModLoader;
using UnityEngine;
using VTOLAPI;

namespace CustomAircraftTemplateJAS39.Patches
{
    [HarmonyPatch(typeof(VTResources), nameof(VTResources.LoadAllCustomCampaignsLOD))]
    public static class VTResourcesPatches
    {
        [HarmonyPostfix]
        static void PostFix()
        {
            var folderName = "campaign";
            var folder = Path.Combine(Main.ModFolder, folderName);

            if (!Directory.Exists(folder))
            {
                Debug.LogError($"The folder does not exist at expected path of {folder}");
                return;
            }

            var vtcFiles = Directory.GetFiles(folder, "*.vtc");

            foreach (var vtcFile in vtcFiles)
            {
                if (!File.Exists(vtcFile))
                {
                    Debug.LogError($"The vtc file does not exist at expected path of {vtcFile}");
                    continue;
                }

                var campaignInfo = VTResources.LoadCustomCampaignLOD(vtcFile);
                if (campaignInfo == null)
                {
                    Debug.LogError($"Tried to load the campaign from {vtcFile} but it turned out null");
                    continue;
                }

                VTResources.customCampaignsLOD.Add(Path.GetFileNameWithoutExtension(vtcFile), campaignInfo);
                Debug.Log($"Finished adding campaign from {vtcFile}");
            }
        }
    }
}
