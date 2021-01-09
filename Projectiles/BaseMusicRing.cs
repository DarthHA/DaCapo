using DaCapo.Buffs;
using DaCapo.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace DaCapo.Projectiles
{
    public abstract class BaseMusicRing : ModProjectile
    {
        public float RangeScale = 1;
        public int RotDir = 1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Movement");
            DisplayName.AddTranslation(GameCulture.Chinese, "乐章");
        }

        public override void SetDefaults()
        {
            projectile.magic = true;
            projectile.width = 1;
            projectile.height = 1;
            projectile.scale = 0f;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 99999;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.damage = 10;
            projectile.penetrate = -1;
            projectile.alpha = 0;
        }

        public override void AI()
        {
            projectile.timeLeft = 9999;
            projectile.hostile = false;
            projectile.friendly = true;
            Player owner = Main.player[projectile.owner];
            if (projectile.ai[0] != 2)
            {
                if (!owner.active || owner.dead || owner.ghost)
                {
                    projectile.ai[0] = 3;
                }
                if (owner.HeldItem.type != ModContent.ItemType<DaCapoItem>())
                {
                    projectile.ai[0] = 3;
                }
                if (!owner.channel)
                {
                    projectile.ai[0] = 3;
                }
            }
            projectile.Center = owner.Center;
            if (projectile.ai[0] == 0)             //生成
            {
                projectile.rotation += 0.01f * RotDir;
                projectile.ai[1]++;
                projectile.scale = projectile.ai[1] / 40;
                if (projectile.ai[1] > 40)
                {
                    projectile.ai[0] = 1;
                    projectile.ai[1] = 0;
                }
            }
            else if (projectile.ai[0] == 1)            //正常
            {
                projectile.rotation += 0.01f * RotDir;
                projectile.scale = 1;
            }
            else if (projectile.ai[0] == 2)         //终曲 0-30缩小 30-90拉伸
            {
                projectile.extraUpdates = 1;
                projectile.ai[1]++;
                if (projectile.ai[1] < 30)
                {
                    //projectile.rotation += 0.02f * RotDir;
                    projectile.scale = 1 - (projectile.ai[1] / 30f * 0.01f);
                }
                if (projectile.ai[1] >= 30)
                {
                    projectile.scale = 0.9f;
                    projectile.Opacity = (90f - projectile.ai[1]) / 60f;
                    //Main.NewText(projectile.Opacity);
                    //Main.NewText(projectile.ai[1]);
                    if (projectile.ai[1] > 90)
                    {
                        projectile.Kill();
                        return;
                    }
                }
            }
            else if (projectile.ai[0] == 3)             //终止
            {
                projectile.rotation += 0.01f * RotDir;
                projectile.ai[1] = 0;
                projectile.scale -= 0.05f;
                if (projectile.scale < 0)
                {
                    projectile.scale = 0;
                    projectile.Kill();
                    return;
                }
            }
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
            Texture2D tex = Main.projectileTexture[projectile.type];
            if (projectile.ai[0] == 2 && projectile.ai[1] > 30)
            {
                float k = (projectile.ai[1] - 30f) / 60f * 1.5f + 1f;
                //k = 1;
                int DrawPosX = (int)(projectile.Center.X - Main.screenPosition.X);// - 256f / 2f * projectile.scale * RangeScale * k);
                //Main.NewText(DrawPosX - projectile.Center.X);
                int DrawPosY = (int)(projectile.Center.Y - Main.screenPosition.Y);// - 256f / 2f * projectile.scale * RangeScale);
                int DrawWidth = (int)(512 * projectile.scale * RangeScale * k);
                int DrawHeight = (int)(512 * projectile.scale * RangeScale);
                Rectangle rectangle = new Rectangle(DrawPosX, DrawPosY, DrawWidth, DrawHeight);
                spriteBatch.Draw(tex, rectangle, null, Color.White * projectile.Opacity, projectile.rotation, tex.Size() / 2, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, tex.Size() / 2, projectile.scale * RangeScale, SpriteEffects.None, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);
            return false;
        }


        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return targetHitbox.Distance(projectile.Center) <= 256f * projectile.scale * RangeScale;
        }
        public override bool CanDamage()
        {
            if (projectile.ai[0] < 2)
            {
                return true;
            }
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (MaxMusicRing(projectile.owner) > 0 && MaxMusicRing(projectile.owner) <= 3)
            {
                bool Chance = Main.rand.Next(25) == 1;
                if (target.GetGlobalNPC<DaCapoNPC>().StaggerResistance < 10)
                {
                    Chance = true;
                }
                if (Chance)
                {
                    target.GetGlobalNPC<DaCapoNPC>().StaggerResistance++;
                }
            }

            if (MaxMusicRing(projectile.owner) > 1)
            {
                target.buffImmune[ModContent.BuffType<FerventAdoration>()] = false;
                target.AddBuff(ModContent.BuffType<FerventAdoration>(), 240);
            }
            if (MaxMusicRing(projectile.owner) > 2)
            {
                target.buffImmune[ModContent.BuffType<FerventAdoration2>()] = false;
                target.buffImmune[ModContent.BuffType<FerventAdoration3>()] = false;
                target.AddBuff(ModContent.BuffType<FerventAdoration2>(), 240);
                target.AddBuff(ModContent.BuffType<FerventAdoration3>(), 100);
            }
            if (MaxMusicRing(projectile.owner) > 3)
            {
                bool Chance = Main.rand.Next(50) == 1;
                if (target.GetGlobalNPC<DaCapoNPC>().StaggerResistance < 15)
                {
                    Chance = true;
                }
                if (Chance)
                {
                    target.GetGlobalNPC<DaCapoNPC>().StaggerResistance++;
                }
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)(damage * (1f + (float)MaxMusicRing(projectile.owner) / 2));
        }
        public static int MaxMusicRing(int owner)
        {
            bool[] result = { false, false, false, false };
            foreach(Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == owner) 
                {
                    if (proj.type == ModContent.ProjectileType<MusicRing1>())
                    {
                        result[0] = true;
                    }
                    if (proj.type == ModContent.ProjectileType<MusicRing2>())
                    {
                        result[1] = true;
                    }
                    if (proj.type == ModContent.ProjectileType<MusicRing3>())
                    {
                        result[2] = true;
                    }
                    if (proj.type == ModContent.ProjectileType<MusicRing4>())
                    {
                        result[3] = true;
                    }
                }
            }

            for(int i = 3; i >= 0; i--)
            {
                if (result[i])
                {
                    return i + 1;
                }
            }
            return 0;
        }
    }
}