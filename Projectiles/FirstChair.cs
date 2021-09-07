using Microsoft.Xna.Framework;
using System;
using Terraria.Localization;

namespace DaCapo.Projectiles
{
    public class FirstChair : BaseChair
    {
        public override int FaceDir => -1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The First Chair");
            DisplayName.AddTranslation(GameCulture.Chinese, "第一演奏者");
        }

        public override void SpecialAI(ref float HeightModifier)
        {
            projectile.ai[1]++;
            float t = projectile.ai[1] % 120;
            if (t < 40)
            {
                projectile.rotation = MathHelper.Lerp(0f, -0.25f, t / 40f);
            }
            else if (t < 80)
            {
                projectile.rotation = MathHelper.Lerp(-0.25f, 0.25f, (t - 40) / 40f);
            }
            else
            {
                projectile.rotation = MathHelper.Lerp(0.25f, 0f, (t - 80) / 40f);
            }
        }
    }
}