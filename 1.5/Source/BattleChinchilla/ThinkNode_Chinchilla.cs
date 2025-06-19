using RimWorld;
using Verse;
using Verse.AI;

namespace BattleChinchilla;

public class ThinkNode_Chinchilla : ThinkNode_Conditional
{
    protected override bool Satisfied(Pawn pawn)
    {
        return pawn.def == DefDatabase<ThingDef>.GetNamed("Chinchilla") && pawn.Faction == Faction.OfPlayerSilentFail;
    }
}
