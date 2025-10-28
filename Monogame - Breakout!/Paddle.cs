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
        private Texture2D _glowTexture;
        private Rectangle _window;
        private bool _canMove;

        public Paddle(Texture2D texture, Rectangle window, Texture2D glowTexture)
        {
            _texture = texture;
            _window = window;
            _glowTexture = glowTexture;
            _hitbox = new Rectangle((_window.Width / 2) - 75, _window.Height - 160, 150, 60);
            _velocity = new Vector2(0, 0);
            _canMove = true;
        }

        public void Update(KeyboardState keyboardState)
        {
            _velocity = Vector2.Zero;
            if (_canMove)
            {
                if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) && _hitbox.Right < _window.Width)
                {
                    _velocity.X = 8;
                }
                if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) && _hitbox.Left > 0)
                {
                    _velocity.X = -8;
                }
                if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) && (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)))
                {
                    _velocity.X = 0;
                }
                _hitbox.Offset(_velocity);
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_glowTexture, new Rectangle(_hitbox.X - 50, _hitbox.Y - 40, _hitbox.Width + 100, _hitbox.Height + 80), Color.White * 0.3f);
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
        public void SetPaddle(Texture2D paddle)
        {
            _texture = paddle;
        }
    }
}
