using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CustomAircraftTemplateJAS39.Patches.TranspilerStuff;
using HarmonyLib;
using RewiredConsts;
using UnityEngine;

namespace CustomAircraftTemplateJAS39.Patches.CampaignStuff
{
    [HarmonyPatch]
    public class CSUIPatch_SetupCampaignsList
    {
    
        static MethodBase TargetMethod(Harmony instance)
        {
            var innerType = AccessTools.FirstInner(typeof(CampaignSelectorUI),
                t => t.Name.StartsWith("<SetupCampaignScreenRoutine"));
            var targetMethod = AccessTools.Method(innerType, "MoveNext");
            return targetMethod;
        }

        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var codeInstructions = new List<CodeInstruction>(instructions);

            // Define labels to jump to.
            var jumpTrueLabel = il.DefineLabel();
            var jumpFalseLabel = il.DefineLabel();
        
            // Bools to prevent finding multiple (no clue if there is)
            var found = false;
            var foundLODHide = false;

            for (int i = 0; i < codeInstructions.Count; i++)
            {
                var codeInstruction = codeInstructions[i];

                // If instruction loads the field from PlayerVehicle.vehicleName do this
                if (HarmonyLib.CodeInstructionExtensions.LoadsField(codeInstruction, TranspilerUtils.PlayerVehicleVehicleName) && !found)
                {
                    // Backup incase i need to go back
                    //codeInstructions[i + 1] = new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(TranspilerUtils), nameof(TranspilerUtils.ReturnTrueIfCustomVehicle)));

                    // Set the original first half of the if statement to skip to the hideFromMenu check if returns true, this is making it an or statement allowing for multiple vehicles to patch.
                    //                          ↓    this will skip to    →     →     →     →    this   ↓
                    // this makes it so ( (campaignVehicle == vehicle || campaignVehicle == modded) || !hideFromMenu )
                    //                                                    the jumpTrueLabel will go here ↑
                    codeInstructions[i + 2] = new CodeInstruction(OpCodes.Brtrue_S, jumpTrueLabel);

                    bool shouldBeTrue = false;
                    
                    for (int j = i + 3; j < codeInstructions.Count; j++)
                    {
                        var nextInstruction = codeInstructions[j];

                        if (HarmonyLib.CodeInstructionExtensions.LoadsField(nextInstruction, TranspilerUtils.LODCampaignHideFromMenu))
                        {
                            break;
                        }

                        // If we hit an instruction before the hideFromMenu that skips the if our instruction should stay in it.
                        if (nextInstruction.opcode == OpCodes.Brfalse)
                            shouldBeTrue = true;
                    }
                    
                    var insertInstructions = new CodeInstruction[]
                    {
                        // Loads the local variable for LODCampaignInfo for the current campaign.
                        new CodeInstruction(OpCodes.Ldloc_S, 5),

                        // Creates a Load field instruction pointing towards LODCampaignInfo.vehicle, loads the field from the local variable from ↑
                        new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(LODCampaignInfo), nameof(LODCampaignInfo.vehicle))),
                    
                        // Creates a Call instruction to return true if the current vehicle is a custom vehicle.
                        new CodeInstruction(OpCodes.Call,
                            AccessTools.Method(typeof(TranspilerUtils), nameof(TranspilerUtils.ReturnTrueIfCustomVehicle))),
                    
                        // This jumps out of the if statement completely, like if campaignVehicle != vehicle or it was hidden from menu.
                        new CodeInstruction(shouldBeTrue ? OpCodes.Brtrue_S : OpCodes.Brfalse_S, shouldBeTrue ? jumpTrueLabel : jumpFalseLabel)
                    };
                    codeInstructions.InsertRange(i + 3, insertInstructions);

                    found = true;
                }

                // If instruction loads the hide from menu field do this
                if (HarmonyLib.CodeInstructionExtensions.LoadsField(codeInstruction, TranspilerUtils.LODCampaignHideFromMenu) && !foundLODHide)
                {
                    foundLODHide = true;
                    // No need to manually mark the jump label things,
                    codeInstructions[i - 1].labels.Add(jumpTrueLabel);
                    codeInstructions[i + 6].labels.Add(jumpFalseLabel);
                }
            }

        
            /*foreach (var codeInstruction in codeInstructions)
            {
                Debug.Log($"Code Instructions {codeInstruction.opcode} | {codeInstruction.operand}");
            }*/
        
            return codeInstructions.AsEnumerable();
        }
    }
}