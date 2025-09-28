using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Monogame___Breakout_
{

    internal class Brick
    {
        private Rectangle _hitbox;
        private Texture2D _texture;
        private int _health;
        private float _opacity;

        public Brick(Rectangle hitbox, Texture2D texture)
        {
            _hitbox = hitbox;
            _texture = texture;
            _health = 15;
            _opacity = 1;
        }

        public void Update()
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _hitbox, null, Color.White * _opacity, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
        }
        public Rectangle Hitbox
        {
            get { return _hitbox; }
        }
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        public float Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }
    }
}
