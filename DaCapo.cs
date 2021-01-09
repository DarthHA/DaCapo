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

namespace DaCapo
{
    public class DaCapo : Mod
    {
        public static DaCapo Instance;
        public static DaCapoConfig config;
        private UserInterface _CurtainUIInterface;
        internal CurtainUI _CurtainUI;
        public static bool SecondBGM;
        public DaCapo()
        {
            Instance = this;
        }
        public override void Load()
        {
            Filters.Scene["DaCapo:DaCapoSky"] = new Filter(new DaCapoSkyScreenShaderData("FilterMiniTower").UseColor(0.9f, 0.9f, 0.9f).UseOpacity(0.2f), EffectPriority.VeryHigh);
            SkyManager.Instance["DaCapo:DaCapoSky"] = new DaCapoSky();

            _CurtainUI = new CurtainUI();
            _CurtainUIInterface = new UserInterface();
            _CurtainUIInterface.SetState(_CurtainUI);
            On.Terraria.Projectile.Kill += new On.Terraria.Projectile.hook_Kill(KillHook);
            On.Terraria.Main.PlaySound_int_int_int_int_float_float += new On.Terraria.Main.hook_PlaySound_int_int_int_int_float_float(PlaySoundHook);


        }

        public static SoundEffectInstance PlaySoundHook(On.Terraria.Main.orig_PlaySound_int_int_int_int_float_float orig,int type, int x = -1, int y = -1, int Style = 1, float volumeScale = 1f, float pitchOffset = 0f)
        {
            if (SecondBGM)
            {
                if (type == 0)
                {
                    return null;
                }
            }
            return orig.Invoke(type, x, y, Style, volumeScale, pitchOffset);
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

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
            {
                StopSecondBGM();
                return;
            }
            if (Main.LocalPlayer.GetModPlayer<DaCapoPlayer>().MusicType >= 0)
            {
                string path = "Sounds/Music/";
                switch (Main.LocalPlayer.GetModPlayer<DaCapoPlayer>().MusicType)
                {
                    case 0:
                        path += "Clap1";
                        break;
                    case 1:
                        if (Main.curMusic != GetSoundSlot(SoundType.Music, path + "Movement1"))
                        {
                            Main.music[Main.curMusic].Stop(AudioStopOptions.Immediate);
                        }
                        path += "Movement1";
                        break;
                    case 2:
                        path += "Movement2";
                        break;
                    case 3:
                        path += "Movement3";
                        break;
                    case 4:
                        path += "Movement4";
                        break;
                    case 5:
                        path += "Final1";
                        break;
                    case 6:
                        if (Main.curMusic != GetSoundSlot(SoundType.Music, path + "Final2"))
                        {
                            Main.music[Main.curMusic].Stop(AudioStopOptions.Immediate);
                        }
                        path += "Final2";
                        break;
                    case 7:
                        path += "Clap2";
                        break;
                    default:
                        break;
                }

                music = GetSoundSlot(SoundType.Music, path);

                if (Main.curMusic == GetSoundSlot(SoundType.Music,"Sounds/Music/Final2"))
                {
                    Main.musicFade[Main.curMusic] = 1;
                }
                if (Main.curMusic == GetSoundSlot(SoundType.Music, "Sounds/Music/Clap2"))
                {
                    Main.musicFade[GetSoundSlot(SoundType.Music, "Sounds/Music/Final2")] = Utils.Clamp(Main.musicFade[GetSoundSlot(SoundType.Music, "Sounds/Music/Final2")] - 0.05f, 0, 1);
                    Main.musicFade[Main.curMusic] = 1;
                }

                priority = MusicPriority.BossHigh;
            }
            UpdateSecondMusic();
        }
        public override void Unload()
        {
            StopSecondBGM();
            SkyManager.Instance["DaCapo:DaCapoSky"].Deactivate();
            config = null;
            Instance = null;
        }

        public static void PlaySecondBGM()
        {
            if (SecondBGM) return;
            if (Main.soundInstanceDig[2] != null)
            {
                Main.soundInstanceDig[2].Stop();
            }
            SecondBGM = true;
            Main.soundInstanceDig[2] = Instance.GetSound("Sounds/Angela3").CreateInstance();
            Main.soundInstanceDig[2].Pan = 0;
            Main.soundInstanceDig[2].Pitch = 0;
            Main.soundInstanceDig[2].IsLooped = true;
            Main.soundInstanceDig[2].Play();
        }
        public static void StopSecondBGM()
        {
            if (!SecondBGM) return;
            SecondBGM = false;
            Main.soundInstanceDig[2].Stop();
        }
        public void UpdateSecondMusic()
        {
            if (!config.UseBGM)
            {
                StopSecondBGM();
                return;
            }
            if (Main.LocalPlayer.GetModPlayer<CurtainPlayer>().Active||
                Main.LocalPlayer.GetModPlayer<CurtainPlayer>().FinaleTimer > 0)
            {
                PlaySecondBGM();
            }
            else
            {
                StopSecondBGM();
            }

            if (SecondBGM)
            {
                Main.soundInstanceDig[2].Volume = Main.musicVolume * 0.3f;
            }
        }
        
    }

    
}