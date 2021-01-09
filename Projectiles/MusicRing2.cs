using DaCapo.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
namespace DaCapo.Projectiles 
{
    public class MusicRing2 : BaseMusicRing
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Second Movement");
            DisplayName.AddTranslation(GameCulture.Chinese, "第二乐章");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangeScale = 1.2f;
            RotDir = -1;
        }

    }
}