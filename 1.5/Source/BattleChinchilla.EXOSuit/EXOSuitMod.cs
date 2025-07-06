using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BattleChinchilla.EXOSuit;

public class EXOSuitMod : Mod
{
    public EXOSuitMod(ModContentPack content)
        : base(content)
    {
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new Harmony("BattleChinchilla.EXOSuit.EXOSuitMod.main");
        harmony.PatchAll();

        Type AnimalGearHelper = AccessTools.TypeByName("AnimalGear.AnimalGearHelper");
        MethodInfo IsAnimalOfAFaction = AccessTools.Method(AnimalGearHelper, "IsAnimalOfAFaction");
        harmony.Patch(IsAnimalOfAFaction, prefix: new HarmonyMethod(typeof(AnimalGearHelper_Patch), nameof(AnimalGearHelper_Patch.IsAnimalOfAFaction_Patch)));

        MethodInfo HasPartsToWear = AccessTools.Method(typeof(ApparelUtility), nameof(ApparelUtility.HasPartsToWear));
        harmony.Patch(HasPartsToWear, postfix: new HarmonyMethod(typeof(AnimalGearHelper_Patch), nameof(AnimalGearHelper_Patch.HasPartsToWear_Patch)));

        Type AnimalGearHarmony = AccessTools.TypeByName("AnimalGear.AnimalGearHarmony");
        Type ApparelGraphicRecordGetter_TryGetGraphicApparel_Patch = AnimalGearHarmony.GetNestedType("ApparelGraphicRecordGetter_TryGetGraphicApparel_Patch");
        Type ApparelGraphicRecordGetterAnimal = ApparelGraphicRecordGetter_TryGetGraphicApparel_Patch.GetNestedType("ApparelGraphicRecordGetterAnimal");
        MethodInfo TryGetGraphicApparelAnimal = AccessTools.Method(ApparelGraphicRecordGetterAnimal, "TryGetGraphicApparelAnimal");
        harmony.Patch(TryGetGraphicApparelAnimal, prefix: new HarmonyMethod(typeof(AnimalGearHelper_Patch), nameof(AnimalGearHelper_Patch.TryGetGraphicApparelAnimal_Patch)));

    }
}

