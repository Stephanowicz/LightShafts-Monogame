using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnowEffect
{
    class Quad
    {
        private VertexPositionTexture[ ] _Vertices;
        private Vector3 _Normal;

        public Quad( )
        {
            _Vertices = new VertexPositionTexture[ 4 ];

            VertexPositionTexture v;
            v = new VertexPositionTexture( );
            v.Position.X = -1f;
            v.Position.Y = -1f;
            v.Position.Z = 0f;
            _Vertices[ 0 ] = v;

            v = new VertexPositionTexture( );
            v.Position.X = 1f;
            v.Position.Y = -1f;
            v.Position.Z = 0f;
            _Vertices[ 1 ] = v;

            v = new VertexPositionTexture( );
            v.Position.X = 1f;
            v.Position.Y = 1f;
            v.Position.Z = 0f;
            _Vertices[ 2 ] = v;

            v = new VertexPositionTexture( );
            v.Position.X = -1f;
            v.Position.Y = 1f;
            v.Position.Z = 0f;
            _Vertices[ 3 ] = v;

            _Normal = new Vector3( 0f, 0f, 1f );
        }
    }
}
