﻿using Verse;

namespace VFETribals
{
    [StaticConstructorOnStartup]
    public static class ModCompatibility
    {
        public static bool VFEClassicalActive = ModsConfig.IsActive("OskarPotocki.VFE.Classical") || ModsConfig.IsActive("OskarPotocki.VFE.Classical_steam");
    }
}
