using DaCapo.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
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
            projectile.scale = 1f;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.timeLeft = 99999;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.damage = 10;
            projectile.penetrate = -1;
           
        }
        public override void AI()
        {
            projectile.timeLeft = 9999;
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
            if (!owner.channel)
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
            if (projectile.ai[0] < 220)          //准备时无敌
            {
                daCapoPlayer.MusicType = 0;
                owner.GetModPlayer<DaCapoPlayer>().DaCapoImmune = 3;
            }
            else if (projectile.ai[0] < 620)     //第一乐章免疫弹幕
            {
                daCapoPlayer.MusicType = 1;
                owner.GetModPlayer<DaCapoPlayer>().DaCapoImmune = 2;
            }
            else if (projectile.ai[0] < 1020)     //第二乐章免疫近战
            {
                daCapoPlayer.MusicType = 2;
                owner.GetModPlayer<DaCapoPlayer>().DaCapoImmune = 1;
            }
            else if (projectile.ai[0] < 1420)       //第三乐章免疫弹幕
            {
                daCapoPlayer.MusicType = 3;
                owner.GetModPlayer<DaCapoPlayer>().DaCapoImmune = 2;
            } 
            else if (projectile.ai[0] < 1820)        //第四乐章免疫近战
            {
                if (projectile.ai[0] < 1760)
                {
                    daCapoPlayer.MusicType = 4;
                }
                else
                {
                    daCapoPlayer.MusicType = 5;
                }
                owner.GetModPlayer<DaCapoPlayer>().DaCapoImmune = 1;
            }
            else                                     //终曲全免
            {
                owner.GetModPlayer<DaCapoPlayer>().DaCapoImmune = 3;
                if (projectile.ai[0] < 1940)
                {
                    owner.GetModPlayer<CurtainPlayer>().ShakeScreen = true;
                    daCapoPlayer.MusicType = 5;
                }
                else if (projectile.ai[0] < 2060)
                {
                    owner.GetModPlayer<CurtainPlayer>().ShakeScreen = true;
                    daCapoPlayer.MusicType = 6;
                }
                else
                {
                    daCapoPlayer.MusicType = 7;
                }

            }

            if (projectile.ai[0] == 220)    //第一乐章     //150
            {
                CurtainPlayer.SetTitle(1);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<MusicRing1>(), projectile.damage, 0, owner.whoAmI);
                Projectile.NewProjectile(owner.Bottom + new Vector2(-100, 0), Vector2.Zero, ModContent.ProjectileType<FirstChair>(), 0, 0, owner.whoAmI);
            }
            if (projectile.ai[0] == 620)    //第二乐章
            {
                CurtainPlayer.SetTitle(2);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<MusicRing2>(), projectile.damage, 0, owner.whoAmI);
                Projectile.NewProjectile(owner.Bottom + new Vector2(100, 0), Vector2.Zero, ModContent.ProjectileType<SecondChair>(), 0, 0, owner.whoAmI);
            }
            if (projectile.ai[0] == 1020)     //第三乐章
            {
                CurtainPlayer.SetTitle(3);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<MusicRing3>(), projectile.damage, 0, owner.whoAmI);
                Projectile.NewProjectile(owner.Bottom + new Vector2(-200, 0), Vector2.Zero, ModContent.ProjectileType<ThirdChair>(), 0, 0, owner.whoAmI);
            }
            if (projectile.ai[0] == 1420)       //第四乐章
            {
                CurtainPlayer.SetTitle(4);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<MusicRing4>(), projectile.damage, 0, owner.whoAmI);
                Projectile.NewProjectile(owner.Bottom + new Vector2(200, 0), Vector2.Zero, ModContent.ProjectileType<FourthChair>(), 0, 0, owner.whoAmI);
            }
            if (projectile.ai[0] == 1820)         //终曲（出现字幕）1
            {
                CurtainPlayer.SetTitle(5);
                //CurtainPlayer.Finale(owner);
            }
            if (projectile.ai[0] == 1940)           //音乐开始扭曲2
            {
                CurtainPlayer.Finale(owner);
            }
            if (projectile.ai[0] == 1943)           //最终伤害
            {
                int protmp = Projectile.NewProjectile(Main.screenPosition, Vector2.Zero, ModContent.ProjectileType<MusicFinalDamage>(), projectile.damage * 40, 0, owner.whoAmI);
                Main.projectile[protmp].width = Main.screenWidth;
                Main.projectile[protmp].height = Main.screenHeight;
            }
            if (projectile.ai[0] == 2060)          //闭幕
            {
                CurtainPlayer.FinaleCurtain(owner);
            }
            if (projectile.ai[0] == 2260 - 10)           //（提前十帧结束）
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