using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace DaCapo.UI
{
    internal class CurtainUI : UIState
    {
        public override void OnInitialize()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.LocalPlayer.GetModPlayer<CurtainPlayer>().Active || Main.LocalPlayer.GetModPlayer<CurtainPlayer>().FinaleTimer > 0) 
            {
            }
            else
            {
                return;
            }

            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CurtainPlayer modplayer = Main.LocalPlayer.GetModPlayer<CurtainPlayer>();
            if (modplayer.Active)
            {
                if (modplayer.Progress >= 0 && modplayer.Progress < 1)
                {
                    DrawFullCurtain(spriteBatch, 0, modplayer.Progress, Color.White);

                    for (float i = 0; i <= 10f; i++)
                    {
                        float k = i / 300f;
                        float a = (10f - i) / 10f;
                        DrawFullCurtain(spriteBatch, modplayer.Progress + k, modplayer.Progress + k + 0.01f, Color.White * a);
                    }

                }
                if (modplayer.Progress >= 1 && modplayer.Progress <= 2)
                {
                    float k = 2 - modplayer.Progress;
                    DrawHalfCurtain(spriteBatch, k, Color.White);
                    Texture2D tex1 = DaCapo.Instance.GetTexture("UI/CurtainLeft");
                    Texture2D tex2 = DaCapo.Instance.GetTexture("UI/CurtainRight");
                    spriteBatch.Draw(tex1, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
                    spriteBatch.Draw(tex2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
                }
            }

            if (modplayer.FinaleTimer > 0)
            {
                if (modplayer.FinaleTimer > 100)
                {
                    float k = 2 - modplayer.FinaleTimer / 100f;
                    DrawHalfCurtain(spriteBatch, k, Color.White);
                    Texture2D tex1 = DaCapo.Instance.GetTexture("UI/CurtainLeft");
                    Texture2D tex2 = DaCapo.Instance.GetTexture("UI/CurtainRight");
                    spriteBatch.Draw(tex1, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
                    spriteBatch.Draw(tex2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
                }
                else
                {
                    float progress = modplayer.FinaleTimer / 100;
                    DrawFullCurtain(spriteBatch, 0, progress, Color.White);

                    for (float i = 0; i <= 10f; i++)
                    {
                        float k = i / 300f;
                        float a = (10f - i) / 10f;
                        DrawFullCurtain(spriteBatch, progress + k, progress + k + 0.01f, Color.White * a);
                    }
                }
            }


            if (modplayer.TitleTimer > 0)
            {
                float Opal;
                if (modplayer.TitleTimer > 60)
                {
                    Opal = (float)(90 - modplayer.TitleTimer) / 30;
                }
                else if (modplayer.TitleTimer > 30)
                {
                    Opal = 1;
                }
                else
                {
                    Opal = (float)modplayer.TitleTimer / 30;
                }
                string path = "UI/";
                switch (modplayer.Title)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        path += "Movement" + modplayer.Title.ToString();
                        break;
                    default:
                        path += "Finale";
                        break;
                }
                Texture2D tex = DaCapo.Instance.GetTexture(path);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
                spriteBatch.Draw(tex, new Vector2(Main.screenWidth / 2, Main.screenHeight / 3 * 2), null, Color.White * Opal, 0, tex.Size() / 2, 1, SpriteEffects.None, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);

            }
        }


        private void DrawFullCurtain(SpriteBatch sb, float d1, float d2, Color color)
        {
            Texture2D tex = DaCapo.Instance.GetTexture("UI/FullCurtain");
            sb.Draw(tex, new Rectangle((int)(Main.screenWidth * d1), 0, (int)(Main.screenWidth * d2), Main.screenHeight), new Rectangle((int)(tex.Width * d1), 0, (int)(tex.Width * d2), tex.Height), color);
        }


        /// <summary>
        /// 0为关，1为开
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="d"></param>
        /// <param name="color"></param>
        private void DrawHalfCurtain(SpriteBatch sb, float d, Color color)
        {
            Texture2D tex = DaCapo.Instance.GetTexture("UI/Curtain");
            int k1 = -Main.screenWidth / 2 + (int)(Main.screenWidth / 2 * (d / 2 + 0.5));
            sb.Draw(tex, new Rectangle(k1, 0, k1 + Main.screenWidth / 2, Main.screenHeight), null, color, 0, Vector2.Zero, SpriteEffects.None, 0);
            int k2 = Main.screenWidth - (int)(Main.screenWidth / 2 * d);
            sb.Draw(tex, new Rectangle(k2, 0, k2 + Main.screenWidth / 2, Main.screenHeight), null, color, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0) ;
        }
    }
}
