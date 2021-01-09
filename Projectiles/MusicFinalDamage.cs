using DaCapo.Buffs;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace DaCapo.Projectiles
{
    public class MusicFinalDamage : ModProjectile
    {
        public static readonly float OffSet = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MusicFinalDamage");
            DisplayName.AddTranslation(GameCulture.Chinese, "终曲伤害");

        }

        public override void SetDefaults()  //512  512   16  0.75
        {
            projectile.magic = true;
            projectile.width = 1;
            projectile.height = 1;
            projectile.scale = 1f;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 2;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.damage = 10;
            projectile.penetrate = -1;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.localNPCHitCooldown = 2;
            projectile.hide = true;
            projectile.alpha = 255;
        }
        public override void AI()
        {
            projectile.position = Main.screenPosition;
            projectile.width = Main.screenWidth;
            projectile.height = Main.screenHeight;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = true;
            if (target.HasBuff(ModContent.BuffType<FerventAdoration>()))
            {
                damage *= 2;
                target.GetGlobalNPC<DaCapoNPC>().StaggerResistance = 19;
                target.GetGlobalNPC<DaCapoNPC>().StaggerResistanceRegen = 600;
                target.DelBuff(target.FindBuffIndex(ModContent.BuffType<FerventAdoration>()));
            }
        }


        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}