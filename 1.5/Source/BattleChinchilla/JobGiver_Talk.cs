using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace BattleChinchilla;

public class JobGiver_Talk : ThinkNode_JobGiver
{
    protected override Job TryGiveJob(Pawn pawn)
    {

        if (pawn.Faction == null) return null;

        if (!(from p in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction)
                where !p.NonHumanlikeOrWildMan() && p != pawn && p.Position.InHorDistOf(pawn.Position, 10f) && pawn.GetRoom() == p.GetRoom() && !p.Position.IsForbidden(pawn) && p.CanCasuallyInteractNow()
                select p).TryRandomElement(out Pawn result))
        {
            return null;
        }
        Job job = JobMaker.MakeJob(BattleChinchillaDefOf.BC_ChinchillaTalking, result);
        job.locomotionUrgency = LocomotionUrgency.Sprint;
        job.expiryInterval = 3000;
        return job;
    }
}
