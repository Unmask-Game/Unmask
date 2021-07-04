namespace DefaultNamespace
{

    public class Constants
    {
        /* Movement Constants */
        public const float VRWalkSpeed = 2f;
        public const float VRSprintSpeed = 3f;
        public const float VRSlowedSpeed = 1f;
        public const float DesktopWalkSpeed = 1.25f;
        public const float DesktopSprintSpeed = 2.5f;
        public const float SpeedMultiplier = 1.2f;

        /* Item Constats */
        public const float ShowItemInfoBubbleTime = 8f;
        public const float AttackCooldownAfterHit = 8f;
        public const float LassoCooldown = 3f;
        public const float LassoRange = 6;
        public const float TaserRange = 3;
        public const float BatonRange = 1.2F;
        public const float HandcuffsRange = 0.8F;
        public const float HitNpcCooldown = 10f;

        /* Game Constants */
        public static int ThiefResistancePoints = 130;
        public static int MasksNeeded = 65;
        public static float WaitAfterGameOver = 7.5f;

        /* NPC Constants */
        public static int NpcCount = 100;
        public static int NpcChangeShopPropability = 25;
        public static int NpcMinWaitTime = 50 * 2;
        public static int NpcMaxWaitTime = 50 * 15;
    }
}