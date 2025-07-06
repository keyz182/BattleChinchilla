using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using WalkerGear;

namespace BattleChinchilla.EXOSuit;

public static class AnimalGearHelper_Patch
{
    public static Lazy<Type> AnimalTracker = new(()=>AccessTools.TypeByName("AnimalGear.AnimalGearHelper"));
    public static Lazy<MethodInfo> InitAllAnimalTracker = new(()=>AccessTools.Method(AnimalTracker.Value, "InitAllAnimalTracker"));

    public static bool IsAnimalOfAFaction_Patch(Pawn pawn, ref bool __result)
    {
        if (Building_MechachillaBay.PawnCache?.Contains(pawn) == true)
        {
            __result = true;
            return false;
        }

        return true;
    }


    [HarmonyAfter(["AnimalGear"])]
    public static void HasPartsToWear_Patch(Pawn p, ThingDef apparel, ref bool __result)
    {
        if(__result) return;
        if(p.def != ThingDef.Named("Chinchilla")) return;
        if (!apparel.HasComp<CompWalkerComponent>()) return;

        InitAllAnimalTracker.Value.Invoke(null, [p]);

        // todo - check if a module covers this modules parts
        __result = true;
    }

    public static bool TryGetGraphicApparelAnimal_Patch(Apparel apparel, Pawn pawn, ref ApparelGraphicRecord rec, ref bool __result)
    {
        if (apparel.def != BattleChinchillaDefOf.BC_ChinchillaBattleArmour) return true;
        if(pawn.def != ThingDef.Named("Chinchilla")) return true;
        if(!pawn.apparel.WornApparel.Any(a => a is WalkerGear_Core)) return true;

        Graphic graphic = GraphicDatabase.Get<Graphic_Multi>("Things/Pawn/Animal/Apparel/emptyGear", ShaderDatabase.Cutout, apparel.def.graphicData.drawSize, apparel.DrawColor);
        rec = new ApparelGraphicRecord(graphic, apparel);
        __result = false;
        return false;
    }


}
