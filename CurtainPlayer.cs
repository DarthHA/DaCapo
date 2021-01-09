using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DaCapo.Items;
using DaCapo.Projectiles;
using Terraria.DataStructures;

namespace DaCapo
{
    public class CurtainPlayer : ModPlayer
    {
        public bool Active = false;
        public float Progress = 0;
        public bool CanChange = true;
        public int TitleTimer = 0;
        public int Title;
        public float FinaleTimer = 0;
        public bool ShakeScreen = false;
        public override void ModifyScreenPosition()
        {
            if (ShakeScreen)
            {
                Main.screenPosition.X += Main.rand.Next(-3, 3);
                Main.screenPosition.Y += Main.rand.Next(-3, 3);
            }
            ShakeScreen = false;
        }
        public override void UpdateBiomeVisuals()
        {
            
            //Main.NewText("FinaleTiner: " + FinaleTimer);
            //Main.NewText("Progress: " + Progress);
            if (CanChange) 
            {
                Active = CheckPlayer();
                CanChange = false;
            }
            if (FinaleTimer > 0)
            {
                Active = false;
            }
            if (Active)
            {
                Progress += 0.01f;
                if (Progress > 2)
                {
                    Progress = 2;
                    CanChange = true;
                }
            }
            else
            {
                Progress -= 0.02f;
                if (Progress < 0)
                {
                    Progress = 0;
                    CanChange = true;
                }
            }
            player.ManageSpecialBiomeVisuals("DaCapo:DaCapoSky", true, default);
            if (TitleTimer > 0)
            {
                TitleTimer--;
            }
            else
            {
                Title = 0;
            }
            if (FinaleTimer > 0)
            {
                FinaleTimer--;
            }
            else
            {
                FinaleTimer = 0;
            }
        }

        public override void UpdateDead()
        {
            Active = false;
            FinaleTimer = 0;
            TitleTimer = 0;
        }




        public bool CheckPlayer()
        {
            return player.HeldItem.type == ModContent.ItemType<DaCapoItem>() && player.channel && AnyDaCapo();
        }

        private bool AnyDaCapo()
        {
            foreach(Projectile proj in Main.projectile)
            {
                if(proj.active && proj.owner == player.whoAmI)
                {
                    if (proj.type == ModContent.ProjectileType<DaCapoHeld>())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static void SetTitle(int title)
        {
            Main.LocalPlayer.GetModPlayer<CurtainPlayer>().Title = title;
            Main.LocalPlayer.GetModPlayer<CurtainPlayer>().TitleTimer = 90;
        } 
        
        public static void FinaleCurtain(Player player)
        {
            player.GetModPlayer<CurtainPlayer>().FinaleTimer = 200;   
        }

        public static void Finale(Player player)
        {
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && projectile.owner == player.whoAmI)
                {
                    if (projectile.type == ModContent.ProjectileType<MusicRing1>() ||
                       projectile.type == ModContent.ProjectileType<MusicRing2>() ||
                       projectile.type == ModContent.ProjectileType<MusicRing3>() ||
                       projectile.type == ModContent.ProjectileType<MusicRing4>())
                    {
                        projectile.ai[0] = 2;
                        projectile.ai[1] = 0;
                    }
                }
            }
        }
    }
}