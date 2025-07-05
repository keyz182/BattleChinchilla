using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using WalkerGear;
using BattleChinchilla;

namespace BattleChinchilla.EXOSuit;

public class Building_MechachillaBay: Building_MaintenanceBay
{
    public static Lazy<FieldInfo> thingRotationInt = new(() => AccessTools.Field(typeof(Thing), "rotationInt") );
    public static Lazy<FieldInfo> _cachePawn = new(() => AccessTools.Field(typeof(Building_MechachillaBay), "cachePawn") );

    public static List<Pawn> PawnCache;

    public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
    {
        Pawn cachedPawn = (Pawn) _cachePawn.Value.GetValue(this);
        if (cachedPawn != null)
        {
            PawnCache.Remove(cachedPawn);
        }
        base.Destroy(mode);
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        if (PawnCache.NullOrEmpty())
        {
            if (Dummy == null)
            {
                ModLog.Error("Dummy is null");
            }
        }
    }


    public override Pawn Dummy
    {
        get
        {
            Pawn cachedPawn = (Pawn) _cachePawn.Value.GetValue(this);
            if (PawnCache.NullOrEmpty())
            {
                PawnCache = new List<Pawn>();
            }
            if (cachedPawn == null)
            {
                cachedPawn = PawnGenerator.GeneratePawn(PawnKindDef.Named("Chinchilla"));
                cachedPawn.SetFaction(Faction.OfPlayer);
                pawnsInBuilding.Add(cachedPawn);
                if (cachedPawn.outfits == null)
                {
                    cachedPawn.outfits = new Pawn_OutfitTracker(cachedPawn);
                }
                if (cachedPawn.equipment == null)
                {
                    cachedPawn.equipment = new Pawn_EquipmentTracker(cachedPawn);
                }
                if (cachedPawn.apparel == null)
                {
                    cachedPawn.apparel = new Pawn_ApparelTracker(cachedPawn);
                }

                cachedPawn.apparel.DestroyAll();
                Apparel armour = (Apparel) ThingMaker.MakeThing(BattleChinchillaDefOf.BC_ChinchillaBattleArmour, BattleChinchillaDefOf.BC_ChinchillaBattleArmour.defaultStuff);
                if(armour != null)
                    cachedPawn.apparel.Wear(armour, true, true);
                thingRotationInt.Value.SetValue(cachedPawn, Rotation.Opposite);
                cachedPawn.drafter = new Pawn_DraftController(cachedPawn)
                {
                    Drafted = true
                };
                _cachePawn.Value.SetValue(this, cachedPawn);
            }
            if (!PawnCache.Contains(cachedPawn))
            {
                PawnCache.Add(cachedPawn);
            }
            return cachedPawn;
        }
    }

    public override AcceptanceReport CanAcceptPawn(Pawn pawn)
    {
        if (pawn.def != ThingDef.Named("Chinchilla") && !pawn.IsColonist && !pawn.IsSlaveOfColony && !pawn.IsPrisonerOfColony && (pawn.IsColonyMutant || pawn.IsGhoul))
        {
            return false;
        }

        if (PowerComp != null && !PowerOn)
        {
            return "NoPower".Translate().CapitalizeFirst();
        }
        if (pawn.apparel.WornApparel.Any(a => a is WalkerGear_Core)) return "WG_Disabled_AlreadyHasCoreFrame".Translate(pawn.Name.ToString()).CapitalizeFirst();
        if (!HasGearCore) return "WG_Disabled_NoCoreFrame".Translate().CapitalizeFirst();
        return true;
    }

    public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
    {
        if (HasGearCore)
        {
            Dummy.Drawer.renderer.DynamicDrawPhaseAt(phase, drawLoc.WithYOffset(1f), neverAimWeapon: true);
        }

        if (phase != DrawPhase.Draw)
            return;
        DrawAt(drawLoc, flip);
    }

}
