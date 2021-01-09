using Terraria.Localization;
namespace DaCapo.Projectiles
{
    public class MusicRing4 : BaseMusicRing
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Fourth Movement");
            DisplayName.AddTranslation(GameCulture.Chinese, "第四乐章");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangeScale = 2.2f;
            RotDir = -1;
        }
    }
}