using Terraria.Localization;

namespace DaCapo.Projectiles
{
    public class FirstChair : BaseChair
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The First Chair");
            DisplayName.AddTranslation(GameCulture.Chinese, "第一演奏者");
        }
    }
}