using HarmonyLib;
using Verse;

namespace VFETribals
{
    [HarmonyPatch(typeof(Game), "InitNewGame")]
    public static class Game_InitNewGame_Patch
    {
        public static void Prefix()
        {
            GameComponent_Tribals.Instance.Notify_GameStarted();
        }
    }
}
