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
    internal class Paddle
    {
        private Rectangle _hitbox;
        private Vector2 _velocity;
        private Texture2D _texture;
        private Rectangle _window;


        public Paddle(Texture2D texture, Rectangle window)
        {
            _texture = texture;
            _window = window;
            _hitbox = new Rectangle((_window.Width / 2) - 150, _window.Height - 100, 150, 20);
            _velocity = new Vector2(0, 0);
        }

        public void Update(KeyboardState keyboardState)
        {
            _velocity = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.Right) && _hitbox.Right < _window.Width)
            {
                _velocity.X = 6;
            }
            if (keyboardState.IsKeyDown(Keys.Left) && _hitbox.Left > 0)
            {
                _velocity.X = -6;
            }
            if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right))
            {
                _velocity.X = 0;
            }
            _hitbox.Offset(_velocity);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_texture, _hitbox, Color.Black);
        }

        public Rectangle Hitbox
        {
            get { return _hitbox; }
        }
    }
}
