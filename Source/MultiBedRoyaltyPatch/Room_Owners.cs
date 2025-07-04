﻿using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MultiBedRoyaltyPatch;

[HarmonyPatch(typeof(Room), nameof(Room.Owners), MethodType.Getter)]
internal class Room_Owners
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

        var foundBed = false;

        foreach (var building_Bed in __instance.ContainedBeds)
        {
            if (!building_Bed.def.building.bed_humanlike)
            {
                continue;
            }

            switch (building_Bed.OwnersForReading.Count)
            {
                case 0:
                    continue;
                case < 3:
                    foundBed = true;
                    continue;
            }

            var returnValue = new HashSet<Pawn>();
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

        return foundBed;
    }
}