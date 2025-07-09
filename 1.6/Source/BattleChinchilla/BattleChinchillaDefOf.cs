using RimWorld;
using Verse;

namespace BattleChinchilla;

[DefOf]
public static class BattleChinchillaDefOf
{
    // Remember to annotate any Defs that require a DLC as needed e.g.
    // [MayRequireBiotech]
    // public static GeneDef YourPrefix_YourGeneDefName;

    public static JobDef BC_ChinchillaTalking;
    public static InteractionDef BC_ChinchillaInteraction;
    public static ThingDef BC_ChinchillaBattleArmour;

    static BattleChinchillaDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(BattleChinchillaDefOf));
}
