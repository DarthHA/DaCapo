using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DaCapo.Buffs
{
    public class FerventAdoration : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Fervent Adoration");
            DisplayName.AddTranslation(GameCulture.Chinese, "狂热崇拜");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
            canBeCleared = false;
        }


    }


}