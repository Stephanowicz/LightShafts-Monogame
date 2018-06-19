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
        private Effect              _LinearFilterEffect;        
        private Effect              _GaussianFilterEffect;
        private Effect              _AdditiveBlendEffect;
        private Effect              _DoFEffect;
        private Effect              _LightShafts;
        private Effect              _ExtractLuminance;
        private Effect              _VolumetricLighting;
        private int                 _Width;
        private int                 _Height;
        private GraphicsDevice      _Device;
        private ChainedRenderTarget _CRT;
        private SpriteBatch         _SpriteBatch;

        // ---------------------------------------------------------
        public PostScreenFilters(
            int Width,
            int Height,
            GraphicsDevice Device,
            ContentManager Content )
        {
            _Device = Device;
            _SpriteBatch = new SpriteBatch( _Device );
            _Width = Width;
            _Height = Height;
            _LinearFilterEffect = Content.Load<Effect>( "Effects\\LinearFilter" );            
            _GaussianFilterEffect = Content.Load<Effect>( "Effects\\GaussianFilter" );
            _AdditiveBlendEffect = Content.Load<Effect>( "Effects\\AdditiveBlend" );
            _DoFEffect = Content.Load<Effect>( "Effects\\DepthOfField" );
            _LightShafts = Content.Load<Effect>( "Effects\\LightShafts" );
            _ExtractLuminance = Content.Load<Effect>( "Effects\\LuminanceFilter" );
            _VolumetricLighting = Content.Load<Effect>( "Effects\\VolumetricLighting" );

            _CRT = new ChainedRenderTarget(
                new SpriteBatch( _Device ),
                _LinearFilterEffect,
                _GaussianFilterEffect,
                _AdditiveBlendEffect,
                _Device,
                Width,
                Height );
        }
        // ---------------------------------------------------------
        public RenderTarget2D[ ] CreateMipMapLevels( 
            RenderTarget2D Source )
        {
            _CRT.RenderTarget = Source;
            _CRT.GenerateMipMapLevels( );
            return _CRT.GetMipLevels( );
        }
        // ---------------------------------------------------------
        public RenderTarget2D AdditiveBlend(
            RenderTarget2D Source )
        {
            _CRT.RenderTarget = Source;
            _CRT.GenerateMipMapLevels( );
            _CRT.ApplyBlur( );
            _CRT.AdditiveBlend( );
            return _CRT.GetUpsampleTarget( );
        }
        // ---------------------------------------------------------
        public void AmbientOcclusion(
            RenderTarget2D Depthmap,
            RenderTarget2D Destination )
        {
            // TODO!!
        }
        // ---------------------------------------------------------
        public void DepthOfField(
            RenderTarget2D WorldCoordinates,
            RenderTarget2D Source,
            RenderTarget2D Destination,
            float Aperture,
            float ZNear,
            float ZFar,            
            float PlaneInFocus,
            float FocalLength  )
        {
            _CRT.RenderTarget = Source;
            _CRT.GenerateMipMapLevels( );
            _CRT.ApplyBlur( );
            _CRT.AdditiveBlend( );
            RenderTarget2D[ ] BlurLevels = _CRT.GetMipLevels( );

            /*
            _Device.SetRenderTarget(  Destination );
            _Device.Clear( Color.Black );
            _SpriteBatch.Begin(
                    SpriteBlendMode.None,
                    SpriteSortMode.Immediate,
                    SaveStateMode.SaveState );
            _SpriteBatch.Draw( BlurLevels[1],
                Vector2.Zero,
                Color.White );
            _SpriteBatch.End( );
            _Device.SetRenderTarget( null );
            */
          
            _Device.SetRenderTarget( Destination );
            _Device.Clear( ClearOptions.Target, Vector4.Zero, 1, 0 );

            _Device.Textures[ 0 ] = WorldCoordinates;
            _Device.Textures[ 1 ] = Source;
            _Device.Textures[ 2 ] = BlurLevels[ 0 ];
            _DoFEffect.CurrentTechnique = _DoFEffect.Techniques[ 0 ];
            _DoFEffect.Parameters[ "ViewportSize" ].SetValue( new Vector2( _Width, _Height ) );
            _DoFEffect.Parameters[ "TextureSize" ].SetValue( new Vector2( _Width, _Height ) );
            _DoFEffect.Parameters[ "MatrixTransform" ].SetValue( Matrix.Identity );
            _DoFEffect.Parameters[ "gAperture" ].SetValue( Aperture );
            _DoFEffect.Parameters[ "gZNear" ].SetValue( ZNear );
            _DoFEffect.Parameters[ "gZFar" ].SetValue( ZFar );
            _DoFEffect.Parameters[ "gPlaneInFocus" ].SetValue( PlaneInFocus );
            _DoFEffect.Parameters[ "gFocalLength" ].SetValue( FocalLength );
            foreach ( EffectPass pass in _DoFEffect.CurrentTechnique.Passes )
            {
                _SpriteBatch.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.Opaque);
                pass.Apply( );
                _SpriteBatch.Draw( WorldCoordinates,
                    Vector2.Zero,
                    Color.White );
                _SpriteBatch.End( );
            } // foreach
            _Device.Textures[ 0 ] = null;
            _Device.Textures[ 1 ] = null;
            _Device.Textures[ 2 ] = null;
            _Device.Textures[ 3 ] = null;
            _Device.Textures[ 4 ] = null;
            _Device.Textures[ 5 ] = null;
            _Device.Textures[ 6 ] = null;
            _Device.Textures[ 7 ] = null;
            _Device.SetRenderTarget(  null );
        }      
        // ---------------------------------------------------------
        public void LightShafts( 
            RenderTarget2D RenderTargetMask,
            RenderTarget2D Destination,
            Vector2 LightPos,            
            float Density,
            float Decay,
            float Weight,
            float Exposure )
        {
            _Device.SetRenderTarget( Destination );
            _Device.Clear( ClearOptions.Target, Vector4.Zero, 1, 0 );

            Effect effect = _LightShafts;
            effect.CurrentTechnique = effect.Techniques[ 0 ];
            effect.Parameters["gScreenLightPos"].SetValue(LightPos);
            effect.Parameters["gDensity"].SetValue(Density);
            effect.Parameters["gDecay"].SetValue(Decay);
            effect.Parameters["gWeight"].SetValue(Weight);
            effect.Parameters["gExposure"].SetValue(Exposure);

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
           // _saveRTasPNG(RenderTargetMask, "RenderTargetMask.png");
           // _saveRTasPNG(Destination, "LightShafts.png");

            _Device.SetRenderTarget( null );
        }
        // ---------------------------------------------------------
        public void VolumetricLighting(
            RenderTarget2D WorldCoordinates,
            RenderTarget2D ShadowMap,
            RenderTarget2D Destination,
            Matrix LightView,
            Matrix LightProjection )
        {
            _Device.SetRenderTarget(  Destination );
            _Device.Clear( ClearOptions.Target, Vector4.Zero, 1, 0 );
 
            Matrix LightViewProjection = LightView * LightProjection;

            Effect effect = _VolumetricLighting;
            effect.CurrentTechnique = effect.Techniques[ 0 ];
            effect.Parameters[ "ViewportSize" ].SetValue( 
                new Vector2( _Width, _Height ) );
            effect.Parameters[ "TextureSize" ].SetValue( 
                new Vector2( _Width, _Height ) );
            effect.Parameters[ "MatrixTransform" ].SetValue( 
                Matrix.Identity );
            effect.Parameters[ "gLightViewProjectionXf" ].SetValue( 
                LightViewProjection );
            effect.Parameters[ "gTextureWorld" ].SetValue(
                WorldCoordinates);
            effect.Parameters[ "gTextureShadow" ].SetValue(
                ShadowMap);

            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Opaque);

            effect.CurrentTechnique.Passes[0].Apply();

            _SpriteBatch.Draw( WorldCoordinates, 
                Vector2.Zero, 
                Color.White );
            _SpriteBatch.End( );

            _Device.Textures[ 0 ] = null;
            _Device.Textures[ 1 ] = null;
            _Device.SetRenderTarget( null );
        }
        // ---------------------------------------------------------
        public void ExtractHighLuminance(
            RenderTarget2D Source,
            RenderTarget2D Destination,
            float Threshold,
            float ScaleFactor )
        {
            _Device.SetRenderTarget(  Destination );
            _Device.Clear( ClearOptions.Target, Vector4.Zero, 1, 0 );

            _Device.Textures[ 0 ] = Source;
            Effect effect = _ExtractLuminance;
            effect.CurrentTechnique = effect.Techniques[ 0 ];

            effect.Parameters[ "gThreshold" ].SetValue( Threshold );
            effect.Parameters[ "gScaleFactor" ].SetValue( ScaleFactor );

            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Opaque);

            effect.CurrentTechnique.Passes[ 0 ].Apply();

            _SpriteBatch.Draw( Source,
                Vector2.Zero,
                Color.White );
            _SpriteBatch.End( );

            //_saveRTasPNG(Source, "ExtractHighLuminance_src.png");
            //_saveRTasPNG(Destination, "ExtractHighLuminance.png");

            _Device.SetRenderTarget( null );
        }
        // ---------------------------------------------------------
        public void SixteenthToFullscreen( 
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
                ( int ) ( _Width / 4 ),
                ( int ) ( _Height / 4 ) );

            RenderTarget2D[ ] TempTargets = _CRT.GetTempLevels( );
            _Device.SetRenderTarget( TempTargets[ 1 ] );
            _SpriteBatch.Begin( );
            _SpriteBatch.Draw(
                Source,
                ScreenSize,
                Color.White );
            _SpriteBatch.End( );
            _Device.SetRenderTarget( null );

            ScreenSize = new Rectangle(
                0,
                0,
                ( int ) ( _Width / 2 ),
                ( int ) ( _Height / 2 ) );

            _Device.SetRenderTarget( TempTargets[ 0 ] );
            _SpriteBatch.Begin( );
            _SpriteBatch.Draw(
                TempTargets[ 1 ],
                ScreenSize,
                Color.White );
            _SpriteBatch.End( );
            _Device.SetRenderTarget( null );

            ScreenSize = new Rectangle(
                0,
                0,
                ( int ) _Width,
                ( int ) _Height );

            _Device.SetRenderTarget(  Destination );
            _SpriteBatch.Begin( );
            _SpriteBatch.Draw(
                TempTargets[ 0 ],
                ScreenSize,
                Color.White );
            _SpriteBatch.End( );
            _Device.SetRenderTarget( null );
        }
        // ---------------------------------------------------------
        public void QuarterToFullscreen( 
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
                ( int ) ( _Width / 2 ),
                ( int ) ( _Height / 2 ) );

            RenderTarget2D[ ] TempTargets = _CRT.GetTempLevels( );

            _Device.SetRenderTarget( TempTargets[ 0 ] );
            _SpriteBatch.Begin( );
            _SpriteBatch.Draw(
                Source,
                ScreenSize,
                Color.White );
            _SpriteBatch.End( );
            _Device.SetRenderTarget( null );

            ScreenSize = new Rectangle(
                0,
                0,
                ( int ) _Width,
                ( int ) _Height );

            _Device.SetRenderTarget(  Destination );
            _SpriteBatch.Begin( );
            _SpriteBatch.Draw(
                TempTargets[ 0 ],
                ScreenSize,
                Color.White );
            _SpriteBatch.End( );
            _Device.SetRenderTarget( null );
        }
        // ---------------------------------------------------------
        public void HalfToFullscreen( 
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
            RenderTarget2D[ ] TempTargets = _CRT.GetTempLevels( );
            _Device.SetRenderTarget(  Destination );
            _SpriteBatch.Begin( );
            _SpriteBatch.Draw(
                Source,
                ScreenSize,
                Color.White );
            _SpriteBatch.End( );
            _Device.SetRenderTarget( null );
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
