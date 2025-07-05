using HarmonyLib;
using Verse;

namespace BattleChinchilla.EXOSuit;

public static class AnimalGearHelper_Patch
{
    public static bool IsAnimalOfAFaction_Patch(Pawn pawn, ref bool __result)
    {
        if (Building_MechachillaBay.PawnCache?.Contains(pawn) == true)
        {
            __result = true;
            return false;
        }

        return true;
    }

}
