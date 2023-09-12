using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace VFETribals
{
    //When it’s off, it doesn’t burn fuel.It provides a lot of light and heat, but it also sends sparks in random directions,
    //which may set flammable objects on fire.
    //The sparks get ejected once every 20~60 ticks and travel up to 2.9 cells away from the fire.
    //If they land on something flammable, they set it on fire.
    //Produces large amounts of blindsmoke when it’s burning.
    //If it has been burning non-stop out in the open (not indoors) for 10 days, an untamed wild man wanders in.
    //If at any point it was turned off, the counter resets.The counter is global, meaning 10 large fires won’t summon 10 wild men.
    //Only 1 will be summoned.
    //While it’s burning, the likelihood of getting a raid is increased by 10%.

    //Large fire - Has a gizmo to turn it on and off, which is like flicking and a pawn will do the job to turn it on and off.
    //Emits large amounts of blindsmoke and sparks that set nearby things on fire. 
    //If it burns uninterrupted for 10 days, a wild man wanders in. 
    //Increases likelihood of getting raids by 10% (non-stacking, so 10 large campfires only increase likelihood by 10% total)

    public class LargeFire : Building_WorkTable
    {
        public bool lightOn;

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
            if (this.lightOn)
            {
                refuelableComp.Notify_UsedThisTick();
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
                    Map.mapDrawer.MapMeshDirty(intVec, MapMeshFlag.Buildings);
                    Map.glowGrid.MarkGlowGridDirty(intVec);
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
        }
    }
}
