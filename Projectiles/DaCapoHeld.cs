using DaCapo.Buffs;
using DaCapo.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static DaCapo.DaCapoPlayer;

namespace DaCapo.Projectiles
{
    public class DaCapoHeld : ModProjectile
    {
        public static readonly float OffSet = 30;
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Da Capo");
            DisplayName.AddTranslation(GameCulture.Chinese, "Da Capo");

        }

        public override void SetDefaults()
        {
            projectile.magic = true;
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 99999;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }
        public override void AI()
        {
            projectile.timeLeft = 9999;
            Lighting.AddLight(projectile.Center, 0.9f, 0.9f, 0.9f);
            Player owner = Main.player[projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                projectile.Kill();
                return;
            }
            if (owner.HeldItem.type != ModContent.ItemType<DaCapoItem>())
            {
                projectile.Kill();
                return;
            }
            if (!RightClickChannel(owner))
            {
                projectile.Kill();
                return;
            }
            owner.itemTime = 2;
            owner.itemAnimation = 2;
            owner.heldProj = projectile.whoAmI;
            int dir = owner.direction;
            float rot = dir < 0 ? MathHelper.Pi : 0;
            owner.itemRotation = (float)Math.Atan2(rot.ToRotationVector2().Y * dir * 3, rot.ToRotationVector2().X * dir);
            owner.velocity = Vector2.Zero;
            projectile.Center = owner.Center - new Vector2(owner.direction, 0);

            projectile.ai[0]++;
            DaCapoPlayer daCapoPlayer = owner.GetModPlayer<DaCapoPlayer>();
            if (projectile.ai[0] < DaCapoTime.Movement1Begin)          //准备时无敌
            {
                daCapoPlayer.CurrentPlayingMusic = MusicType.Beginning;
                owner.GetModPlayer<DaCapoPlayer>().DaCapoImmune = DamageType.All;
            }
            else if (projectile.ai[0] < DaCapoTime.Movement2Begin)     //第一乐章免疫弹幕
            {
                daCapoPlayer.CurrentPlayingMusic = MusicType.Movement1;
                owner.GetModPlayer<DaCapoPlayer>().DaCapoImmune = DamageType.Projectile;
                owner.AddBuff(ModContent.BuffType<DaCapoMovement1Buff>(), 2);
            }
            else if (projectile.ai[0] < DaCapoTime.Movement3Begin)     //第二乐章免疫近战
            {
                daCapoPlayer.CurrentPlayingMusic = MusicType.Movement2;
                owner.GetModPlayer<DaCapoPlayer>().DaCapoImmune = DamageType.Melee;
                owner.AddBuff(ModContent.BuffType<DaCapoMovement2Buff>(), 2);
            }
            else if (projectile.ai[0] < DaCapoTime.Movement4Begin)       //第三乐章免疫弹幕
            {
                daCapoPlayer.CurrentPlayingMusic = MusicType.Movement3;
                owner.GetModPlayer<DaCapoPlayer>().DaCapoImmune = DamageType.Projectile;
                owner.AddBuff(ModContent.BuffType<DaCapoMovement3Buff>(), 2);
            }
            else if (projectile.ai[0] < DaCapoTime.FinalBegin)        //第四乐章免疫近战
            {
                daCapoPlayer.CurrentPlayingMusic = MusicType.Movement4;
                owner.GetModPlayer<DaCapoPlayer>().DaCapoImmune = DamageType.Melee;
                owner.AddBuff(ModContent.BuffType<DaCapoMovement4Buff>(), 2);
            }
            else                                     //终曲全免
            {
                owner.GetModPlayer<DaCapoPlayer>().DaCapoImmune = DamageType.All;
                if (projectile.ai[0] < DaCapoTime.FinalDamage)
                {
                    owner.GetModPlayer<CurtainPlayer>().ShakeScreen = true;
                    daCapoPlayer.CurrentPlayingMusic = MusicType.Final1;
                    owner.AddBuff(ModContent.BuffType<DaCapoFinalBuff>(), 2);
                }
                else if (projectile.ai[0] < DaCapoTime.EndClap)
                {
                    owner.GetModPlayer<CurtainPlayer>().ShakeScreen = true;
                    daCapoPlayer.CurrentPlayingMusic = MusicType.Final2;
                    owner.AddBuff(ModContent.BuffType<DaCapoFinalBuff>(), 2);
                }
                else
                {
                    daCapoPlayer.CurrentPlayingMusic = MusicType.End;
                }

            }

            if (projectile.ai[0] == DaCapoTime.Movement1Begin - 120)
            {
                Projectile.NewProjectile(owner.Bottom + new Vector2(-100, 5), Vector2.Zero, ModContent.ProjectileType<FirstChair>(), 0, 0, owner.whoAmI);
            }
            if (projectile.ai[0] == DaCapoTime.Movement1Begin)    //第一乐章     //150
            {
                CurtainPlayer.SetTitle(1);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<MusicRing1>(), projectile.damage, 0, owner.whoAmI);
            }
            if (projectile.ai[0] == DaCapoTime.Movement2Begin - 120)
            {
                Projectile.NewProjectile(owner.Bottom + new Vector2(100, 5), Vector2.Zero, ModContent.ProjectileType<SecondChair>(), 0, 0, owner.whoAmI);
            }
            if (projectile.ai[0] == DaCapoTime.Movement2Begin)    //第二乐章
            {
                CurtainPlayer.SetTitle(2);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<MusicRing2>(), projectile.damage, 0, owner.whoAmI);
            }
            if (projectile.ai[0] == DaCapoTime.Movement3Begin - 120)
            {
                Projectile.NewProjectile(owner.Bottom + new Vector2(-60, 5), Vector2.Zero, ModContent.ProjectileType<ThirdChair>(), 0, 0, owner.whoAmI);
            }
            if (projectile.ai[0] == DaCapoTime.Movement3Begin)     //第三乐章
            {
                CurtainPlayer.SetTitle(3);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<MusicRing3>(), projectile.damage, 0, owner.whoAmI);
            }
            if (projectile.ai[0] == DaCapoTime.Movement4Begin - 120)
            {
                Projectile.NewProjectile(owner.Bottom + new Vector2(60, 5), Vector2.Zero, ModContent.ProjectileType<FourthChair>(), 0, 0, owner.whoAmI);
            }
            if (projectile.ai[0] == DaCapoTime.Movement4Begin)       //第四乐章
            {
                CurtainPlayer.SetTitle(4);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<MusicRing4>(), projectile.damage, 0, owner.whoAmI);
            }
            if (projectile.ai[0] == DaCapoTime.FinalBegin)         //终曲（出现字幕）1
            {
                CurtainPlayer.SetTitle(5);
                //CurtainPlayer.Finale(owner);
                BaseChair.EnterFinal();
            }
            if (projectile.ai[0] == DaCapoTime.FinalDamage)           //音乐开始扭曲2
            {
                CurtainPlayer.Finale(owner);
            }
            if (projectile.ai[0] == DaCapoTime.FinalDamage + 3)           //最终伤害
            {
                int protmp = Projectile.NewProjectile(Main.screenPosition, Vector2.Zero, ModContent.ProjectileType<MusicFinalDamage>(), projectile.damage * 60, 0, owner.whoAmI);
                Main.projectile[protmp].width = Main.screenWidth;
                Main.projectile[protmp].height = Main.screenHeight;
            }
            if (projectile.ai[0] == DaCapoTime.EndClap)          //闭幕
            {
                CurtainPlayer.FinaleCurtain(owner);
            }
            if (projectile.ai[0] == DaCapoTime.End - 10)           //（提前十帧结束）
            {
                projectile.Kill();
                owner.itemTime = 10;
                owner.itemAnimation = 10;
                return;
            }
        }

        public override bool CanDamage()
        {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player owner = Main.player[projectile.owner];
            Texture2D tex = Main.projectileTexture[projectile.type];
            SpriteEffects SP = owner.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, projectile.scale * 0.6f, SP, 0);

            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}