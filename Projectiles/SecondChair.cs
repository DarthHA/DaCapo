using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace DaCapo.Projectiles
{
    public class SecondChair : BaseChair
    {
        public override int FaceDir => 1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Second Chair");
            DisplayName.AddTranslation(GameCulture.Chinese, "第二演奏者");
        }

        public override void SpecialAI(ref float HeightModifier)
        {
            projectile.ai[1]++;
            float t = projectile.ai[1] % 100;
            if (t < 25)
            {
                projectile.rotation = MathHelper.Lerp(0f, -0.2f, t / 25f);
                HeightModifier = MathHelper.Lerp(1f, 1.1f, t / 25f);
            }
            else if (t < 75)
            {
                projectile.rotation = MathHelper.Lerp(-0.2f, 0.2f, (t - 25) / 50f);
                if (t < 50)
                {
                    HeightModifier = MathHelper.Lerp(1.1f, 1f, (t - 25) / 25f);
                }
                else
                {
                    HeightModifier = MathHelper.Lerp(1f, 1.1f, (t - 50) / 25f);
                }
            }
            else
            {
                projectile.rotation = MathHelper.Lerp(0.2f, 0f, (t - 75) / 25f);
                HeightModifier = MathHelper.Lerp(1.1f, 1f, (t - 75) / 25f);
            }
            
        }
    }
}