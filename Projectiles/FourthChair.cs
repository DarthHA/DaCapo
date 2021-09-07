using Microsoft.Xna.Framework;
using System;
using Terraria.Localization;

namespace DaCapo.Projectiles
{
    public class FourthChair : BaseChair
    {
        public override int FaceDir => 1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Fourth Chair");
            DisplayName.AddTranslation(GameCulture.Chinese, "第4444演奏者");
        }

        public override void SpecialAI(ref float HeightModifier)
        {
            projectile.ai[1]++;
            float t = projectile.ai[1] % 112;
            if (t < 28)
            {
                projectile.rotation = MathHelper.Lerp(0f, -0.2f, t / 28f);
            }
            else if (t < 84)
            {
                projectile.rotation = MathHelper.Lerp(-0.2f, 0.2f, (t - 28) / 56f);
            }
            else
            {
                projectile.rotation = MathHelper.Lerp(0.2f, 0f, (t - 84) / 28f);
            }
            HeightModifier = (float)Math.Sin(MathHelper.Pi * projectile.ai[1] / 14f) * 0.05f + 1f;
        }
    }
}