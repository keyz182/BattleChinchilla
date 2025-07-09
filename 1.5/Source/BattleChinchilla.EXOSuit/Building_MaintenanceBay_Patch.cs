using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
using WalkerGear;

namespace BattleChinchilla.EXOSuit;

[HarmonyPatch(typeof(Building_MaintenanceBay))]
public static class Building_MaintenanceBay_Patch
{
    public static Lazy<ThingDef> Core = new(() => ThingDef.Named("BC_Apparel_Mechachilla_Core") );

    [HarmonyPatch(nameof(Building_MaintenanceBay.ModuleStorage), MethodType.Getter)]
    [HarmonyPostfix]
    public static void ModuleStorage_Getter_Patch(Building_MaintenanceBay __instance, ref List<Apparel> __result)
    {
        if (__instance is Building_MechachillaBay)
        {
            __result.RemoveAll(app => app is WalkerGear_Core && app.def != Core.Value);
        }
        else
        {
            __result.RemoveAll(app => app.def == Core.Value);
        }
    }

    [HarmonyPatch(nameof(Building_MaintenanceBay.GetAvailableModules))]
    [HarmonyPostfix]
    public static void GetAvailableModules_Patch(Building_MaintenanceBay __instance, SlotDef slotDef, bool IsCore, ref IEnumerable<Thing> __result)
    {
        if(!IsCore) return;

        List<Thing> res = __result.ToList();

        if (__instance is Building_MechachillaBay)
        {
            res.RemoveAll(t => !t.def.HasModExtension<ExoModExtension>());
        }
        else
        {
            res.RemoveAll(t => t.def.HasModExtension<ExoModExtension>());
        }

        __result = res;
    }

}
