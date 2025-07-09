using RimWorld;
using Verse;

namespace BattleChinchilla;

public class InteractionWorker_AnimalSpeak : InteractionWorker
{
    public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
    {
        if (initiator.def == DefDatabase<ThingDef>.GetNamed("Chinchilla") && recipient.RaceProps.Humanlike)
        {
            return 1f;
        }

        return 0;
    }
}
