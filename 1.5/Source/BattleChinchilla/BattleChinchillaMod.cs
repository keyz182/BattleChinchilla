using Verse;
using UnityEngine;
using HarmonyLib;

namespace BattleChinchilla;

public class BattleChinchillaMod : Mod
{
    public static Settings settings;

    public BattleChinchillaMod(ModContentPack content) : base(content)
    {
        Log.Message("Hello world from BattleChinchilla");

        // initialize settings
        settings = GetSettings<Settings>();
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new Harmony("keyz182.rimworld.BattleChinchilla.main");	
        harmony.PatchAll();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        settings.DoWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "BattleChinchilla_SettingsCategory".Translate();
    }
}
