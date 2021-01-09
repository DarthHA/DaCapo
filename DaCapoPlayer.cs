using Terraria;
using Terraria.ModLoader;
using DaCapo.Projectiles;
using Microsoft.Xna.Framework;
using DaCapo.Items;

namespace DaCapo
{
    public class DaCapoPlayer : ModPlayer
    {
        public int NormalAtkType = 0;
        public int DaCapoImmune = 0;
        public int MusicType = -1;   //0为开幕，1为第一乐章，2为第二乐章，3为第三乐章，4为第四乐章，5为终章前，6为终章后，7为结束掌声
        public int DaCapoCD = 0;

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (DaCapoImmune == 1 || DaCapoImmune == 3) return false;
            if (player.GetModPlayer<CurtainPlayer>().FinaleTimer > 0)
            {
                return false;
            }
            return true;
        }
        public override void PostUpdateEquips()
        {
            if (DaCapoImmune > 0)
            {
                //Main.NewText("a");
                player.noKnockback = true;
                player.noFallDmg = true;
                if (player.gravDir < 0)
                {
                    player.gravDir = -player.gravDir;
                }
                player.mount.Dismount(player);
                player.controlMount = false;
                player.cMinecart = 0;
                player.longInvince = true;
               
                player.velocity = Vector2.Zero;
            }
            if (player.GetModPlayer<CurtainPlayer>().Progress > 0 ||
                player.GetModPlayer<CurtainPlayer>().FinaleTimer > 0 ||
                DaCapoImmune > 0)
            {
                DaCapoCD = 300;
            }
            if (player.GetModPlayer<CurtainPlayer>().FinaleTimer == 100)   //Da Capo
            {
                player.statLife = player.statLifeMax2;
                player.statMana = player.statManaMax2;
            }
            if (DaCapoCD > 0)
            {
                DaCapoCD--;
            }
            else
            {
                DaCapoCD = 0;
            }
        }
        public override void SetControls()
        {
            if (DaCapoImmune > 0)
            {
                player.controlDown = false;
                player.controlJump = false;
                player.controlUp = false;
                player.controlHook = false;
                player.controlMount = false;
                player.gravControl = false;
                player.gravControl2 = false;

            }
        }
        public override void UpdateBadLifeRegen()
        {
            if (DaCapoImmune > 0)
            {
                if (player.lifeRegen < 0)
                {
                    player.lifeRegen = 0;
                }
            }
        }
        public override void PreUpdateMovement()
        {
            if (DaCapoImmune > 0)
            {
                player.velocity = Vector2.Zero;
            }
        }
        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (DaCapoImmune == 2)
            {
                damage /= 5;
            }
        }
        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (DaCapoImmune == 1)
            {
                damage /= 5;
            }
        }
        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (DaCapoImmune == 2 || DaCapoImmune == 3) return false;
            if (player.GetModPlayer<CurtainPlayer>().FinaleTimer > 0)
            {
                return false;
            }
            return true;
        }
        public override void ResetEffects()
        {
            if (!AnyDaCapo())
            {
                DaCapoImmune = 0;
                MusicType = -1;
            }
        }

        private bool AnyDaCapo()
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == player.whoAmI)
                {
                    if (proj.type == ModContent.ProjectileType<DaCapoHeld>())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override void UpdateDead()
        {
            DaCapoImmune = 0;
        }

        public bool CheckPlayer()
        {
            return player.HeldItem.type == ModContent.ItemType<DaCapoItem>() && player.channel && AnyDaCapo();
        }


    }
}