using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DaCapo.Buffs
{
    public class DaCapoCDBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Da Capo Cooldown");
            DisplayName.AddTranslation(GameCulture.Chinese, "Da Capo冷却");
            Description.SetDefault("The conductor will end the performance when it reaches the finale.");
            Description.AddTranslation(GameCulture.Chinese, "指挥家会在演奏结束时休息。");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            canBeCleared = false;
            longerExpertDebuff = false;

        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            player.buffTime[buffIndex] = time;
            return true;
        }
    }
}
