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
    public class DaCapoSlash : ModProjectile
    {
        public static readonly float OffSet = 30;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Presto Passionato");
            DisplayName.AddTranslation(GameCulture.Chinese, "热情的急板");
            Main.projFrames[projectile.type] = 9;

        }

        public override void SetDefaults()  //420  192
                                              //319 127
        {
            projectile.magic = true;
            projectile.width = 192;
            projectile.height = 192;
            projectile.scale = 1f;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 99999;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.damage = 10;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 3;
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
            projectile.rotation = projectile.velocity.ToRotation();
            owner.itemTime = 2;
            owner.itemAnimation = 2;
            owner.direction = Math.Sign(projectile.velocity.X + 0.01f);
            owner.heldProj = projectile.whoAmI;
            int dir = owner.direction;
            owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);
            projectile.Center = owner.Center + projectile.rotation.ToRotationVector2() * OffSet;
            if (owner.mount.Active)
            {
                projectile.Center = owner.MountedCenter + projectile.rotation.ToRotationVector2() * OffSet;
            }

            if (projectile.ai[0] == 0)
            {
                if (++projectile.frameCounter > 1)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                }
                if (projectile.frame >= 8)
                {
                    projectile.ai[0] = 1;
                }
            }
            else if (projectile.ai[0] == 1) 
            {
                projectile.ai[1]++;
                if (projectile.ai[1] >= 20)
                {
                    projectile.Kill();
                    return;
                }

            }

        }

        public override bool CanDamage()
        {
            if (projectile.ai[0] == 1)
            {
                return false;
            }
            return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player owner = Main.player[projectile.owner];

            Texture2D tex = Main.projectileTexture[projectile.type];
            Rectangle rectangle = new Rectangle(0, tex.Height / 9 * projectile.frame, tex.Width, tex.Height / 9);
            SpriteEffects SP = owner.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            if (owner.direction < 0)
            {
                float r = projectile.rotation + MathHelper.Pi;
                if (owner.gravDir < 0)
                {
                    SP = SpriteEffects.None;
                    r -= MathHelper.Pi;
                }
                spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, rectangle, Color.White, r, rectangle.Size() / 2, projectile.scale * 0.6f, SP, 0);
            }
            else
            {
                if (owner.gravDir < 0)
                {
                    SP = SpriteEffects.FlipVertically;
                }
                spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, rectangle, Color.White, projectile.rotation, rectangle.Size() / 2, projectile.scale * 0.6f, SP, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            
            if (projectile.ai[0] == 1)
            {
                tex = mod.GetTexture("Projectiles/DaCapoEnd");
                SpriteEffects SP2 = owner.direction < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None;
                if (owner.direction < 0)
                {
                    Vector2 origin = new Vector2(53, tex.Height - 40);
                    float r = projectile.rotation + MathHelper.Pi;
                    float offset2 = owner.mount.Active ? 6 : 0;
                    spriteBatch.Draw(tex, owner.Center - new Vector2(0, offset2) - Main.screenPosition, null, Color.White, r + MathHelper.Pi / 8, origin, projectile.scale * 0.6f, SP2, 0);
                }
                else
                {
                    Vector2 origin = new Vector2(53, 40);
                    float r = projectile.rotation;
                    float offset2 = owner.mount.Active ? 6 : 0;
                    spriteBatch.Draw(tex, owner.Center - new Vector2(0, offset2) - Main.screenPosition, null, Color.White, r + MathHelper.Pi / 8 * 7, origin, projectile.scale * 0.6f, SP2, 0);
                }

            }
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center - projectile.rotation.ToRotationVector2() * 190, projectile.Center + projectile.rotation.ToRotationVector2() * 190, 192, ref point);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.GetGlobalNPC<DaCapoNPC>().StaggerResistance < 15)
            {
                target.GetGlobalNPC<DaCapoNPC>().StaggerResistance++;
            }
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center - projectile.rotation.ToRotationVector2() * 190 * projectile.scale, projectile.Center + projectile.rotation.ToRotationVector2() * 190 * projectile.scale, 192 * projectile.scale, DelegateMethods.CutTiles);
        }
    }
}