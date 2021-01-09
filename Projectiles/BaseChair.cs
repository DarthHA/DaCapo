using DaCapo.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace DaCapo.Projectiles
{
    public abstract class BaseChair : ModProjectile    //252 512
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Performer");
            DisplayName.AddTranslation(GameCulture.Chinese, "бнзреп");
        }

        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.scale = 1f;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.timeLeft = 99999;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.damage = 0;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                projectile.Kill();
                return;
            }
            if (!owner.GetModPlayer<DaCapoPlayer>().CheckPlayer())
            {
                projectile.Kill();
                return;
            }
            if (owner.GetModPlayer<CurtainPlayer>().FinaleTimer == 100)
            {
                projectile.Kill();
                return;
            }
            if (projectile.ai[0] < 360)
            {
                projectile.ai[0]++;
            }
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float k = projectile.ai[0] / 360f;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
            Texture2D tex = Main.projectileTexture[projectile.type];
            int Left = (int)projectile.Center.X - 25 - (int)Main.screenPosition.X;
            int Width = 50;
            int Height = (int)(100f * k);
            //Main.NewText(Height);
            int Top = (int)projectile.Center.Y - Height - (int)Main.screenPosition.Y;
            spriteBatch.Draw(tex, new Rectangle(Left, Top, Width, Height), new Rectangle(0, 0, tex.Width, (int)(tex.Height * k)), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);
            return false;
        }



        public override bool ShouldUpdatePosition()
        {
            return false;
        }


    }
}