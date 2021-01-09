using DaCapo.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DaCapo.Items
{
	public class DaCapoItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Da Capo");
			DisplayName.AddTranslation(GameCulture.Chinese, "Da Capo");
			Tooltip.SetDefault("\"A scythe that swings silently and with discipline like a conductor's gestures and baton.\n" +
				"If there were a score for this song, it would be one that sings of the apocalypse.\n" +
				"The wielder of this E.G.O is able to indulge in the music of silence that no one else can hear.\n" +
				"The conductor does not wish to end the performance until it reaches the finale.\n" +
                "However, it may be wise to leave some audience alive to applaud at the end.\"\n" +
				"[c/FF0000:ALEPH] E.G.O. Weapon\n" +
				"Right click to launch melee attacks that weaken enemies' immunity frames\n" +
				"Hold the left button to play, but user cannot move at this time.\n" +
				"During the performance, you will alternately be immune to melee and projectile damage, and the other damage will be reduced to 20%\n" +
				"As the performers join, the surrounding enemies will receive higher damage and stronger effects\n" +
				"At this time, enemys' immunity frame will be weakened, their speed will be reduced, and they will hurt each other\n" +
				"When the performance is near the end, it will cause a huge damage to the enemies in screen\n" +
				"If the enemy has been previously affected by the performance, the effect will increase\n" +
				"Your health and mana will return to the peak after finishing playing\n" +
				"Each performance has a 5 second cooldown.");
			Tooltip.AddTranslation(GameCulture.Chinese, "持有者挥舞这把镰刀时宛如一名沉默而温和的指挥家，\n" +
				"而它的总谱就是对世界末日的警告。\n" +
				"持有这把E.G.O武器的员工会被除他以外无人能听到的音乐所淹没。\n" +
				"指挥家只有在演奏结束时才会休息，\n" +
				"请务必为这场音乐会的闭幕献上热烈的掌声。\n" +
				"[c/FF0000:ALEPH]级E.G.O.武器\n" +
				"右键可以发动近战攻击并削弱敌人无敌帧\n" +
				"按住左键可以进行演奏，此时无法移动\n" +
				"演奏中你会轮流免疫近战和弹幕伤害，不免疫的效果会获得80%减伤\n" +
				"随着演奏者的加入，周围的敌人会受到越来越高的伤害和越来越强的效果\n" +
				"此时敌人的无敌帧会被削弱，速度降低，并且会互相伤害\n" +
				"当演奏接近尾声时，会对全屏造成一次巨大的伤害\n" +
				"如果敌人先前受到过演奏的影响，那么该效应会加剧\n" +
				"结束演奏时你的生命和魔力均会回归巅峰。\n" +
				"每场演奏有5秒钟的冷却。");
		}

		public override void SetDefaults() 
		{
			item.damage = 100;
			item.magic = true;
			item.width = 68;
			item.height = 72;
			item.scale = 0.5f;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 1;
			item.value = Item.sellPrice(50, 0, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<DaCapoSlash>();
			item.noMelee = true;
			item.useTurn = false;
			item.noUseGraphic = true;
		}
		
		public override void ModifyTooltips(List<TooltipLine> list)
		{
			foreach (TooltipLine tooltipLine in list)
			{
				if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
				{
					tooltipLine.overrideColor = new Color?(Color.Lerp(Color.White, Color.Blue, (float)Main.DiscoR / 255));
				}
			}
		}

		public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
			{
				item.mana = 1;
				item.autoReuse = true;
				item.channel = false;
				item.useTurn = false;
            }
            else
            {
				item.mana = 200;
				item.autoReuse = false;
				item.channel = true;
				item.useTurn = true;
			}
            if (player.altFunctionUse != 2)
            {
				if(player.GetModPlayer<CurtainPlayer>().Progress > 0)
                {
					return false;
                }
				if (player.GetModPlayer<CurtainPlayer>().FinaleTimer > 0)
                {
					return false;
                }
				if (player.GetModPlayer<DaCapoPlayer>().DaCapoCD > 0)
                {
					return false;
                }

			}
			return true;
        }

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{
				Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
				if (player.GetModPlayer<DaCapoPlayer>().NormalAtkType == 0)
				{
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/HeadAtk"), player.Center);
					Projectile.NewProjectile(player.Center, ShootVel, ModContent.ProjectileType<DaCapoStrike>(), (int)(damage * 5f), knockBack * 5, player.whoAmI);
				}
				else if (player.GetModPlayer<DaCapoPlayer>().NormalAtkType == 1)
				{
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/HeadAtk"), player.Center);
					Projectile.NewProjectile(player.Center, ShootVel, ModContent.ProjectileType<DaCapoSlashUp>(), (int)(damage * 7.5f), knockBack * 3, player.whoAmI);
				}
				else
				{
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/ChorAtk"), player.Center);
					Projectile.NewProjectile(player.Center, ShootVel, ModContent.ProjectileType<DaCapoSlash>(), damage, knockBack / 2, player.whoAmI);
				}
				player.GetModPlayer<DaCapoPlayer>().NormalAtkType = (player.GetModPlayer<DaCapoPlayer>().NormalAtkType + 1) % 3;

			}
			else
			{
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<DaCapoHeld>(), (int)(damage * 1.35f), knockBack, player.whoAmI);
			}
			return false;
		}

    }
}