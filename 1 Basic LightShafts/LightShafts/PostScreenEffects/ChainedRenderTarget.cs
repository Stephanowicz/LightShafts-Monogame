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

                Factor *= 2;
            } // for
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
        }

        // ---------------------------------------------------------
        public RenderTarget2D RenderTarget { get; set; }
         // ---------------------------------------------------------
        public RenderTarget2D[ ] GetMipLevels( )
        {
            return _DownSampleTargets;
        }
        // ---------------------------------------------------------
     }
}