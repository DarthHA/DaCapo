using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DaCapo.Items;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace DaCapo
{
    public class ItemGetNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool CanDropGift = true;      //月总用ai3标记眼睛
        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            if (item.type != ItemID.TheAxe || damage > 500)
            {
                if (npc.type == NPCID.MoonLordCore)
                {
                    Failed(npc);
                }
                if (npc.type == NPCID.MoonLordHand || npc.type == NPCID.MoonLordHead)
                {
                    if (Main.npc[(int)npc.ai[3]].active)
                    {
                        Failed(Main.npc[(int)npc.ai[3]]);
                    }
                }
            }
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (projectile.npcProj) return;
            if ((projectile.type != ProjectileID.EighthNote && projectile.type != ProjectileID.QuarterNote && projectile.type != ProjectileID.TiedEighthNote && projectile.type != ProjectileID.SpectreWrath) ||
            damage > 500)
            {
                if (npc.type == NPCID.MoonLordCore)
                {
                    Failed(npc);
                }

                if (npc.type == NPCID.MoonLordHand || npc.type == NPCID.MoonLordHead)
                {
                    if (Main.npc[(int)npc.ai[3]].active)
                    {
                        Failed(Main.npc[(int)npc.ai[3]]);
                    }
                }
            }
        }

        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.MoonLordCore)
            {
                if (CanDropGift)
                {
                    CombatText.NewText(npc.Hitbox, Color.Blue, "Da Capo!");
                    Item.NewItem(npc.Hitbox, ModContent.ItemType<DaCapoItem>());
                }
            }
        }

        public void Failed(NPC npc)
        {
            if (npc.GetGlobalNPC<ItemGetNPC>().CanDropGift)
            {
                npc.GetGlobalNPC<ItemGetNPC>().CanDropGift = false;
                if (Main.LocalPlayer.HeldItem.type == ItemID.TheAxe || Main.LocalPlayer.HeldItem.type == ItemID.MagicalHarp)
                {
                    if (Language.ActiveCulture == GameCulture.Chinese)
                    {
                        CombatText.NewText(Main.LocalPlayer.Hitbox, Color.Red, "挑战失败!");
                    }
                    else
                    {
                        CombatText.NewText(Main.LocalPlayer.Hitbox, Color.Red, "Failed!");
                    }
                }
            }
        }
    }
}