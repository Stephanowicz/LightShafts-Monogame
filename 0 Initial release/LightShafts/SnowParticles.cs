using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnowEffect
{
    class SnowParticles
    {
        #region Particle Stuff
        private List<Particle>              _ParticleList;
        private Vector3                     _WindDirection;
        private float                       _Gravity = 0.1f;
        private Model                       _FlakeModel;
        private Effect                      _FlakeMaterial;
        private int                         _NumParticles;
        private GraphicsDevice              _Device;
        #endregion

        #region Rendering Stuff
        private VertexPositionTexture[ ]    _Vertices;
        private int[ ]                      _Indices;
        private VertexBuffer                _VertexBuffer;
        private IndexBuffer                 _IndexBuffer;
        private VertexDeclaration           _VertexDeclaration;
        private Vector3[ ]                  _UnitQuad;
        #endregion
        // ---------------------------------------------------------
        public SnowParticles(
            Model FlakeModel,
            Effect FlakeMaterial,
            int NumParticles,
            GraphicsDevice Device)
        {
            _FlakeModel = FlakeModel;
            _FlakeMaterial = FlakeMaterial;
            _NumParticles = NumParticles;
            _Device = Device;

            foreach ( ModelMesh mesh in _FlakeModel.Meshes )
            {
                foreach ( ModelMeshPart part in mesh.MeshParts )
                {
                    part.Effect = _FlakeMaterial;
                }
            }  

            InitializeVertices( );
            InitializeIndices( );
            InitializeFlakes( );

            _UnitQuad = new Vector3[ 4 ];
            _UnitQuad[ 0 ].X = -1f;
            _UnitQuad[ 0 ].Y = -1f;
            _UnitQuad[ 0 ].Z = 0f;
            _UnitQuad[ 1 ].X = 1f;
            _UnitQuad[ 1 ].Y = -1f;
            _UnitQuad[ 1 ].Z = 0f;
            _UnitQuad[ 2 ].X = 1f;
            _UnitQuad[ 2 ].Y = 1f;
            _UnitQuad[ 2 ].Z = 0f;
            _UnitQuad[ 3 ].X = -1f;
            _UnitQuad[ 3 ].Y = 1f;
            _UnitQuad[ 3 ].Z = 0f;

            _WindDirection = new Vector3( 0f, 0f, 1f );
        }
        // ---------------------------------------------------------
        private void InitializeVertices( )
        {
            _Vertices = new VertexPositionTexture[ _NumParticles * 4 ];
        }
        // ---------------------------------------------------------
        private void InitializeIndices( )
        {
            _Indices = new int[ _NumParticles * 6 ];
            int c = 0;
            for ( int i = 0; i < _NumParticles * 4; i += 4 )
            {
                // tri 1
                _Indices[ c + 0 ] = i;
                _Indices[ c + 1 ] = i + 1;
                _Indices[ c + 2 ] = i + 2;
                // tri 2
                _Indices[ c + 3 ] = i;
                _Indices[ c + 4 ] = i + 2;
                _Indices[ c + 5 ] = i + 3;

                c += 6;
            } // for
        }
        // ---------------------------------------------------------
        private void InitializeFlakes( )
        {
            _ParticleList = new List<Particle>( _NumParticles );
            Vector3 Position;
            Vector3 Direction = new Vector3( );
            Vector3 RotationAngles;
            float Velocity = 0f;
            float x, y, z;
            Random rand = new Random( );
            for ( int i = 0; i < _NumParticles; ++i )
            {
                x = 0f;//( float )( rand.NextDouble( ) * 2 - 1 ) * 20;
                //y = ( float )( rand.NextDouble( ) * 2 - 1 ) * 50 + 80f;
                //y = ( float )( rand.NextDouble( ) * 2 - 1 ) + 20f;
                y = ( float )( rand.NextDouble( ) * 2 - 1 );
                z = ( float )( rand.NextDouble( ) * 2 - 1 ) - 20f;
                Position = new Vector3( x, y, z );

                //x = ( float )( rand.NextDouble( ) * 2 * Math.PI );
                //y = ( float )( rand.NextDouble( ) * 2 * Math.PI );
                //z = ( float )( rand.NextDouble( ) * 2 * Math.PI );
                x = -1f;
                y = ( float )( Math.PI / 2 );
                z = 0f;
                RotationAngles = new Vector3( x, y, z );

                Particle p = new Particle( 
                    Position,
                    Direction,
                    RotationAngles,
                    Velocity );
                _ParticleList.Add( p );
            } // for
        }
        // ---------------------------------------------------------
        public void Update( GameTime gameTime )
        {
            for ( int i = 0; i < _NumParticles * 4; i += 4 )
            {
                int ParticleIndex = i / 4;
                Particle p = _ParticleList[ ParticleIndex ];

                Matrix RotX = Matrix.CreateRotationX(
                    p.RotationAngles.X );
                Matrix RotY = Matrix.CreateRotationY(
                    p.RotationAngles.Y );
                Matrix RotZ = Matrix.CreateRotationZ(
                    p.RotationAngles.Z );

                Matrix Translate = Matrix.CreateTranslation( p.Position );

                Matrix World = RotX * RotY * RotZ * Translate;
                Matrix WorldIT = Matrix.Invert( Matrix.Transpose( World ) );

                // rotate unit-quad
                _Vertices[ i + 0 ].Position = Vector3.Transform( _UnitQuad[ 0 ], World );
                _Vertices[ i + 1 ].Position = Vector3.Transform( _UnitQuad[ 1 ], World );
                _Vertices[ i + 2 ].Position = Vector3.Transform( _UnitQuad[ 2 ], World );
                _Vertices[ i + 3 ].Position = Vector3.Transform( _UnitQuad[ 3 ], World );

                // create normal
                p.Normal = Vector3.Transform( Vector3.Forward, WorldIT );

                // gravity
                float AngleDownwards = Math.Abs( Vector3.Dot(
                    p.Normal,
                    Vector3.Down ) );

                float GravityEpsilon = _Gravity / 5f; // yet magic number
                float GravityFactor = _Gravity * ( 1f - AngleDownwards )
                    + GravityEpsilon;                

                Vector3 np = new Vector3( p.Normal.Z, p.Normal.X, -p.Normal.Y );
                //np.Z = 0f;// Math.Abs( np.Z );               

                Vector3 Direction = np;
                //float min = Math.Min( Math.Abs( np.X ), Math.Min( Math.Abs( np.Y ), Math.Abs( np.Z ) ) );
                float min = Math.Min( Math.Abs( np.Z ), Math.Abs( np.Y ) );
                Direction *= min;

                Console.WriteLine( np );

                //p.Position += (Vector3.Down + Direction ) * GravityFactor;
                p.Position += Direction * 0.1f;

                //Console.WriteLine( p.Position );

                // random rotation
                //float RotSpeed = 0.01f;
                //Vector3 RotAngles = p.RotationAngles;
                //if ( RotAngles.X < Math.PI * 2 )
                //{
                //    RotAngles.X += RotSpeed;
                //}
                //else
                //{
                //    RotAngles.X = 0f;
                //}
                //if ( RotAngles.Y < Math.PI * 2 )
                //{
                //    RotAngles.Y += RotSpeed;
                //}
                //else
                //{
                //    RotAngles.Y = 0f;
                //}
                //if ( RotAngles.Z < Math.PI * 2 )
                //{
                //    RotAngles.Z += RotSpeed;
                //}
                //else
                //{
                //    RotAngles.Z = 0f;
                //}
                //p.RotationAngles = RotAngles;

            } // for
        }
        // ---------------------------------------------------------
        public void Draw(
            Matrix View,
            Matrix Projection )
        {
            // save render state
            CullMode cm = _Device.RasterizerState.CullMode;
            _Device.RasterizerState.CullMode = CullMode.None;

            _VertexBuffer = new VertexBuffer( 
                _Device,
                typeof(VertexPositionTexture),
                _Vertices.Length, 
                BufferUsage.WriteOnly );
            _VertexBuffer.SetData( _Vertices );

            _IndexBuffer = new IndexBuffer( 
                _Device, 
                typeof( int ),
                _Indices.Length, 
                BufferUsage.WriteOnly );
            _IndexBuffer.SetData( _Indices );

            //_VertexDeclaration = new VertexDeclaration(
            //    _Device,
            //    VertexPositionTexture.VertexElements );

            Matrix Scale = Matrix.CreateScale( 0.5f );
            Matrix Translation = Matrix.CreateTranslation( 
                new Vector3( 0f, 0f, 0f ) );
            Matrix World = Scale * Translation;

            Effect effect = _FlakeMaterial;
            effect.CurrentTechnique = effect.Techniques[ 0 ];
            effect.Parameters[ "gWorldXf" ].SetValue( World );
            effect.Parameters[ "gViewXf" ].SetValue( View );
            effect.Parameters[ "gProjectionXf" ].SetValue( Projection );
            effect.Parameters[ "gWorldITXf" ].SetValue( Matrix.Invert( World ) );
            _Device.SetVertexBuffer(_VertexBuffer);
                _Device.Indices = _IndexBuffer;

            foreach ( EffectPass pass in effect.CurrentTechnique.Passes )
            {
                _Device.DrawIndexedPrimitives(PrimitiveType.TriangleList,0,0,_Vertices.Length,0,_Indices.Length / 3 );

                pass.Apply();
            }

            // restore render state
            _Device.RasterizerState.CullMode = cm;
        }
        // ---------------------------------------------------------
        // ---------------------------------------------------------
    }
}
