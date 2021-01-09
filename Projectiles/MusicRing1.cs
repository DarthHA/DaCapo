using Terraria.Localization;
namespace DaCapo.Projectiles
{
    public class MusicRing1 : BaseMusicRing
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The First Movement");
            DisplayName.AddTranslation(GameCulture.Chinese, "第一乐章");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangeScale = 0.7f;
            RotDir = 1;
        }

    }
}