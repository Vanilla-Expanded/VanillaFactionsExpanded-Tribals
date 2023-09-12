using RimWorld;
using UnityEngine;

namespace VFETribals
{
    public class ThrowspikesThrown : Bullet
    {
        public override Quaternion ExactRotation => Quaternion.AngleAxis(angle, Vector3.up);
        public float angle;
        public override void Tick()
        {
            base.Tick();
            angle += 10;
            if (angle > 360)
            {
                angle = 0;
            }
        }
    }
}
