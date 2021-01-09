using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DaCapo.Buffs;
using Microsoft.Xna.Framework.Graphics;

namespace DaCapo
{
    public class DaCapoNPC : GlobalNPC
    {
		public override bool InstancePerEntity => true;
		public int StaggerResistance = 0;
		public int StaggerResistanceRegen = 0;
		public int SavedImmune = 114514;
		public override void PostAI(NPC npc)
		{
            if (StaggerResistanceRegen > 0)
            {
				StaggerResistanceRegen--;
            }
            else
            {
				StaggerResistanceRegen = 60;
				if (StaggerResistance > 0)
				{
					StaggerResistance--;
				}
                else
                {
					StaggerResistance = 0;
                }
			}
            if (StaggerResistance > 19)
            {
				StaggerResistance = 19;
            }
            if (npc.immune[Main.myPlayer] < SavedImmune)
            {
				SavedImmune = npc.immune[Main.myPlayer];
			}
            else
            {
				if (StaggerResistance > 0)
				{
					npc.immune[Main.myPlayer] -= (int)(npc.immune[Main.myPlayer] * (float)(StaggerResistance / 20f));
				}
				SavedImmune = npc.immune[Main.myPlayer];
			}
            
			
			//npc.buffImmune[ModContent.BuffType<FerventAdoration>()] = false;
            if (npc.friendly)
            {
				int index = npc.FindBuffIndex(ModContent.BuffType<FerventAdoration>());
				if (index != -1)
                {
					npc.DelBuff(index);
                }
            }

            if (npc.HasBuff(ModContent.BuffType<FerventAdoration3>()))
            {
				npc.position -= npc.velocity * 0.5f;
            }

			if (npc.HasBuff(ModContent.BuffType<FerventAdoration>()))
			{
				if (npc.dontTakeDamage)
				{
					return;
				}
				int specialHitSitter = 1;
				if (npc.immune[255] == 0)
				{
					int immuneTime = 10;
					Rectangle hitbox = npc.Hitbox;
					foreach (NPC target in Main.npc)
					{
						if (target.active && target.whoAmI != npc.whoAmI && CheckRealLife(npc, target)) 
						{
							if (!target.friendly && target.damage > 0)
							{
								Rectangle hitbox2 = target.Hitbox;
								float damageMultiplier = 1f;
								NPC.GetMeleeCollisionData(hitbox, target.whoAmI, ref specialHitSitter, ref damageMultiplier, ref hitbox2);
								bool? flag = NPCLoader.CanHitNPC(target, npc);
								if ((flag == null || flag.Value) && hitbox.Intersects(hitbox2) && ((flag != null && flag.Value) || npc.type != NPCID.SkeletonMerchant || !NPCID.Sets.Skeletons.Contains(target.netID)))
								{
									int dmg = target.damage;
									if (npc.HasBuff(ModContent.BuffType<FerventAdoration2>()))
									{
										dmg *= 5;
									}
									float kb = 6f;
									int hitDirection = 1;
									if (target.Center.X > npc.Center.X)
									{
										hitDirection = -1;
									}
									bool crit = Main.rand.NextBool();
									NPCLoader.ModifyHitNPC(target, npc, ref dmg, ref kb, ref crit);
									double realDmg = npc.StrikeNPCNoInteraction(dmg, kb, hitDirection, crit, false, false);
									if (Main.netMode != NetmodeID.SinglePlayer)
									{
										NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, dmg, kb, hitDirection, 0, 0, 0);
									}
									npc.netUpdate = true;
									npc.immune[255] = immuneTime;
									NPCLoader.OnHitNPC(target, npc, (int)realDmg, kb, crit);
		
								}
							}
						}
					}
				}
			}


			
		}

		public bool CheckRealLife(NPC npc1,NPC npc2)
        {
			if (npc1.realLife == -1 && npc2.realLife == -1) return true;
			if (npc1.realLife != -1 && npc2.realLife == -1)
			{
				if (npc1.realLife == npc2.whoAmI)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			if (npc1.realLife == -1 && npc2.realLife != -1)
			{
				if (npc2.realLife == npc1.whoAmI)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			if (npc1.realLife != -1 && npc2.realLife != -1)
            {
                if (npc1.realLife == npc2.realLife)
                {
					return false;
                }
                else
                {
					return true;
                }
            }
			if (npc1.type >= NPCID.EaterofWorldsHead && npc1.type <= NPCID.EaterofWorldsTail)
			{
				return false;
			}
			return true;
		}

		/*
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, StaggerResistance.ToString(), npc.Center.X - Main.screenPosition.X, npc.Top.Y - 20 - Main.screenPosition.Y, Color.White, Color.Black, Vector2.Zero, 1);
			return true;
        }
		*/
    }
}