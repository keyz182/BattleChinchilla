using Verse;
using UnityEngine;
using HarmonyLib;

namespace BattleChinchilla;

public class BattleChinchillaMod : Mod
{
    public BattleChinchillaMod(ModContentPack content) : base(content)
    {
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new Harmony("keyz182.rimworld.BattleChinchilla.main");
        harmony.PatchAll();
    }
}
