using Microsoft.Xna.Framework.Audio;
using Terraria;

namespace DaCapo
{
    public class EnvirSound
    {
        public SoundEffectInstance sound;
        public float MaxVolume = 1;
        public float Step = 0.015f;
        public EnvirSound(SoundEffect soundEffect, float maxVolume = 1, float step = 0.015f)
        {
            sound = soundEffect.CreateInstance();
            sound.Pan = 0;
            sound.Pitch = 0;
            sound.IsLooped = true;
            MaxVolume = maxVolume;
            
        }

        public void QuickBegin()
        {
            if (sound == null) return;
            if (sound.State != SoundState.Playing)
            {
                sound.Play();
            }
        }

        public void QuickStop()
        {
            if (sound == null) return;
            if (sound.State != SoundState.Stopped)
            {
                sound.Stop(true);
            }
        }

        public void Update()
        {
            if (sound == null) return;
            if (sound.State == SoundState.Playing)
            {
                sound.Volume = MaxVolume * Main.soundVolume;
            }
        }
    }
}