﻿using RimWorld;
using UnityEngine;
using Verse;

namespace VFETribals
{
    [StaticConstructorOnStartup]
    public class CompFireOverlay_LargeFire : CompFireOverlayBase
    {
        protected CompRefuelable refuelableComp;

        public static readonly Graphic FireGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Fire", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);

        public new CompProperties_FireOverlay Props => (CompProperties_FireOverlay)props;

        public override void PostDraw()
        {
            base.PostDraw();
            if ((this.parent as LargeFire).lightOn && refuelableComp.HasFuel)
            {
                Vector3 drawPos = parent.DrawPos;
                drawPos.y += 3f / 74f;
                FireGraphic.Draw(drawPos, Rot4.North, parent);
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            refuelableComp = parent.GetComp<CompRefuelable>();
        }

        public override void CompTick()
        {
            if ((refuelableComp == null || refuelableComp.HasFuel) && startedGrowingAtTick < 0)
            {
                startedGrowingAtTick = GenTicks.TicksAbs;
            }
        }
    }
}
