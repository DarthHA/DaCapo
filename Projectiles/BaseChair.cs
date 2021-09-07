using DaCapo.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static DaCapo.VertexInfo;

namespace DaCapo.Projectiles
{
    public abstract class BaseChair : ModProjectile    //252 512
    {
        public static float Timer = 120;
        private float HeightModifier = 1;
        public bool Phase2 = false;
        public virtual int FaceDir => 1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Performer");
            DisplayName.AddTranslation(GameCulture.Chinese, "演奏者");
        }

        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.scale = 1f;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.timeLeft = 99999;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.damage = 0;
            projectile.penetrate = -1;
            projectile.hide = true;
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindProjectiles.Add(index);
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0.9f, 0.9f, 0.9f);
            Player owner = Main.player[projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                projectile.Kill();
                return;
            }
            if (!owner.GetModPlayer<DaCapoPlayer>().CheckPlayerPlaying())
            {
                projectile.Kill();
                return;
            }
            if (owner.GetModPlayer<CurtainPlayer>().FinaleTimer == 100)
            {
                projectile.Kill();
                return;
            }
            if (projectile.ai[0] < Timer)
            {
                projectile.ai[0]++;
                int Width = 25;
                for (int i = 0; i < 20; i++)
                {
                    Vector2 RandomPos = projectile.Center + new Vector2((Main.rand.NextFloat() * 2 - 1) * Width, Main.rand.Next(-4, 0));
                    int dust = Dust.NewDust(RandomPos, 1, 1, 20, 0, 0, 0, default, Main.rand.NextFloat() * 0.75f + 0.75f);
                    Main.dust[dust].noLight = false;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity = Vector2.Zero;
                }
            }
            else
            {
                if (projectile.localAI[0] == 0)
                {
                    SpecialAI(ref HeightModifier);
                }
                else
                {
                    FinalAI();
                }
            }
        }

        public virtual void SpecialAI(ref float HeightModifier)
        {
            
        }

        public static void EnterFinal()
        {
            foreach(Projectile proj in Main.projectile)
            {
                if (proj.active && (
                    proj.type == ModContent.ProjectileType<FirstChair>() ||
                    proj.type == ModContent.ProjectileType<SecondChair>() ||
                    proj.type == ModContent.ProjectileType<ThirdChair>() ||
                    proj.type == ModContent.ProjectileType<FourthChair>()))
                {
                    proj.ai[1] = 0;
                    proj.localAI[0] = 1;
                }
            }
        }

        public void FinalAI()
        {
            projectile.ai[1]++;
            if (projectile.ai[1] < 100)    //2620 - 2720
            {
                if (projectile.ai[1] < 10)
                {
                    if (projectile.ai[1] == 1) projectile.localAI[1] = projectile.rotation;
                    float DestRot = -FaceDir * 0.3f;
                    projectile.rotation = MathHelper.Lerp(projectile.localAI[1], DestRot, projectile.ai[1] / 10);
                    if (HeightModifier < 1)
                    {
                        HeightModifier += 0.015f;
                        if (HeightModifier > 1)
                        {
                            HeightModifier = 1;
                        }
                    }
                    else
                    {
                        HeightModifier -= 0.015f;
                        if (HeightModifier < 1)
                        {
                            HeightModifier = 1;
                        }
                    }
                }
                else
                {
                    float DestRot = -FaceDir * 0.3f;
                    projectile.rotation = DestRot + (float)Math.Sin(MathHelper.Pi * (projectile.ai[1] - 10) / 6) * 0.1f * FaceDir;
                }
            }
            else if (projectile.ai[1] < 320)             //2720 - 2960
            {
                if (projectile.ai[1] < 130)
                {
                    projectile.rotation = MathHelper.Lerp(-FaceDir * 0.25f, FaceDir * 0.3f, (projectile.ai[1] - 100) / 30);
                }
                else
                {
                    projectile.rotation = FaceDir * 0.3f + (float)Math.Sin(MathHelper.Pi * (projectile.ai[1] - 130) / 10) * 0.05f * FaceDir; ;
                }
            }
            else            //2960 -
            {
                if (projectile.ai[1] < 340)
                {
                    if (projectile.ai[1] == 320) projectile.localAI[1] = projectile.rotation;
                    projectile.rotation = MathHelper.Lerp(projectile.localAI[1], 0, (projectile.ai[1] - 320) / 20f);
                }
                else
                {
                    projectile.rotation = 0;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.ai[0] < Timer)
            {
                Draw1();
            }
            else
            {
                Draw2();
            }
            return false;
        }

        public void Draw1()
        {
            float k = projectile.ai[0] / Timer;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = Main.projectileTexture[projectile.type];
            int Width = 50;
            int Height = (int)(100f * k);
            int Left = (int)projectile.Center.X - Width / 2 - (int)Main.screenPosition.X;
            int Top = (int)projectile.Center.Y - Height - (int)Main.screenPosition.Y;
            Main.spriteBatch.Draw(tex, new Rectangle(Left, Top, Width, Height), new Rectangle(0, 0, tex.Width, (int)(tex.Height * k)), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public void Draw2()            //?
        {
            int Width = 50;
            float Height = 100 * HeightModifier;
            int NodeCount = 20;
            float dist = Height / NodeCount;
            List<CustomVertexInfo> bars = new List<CustomVertexInfo>();

            for (int i = 1; i <= NodeCount; i++)
            {
                float Rot2 = projectile.rotation / NodeCount * i - MathHelper.Pi / 2;
                float Rot1 = projectile.rotation / NodeCount * (i - 1) - MathHelper.Pi / 2;

                Vector2 TopLeft = projectile.Center + Rot2.ToRotationVector2() * dist * i + (Rot2 - MathHelper.Pi / 2).ToRotationVector2() * Width / 2;
                Vector2 BottomRight = projectile.Center + Rot1.ToRotationVector2() * dist * (i - 1) + (Rot1 + MathHelper.Pi / 2).ToRotationVector2() * Width / 2;
                bars.Add(new CustomVertexInfo(TopLeft, Color.White, new Vector3(0, 1 - (float)i / NodeCount, 0)));
                bars.Add(new CustomVertexInfo(BottomRight, Color.White, new Vector3(1, 1 - (i - 1f) / NodeCount, 0)));
            }

            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();

            if (bars.Count > 2)
            {
                // 按照顺序连接三角形
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    triangleList.Add(bars[i]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 1]);

                    triangleList.Add(bars[i + 1]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 3]);
                }


                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

                var screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
                var screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;
                var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
                var screenPos = screenCenter - screenSize / 2f;
                var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));


                // 把变换和所需信息丢给shader
                DaCapo.ChairEffect.Parameters["uTransform"].SetValue(model * projection);
                Main.graphics.GraphicsDevice.Textures[0] = Main.projectileTexture[projectile.type];
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

                DaCapo.ChairEffect.CurrentTechnique.Passes[0].Apply();

                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }



        public override bool ShouldUpdatePosition()
        {
            return false;
        }


    }
}