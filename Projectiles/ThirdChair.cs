using Terraria.Localization;

namespace DaCapo.Projectiles
{
    public class ThirdChair : BaseChair
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Third Chair");
            DisplayName.AddTranslation(GameCulture.Chinese, "第三演奏者");
        }
    }
}