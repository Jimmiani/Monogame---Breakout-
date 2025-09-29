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
        private bool _canMove;

        public Paddle(Texture2D texture, Rectangle window)
        {
            _texture = texture;
            _window = window;
            _hitbox = new Rectangle((_window.Width / 2) - 75, _window.Height - 160, 150, 60);
            _velocity = new Vector2(0, 0);
            _canMove = true;
        }

        public void Update(KeyboardState keyboardState)
        {
            _velocity = Vector2.Zero;
            if (_canMove)
            {
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
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_texture, _hitbox, Color.White);
        }

        public Rectangle Hitbox
        {
            get { return _hitbox; }
        }
        public bool CanMove
        {
            get { return  _canMove; }
            set { _canMove = value; }
        }
    }
}
