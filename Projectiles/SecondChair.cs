using Terraria.Localization;

namespace DaCapo.Projectiles
{
    public class SecondChair : BaseChair
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Second Chair");
            DisplayName.AddTranslation(GameCulture.Chinese, "第二演奏者");
        }
    }
}