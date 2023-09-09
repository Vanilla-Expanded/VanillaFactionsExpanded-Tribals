using Verse.AI;
using Verse.AI.Group;

namespace VFETribals
{
    public class LordJob_Flee : LordJob
    {
        public override bool GuiltyOnDowned => true;
        public LordJob_Flee()
        {
        }
        public override StateGraph CreateGraph()
        {
            StateGraph stateGraph = new StateGraph();
            LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, canDig: false, interruptCurrentJob: true);
            lordToil_ExitMap.useAvoidGrid = true;
            stateGraph.AddToil(lordToil_ExitMap);
            return stateGraph;
        }
    }

}
