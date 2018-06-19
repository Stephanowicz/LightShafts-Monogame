/// <summary>
/// This is a ported monogame version
/// Original by Nicolas Menzel
/// Tutorial "Volumetric Lighting"
/// http://www.mathematik.uni-marburg.de/~menzel/index.php?seite=tutorials&id=2
/// 
/// Ported and extended by Steph
/// 
/// </summary>

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PostscreenEffects;
using System;

namespace LightShafts
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        #region Application Controls
        public Vector2 LightMapPosition  { get; set; } = new Vector2(0.189f, 0.259f);
        public Vector2 LightMapOffset { get; set; } = new Vector2(0f, 0f);
        public float texFactor { get; set; } = 1.49f;
        public float LightShaftExposure { get; set; } = 0.351f;
        public float LightShaftDecay { get; set; } = 0.986f;
        public float LightShaftDensity { get; set; } = 1.056f;
        public float LightShaftWeight { get; set; } = 0.691f;
        public float ModelExposure { get; set; } = 0.453f;
        public float LuminanceThreshold { get; set; } = 1.0f;
        public float LuminanceScaleFactor { get; set; } = 2.0f;
        Controls controls;
        #endregion

        private int _Width = 1024;
        private int _Height = 768;
        private Effect _EffectBlack;
        BasicEffect basicEffect;

        private Matrix _WorldView;
        private Matrix _WorldProjection;
        private RenderTarget2D _RenderTargetColor;
        private RenderTarget2D _RenderTargetTemp;
        private RenderTarget2D _RenderTargetMask;
        private RenderTarget2D _RenderTargetShaftsHalf;
        private RenderTarget2D _RenderTargetShaftsFull;
        private RenderTarget2D _RenderTargetFinal;
        private Texture2D _BackgroundTexture;
        Viewport viewport;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private PostScreenFilters _PostScreenFilters;

        #region Model        
        private Model _model;
        private Vector3[] _ModelPosition;
        private float[] _ModelRotationAngle;
        private float[] _ModelRotationSpeed;
        private Effect _ModelMaterial;
        #endregion



        // ---------------------------------------------------------
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredBackBufferWidth = _Width;
            graphics.PreferredBackBufferHeight = _Height;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }
        // ---------------------------------------------------------
        protected override void Initialize()
        {
            SetupMatrices();
            SetupRenderTargets();

            _PostScreenFilters = new PostScreenFilters(
                _Width,
                _Height,
                GraphicsDevice,
                Content);

            InitializeModels();
            base.Initialize();
            basicEffect = new BasicEffect(GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = true,
            };
            controls = new Controls(this); 
            controls.Show();
        }
        // ---------------------------------------------------------
        private void SetupMatrices()
        {
            float AspectRatio = (float)_Width
                / (float)_Height;

            Vector3 CameraPosition = new Vector3(0f, 0f, 100f);

            _WorldProjection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                AspectRatio,
                1.0f,
                1000.0f);

            _WorldView = Matrix.CreateLookAt(
                CameraPosition,
                Vector3.Forward,
                Vector3.Up);
        }
        // ---------------------------------------------------------
        private void SetupRenderTargets()
        {
            // just the models with lighting and material
            _RenderTargetColor = new RenderTarget2D(
                 GraphicsDevice,
                 _Width,
                 _Height,
                 false,
                 SurfaceFormat.HalfVector4, DepthFormat.Depth24);


            // temporal render target
            _RenderTargetTemp = new RenderTarget2D(
                GraphicsDevice,
                _Width,
                _Height,
                false,
                SurfaceFormat.HalfVector4, DepthFormat.Depth24);

            // the models drawn black
            _RenderTargetMask = new RenderTarget2D(
                GraphicsDevice,
                _Width,
                _Height,
                false,
                SurfaceFormat.HalfVector4, DepthFormat.Depth24);

            // post-processing: the screen in half resolution
            _RenderTargetShaftsHalf = new RenderTarget2D(
                GraphicsDevice,
                _Width / 2,
                _Height / 2,
                false,
                SurfaceFormat.HalfVector4, DepthFormat.Depth24);

            // post-processing: the screen scaled to full 
            // resolution
            _RenderTargetShaftsFull = new RenderTarget2D(
                GraphicsDevice,
                _Width,
                _Height,
                false,
                SurfaceFormat.HalfVector4, DepthFormat.Depth24);

            // all render targets combined
            _RenderTargetFinal = new RenderTarget2D(
                GraphicsDevice,
                _Width,
                _Height,
                false,
                SurfaceFormat.HalfVector4, DepthFormat.Depth24);
        }
        // ---------------------------------------------------------
        private void InitializeModels()
        {
            _ModelPosition = new Vector3[3];
            _ModelPosition[0] = new Vector3();
            _ModelPosition[0].X = 0f;
            _ModelPosition[0].Y = 0f;
            _ModelPosition[0].Z = 0f;

            _ModelPosition[1] = new Vector3();
            _ModelPosition[1].X = -22f;
            _ModelPosition[1].Y = -10f;
            _ModelPosition[1].Z = 5f;

            _ModelPosition[2] = new Vector3();
            _ModelPosition[2].X = -20f;
            _ModelPosition[2].Y = 10f;
            _ModelPosition[2].Z = -40f;

            _ModelRotationAngle = new float[3];
            _ModelRotationAngle[0] = 0.0f;
            _ModelRotationAngle[1] = 0.0f;
            _ModelRotationAngle[2] = 0.0f;
            _ModelRotationSpeed = new float[3];
            _ModelRotationSpeed[0] = 0.002f;
            _ModelRotationSpeed[1] = 0.003f;
            _ModelRotationSpeed[2] = 0.001f;
        }
        // ---------------------------------------------------------
        protected override void LoadContent()
        {
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _ModelMaterial = Content.Load<Effect>("Effects\\ModelMaterial");
            _EffectBlack = Content.Load<Effect>("Effects\\BlackShader");
            _BackgroundTexture = Content.Load<Texture2D>("Textures\\flare");
            _model = Content.Load<Model>("Models\\Cubical_Sphere");
        }
        // ---------------------------------------------------------
        private void RenderScene(
            RenderTarget2D Destination,
            Effect Effect,
            Matrix View,
            Matrix Projection)
        {
            // world, world inverse transpose and view inverse
            Matrix World, WorldIT, ViewI;

            // apply effect parameter to model
            Model CurModel = _model;
            foreach (ModelMesh mesh in CurModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = Effect;
                }
            }

            // save render states
            bool OldDepthBufferEnable = GraphicsDevice.DepthStencilState.DepthBufferEnable;
            CullMode OldCullMode = GraphicsDevice.RasterizerState.CullMode;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.SetRenderTarget(Destination);

            GraphicsDevice.Clear(
                ClearOptions.Target,
                new Vector4(0f, 0f, 0f, 0f),
                1,
                0);

            // draw all three models
            int NumModels = 3;
            for (int i = 0; i < NumModels; ++i)
            {
                World = Matrix.CreateScale(0.2f, 0.2f, 0.2f)
                    * Matrix.CreateRotationY(_ModelRotationAngle[i])
                    * Matrix.CreateTranslation(_ModelPosition[i]);

                WorldIT = Matrix.Invert(Matrix.Transpose(World));
                ViewI = Matrix.Invert(View);

                foreach (ModelMesh mesh in CurModel.Meshes)
                {
                    foreach (Effect currentEffect in mesh.Effects)
                    {
                        currentEffect.CurrentTechnique = currentEffect.Techniques[0];
                        currentEffect.Parameters["gWorldXf"].SetValue(World);
                        currentEffect.Parameters["gViewXf"].SetValue(View);
                        currentEffect.Parameters["gProjectionXf"].SetValue(Projection);
                        currentEffect.Parameters["gWorldITXf"].SetValue(WorldIT);
                        currentEffect.Parameters["gViewIXf"].SetValue(ViewI);
                        currentEffect.Parameters["gExposure"].SetValue(ModelExposure);
                    } // foreach
                    mesh.Draw();
                } // foreach
            } // for     

            // restore render states
            GraphicsDevice.Textures[0] = null;
            GraphicsDevice.Textures[1] = null;
            GraphicsDevice.SetRenderTarget(null);
            DepthStencilState depthStencilState = new DepthStencilState
            {
                DepthBufferEnable = OldDepthBufferEnable
            };
            GraphicsDevice.DepthStencilState = depthStencilState;
            rasterizerState = new RasterizerState();
            rasterizerState.CullMode = OldCullMode;
            GraphicsDevice.RasterizerState = rasterizerState;
        }
        // ---------------------------------------------------------
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        // ---------------------------------------------------------
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            int NumModels = 3;
            for( int i = 0; i < NumModels; ++i )
            {
                if( _ModelRotationAngle[ i ] < Math.PI * 2 )
                {
                    _ModelRotationAngle[ i ] += 
                        _ModelRotationSpeed[ i ];
                }
                else
                {
                    _ModelRotationAngle[ i ] = 0f;
                }
            }

            viewport = GraphicsDevice.Viewport;
            basicEffect.Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) *
                          Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            // render the scene in black
            RenderScene(
                _RenderTargetMask,
                _EffectBlack,
                _WorldView,
                _WorldProjection);
            // render the scene with material and lighting
            RenderScene(
                _RenderTargetColor,
                _ModelMaterial,
                _WorldView,
                _WorldProjection);

           // _saveRTasPNG(_RenderTargetColor, "RenderTargetColor.png");

            // additively render the mask over the background
            GraphicsDevice.SetRenderTarget(_RenderTargetTemp);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, basicEffect, Matrix.CreateScale(1f/ (float)texFactor));
            spriteBatch.Draw(_BackgroundTexture, new Rectangle((int)((2*LightMapPosition.X + LightMapOffset.X) * GraphicsDevice.Viewport.Width - _BackgroundTexture.Width / texFactor / 2), (int)((2*LightMapPosition.Y + LightMapOffset.Y) * GraphicsDevice.Viewport.Height - _BackgroundTexture.Height / texFactor / 2), (int)(_BackgroundTexture.Width/ texFactor),(int)( _BackgroundTexture.Height/ texFactor)), Color.White);
            spriteBatch.Draw(_RenderTargetMask, Vector2.Zero, Color.Black);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

