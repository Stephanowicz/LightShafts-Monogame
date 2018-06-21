using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PostscreenEffects
{
    public class PostScreenFilters
    {
        private Effect _LinearFilterEffect;
        private Effect _LightShafts;
        private int _Width;
        private int _Height;
        private GraphicsDevice _Device;
        private SpriteBatch _SpriteBatch;

        // ---------------------------------------------------------
        public PostScreenFilters(
            int Width,
            int Height,
            GraphicsDevice Device,
            ContentManager Content)
        {
            _Device = Device;
            _SpriteBatch = new SpriteBatch(_Device);
            _Width = Width;
            _Height = Height;
            _LinearFilterEffect = Content.Load<Effect>("Effects\\PostScreenEffects\\LinearFilter");
            _LightShafts = Content.Load<Effect>("Effects\\PostScreenEffects\\LightShafts");
        }
        // ---------------------------------------------------------
        public void linearFilter(
            RenderTarget2D rtSource,
            RenderTarget2D rtDestination,
            int sampleSize)
        {
            Vector2 TextureSize = new Vector2(_Width/ sampleSize, _Height/ sampleSize);
            Rectangle Rect = new Rectangle(
                0,
                0,
                (int)_Width/ sampleSize,
                (int)_Height/ sampleSize);

            Effect effect = _LinearFilterEffect;
            _Device.SetRenderTarget(rtDestination);
            _Device.Clear(Color.Black);
            effect.CurrentTechnique = effect.Techniques[0];
            effect.Parameters["gTextureSize"].SetValue(TextureSize);
            _SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

            effect.CurrentTechnique.Passes[0].Apply();
            _SpriteBatch.Draw(rtSource, Rect, Color.White);
            _SpriteBatch.End();
            _Device.SetRenderTarget(null);
        //    _saveRTasPNG(rtDestination, "linearFilter.png");
        }
        // ---------------------------------------------------------
        public void LightShafts(  
            RenderTarget2D RenderTargetMask,
            RenderTarget2D Destination,
            Vector2 LightPos,            
            float Density,
            float Decay,
            float Weight,
            float Exposure,
            int numSamples)
        {
            _Device.SetRenderTarget( Destination );
            _Device.Clear( ClearOptions.Target, Vector4.Zero, 1, 0 );

            Effect effect = _LightShafts;
            effect.CurrentTechnique = effect.Techniques[ 0 ];
            effect.Parameters["gScreenLightPos"].SetValue(LightPos* 2);
            effect.Parameters["gDensity"].SetValue(Density);
            effect.Parameters["gDecay"].SetValue(Decay);
            effect.Parameters["gWeight"].SetValue(Weight);
            effect.Parameters["gExposure"].SetValue(Exposure);
            effect.Parameters["NUM_SAMPLES"].SetValue(numSamples);

            foreach ( EffectPass pass in effect.CurrentTechnique.Passes )
            {
                _SpriteBatch.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.Opaque);
                pass.Apply();
                _SpriteBatch.Draw( RenderTargetMask,
                    Vector2.Zero,
                    Color.White );
                _SpriteBatch.End( );
            }
            //_saveRTasPNG(RenderTargetMask, "RenderTargetMask.png");
            //_saveRTasPNG(Destination, "LightShafts.png");

            _Device.SetRenderTarget( null );
        }
        // ---------------------------------------------------------
        public void _ToFullscreen( 
            RenderTarget2D Source,
            RenderTarget2D Destination )
        {
            /*
             * Indices:
             * [ 0 ] = Half
             * [ 1 ] = Quarter
             * [ 2 ] = Eighth
             */
            Rectangle ScreenSize = new Rectangle( 
                0,
                0,
                ( int ) _Width, 
                ( int ) _Height );
            _Device.SetRenderTarget(  Destination );
            _SpriteBatch.Begin( );
            _SpriteBatch.Draw(
                Source,
                ScreenSize,
                Color.White );
            _SpriteBatch.End( );
            _Device.SetRenderTarget( null );
//            _saveRTasPNG(Destination, "HalfToFullscreen.png");
        }
        // ---------------------------------------------------------
        static System.IO.Stream stream;
        public static void _saveRTasPNG(Texture2D rt, string filename)
        {
            stream = System.IO.File.Create(filename);
            rt.SaveAsPng(stream, rt.Width, rt.Height);
            stream.Close();

        }

    }
}
