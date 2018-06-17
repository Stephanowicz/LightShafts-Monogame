using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnowEffect
{
    class Particle
    {
        protected Vector3 _Position;
        protected Vector3 _Direction;
        protected Vector3 _Normal;
        protected float _Velocity;
        protected Vector3 _RotationAngles;
        // ---------------------------------------------------------
        public Particle(
            Vector3 Position,
            Vector3 Direction,
            Vector3 RotationAngles,
            float Velocity )
        {
            _Position = Position;
            _Direction = Direction;
            _RotationAngles = RotationAngles;
            _Velocity = Velocity;
        }
        // ---------------------------------------------------------
        public Vector3 Position
        {
            get 
            { 
                return _Position; 
            }
            set 
            { 
                _Position = value; 
            }
        }
        // ---------------------------------------------------------
        public Vector3 Normal
        {
            get 
            { 
                return _Normal; 
            }
            set 
            { 
                _Normal = value; 
            }
        }
        // ---------------------------------------------------------
        public Vector3 RotationAngles
        {
            get
            {
                return _RotationAngles;
            }
            set
            {
                _RotationAngles = value;
            }
        }
        // ---------------------------------------------------------
        public void Update( )
        {

        }
        // ---------------------------------------------------------
        // ---------------------------------------------------------
    }
}
