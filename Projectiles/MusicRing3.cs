using DaCapo.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace DaCapo.Projectiles
{
    public class MusicRing3 : BaseMusicRing
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Third Movement");
            DisplayName.AddTranslation(GameCulture.Chinese, "第三乐章");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangeScale = 1.4f;
            RotDir = 1;
        }

    }
}