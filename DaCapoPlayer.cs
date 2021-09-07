using Terraria;
using Terraria.ModLoader;
using DaCapo.Projectiles;
using Microsoft.Xna.Framework;
using DaCapo.Items;
using DaCapo.Buffs;
using Terraria.ID;

namespace DaCapo
{
    public class DaCapoPlayer : ModPlayer
    {
        /// <summary>
        /// 平A种类
        /// </summary>
        public int NormalAtkType = 0;

        public enum DamageType
        {
            None,
            Melee,
            Projectile,
            All
        }
        /// <summary>
        /// 伤害免疫种类
        /// </summary>
        public DamageType DaCapoImmune = DamageType.None;

        public enum MusicType
        {
            None,
            Beginning,
            Movement1,
            Movement2,
            Movement3,
            Movement4,
            Final1,
            Final2,
            End
        }

        /// <summary>
        /// 当前播放音乐种类
        /// 0为开幕
        /// 1为第一乐章
        /// 2为第二乐章
        /// 3为第三乐章
        /// 4为第四乐章
        /// 5为终章前
        /// 6为终章后
        /// 7为结束掌声
        /// </summary>
        public MusicType CurrentPlayingMusic = MusicType.None;

        /// <summary>
        /// 演奏CD
        /// </summary>
        public int DaCapoCD = 0;

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (DaCapoImmune == DamageType.Melee || DaCapoImmune == DamageType.All) return false;
            if (player.GetModPlayer<CurtainPlayer>().FinaleTimer > 0)
            {
                return false;
            }
            return true;
        }
        public override void PostUpdateEquips()
        {
            if (DaCapoImmune > DamageType.None)
            {
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
                player.buffImmune[BuffID.Frozen] = true;
                player.buffImmune[BuffID.Cursed] = true;
                player.buffImmune[BuffID.VortexDebuff] = true;
                player.buffImmune[BuffID.TheTongue] = true;
                player.buffImmune[BuffID.Webbed] = true;
                player.buffImmune[BuffID.Stoned] = true;
                player.buffImmune[BuffID.Burning] = true;
                player.buffImmune[BuffID.WindPushed] = true;
                player.buffImmune[BuffID.Suffocation] = true;
                player.breath = player.breathMax;
                player.lavaImmune = true;
                player.fireWalk = true;

                player.manaRegen = 0;
            }
            if (player.GetModPlayer<CurtainPlayer>().Progress > 0 ||
                player.GetModPlayer<CurtainPlayer>().FinaleTimer > 0 ||
                DaCapoImmune > DamageType.None)
            {
                DaCapoCD = 600;
            }

            if (DaCapoCD > 0)
            {
                DaCapoCD--;
                if (!Main.LocalPlayer.GetModPlayer<CurtainPlayer>().Active &&
                Main.LocalPlayer.GetModPlayer<CurtainPlayer>().FinaleTimer == 0)
                {
                    player.AddBuff(ModContent.BuffType<DaCapoCDBuff>(), DaCapoCD);
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (player.GetModPlayer<CurtainPlayer>().FinaleTimer == 100)   //Da Capo
            {
                player.statLife = player.statLifeMax2;

                player.immune = true;
                player.immuneTime = (int)MathHelper.Max(80, player.immuneTime);
                player.immuneNoBlink = true;
                for(int i = 0; i < player.hurtCooldowns.Length; i++)
                {
                    player.hurtCooldowns[i] = (int)MathHelper.Max(80, player.hurtCooldowns[i]);
                }

                for(int i = 0; i < player.buffTime.Length; i++)
                {
                    if (player.buffTime[i] > 0)
                    {
                        int type = player.buffType[i];
                        if (Main.debuff[type] && !Main.buffNoTimeDisplay[type] &&
                            type != BuffID.PotionSickness && type != BuffID.ManaSickness)
                        {
                            player.ClearBuff(type);
                        }
                    }
                }

                //player.statMana = player.statManaMax2;
            }
            if (DaCapoImmune > DamageType.None)
            {
                player.endurance += (1 - player.endurance) * 0.75f;
            }
        }
        public override void SetControls()
        {
            if (DaCapoImmune > DamageType.None)
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
            if (DaCapoImmune > DamageType.None)
            {
                if (player.lifeRegen < 0)
                {
                    player.lifeRegen = 0;
                }
            }
        }
        public override void PreUpdateMovement()
        {
            if (DaCapoImmune > DamageType.None)
            {
                player.velocity = Vector2.Zero;
            }
        }
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            /*
            if (DaCapoImmune > 0)
            {
                player.immune = true;
                if (player.immuneTime < 120) 
                {
                    player.immuneTime = 120; 
                }
                for(int i = 0; i < player.hurtCooldowns.Length; i++)
                {
                    if (player.hurtCooldowns[i] < 120)
                    {
                        player.hurtCooldowns[i] = 120;
                    }
                }
            }
            */
        }
        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (DaCapoImmune == DamageType.Projectile || DaCapoImmune == DamageType.All) return false;
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
                DaCapoImmune = DamageType.None;
                CurrentPlayingMusic = MusicType.None;
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
            DaCapoImmune = DamageType.None;
        }

        public bool CheckPlayerPlaying()
        {
            return player.HeldItem.type == ModContent.ItemType<DaCapoItem>() && RightClickChannel(player) && AnyDaCapo();
        }
        
        public static bool RightClickChannel(Player player)
        {
            if(Main.mouseRight && !player.mouseInterface && !Main.blockMouse)
            {
                return true;
            }
            return false;
        }
    }

    public class DaCapoGItem : GlobalItem
    {
        public override bool CanPickup(Item item, Player player)
        {
            if (player.GetModPlayer<DaCapoPlayer>().DaCapoImmune > DaCapoPlayer.DamageType.None)
            {
                return false;
            }
            return true;
        }
    }
}