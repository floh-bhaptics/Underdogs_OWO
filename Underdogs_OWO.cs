using MelonLoader;
using MyBhapticsTactsuit;
using HarmonyLib;
using Il2Cpp;

[assembly: MelonInfo(typeof(Underdogs_OWO.Underdogs_OWO), "Underdogs_OWO", "1.0.0", "Florian Fahrenberger")]
[assembly: MelonGame("One Hamsa", "UNDERDOGS")]


namespace Underdogs_OWO
{
    public class Underdogs_OWO : MelonMod
    {
        public static TactsuitVR tactsuitVr = null!;

        public override void OnInitializeMelon()
        {
            tactsuitVr = new TactsuitVR();
        }

        [HarmonyPatch(typeof(PlayerMech), "ShakeCockpit", new Type[] { typeof(float), typeof(float) })]
        public class bhaptics_ShakeCockpit
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerMech __instance)
            {
                tactsuitVr.PlayBackFeedback("CockpitShake");
            }
        }

        [HarmonyPatch(typeof(PlayerMech), "DetachLeft", new Type[] { })]
        public class bhaptics_DetachLeft
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerMech __instance)
            {
                tactsuitVr.PlayBackFeedback("DetachArm_L");
            }
        }

        [HarmonyPatch(typeof(PlayerMech), "DetachRight", new Type[] { })]
        public class bhaptics_DetachRight
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerMech __instance)
            {
                tactsuitVr.PlayBackFeedback("DetachArm_R");
            }
        }

        [HarmonyPatch(typeof(PlayerPunchBehavior), "OnPunchResolved", new Type[] { typeof(EntityInteraction) })]
        public class bhaptics_OnPunchResolved
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerPunchBehavior __instance, EntityInteraction punchInteraction)
            {
                float intensity = (0.6f + punchInteraction.result.totalImpact / 60f * 0.4f);
                if (__instance.mechArm.side == VRSide.Right) tactsuitVr.PlayBackFeedback("Punch_R");
                else tactsuitVr.PlayBackFeedback("Punch_L");
            }
        }

        [HarmonyPatch(typeof(BashGuardEntity), "OnBashResolved", new Type[] { typeof(EntityInteraction) })]
        public class bhaptics_OnBashResolved
        {
            [HarmonyPostfix]
            public static void Postfix(BashGuardEntity __instance, EntityInteraction bashInteraction)
            {
                if (bashInteraction.totalHealthDamage > 0f) tactsuitVr.PlayBackFeedback("Bash");
                if (bashInteraction.hasImpact) tactsuitVr.PlayBackFeedback("Bash");
                if (bashInteraction.result.totalImpact > 0f) tactsuitVr.PlayBackFeedback("Bash");
            }
        }

        [HarmonyPatch(typeof(PlayerMech), "OnChassisHealthChange", new Type[] { typeof(IHealthComponent), typeof(int), typeof(IEntityBehavior) })]
        public class bhaptics_HealthChanged
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerMech __instance, IHealthComponent healthcomponent)
            {
                if (healthcomponent.health <= healthcomponent.maxHealth * 0.25f) tactsuitVr.PlayBackFeedback("ThreeHeartBeats");
            }
        }

        [HarmonyPatch(typeof(Bomb), "Explode", new Type[] { })]
        public class bhaptics_BombBExplode
        {
            [HarmonyPostfix]
            public static void Postfix(Bomb __instance)
            {
                tactsuitVr.PlayBackFeedback("Explosion");
            }
        }

    }
}