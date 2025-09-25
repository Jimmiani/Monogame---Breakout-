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
    enum BallState
    {
        Stopped,
        Start,
        Moving
    }
    internal class Ball
    {
        private Random _generator;
        private Rectangle _hitbox;
        private Vector2 _velocity;
        private Texture2D _texture;
        private Rectangle _window;
        private Paddle _paddle;
        private Vector2 _prevLocation;
        private BallState _ballState;


        public Ball(Texture2D texture, Rectangle window, Paddle paddle)
        {
            _texture = texture;
            _window = window;
            _paddle = paddle;
            _velocity = new Vector2(0, 0);
            _hitbox = new Rectangle((window.Width / 2) - 15, (window.Height / 2) - 15, 30, 30);
            _prevLocation = new Vector2(0, 0);
            _ballState = BallState.Stopped;
            _generator = new Random();
        }

        public void Update(KeyboardState keyboardState)
        {
            if (_ballState == BallState.Stopped)
            {
                _velocity = Vector2.Zero;
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    _ballState = BallState.Start;
                }
            }
            else if (_ballState == BallState.Start)
            {
                float angle = MathHelper.ToRadians(45 + (float)_generator.NextDouble() * 90);
                _velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                ChangeSpeed(10);
                _ballState = BallState.Moving;
            }
            else if (_ballState == BallState.Moving)
            {
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

                        float paddleCenter = _paddle.Hitbox.Center.X;
                        float ballCenter = _hitbox.Center.X;
                        float centerDistance = (ballCenter - paddleCenter) / (_paddle.Hitbox.Width / 2);
                        float speed = _velocity.Length();
                        float maxBounceAngle = MathHelper.ToRadians(45);
                        float bounceAngle = centerDistance * maxBounceAngle;
                        float angle = MathHelper.PiOver2 + bounceAngle;
                        _velocity = new Vector2((float)Math.Cos(angle), -(float)Math.Sin(angle)) * speed;
                    }
                }

                _prevLocation.Y = _hitbox.Bottom;
                _prevLocation.X = _hitbox.Top;
                _hitbox.Offset(_velocity);
            }
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
        public void ChangeSpeed(int speed)
        {
            if (_velocity != Vector2.Zero)
            {
                _velocity.Normalize();
                _velocity *= speed;
            }
        }
    }
}
