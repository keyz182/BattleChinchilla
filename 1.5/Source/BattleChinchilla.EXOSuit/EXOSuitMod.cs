using System;
using System.Reflection;
using HarmonyLib;
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
    }
}
