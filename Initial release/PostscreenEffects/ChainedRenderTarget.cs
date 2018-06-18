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
        private static float        SCALE_FACTOR = 2.0f;
        private static String       BLUR_QUALITY = "Quality3x3";

        private RenderTarget2D      _RenderTarget;
        private RenderTarget2D[ ]   _DownSampleTargets;
        private RenderTarget2D[ ]   _UpSampleTargets;
        private RenderTarget2D[ ]   _TempTargets;
        private GraphicsDevice      _Device;
        private int                 _Width;
        private int                 _Height;
        private Effect              _LinearFilterEffect;
        private Effect              _GaussianFilterEffect;
        private Effect              _AdditiveBlendEffect;
        private SpriteBatch         _SpriteBatch;

        // ---------------------------------------------------------
        public ChainedRenderTarget(
            SpriteBatch Sprite,
            Effect LinearFilterEffect,
            Effect GaussianFilterEffect,
            Effect AdditiveBlendEffect,
            GraphicsDevice Device,
            int Width,
            int Height )
        {
            _SpriteBatch = Sprite;
            _LinearFilterEffect = LinearFilterEffect;
            _GaussianFilterEffect = GaussianFilterEffect;
            _AdditiveBlendEffect = AdditiveBlendEffect;
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
            Texture2D CurrentTex = _RenderTarget;
            System.IO.FileStream stream;// = System.IO.File.Create("levelStart.png");
            //CurrentTex.SaveAsPng(stream, CurrentTex.Width, CurrentTex.Height);
            //stream.Close();

            for ( int i = 0; i < MAX_DOWNSAMPLE; ++i )
            {

                TextureSize /= 2f;
                Rect.Width /= 2;
                Rect.Height /= 2;

                _Device.SetRenderTarget(_DownSampleTargets[ i ] );

                //stream = System.IO.File.Create("levelSetRenderTarget" + i + ".png");
                //CurrentTex.SaveAsPng(stream, CurrentTex.Width, CurrentTex.Height);
                //stream.Close();

                 //_Device.Clear( ClearOptions.Target, Vector4.Zero, 1, 0 );
                _Device.Clear(Color.Black);
//                _Device.Textures[ 0 ] = CurrentTex;
                effect.CurrentTechnique = effect.Techniques[ 0 ];
                effect.Parameters[ "gTextureSize" ].SetValue( TextureSize );
                _SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

                effect.CurrentTechnique.Passes[ 0 ].Apply();
                _SpriteBatch.Draw( CurrentTex, Rect, Color.White );
                _SpriteBatch.End( );

                CurrentTex = _DownSampleTargets[ i ];
                //stream = System.IO.File.Create("level" + i + ".png");
                //CurrentTex.SaveAsPng(stream, CurrentTex.Width, CurrentTex.Height);
                //stream.Close();
                _Device.SetRenderTarget( null );         

            } // for 
            _Device.SetRenderTarget(null );
            //stream = System.IO.File.Create("_DownSampleTargets5.png");
            //_DownSampleTargets[5].SaveAsPng(stream, _DownSampleTargets[5].Width, _DownSampleTargets[5].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets4.png");
            //_DownSampleTargets[4].SaveAsPng(stream, _DownSampleTargets[4].Width, _DownSampleTargets[4].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets3.png");
            //_DownSampleTargets[3].SaveAsPng(stream, _DownSampleTargets[3].Width, _DownSampleTargets[3].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets2.png");
            //_DownSampleTargets[2].SaveAsPng(stream, _DownSampleTargets[2].Width, _DownSampleTargets[2].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets1.png");
            //_DownSampleTargets[1].SaveAsPng(stream, _DownSampleTargets[1].Width, _DownSampleTargets[1].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets0.png");
            //_DownSampleTargets[0].SaveAsPng(stream, _DownSampleTargets[0].Width, _DownSampleTargets[0].Height);
            //stream.Close();
        }
        // ---------------------------------------------------------   
        /*
        public void AdditiveBlend( )
        {
            //Rectangle Rect = new Rectangle( 0, 0, _Width, _Height );
            //int Factor = 32;
            //for ( int i = 0; i > MAX_DOWNSAMPLE; ++i )
            //{
            //    _Device.SetRenderTarget( _UpSampleTargets[ MAX_DOWNSAMPLE - i ] );
            //    _Device.Clear( ClearOptions.Target, Vector4.Zero, 1, 0 );
            //    Rect.Width = _Width / Factor;
            //    Rect.Height = _Height / Factor;
            //    _SpriteBatch.Begin( );
            //    _SpriteBatch.Draw(
            //        _DownSampleTargets[ 5 ],
            //        Rect,
            //        Color.White );
            //    _SpriteBatch.End( );
            //    _Device.SetRenderTarget( null );
            //    Factor /= 2;
            //}

            _Device.SetRenderTarget( _UpSampleTargets[ 0 ] );
            _Device.Clear( ClearOptions.Target, Vector4.Zero, 1, 0 );
            _Device.Textures[ 0 ] = _DownSampleTargets[ 5 ];
            _AdditiveBlendEffect.CurrentTechnique = _AdditiveBlendEffect.Techniques[ 0 ];
            _AdditiveBlendEffect.Parameters[ "gFactor" ].SetValue( 1.0f );
            _AdditiveBlendEffect.Begin( );
            EffectPass pass = _AdditiveBlendEffect.CurrentTechnique.Passes[ 0 ];
            _SpriteBatch.Begin(
                SpriteBlendMode.None,
                SpriteSortMode.Immediate,
                SaveStateMode.SaveState );
            pass.Apply();
            _SpriteBatch.Draw(
                _DownSampleTargets[ 5 ],
                new Rectangle( 0, 0, _Width, _Height ),
                Color.White );
            //pass.End( );
            _SpriteBatch.End( );
            _AdditiveBlendEffect.End( );
            _Device.SetRenderTarget( null );
        }
        /*

        public void AdditiveBlend( )
        {          
            Rectangle Rect = new Rectangle( 0, 0, _Width, _Height );

            int Factor = 32;
            for ( int i = MAX_DOWNSAMPLE - 1; i > 0; --i )
            {
                Rect.Width = _Width / Factor;
                Rect.Height = _Height / Factor;

                _Device.SetRenderTarget( _UpSampleTargets[ i ] );
                _Device.Clear( ClearOptions.Target, Vector4.Zero, 1, 0 );
                _SpriteBatch.Begin(
                    SpriteBlendMode.Additive,
                    SpriteSortMode.Immediate,
                    SaveStateMode.SaveState );

                _SpriteBatch.Draw(
                    _DownSampleTargets[ i ],
                    Rect,
                    Color.White );
                _SpriteBatch.Draw(
                    _DownSampleTargets[ i - 1 ],
                    Rect,
                    Color.White );
                
                _SpriteBatch.End( );
                _Device.SetRenderTarget( null );
                Factor /= 2;
            } // for      

            _Device.SetRenderTarget( _UpSampleTargets[ 0 ] );
            _Device.Clear( ClearOptions.Target, Vector4.Zero, 1, 0 );
            _Device.Textures[ 0 ] = _UpSampleTargets[ 1 ];
            _AdditiveBlendEffect.CurrentTechnique = _AdditiveBlendEffect.Techniques[ 0 ];
            _AdditiveBlendEffect.Parameters[ "gFactor" ].SetValue( 2.0f );
            _AdditiveBlendEffect.Begin( );
            EffectPass pass = _AdditiveBlendEffect.CurrentTechnique.Passes[ 0 ];
            _SpriteBatch.Begin( 
                SpriteBlendMode.None,
                SpriteSortMode.Immediate,
                SaveStateMode.SaveState );
            pass.Apply();
            _SpriteBatch.Draw(
                _UpSampleTargets[ 1 ],
                new Rectangle( 0, 0, _Width, _Height ),
                Color.White );
            //pass.End( );
            _SpriteBatch.End( );
            _AdditiveBlendEffect.End( );
            _Device.SetRenderTarget( null );
        } 
        */
        
        public void AdditiveBlend( )
        {
            // this is one ugly ass method: 
            // all the blurred targets are combined to one target, again

            //m_Device.Textures[ 0 ] = m_DownSampleTargets[ 5 ];
            //m_Device.Textures[ 1 ] = m_DownSampleTargets[ 4 ];

            // 1/64x1/64->1/32x1/32
            _Device.SetRenderTarget(_UpSampleTargets[ 5 ] );
            _Device.Clear( Color.Black );
            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Additive );
            _SpriteBatch.Draw(
                _DownSampleTargets[ 5 ],
                new Rectangle( 0, 0, _Width / 32, _Height / 32 ),
                Color.White );
            _SpriteBatch.Draw(
                _DownSampleTargets[ 4 ],
                new Rectangle( 0, 0, _Width / 32, _Height / 32 ),
                Color.White );
            _SpriteBatch.End( );

            // 1/32x1/32->1/16x1/16
            _Device.SetRenderTarget( _UpSampleTargets[ 4 ] );
            _Device.Clear( Color.Black );
            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Additive);
            _SpriteBatch.Draw(
                _UpSampleTargets[ 5 ],
                new Rectangle( 0, 0, _Width / 16, _Height / 16 ),
                Color.White );
            _SpriteBatch.Draw(
                _DownSampleTargets[ 3 ],
                new Rectangle( 0, 0, _Width / 16, _Height / 16 ),
                Color.White );
            _SpriteBatch.End( );

            // 1/16x1/16->1/8x1/8
            _Device.SetRenderTarget(_UpSampleTargets[ 3 ] );
            _Device.Clear( Color.Black );
            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Additive);
            _SpriteBatch.Draw(
                _UpSampleTargets[ 4 ],
                new Rectangle( 0, 0, _Width / 8, _Height / 8 ),
                Color.White );
            _SpriteBatch.Draw(
                _DownSampleTargets[ 2 ],
                new Rectangle( 0, 0, _Width / 8, _Height / 8 ),
                Color.White );
            _SpriteBatch.End( );

            // 1/8x1/8->1/4x1/4
            _Device.SetRenderTarget(_UpSampleTargets[ 2 ] );
            _Device.Clear( Color.Black );
            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Additive);
            _SpriteBatch.Draw(
                _UpSampleTargets[ 3 ],
                new Rectangle( 0, 0, _Width / 4, _Height / 4 ),
                Color.White );
            _SpriteBatch.Draw(
                _DownSampleTargets[ 1 ],
                new Rectangle( 0, 0, _Width / 4, _Height / 4 ),
                Color.White );
            _SpriteBatch.End( );

            // 1/4x1/4->1/2x1/2
            _Device.SetRenderTarget( _UpSampleTargets[ 1 ] );
            _Device.Clear( Color.Black );
            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Additive);
            _SpriteBatch.Draw(
                _UpSampleTargets[ 2 ],
                new Rectangle( 0, 0, _Width / 2, _Height / 2 ),
                Color.White );
            _SpriteBatch.Draw(
                _DownSampleTargets[ 0 ],
                new Rectangle( 0, 0, _Width / 2, _Height / 2 ),
                Color.White );
            _SpriteBatch.End( );
            //System.IO.Stream stream = System.IO.File.Create("_DownSampleTargets5.png");
            //_DownSampleTargets[5].SaveAsPng(stream, _DownSampleTargets[5].Width, _DownSampleTargets[5].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets4.png");
            //_DownSampleTargets[4].SaveAsPng(stream, _DownSampleTargets[4].Width, _DownSampleTargets[4].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets3.png");
            //_DownSampleTargets[3].SaveAsPng(stream, _DownSampleTargets[3].Width, _DownSampleTargets[3].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets2.png");
            //_DownSampleTargets[2].SaveAsPng(stream, _DownSampleTargets[2].Width, _DownSampleTargets[2].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets1.png");
            //_DownSampleTargets[1].SaveAsPng(stream, _DownSampleTargets[1].Width, _DownSampleTargets[1].Height);
            //stream.Close();

            // 1/2x1/2->1x1
            _Device.SetRenderTarget( _UpSampleTargets[ 0 ] );
            _Device.Clear( ClearOptions.Target, Vector4.Zero, 1, 0 );
            //_Device.Textures[ 0 ] = _UpSampleTargets[ 1 ];
            //_AdditiveBlendEffect.Parameters["tex"].SetValue(_UpSampleTargets[1]);

            _AdditiveBlendEffect.CurrentTechnique = _AdditiveBlendEffect.Techniques[ 0 ];
            _AdditiveBlendEffect.Parameters["gFactor"].SetValue( 1.0f );
         //   _AdditiveBlendEffect.Begin( );
            EffectPass pass = _AdditiveBlendEffect.CurrentTechnique.Passes[ 0 ];
            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Additive);
            pass.Apply( );
            _SpriteBatch.Draw(
                _UpSampleTargets[ 1 ],
                new Rectangle( 0, 0, _Width, _Height ),
                Color.White );
         //   //pass.End( );
            _SpriteBatch.End( );
        //    _AdditiveBlendEffect.End( );
            _Device.SetRenderTarget(null );
        }
        
        // ---------------------------------------------------------
        /*
        public void ApplyBlur( )
        {
            Effect effect = _GaussianFilterEffect;
            Vector2 Size;

            for ( int i = 0; i < MAX_DOWNSAMPLE; ++i )
            {
                _Device.SetRenderTarget( _TempTargets[ i ] );
                _Device.Clear( ClearOptions.Target, Vector4.Zero, 1, 0 );
                _SpriteBatch.Begin( );
                _SpriteBatch.Draw( _DownSampleTargets[ i ],
                    Vector2.Zero,
                    Color.White );
                _SpriteBatch.End( );
                _Device.SetRenderTarget( null );
            } // for

            float Factor = 64.0f;
            for ( int i = MAX_DOWNSAMPLE - 1; i >= 0; --i )
            {
                Size.X = _Width / Factor;
                Size.Y = _Height / Factor;

                _Device.SetRenderTarget( _DownSampleTargets[ i ] );
                _Device.Clear( ClearOptions.Target, Vector4.Zero, 1, 0 );
                _Device.Textures[ 0 ] = _TempTargets[ i ];
                effect.CurrentTechnique = effect.Techniques[ BLUR_QUALITY ];
                effect.Parameters[ "ViewportSize" ].SetValue( Size );
                effect.Parameters[ "TextureSize" ].SetValue( Size );
                effect.Parameters[ "MatrixTransform" ].SetValue( Matrix.Identity );
                effect.Parameters[ "gScreenWidth" ].SetValue( Size.X );
                effect.Parameters[ "gScreenHeight" ].SetValue( Size.Y );
                effect.Parameters[ "gScaleFactor" ].SetValue( SCALE_FACTOR );
                //effect.Begin( );
                EffectPass pass = effect.CurrentTechnique.Passes[ 0 ];
                _SpriteBatch.Begin(
                    SpriteBlendMode.None,
                    SpriteSortMode.Immediate,
                    SaveStateMode.SaveState );
                pass.Apply();
                _SpriteBatch.Draw( _TempTargets[ i ],
                    Vector2.Zero,
                    Color.White );
                //pass.End( );
                _SpriteBatch.End( );
                //effect.End( );

                Factor /= 2f;
                _Device.SetRenderTarget( null );
            } // for            
        }
         * */
        public void ApplyBlur( )
        {
            // apply blur to all scales of the original target
            // again, this is done manually and might be done in
            // an array
            // ps: the temporary targets are used, here

            // custom spritebatch vertexshader
            Vector2 Size;

            /*
             * copy Rendertargets to temporary buffers
             */
            _Device.SetRenderTarget( _TempTargets[ 5 ] );
            _Device.Clear( Color.Black );
            _SpriteBatch.Begin( );
            _SpriteBatch.Draw( _DownSampleTargets[ 5 ],
                Vector2.Zero,
                Color.White );
            _SpriteBatch.End( );

            _Device.SetRenderTarget(_TempTargets[ 4 ] );
            _Device.Clear( Color.Black );
            _SpriteBatch.Begin( );
            _SpriteBatch.Draw( _DownSampleTargets[ 4 ],
                Vector2.Zero,
                Color.White );
            _SpriteBatch.End( );

            _Device.SetRenderTarget( _TempTargets[ 3 ] );
            _Device.Clear( Color.Black );
            _SpriteBatch.Begin( );
            _SpriteBatch.Draw( _DownSampleTargets[ 3 ],
                Vector2.Zero,
                Color.White );
            _SpriteBatch.End( );

            _Device.SetRenderTarget( _TempTargets[ 2 ] );
            _Device.Clear( Color.Black );
            _SpriteBatch.Begin( );
            _SpriteBatch.Draw( _DownSampleTargets[ 2 ],
                Vector2.Zero,
                Color.White );
            _SpriteBatch.End( );

            _Device.SetRenderTarget( _TempTargets[ 1 ] );
            _Device.Clear( Color.Black );
            _SpriteBatch.Begin( );
            _SpriteBatch.Draw( _DownSampleTargets[ 1 ],
                Vector2.Zero,
                Color.White );
            _SpriteBatch.End( );

            _Device.SetRenderTarget( _TempTargets[ 0 ] );
            _Device.Clear( Color.Black );
            _SpriteBatch.Begin( );
            _SpriteBatch.Draw( _DownSampleTargets[ 0 ],
                Vector2.Zero,
                Color.White );
            _SpriteBatch.End( );

            System.IO.Stream stream;
            //stream = System.IO.File.Create("_DownSampleTargets5.png");
            //_DownSampleTargets[5].SaveAsPng(stream, _DownSampleTargets[5].Width, _DownSampleTargets[5].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets4.png");
            //_DownSampleTargets[4].SaveAsPng(stream, _DownSampleTargets[4].Width, _DownSampleTargets[4].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets3.png");
            //_DownSampleTargets[3].SaveAsPng(stream, _DownSampleTargets[3].Width, _DownSampleTargets[3].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets2.png");
            //_DownSampleTargets[2].SaveAsPng(stream, _DownSampleTargets[2].Width, _DownSampleTargets[2].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets1.png");
            //_DownSampleTargets[1].SaveAsPng(stream, _DownSampleTargets[1].Width, _DownSampleTargets[1].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets0.png");
            //_DownSampleTargets[0].SaveAsPng(stream, _DownSampleTargets[0].Width, _DownSampleTargets[0].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_TempTargets5.png");
            //_TempTargets[5].SaveAsPng(stream, _TempTargets[5].Width, _TempTargets[5].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_TempTargets4.png");
            //_TempTargets[4].SaveAsPng(stream, _TempTargets[4].Width, _TempTargets[4].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_TempTargets3.png");
            //_TempTargets[3].SaveAsPng(stream, _TempTargets[3].Width, _TempTargets[3].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_TempTargets2.png");
            //_TempTargets[2].SaveAsPng(stream, _TempTargets[2].Width, _TempTargets[2].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_TempTargets1.png");
            //_TempTargets[1].SaveAsPng(stream, _TempTargets[1].Width, _TempTargets[1].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_TempTargets0.png");
            //_TempTargets[0].SaveAsPng(stream, _TempTargets[0].Width, _TempTargets[0].Height);
            //stream.Close();


            /*
             * render temporary buffers to downscaled targets
             */
            Size.X = _Width / 64;
            Size.Y = _Height / 64;

            Effect effect = _GaussianFilterEffect;

            _Device.SetRenderTarget(_DownSampleTargets[ 5 ] );
            _Device.Clear( Color.Black );
         //   _Device.Textures[ 0 ] = _TempTargets[ 5 ];
            effect.CurrentTechnique = effect.Techniques[ BLUR_QUALITY ];
            //effect.Parameters[ "ViewportSize" ].SetValue( Size );
            //effect.Parameters[ "TextureSize" ].SetValue( Size );
            //effect.Parameters[ "MatrixTransform" ].SetValue( Matrix.Identity );
            effect.Parameters[ "gScreenWidth" ].SetValue( Size.X );
            effect.Parameters[ "gScreenHeight" ].SetValue( Size.Y );
            effect.Parameters[ "gScaleFactor" ].SetValue( SCALE_FACTOR );
         //   //effect.Begin( );
            EffectPass pass = effect.CurrentTechnique.Passes[ 0 ];
            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Opaque );
            pass.Apply( );
            _SpriteBatch.Draw( _TempTargets[ 5 ],
                Vector2.Zero,
                Color.White );
         //   //pass.End( );
            _SpriteBatch.End( );
        //    //effect.End( );

            Size.X = _Width / 32;
            Size.Y = _Height / 32;
            _Device.SetRenderTarget( _DownSampleTargets[ 4 ] );
            _Device.Clear( Color.Black );
       //     _Device.Textures[ 0 ] = _TempTargets[ 4 ];
            effect.CurrentTechnique = effect.Techniques[ BLUR_QUALITY ];
            //effect.Parameters[ "ViewportSize" ].SetValue( Size );
            //effect.Parameters[ "TextureSize" ].SetValue( Size );
            //effect.Parameters[ "MatrixTransform" ].SetValue( Matrix.Identity );
            effect.Parameters[ "gScreenWidth" ].SetValue( Size.X );
            effect.Parameters[ "gScreenHeight" ].SetValue( Size.Y );
            effect.Parameters[ "gScaleFactor" ].SetValue( SCALE_FACTOR );
         //   //effect.Begin( );
            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Opaque );
            effect.CurrentTechnique.Passes[0].Apply( );
            _SpriteBatch.Draw( _TempTargets[ 4 ],
                Vector2.Zero,
                Color.White );
         //   //pass.End( );
            _SpriteBatch.End( );
        //    //effect.End( );

            Size.X = _Width / 16;
            Size.Y = _Height / 16;
            _Device.SetRenderTarget( _DownSampleTargets[ 3 ] );
            _Device.Clear( Color.Black );
       //     _Device.Textures[ 0 ] = _TempTargets[ 3 ];
            effect.CurrentTechnique = effect.Techniques[ BLUR_QUALITY ];
            //effect.Parameters[ "ViewportSize" ].SetValue( Size );
            //effect.Parameters[ "TextureSize" ].SetValue( Size );
            //effect.Parameters[ "MatrixTransform" ].SetValue( Matrix.Identity );
            effect.Parameters[ "gScreenWidth" ].SetValue( Size.X );
            effect.Parameters[ "gScreenHeight" ].SetValue( Size.Y );
            effect.Parameters[ "gScaleFactor" ].SetValue( SCALE_FACTOR );
         //   //effect.Begin( );
            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Opaque );
            effect.CurrentTechnique.Passes[0].Apply();
            _SpriteBatch.Draw( _TempTargets[ 3 ],
                Vector2.Zero,
                Color.White );
          //  //pass.End( );
            _SpriteBatch.End( );
          //  //effect.End( );

            Size.X = _Width / 8;
            Size.Y = _Height / 8;
            _Device.SetRenderTarget( _DownSampleTargets[ 2 ] );
            _Device.Clear( Color.Black );
       //     _Device.Textures[ 0 ] = _TempTargets[ 2 ];
            effect.CurrentTechnique = effect.Techniques[ BLUR_QUALITY ];
            //effect.Parameters[ "ViewportSize" ].SetValue( Size );
            //effect.Parameters[ "TextureSize" ].SetValue( Size );
            //effect.Parameters[ "MatrixTransform" ].SetValue( Matrix.Identity );
            effect.Parameters[ "gScreenWidth" ].SetValue( Size.X );
            effect.Parameters[ "gScreenHeight" ].SetValue( Size.Y );
            effect.Parameters[ "gScaleFactor" ].SetValue( SCALE_FACTOR );
         //   //effect.Begin( );
            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Opaque );
            effect.CurrentTechnique.Passes[0].Apply();
            _SpriteBatch.Draw( _TempTargets[ 2 ],
                Vector2.Zero,
                Color.White );
        //    //pass.End( );
            _SpriteBatch.End( );
        //    //effect.End( );

            Size.X = _Width / 4;
            Size.Y = _Height / 4;
            _Device.SetRenderTarget( _DownSampleTargets[ 1 ] );
            _Device.Clear( Color.Black );
      //      _Device.Textures[ 0 ] = _TempTargets[ 1 ];
            effect.CurrentTechnique = effect.Techniques[ BLUR_QUALITY ];
            //effect.Parameters[ "ViewportSize" ].SetValue( Size );
            //effect.Parameters[ "TextureSize" ].SetValue( Size );
            //effect.Parameters[ "MatrixTransform" ].SetValue( Matrix.Identity );
            effect.Parameters[ "gScreenWidth" ].SetValue( Size.X );
            effect.Parameters[ "gScreenHeight" ].SetValue( Size.Y );
            effect.Parameters[ "gScaleFactor" ].SetValue( SCALE_FACTOR );
        //    //effect.Begin( );
            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Opaque );
            effect.CurrentTechnique.Passes[0].Apply();
            _SpriteBatch.Draw( _TempTargets[ 1 ],
                Vector2.Zero,
                Color.White );
        //    //pass.End( );
            _SpriteBatch.End( );
        //    //effect.End( );

            Size.X = _Width / 2;
            Size.Y = _Height / 2;
            _Device.SetRenderTarget( _DownSampleTargets[ 0 ] );
            _Device.Clear( Color.Black );
     //       _Device.Textures[ 0 ] = _TempTargets[ 0 ];
            effect.CurrentTechnique = effect.Techniques[ BLUR_QUALITY ];
            //effect.Parameters[ "ViewportSize" ].SetValue( Size );
            //effect.Parameters[ "TextureSize" ].SetValue( Size );
            //effect.Parameters[ "MatrixTransform" ].SetValue( Matrix.Identity );
            effect.Parameters[ "gScreenWidth" ].SetValue( Size.X );
            effect.Parameters[ "gScreenHeight" ].SetValue( Size.Y );
            effect.Parameters[ "gScaleFactor" ].SetValue( SCALE_FACTOR );
        //    //effect.Begin( );
            _SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Opaque );
            effect.CurrentTechnique.Passes[0].Apply();
            _SpriteBatch.Draw( _TempTargets[ 0 ],
                Vector2.Zero,
                Color.White );
         //   //pass.End( );
            _SpriteBatch.End( );
        //    //effect.End( );

            _Device.SetRenderTarget( null );
            //stream = System.IO.File.Create("_TempTargets5.png");
            //_TempTargets[5].SaveAsPng(stream, _TempTargets[5].Width, _TempTargets[5].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_TempTargets4.png");
            //_TempTargets[4].SaveAsPng(stream, _TempTargets[4].Width, _TempTargets[4].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_TempTargets3.png");
            //_TempTargets[3].SaveAsPng(stream, _TempTargets[3].Width, _TempTargets[3].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_TempTargets2.png");
            //_TempTargets[2].SaveAsPng(stream, _TempTargets[2].Width, _TempTargets[2].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_TempTargets1.png");
            //_TempTargets[1].SaveAsPng(stream, _TempTargets[1].Width, _TempTargets[1].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_TempTargets0.png");
            //_TempTargets[0].SaveAsPng(stream, _TempTargets[0].Width, _TempTargets[0].Height);
            //stream.Close();

            //stream = System.IO.File.Create("_DownSampleTargets5.png");
            //_DownSampleTargets[5].SaveAsPng(stream, _DownSampleTargets[5].Width, _DownSampleTargets[5].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets4.png");
            //_DownSampleTargets[4].SaveAsPng(stream, _DownSampleTargets[4].Width, _DownSampleTargets[4].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets3.png");
            //_DownSampleTargets[3].SaveAsPng(stream, _DownSampleTargets[3].Width, _DownSampleTargets[3].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets2.png");
            //_DownSampleTargets[2].SaveAsPng(stream, _DownSampleTargets[2].Width, _DownSampleTargets[2].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets1.png");
            //_DownSampleTargets[1].SaveAsPng(stream, _DownSampleTargets[1].Width, _DownSampleTargets[1].Height);
            //stream.Close();
            //stream = System.IO.File.Create("_DownSampleTargets0.png");
            //_DownSampleTargets[0].SaveAsPng(stream, _DownSampleTargets[0].Width, _DownSampleTargets[0].Height);
            //stream.Close();
        }
        // ---------------------------------------------------------
        public void SaveMipLevels( String Directory, String Prefix )
        {
            for( int i = 0; i < MAX_DOWNSAMPLE; ++i )
            {
                Texture2D tex;
                tex = _DownSampleTargets[ i ];
                DDSLib.DDSToFile(Directory + Prefix + i + ".dds", true, tex, false);
              //  tex.Save( Directory + Prefix + i + ".dds", ImageFileFormat.Dds );
            } // for
        }
        // ---------------------------------------------------------
        public RenderTarget2D GetUpsampleTarget( )
        {
            return _UpSampleTargets[ 0 ];
            //return _RenderTarget;
        }
        // ---------------------------------------------------------
        public RenderTarget2D RenderTarget
        {
            get
            {
                return _RenderTarget;
            }
            set
            {
                _RenderTarget = value;
            }
        }
        // ---------------------------------------------------------
        public RenderTarget2D GetTarget( int MipMapLevel )
        {
            if ( MipMapLevel == 0 )
            {
                return _RenderTarget;
            }
            else
            {
                return _DownSampleTargets[ MipMapLevel - 1 ];
            }
        }
        // ---------------------------------------------------------
        public Texture2D GetTexture( int MipMapLevel )
        {
            if ( MipMapLevel == 0 )
            {
                return _RenderTarget;
            }
            else
            {
                return _DownSampleTargets[ MipMapLevel - 1 ];
            }
        }
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