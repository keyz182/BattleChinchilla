using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace BattleChinchilla.EXOSuit;

[HarmonyPatch(typeof(FloatMenuMakerMap))]
public static class FloatMenuMakerMap_Patch
{
    public static Lazy<FieldInfo> _cachedThings = new(() => AccessTools.Field(typeof(FloatMenuMakerMap), "cachedThings") );
    public static List<Thing> cachedThings
    {
        get {
            return _cachedThings.Value.GetValue(null) as List<Thing>;
        }
    }

    [HarmonyPatch("ChoicesAtFor")]
    [HarmonyPostfix]
    public static void ChoicesAtFor_Patch(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> __result)
    {
        if (pawn.def != ThingDef.Named("Chinchilla")) return;
        if (!pawn.IsPlayerControlled || pawn.Downed) return;

        IntVec3 clickCell = IntVec3.FromVector3(clickPos);
        foreach (Building_MechachillaBay bay in pawn.Map.thingGrid.ThingsAt(clickCell).OfType<Building_MechachillaBay>())
        {
            foreach (FloatMenuOption floatMenuOption in bay.GetFloatMenuOptions(pawn))
            {
                cachedThings.Add(bay);
                __result.Add(floatMenuOption);
            }
        }
    }
}
