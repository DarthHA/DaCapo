using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DaCapo.Buffs
{
    public class BaseDaCapoMovementBuff : ModBuff
    {
        public virtual string BuffName => "";
        public virtual string BuffNameCN => "";

        public virtual string BuffDescription => "";
        public virtual string BuffDescriptionCD => "";

        public virtual int Color => ItemRarityID.White;

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "DaCapo/Buffs/BaseDaCapoMovementBuff";
            return true;
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault(BuffName);
            DisplayName.AddTranslation(GameCulture.Chinese, BuffNameCN);
            Description.SetDefault(BuffDescription);
            Description.AddTranslation(GameCulture.Chinese, BuffDescriptionCD);
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
            longerExpertDebuff = false;
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            rare = Color;
        }

    }
}
