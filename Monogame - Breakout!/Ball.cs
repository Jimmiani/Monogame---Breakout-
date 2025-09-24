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
    internal class Ball
    {
        private Rectangle _hitbox;
        private Vector2 _velocity;
        private Texture2D _texture;
        private Rectangle _window;
        private Paddle _paddle;
        private Vector2 _prevLocation;


        public Ball(Texture2D texture, Rectangle window, Paddle paddle)
        {
            _texture = texture;
            _window = window;
            _paddle = paddle;
            _velocity = new Vector2(10, 10);
            _hitbox = new Rectangle((window.Width / 2) - 15, (window.Height / 2) - 15, 30, 30);
            _prevLocation = new Vector2(0, 0);
        }

        public void Update()
        {

            // Window bounces

            if (_hitbox.Top < 0 || _hitbox.Bottom > _window.Height)
            {
                if (_hitbox.Top < 0)
                {
                    _hitbox.Y = 0;
                }
                else
                {
                    _hitbox.Y = _window.Height - _hitbox.Height;
                }
                _velocity.Y = -_velocity.Y;
            }
            if (_hitbox.Left < 0 || _hitbox.Right > _window.Width)
            {
                if (_hitbox.Left < 0)
                {
                    _hitbox.X = 0;
                }
                else
                {
                    _hitbox.X = _window.Width - _hitbox.Width;
                }
                _velocity.X = -_velocity.X;
            }

            // Paddle bounces

            if (_hitbox.Intersects(_paddle.Hitbox))
            {
                if (_hitbox.Right > _paddle.Hitbox.Right && _hitbox.Left < _paddle.Hitbox.Right && _prevLocation.Y - _paddle.Hitbox.Top > 0)
                {
                    _hitbox.X = _paddle.Hitbox.Right;
                }
                else if (_hitbox.Left < _paddle.Hitbox.Left && _hitbox.Right > _paddle.Hitbox.Left && _prevLocation.Y - _paddle.Hitbox.Top > 0)
                {
                    _hitbox.X = _paddle.Hitbox.Left - _hitbox.Width;
                }
                else
                {
                    _hitbox.Y = _paddle.Hitbox.Top - _hitbox.Height;
                    _velocity.Y = -_velocity.Y;
                }
            }
            
            _prevLocation.Y = _hitbox.Bottom;
            _prevLocation.X = _hitbox.Top;
            _hitbox.Offset(_velocity);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _hitbox, Color.Black);
        }
        public Rectangle Hitbox
        {
            get { return _hitbox; }
        }
        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        public float PreviousTop
        {
            get { return _prevLocation.X; }
        }
        public float PreviousBottom
        {
            get { return _prevLocation.Y; }
        }
    }
}
