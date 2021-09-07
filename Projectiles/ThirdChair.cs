using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace DaCapo.Projectiles
{
    public class ThirdChair : BaseChair
    {
        public override int FaceDir => -1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Third Chair");
            DisplayName.AddTranslation(GameCulture.Chinese, "第三演奏者");
        }

        public override void SpecialAI(ref float HeightModifier)
        {
            projectile.ai[1]++;
            float t = projectile.ai[1] % 108;
            if (t < 27)
            {
                projectile.rotation = MathHelper.Lerp(0f, -0.15f, t / 27f);
            }
            else if (t < 81)
            {
                projectile.rotation = MathHelper.Lerp(-0.15f, 0.15f, (t - 27) / 54f);
            }
            else
            {
                projectile.rotation = MathHelper.Lerp(0.15f, 0f, (t - 81) / 27f);
            }
        }
    }
}