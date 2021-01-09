using System.ComponentModel;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace DaCapo
{
    public class DaCapoConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(true)]
        [Label("$Mods.DaCapo.UseBGM")]
        public bool UseBGM;

        


        public override ModConfig Clone()
        {
            var clone = (DaCapoConfig)base.Clone();
            return clone;
        }

        public override void OnLoaded()
        {
            DaCapo.config = this;
            ModTranslation modTranslation = DaCapo.Instance.CreateTranslation("UseBGM");
            modTranslation.SetDefault("Angela 3 will play during the performance");
            modTranslation.AddTranslation(GameCulture.Chinese, "演奏时播放Angela 3");
            DaCapo.Instance.AddTranslation(modTranslation);
        }


        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string messageline)
        {
            string message = "";
            string messagech = "";

            if (Language.ActiveCulture == GameCulture.Chinese)
            {
                messageline = messagech;
            }
            else
            {
                messageline = message;
            }

            if (whoAmI == 0)
            {
                //message = "Changes accepted!";
                //messagech = "设置改动成功!";
                return true;
            }
            if (whoAmI != 0)
            {
                //message = "You have no rights to change config.";
                //messagech = "你没有设置改动权限.";
                return false;
            }
            return false;
        }
    }
}