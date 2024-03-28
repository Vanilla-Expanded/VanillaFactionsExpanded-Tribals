using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace VFETribals
{
    public class LargeFire : Building_WorkTable
    {
        public bool lightOn;

        public int nextSparkTick;

        public static HashSet<LargeFire> largeFires = new HashSet<LargeFire>();

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            largeFires.Add(this);
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            largeFires.Remove(this);
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            base.Destroy(mode);
            largeFires.Remove(this);
        }
        public override void Tick()
        {
            base.Tick();
            if (refuelableComp.HasFuel is false)
            {
                this.lightOn = false;
            }
            if (this.lightOn)
            {
                refuelableComp.Notify_UsedThisTick();
                if (nextSparkTick <= 0)
                {
                    SetNextSparkTick();
                }
                else if (Find.TickManager.TicksGame >= nextSparkTick)
                {
                    SpawnSpark(); 
                    SetNextSparkTick();
                }
                GasUtility.AddGas(this.OccupiedRect().RandomCell, Map, GasType.BlindSmoke, 1);
                if (Position.Roofed(Map) is false && Faction == Faction.OfPlayer)
                {
                    GameComponent_Tribals.Instance.IncrementLargeFireCounter(this);
                }
            }

            void SetNextSparkTick()
            {
                nextSparkTick = Find.TickManager.TicksGame + Rand.RangeInclusive(20, 60);
            }
        }

        protected void SpawnSpark()
        {
            IntVec3 position = GenRadial.RadialCellsAround(Position, 2.9f, true)
                .Where(x => x != base.Position).RandomElement();
            if (!position.InBounds(base.Map))
            {
                return;
            }
            CellRect startRect = CellRect.SingleCell(base.Position);
            CellRect endRect = CellRect.SingleCell(position);
            if (GenSight.LineOfSight(base.Position, position, base.Map, startRect, endRect))
            {
                ((Spark)GenSpawn.Spawn(VFET_DefOf.VFET_LargeFireSpark, base.Position, base.Map)).Launch(this, position, position, ProjectileHitFlags.All);
            }
        }

        public void Light()
        {
            this.lightOn = true;
            var des = Map.designationManager.DesignationOn(this, VFET_DefOf.VFET_LightLargeFire);
            if (des != null)
            {
                Map.designationManager.RemoveDesignation(des);
            }
            RefreshGlow();
        }

        public void Extinguish()
        {
            this.lightOn = false;
            var des = Map.designationManager.DesignationOn(this, VFET_DefOf.VFET_ExtinguishLargeFire);
            if (des != null)
            {
                Map.designationManager.RemoveDesignation(des);
            }
            RefreshGlow();
        }

        private void RefreshGlow()
        {
            CellRect cellRect = this.OccupiedRect();
            for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
            {
                for (int j = cellRect.minX; j <= cellRect.maxX; j++)
                {
                    IntVec3 intVec = new IntVec3(j, 0, i);
                    Map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.Buildings);
                    Map.glowGrid.DirtyCache(intVec);
                }
            }
            this.GetComp<CompGlower_LargeFire>().UpdateLit(Map);
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (var g in base.GetGizmos())
            {
                yield return g;
            }

            var lightDesignation = Map.designationManager.DesignationOn(this, VFET_DefOf.VFET_LightLargeFire);
            var extinguishDesignation = Map.designationManager.DesignationOn(this, VFET_DefOf.VFET_ExtinguishLargeFire);
            bool lightDesignationOn = lightDesignation != null;
            bool extinguishDesignationOn = extinguishDesignation != null;
            if (lightDesignationOn is false && !lightOn)
            {
                var openButton = new Command_Action
                {
                    defaultLabel = "VFET.LightLargeFire".Translate(),
                    defaultDesc = "VFET.LightLargeFireDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/Icons/LightLargeFire"),
                    action = delegate
                    {
                        var designation = Map.designationManager.DesignationOn(this, VFET_DefOf.VFET_LightLargeFire);
                        if (designation == null)
                        {
                            Map.designationManager.AddDesignation(new Designation(this, VFET_DefOf.VFET_LightLargeFire));
                        }
                        Map.designationManager.DesignationOn(this, VFET_DefOf.VFET_ExtinguishLargeFire)?.Delete();
                    }
                };
                yield return openButton;
            }
            else if (lightDesignationOn && !lightOn)
            {
                var cancelButton = new Command_Action
                {
                    defaultLabel = "Cancel".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel"),
                    action = delegate
                    {
                        Map.designationManager.RemoveDesignation(lightDesignation);
                    }
                };
                yield return cancelButton;
            }
            else if (extinguishDesignationOn is false && lightOn)
            {
                var closeButton = new Command_Action
                {
                    defaultLabel = "VFET.ExtinguishLargeFire".Translate(),
                    defaultDesc = "VFET.LightLargeFireDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/Icons/ExtinguishLargeFire"),
                    action = delegate
                    {
                        var designation = Map.designationManager.DesignationOn(this, VFET_DefOf.VFET_ExtinguishLargeFire);
                        if (designation == null)
                        {
                            Map.designationManager.AddDesignation(new Designation(this, VFET_DefOf.VFET_ExtinguishLargeFire));
                        }
                        Map.designationManager.DesignationOn(this, VFET_DefOf.VFET_LightLargeFire)?.Delete();
                    }
                };
                yield return closeButton;
            }
            else if (extinguishDesignationOn && lightOn)
            {
                var cancelButton = new Command_Action
                {
                    defaultLabel = "Cancel".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel"),
                    action = delegate
                    {
                        Map.designationManager.RemoveDesignation(extinguishDesignation);
                    }
                };
                yield return cancelButton;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref lightOn, "lightOn");
            Scribe_Values.Look(ref nextSparkTick, "nextSparkTick");
        }
    }
}
