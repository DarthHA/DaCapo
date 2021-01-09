using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;

namespace DaCapo.Sky
{
    public class DaCapoSky : CustomSky
	{
		bool isActive = false;
		public override void Update(GameTime gameTime) 
		{
			
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			CurtainPlayer modplayer = Main.LocalPlayer.GetModPlayer<CurtainPlayer>();
			if (maxDepth >= 0 && minDepth < 0)
			{
				if (isActive)
				{
					float intensity = Utils.Clamp(modplayer.Progress, 0, 1);
                    if (modplayer.FinaleTimer >= 100)
                    {
						intensity = 1;
                    }
					DrawSky(spriteBatch, 0, intensity, Color.White);
					if (intensity > 0 && intensity < 1)
					{
						for (float i = 0; i <= 10f; i++)
						{
							float k = i / 300f;
							float a = (10f - i) / 10f;
							DrawSky(spriteBatch, intensity + k, intensity + k + 0.01f, Color.White * a);
						}
					}
				}
                else
                {
					float intensity = modplayer.Progress / 2;
					DrawSky(spriteBatch, 0, intensity, Color.White);
					if (intensity > 0 && intensity < 1)
					{
						for (float i = 0; i <= 10f; i++)
						{
							float k = i / 300f;
							float a = (10f - i) / 10f;
							DrawSky(spriteBatch, intensity + k, intensity + k + 0.01f, Color.White * a);
						}
					}
				}
				
			}
		}

		private void DrawSky(SpriteBatch sb, float d1, float d2, Color color)
		{
			Texture2D tex = DaCapo.Instance.GetTexture("Sky/DaCapoSky");
			sb.Draw(tex, new Rectangle((int)(Main.screenWidth * d1), 0, (int)(Main.screenWidth * d2), Main.screenHeight), new Rectangle((int)(tex.Width * d1), 0, (int)(tex.Width * d2), tex.Height), color);
		}

		public override float GetCloudAlpha() 
		{
			return 0f;
		}

		
		public override void Activate(Vector2 position, params object[] args) 
		{
			isActive = true;
			return;
			//Main.LocalPlayer.GetModPlayer<CurtainPlayer>().Active = true;
		}

		
		public override void Deactivate(params object[] args) 
		{
			isActive = false;
			return;
			//Main.LocalPlayer.GetModPlayer<CurtainPlayer>().Active = false;
		}

		public override void Reset() 
		{
			isActive = false;
			return;
			//Main.LocalPlayer.GetModPlayer<CurtainPlayer>().Active = false;
		}

		public override bool IsActive() 
		{
			return isActive;
		}
		
	}
}