using DaCapo.Buffs;
using DaCapo.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
namespace DaCapo.Projectiles
{
    public class DaCapoStrike : ModProjectile
    {
        public static readonly float OffSet = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moderato");
            DisplayName.AddTranslation(GameCulture.Chinese, "中板");

        }

        public override void SetDefaults()  //512  512   16  0.75
        {
            projectile.magic = true;
            projectile.width = 1;
            projectile.height = 1;
            projectile.scale = 1f;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 99999;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.damage = 10;
            projectile.penetrate = -1;
            projectile.alpha = 255;
        }
        public override void AI()
        {
            projectile.timeLeft = 9999;
            projectile.hostile = false;
            projectile.friendly = true;
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
            //projectile.rotation = projectile.velocity.ToRotation();
            owner.itemTime = 2;
            owner.itemAnimation = 2;
            owner.direction = Math.Sign(projectile.velocity.X + 0.01f);
            owner.heldProj = projectile.whoAmI;
            //int dir = owner.direction;
            //owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);
            projectile.Center = owner.Center;
            if (owner.mount.Active)
            {
                projectile.Center = owner.MountedCenter;
            }

            if (projectile.ai[0] == 0)
            {
                int dir = owner.direction;
                float rot = projectile.ai[1] / 16 * MathHelper.Pi;
                if (owner.direction < 0)
                {
                    rot = MathHelper.Pi / 4 * 3 - (rot - MathHelper.Pi);
                }
                else
                {
                    rot = MathHelper.Pi / 4 + (rot - MathHelper.Pi);
                }
                owner.itemRotation = (float)Math.Atan2(rot.ToRotationVector2().Y * dir, rot.ToRotationVector2().X * dir);
                
                projectile.ai[1]++;
                projectile.Opacity = projectile.ai[1] / 16;
                if (projectile.alpha >= 16)
                {
                    projectile.alpha = 0;
                    projectile.ai[0] = 1;
                    projectile.ai[1] = 0;
                }
            }
            else if (projectile.ai[0] == 1) 
            {
                int dir = owner.direction;
                float rot;
                if (owner.direction < 0)
                {
                    rot = MathHelper.Pi / 4 * 3;
                }
                else
                {
                    rot = MathHelper.Pi / 4;
                }
                owner.itemRotation = (float)Math.Atan2(rot.ToRotationVector2().Y * dir, rot.ToRotationVector2().X * dir);
                projectile.ai[1]++;
                projectile.Opacity = 1 - projectile.ai[1] / 30;
                if (projectile.ai[1] >= 20)
                {
                    projectile.alpha = 255;
                    projectile.Kill();
                }

            }

        }

        public override bool CanDamage()
        {
            if (projectile.ai[0] == 1 && projectile.alpha > 150)
            {
                return false;
            }
            return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player owner = Main.player[projectile.owner];

            Texture2D tex = Main.projectileTexture[projectile.type];
            SpriteEffects SP = owner.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
            spriteBatch.Draw(tex, owner.Center - Main.screenPosition, null, Color.White * projectile.Opacity, 0, tex.Size() / 2, projectile.scale * 0.3f, SP, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);

            if (projectile.ai[0] == 1)
            {
                tex = mod.GetTexture("Projectiles/DaCapoEnd");
                SpriteEffects SP2 = owner.direction < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None;
                if (owner.direction < 0)
                {
                    Vector2 origin = new Vector2(53, tex.Height - 40);
                    float r = MathHelper.Pi / 4 * 3;
                    spriteBatch.Draw(tex, owner.Center - Main.screenPosition, null, Color.White, r, origin, projectile.scale * 0.6f, SP2, 0);
                }
                else
                {
                    Vector2 origin = new Vector2(53, 40);
                    float r = MathHelper.Pi / 4;
                    spriteBatch.Draw(tex, owner.Center - Main.screenPosition, null, Color.White, r, origin, projectile.scale * 0.6f, SP2, 0);
                }

            }
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return targetHitbox.Distance(projectile.Center) <= 154 * projectile.scale;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<FerventAdoration>(), 100);
        }
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center - new Vector2(150 * projectile.scale, 0), projectile.Center + new Vector2(150 * projectile.scale, 0), 300 * projectile.scale, DelegateMethods.CutTiles);
        }
    }
}