using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MultiBedRoyaltyPatch
{
    [HarmonyPatch(typeof(Room))]
    [HarmonyPatch("Owners", MethodType.Getter)]
    internal class Room_Owners_Patch
    {
        // ReSharper disable once RedundantAssignment
        private static bool Prefix(Room __instance, ref IEnumerable<Pawn> __result)
        {
            __result = new List<Pawn>();
            if (__instance.TouchesMapEdge)
            {
                return false;
            }

            if (__instance.IsHuge)
            {
                return false;
            }

            if (__instance.Role != RoomRoleDefOf.Bedroom && __instance.Role != RoomRoleDefOf.PrisonCell &&
                __instance.Role != RoomRoleDefOf.Barracks && __instance.Role != RoomRoleDefOf.PrisonBarracks)
            {
                return false;
            }

            foreach (var building_Bed in __instance.ContainedBeds)
            {
                if (!building_Bed.def.building.bed_humanlike)
                {
                    continue;
                }

                if (building_Bed.OwnersForReading.Count == 0)
                {
                    continue;
                }

                if (building_Bed.OwnersForReading.Count < 3)
                {
                    return true;
                }

                var returnValue = new List<Pawn>();
                for (var index = 0; index < building_Bed.OwnersForReading.Count; index++)
                {
                    var pawn = building_Bed.OwnersForReading[index];
                    if (pawn == null)
                    {
                        continue;
                    }

                    if (index == building_Bed.OwnersForReading.Count - 1)
                    {
                        returnValue.Add(pawn);
                    }

                    var pawnHasLover = false;
                    for (var otherIndex = 0; otherIndex < building_Bed.OwnersForReading.Count; otherIndex++)
                    {
                        if (otherIndex == index)
                        {
                            continue;
                        }

                        var otherPawn = building_Bed.OwnersForReading[otherIndex];
                        if (!LovePartnerRelationUtility.LovePartnerRelationExists(pawn, otherPawn))
                        {
                            continue;
                        }

                        pawnHasLover = true;
                        break;
                    }

                    if (!pawnHasLover)
                    {
                        return false;
                    }

                    returnValue.Add(pawn);
                }

                __result = returnValue;
                break;
            }

            return false;
        }
    }
}