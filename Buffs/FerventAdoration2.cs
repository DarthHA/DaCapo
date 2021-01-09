using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DaCapo.Buffs
{
    public class FerventAdoration2 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Vulnerable");
            DisplayName.AddTranslation(GameCulture.Chinese, "易损");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
            canBeCleared = false;
        }


    }


}