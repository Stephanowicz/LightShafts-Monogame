using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PostscreenEffects
{
    class ChainedRenderTarget
    {
        private static int          MAX_DOWNSAMPLE = 6;
        private RenderTarget2D[ ]   _DownSampleTargets;
        private RenderTarget2D[ ]   _UpSampleTargets;
        private RenderTarget2D[ ]   _TempTargets;
        private GraphicsDevice      _Device;
        private int                 _Width;
        private int                 _Height;
        private Effect              _LinearFilterEffect;
        private SpriteBatch         _SpriteBatch;

        // ---------------------------------------------------------
        public ChainedRenderTarget(
            SpriteBatch Sprite,
            Effect LinearFilterEffect,
            GraphicsDevice Device,
            int Width,
            int Height )
        {
            _SpriteBatch = Sprite;
            _LinearFilterEffect = LinearFilterEffect;
            _Device = Device;
            _Width = Width;
            _Height = Height;

            _DownSampleTargets = new RenderTarget2D[ MAX_DOWNSAMPLE ];
            _TempTargets = new RenderTarget2D[ MAX_DOWNSAMPLE ];
            _UpSampleTargets = new RenderTarget2D[ MAX_DOWNSAMPLE ];

            PrepareMipMapLevels( );
        }
        // ---------------------------------------------------------
        public void PrepareMipMapLevels( )
        {
            int Factor = 1;
            for ( int i = 0; i < MAX_DOWNSAMPLE; ++i )
            {
                _DownSampleTargets[ i ] = new RenderTarget2D(
                    _Device,
                    _Width / Factor,
                    _Height / Factor,
                    false,
                    SurfaceFormat.HalfVector4,DepthFormat.Depth24,
                    0, 
                    RenderTargetUsage.DiscardContents);

                _TempTargets[ i ] = new RenderTarget2D(
                    _Device,
                    _Width / Factor,
                    _Height / Factor,
                    false,
                    SurfaceFormat.HalfVector4, DepthFormat.Depth24,
                    0,
                    RenderTargetUsage.DiscardContents);

                if ( i < MAX_DOWNSAMPLE - 1 )
                {
                    _UpSampleTargets[ i + 1 ] = new RenderTarget2D(
                        _Device,
                        _Width / Factor,
                        _Height / Factor,
                        false,
                    SurfaceFormat.HalfVector4, DepthFormat.Depth24,
                    0,
                    RenderTargetUsage.DiscardContents);
                } // if

                Factor *= 2;
            } // for

            _UpSampleTargets[ 0 ] = new RenderTarget2D(
                _Device,
                _Width,
                _Height,
                false,
                SurfaceFormat.HalfVector4, DepthFormat.Depth24,
                0,
                RenderTargetUsage.DiscardContents);
        }
        // ---------------------------------------------------------
        public void GenerateMipMapLevels( )
        {
            Vector2 TextureSize = new Vector2( _Width, _Height );
            Rectangle Rect = new Rectangle( 
                0, 
                0,
                ( int ) _Width,
                ( int ) _Height );

            Effect effect = _LinearFilterEffect;
            Texture2D CurrentTex = RenderTarget;

           // PostScreenFilters._saveRTasPNG(RenderTarget, "levelStart.png");

            for ( int i = 0; i < MAX_DOWNSAMPLE; ++i )
            {

                TextureSize /= 2f;
                Rect.Width /= 2;
                Rect.Height /= 2;

                _Device.SetRenderTarget(_DownSampleTargets[ i ] );

            //    PostScreenFilters._saveRTasPNG(CurrentTex, "levelSetRenderTarget" + i + ".png");

                _Device.Clear(Color.Black);
                effect.CurrentTechnique = effect.Techniques[ 0 ];
                effect.Parameters[ "gTextureSize" ].SetValue( TextureSize );
                _SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

                effect.CurrentTechnique.Passes[ 0 ].Apply();
                _SpriteBatch.Draw( CurrentTex, Rect, Color.White );
                _SpriteBatch.End( );

                CurrentTex = _DownSampleTargets[ i ];
                //PostScreenFilters._saveRTasPNG(CurrentTex, "level" + i + ".png");
                _Device.SetRenderTarget( null );         

            } // for 
            _Device.SetRenderTarget(null );
            //PostScreenFilters._saveRTasPNG(_DownSampleTargets[5], "_DownSampleTargets5.png");
            //PostScreenFilters._saveRTasPNG(_DownSampleTargets[4], "_DownSampleTargets4.png");
            //PostScreenFilters._saveRTasPNG(_DownSampleTargets[3], "_DownSampleTargets3.png");
            //PostScreenFilters._saveRTasPNG(_DownSampleTargets[2], "_DownSampleTargets2.png");
            //PostScreenFilters._saveRTasPNG(_DownSampleTargets[1], "_DownSampleTargets1.png");
            //PostScreenFilters._saveRTasPNG(_DownSampleTargets[0], "_DownSampleTargets0.png");
        }

        // ---------------------------------------------------------
        public RenderTarget2D GetUpsampleTarget( )
        {
            return _UpSampleTargets[ 0 ];
            //return _RenderTarget;
        }
        // ---------------------------------------------------------
        public RenderTarget2D RenderTarget { get; set; }
         // ---------------------------------------------------------
        public RenderTarget2D[ ] GetMipLevels( )
        {
            return _DownSampleTargets;
        }
        // ---------------------------------------------------------
        public RenderTarget2D[ ] GetTempLevels( )
        {
            return _TempTargets;
        }
        // ---------------------------------------------------------
    }
}