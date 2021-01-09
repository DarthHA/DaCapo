using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DaCapo.Buffs
{
    public class FerventAdoration3 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Slowness");
            DisplayName.AddTranslation(GameCulture.Chinese, "减速");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
            canBeCleared = false;
        }


    }


}