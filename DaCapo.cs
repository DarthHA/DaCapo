using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using DaCapo.Sky;
using Terraria.UI;
using System.Collections.Generic;
using Terraria;
using Microsoft.Xna.Framework;
using DaCapo.UI;
using DaCapo.Projectiles;
using Microsoft.Xna.Framework.Audio;
using static DaCapo.DaCapoPlayer;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace DaCapo
{
    public class DaCapo : Mod
    {
        public static DaCapo Instance;
        public static DaCapoConfig config;
        private UserInterface _CurtainUIInterface;
        internal CurtainUI _CurtainUI;

        public static EnvirSound SecondBGM;
        public static bool SoundLoaded = false;

        public static Effect ChairEffect;

        public DaCapo()
        {
            Instance = this;
        }
        public override void Load()
        {
            Filters.Scene["DaCapo:DaCapoSky"] = new Filter(new DaCapoSkyScreenShaderData("FilterMiniTower").UseColor(1.0f, 1.0f, 1.0f).UseOpacity(0.0f), EffectPriority.VeryHigh);
            SkyManager.Instance["DaCapo:DaCapoSky"] = new DaCapoSky();

            _CurtainUI = new CurtainUI();
            _CurtainUIInterface = new UserInterface();
            _CurtainUIInterface.SetState(_CurtainUI);
            On.Terraria.Projectile.Kill += new On.Terraria.Projectile.hook_Kill(KillHook);

            SecondBGM = new EnvirSound(Instance.GetSound("Sounds/Angela3"), 0.3f);
            SoundLoaded = true;

            ChairEffect = GetEffect("Effects/Content/ChairEffect");

        }


        public static void KillHook(On.Terraria.Projectile.orig_Kill orig, Projectile self)
        {
            if (self.type == ModContent.ProjectileType<MusicRing1>() ||
                self.type == ModContent.ProjectileType<MusicRing2>() ||
                self.type == ModContent.ProjectileType<MusicRing3>()||
                    self.type == ModContent.ProjectileType<MusicRing4>())
            {
                if (self.ai[0] <= 1)
                {
                    self.penetrate = -1;
                    self.timeLeft = 99999;
                    return;
                }
            }
            orig.Invoke(self);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int CurtainUIIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interface Logic 1"));
            if (CurtainUIIndex != -1)
            {
                layers.Insert(CurtainUIIndex, new LegacyGameInterfaceLayer(
                    "DaCapo: BossState",
                    delegate
                    {
                        _CurtainUIInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void MidUpdatePlayerNPC()
        {
            if (Main.musicVolume == 0)
            {
                if (SoundLoaded)
                {
                    UpdateSecondMusic();
                }
            }
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
            {
                if (SoundLoaded)
                {
                    SecondBGM.QuickStop();
                }
                return;
            }
            if (Main.LocalPlayer.GetModPlayer<DaCapoPlayer>().CurrentPlayingMusic > MusicType.None)
            {
                string path = "Sounds/Music/";
                bool ShouldReplace = false;
                switch (Main.LocalPlayer.GetModPlayer<DaCapoPlayer>().CurrentPlayingMusic)
                {
                    case MusicType.Beginning:
                        path += "Clap1";
                        break;
                    case MusicType.Movement1:
                        path += "Movement1";
                        ShouldReplace = true;
                        break;
                    case MusicType.Movement2:
                        path += "Movement2";
                        break;
                    case MusicType.Movement3:
                        path += "Movement3";
                        ShouldReplace = true;
                        break;
                    case MusicType.Movement4:
                        path += "Movement4";
                        ShouldReplace = true;
                        break;
                    case MusicType.Final1:
                        path += "Final1";
                        ShouldReplace = true;
                        break;
                    case MusicType.Final2:
                        path += "Final2";
                        ShouldReplace = true;
                        break;
                    case MusicType.End:
                        path += "Clap2";
                        ShouldReplace = true;
                        break;
                    default:
                        break;
                }

                if (ShouldReplace && Main.curMusic != -1)
                {
                    if (Main.curMusic != GetSoundSlot(SoundType.Music, path))
                    {
                        if (Main.music[Main.curMusic].IsPlaying)
                        {
                            Main.music[Main.curMusic].Stop(AudioStopOptions.Immediate);
                        }
                    }
                }

                music = GetSoundSlot(SoundType.Music, path);

                if (ShouldReplace && Main.curMusic != -1)
                {
                    Main.musicFade[Main.curMusic] = 1;
                    if (Main.curMusic == GetSoundSlot(SoundType.Music, "Sounds/Music/Clap2"))
                    {
                        Main.musicFade[GetSoundSlot(SoundType.Music, "Sounds/Music/Final2")] = Utils.Clamp(Main.musicFade[GetSoundSlot(SoundType.Music, "Sounds/Music/Final2")] - 0.05f, 0, 1);
                    }
                }

                priority = MusicPriority.BossHigh;
            }

            if (SoundLoaded)
            {
                UpdateSecondMusic();
            }

        }

        public override void Unload()
        {
            SkyManager.Instance["DaCapo:DaCapoSky"].Deactivate();
            config = null;
            ChairEffect = null;
            SecondBGM = null;
            SoundLoaded = false;
            Instance = null;
        }

        public void UpdateSecondMusic()
        {
            if (!config.UseBGM)
            {
                SecondBGM.QuickStop();
                return;
            }
            if (Main.LocalPlayer.GetModPlayer<CurtainPlayer>().Active||
                Main.LocalPlayer.GetModPlayer<CurtainPlayer>().FinaleTimer > 0)
            {
                SecondBGM.QuickBegin();
            }
            else
            {
                SecondBGM.QuickStop();
            }
            SecondBGM.Update();
        }

        /*
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.PlatinumBar);
            recipe.SetResult(ItemID.PlatinumCoin, 1);
            recipe.AddTile(TileID.Furnaces);
            recipe.AddRecipe();
        }
        */

    }

    
}