//             _saveRTasPNG(_RenderTargetTemp, "blend.png");

            // create mipmap levels of the resulting target
            //applying the 'linearFilter' effect 
            RenderTarget2D[] MipLevels =
                _PostScreenFilters.CreateMipMapLevels(
                _RenderTargetTemp);

            //for (int i = 0; i < MipLevels.Length; ++i)
            //_saveRTasPNG(MipLevels[ i ], "mipLevel_" + i + ".png");
            //_saveRTasPNG(MipLevels[0], "mipLevel_0.png");

            // perform the light shafts on half of the resolution
            _PostScreenFilters.LightShafts(
                MipLevels[0],
                _RenderTargetShaftsHalf,
                LightMapPosition,
                LightShaftDensity,
                LightShaftDecay,
                LightShaftWeight,
                LightShaftExposure);

            // _saveRTasPNG(_RenderTargetShaftsHalf, "RenderTargetShaftsHalf.png");

            // up-scale the effect
            _PostScreenFilters.HalfToFullscreen(
                 _RenderTargetShaftsHalf,
                 _RenderTargetShaftsFull);

            // _saveRTasPNG(_RenderTargetShaftsFull, "RenderTargetShaftsFull.png");


            // combine all the targets
            GraphicsDevice.SetRenderTarget( _RenderTargetFinal);
            GraphicsDevice.Clear(ClearOptions.Target, Vector4.Zero, 1, 0);
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Additive);
            spriteBatch.Draw(_RenderTargetShaftsFull, Vector2.Zero, Color.White);
            spriteBatch.Draw(_RenderTargetColor, Vector2.Zero, Color.White);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);


            // draw them on the screen
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Opaque);
            spriteBatch.Draw(
                _RenderTargetFinal,
                Vector2.Zero,
                Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        System.IO.Stream stream;
        void _saveRTasPNG(RenderTarget2D rt, string filename)
        {
            stream = System.IO.File.Create(filename);
            rt.SaveAsPng(stream, rt.Width, rt.Height);
            stream.Close();

        }

        // ---------------------------------------------------------
        // ---------------------------------------------------------
        // ---------------------------------------------------------
    }
}
