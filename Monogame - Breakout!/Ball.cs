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
        Return,
        Ready,
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
        private Vector2 _prevLocation;
        private BallState _ballState;
        private float _returnTimer;
        private Vector2 _position;
        private Vector2 _returnTarget;
        private Vector2 _returnDistance;
        private float _returnSpeed;
        private bool _canStart;
        private Texture2D _glowTexture;
        private Rectangle _glowRect;
        private int _startSpeed;
        public Color Color { get; set; } = Color.White;

        public Ball(Texture2D texture, Rectangle window, Texture2D glowTexture)
        {
            _texture = texture;
            _window = window;
            _glowTexture = glowTexture;
            _velocity = new Vector2(0, 0);
            _hitbox = new Rectangle((window.Width / 2) - 15, (window.Height / 2) - 15, 30, 30);
            _position = new Vector2((_window.Width / 2) - (_hitbox.Width / 2), (_window.Height / 2) - (_hitbox.Height / 2));
            _glowRect = new Rectangle((window.Width / 2) - 50, (window.Height / 2) - 50, 600, 600);
            _returnTarget = _position;
            _prevLocation = new Vector2(0, 0);
            _ballState = BallState.Ready;
            _generator = new Random();
            _returnTimer = 0;
            _canStart = true;
            _returnDistance = Vector2.Zero;
            _returnSpeed = 0;
            _startSpeed = 8;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            _glowRect.X = _hitbox.Center.X - _glowRect.Width / 2;
            _glowRect.Y = _hitbox.Center.Y - _glowRect.Height / 2;
            if (_ballState == BallState.Ready)
            {
                _velocity = Vector2.Zero;
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    if (_canStart)
                        _ballState = BallState.Start;
                }
            }
            else if (_ballState == BallState.Start)
            {
                float angle = MathHelper.ToRadians(75 + (float)_generator.NextDouble() * 30);
                _velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                ChangeSpeed(_startSpeed);
                _ballState = BallState.Moving;
            } 
            else if (_ballState == BallState.Moving)
            {
                if (_hitbox.Bottom > _window.Height)
                {
                    _position.Y = _window.Height - _hitbox.Height;
                    _velocity.Y = -_velocity.Y;
                }
                if (_hitbox.Left < 0 || _hitbox.Right > _window.Width)
                {
                    if (_hitbox.Left < 0)
                    {
                        _position.X = 0;
                    }
                    else
                    {
                        _position.X = _window.Width - _hitbox.Width;
                    }
                    _velocity.X = -_velocity.X;
                }
                
                _prevLocation.Y = _hitbox.Bottom;
                _prevLocation.X = _hitbox.Top;

                _position += _velocity;

                _hitbox.X = (int)_position.X;
                _hitbox.Y = (int)_position.Y;
            }
            else if (_ballState == BallState.Stopped)
            {
                _returnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_returnTimer <= 3.93f)
                {
                    _position = Vector2.Lerp(_position, _returnTarget, 0.03f);  
                    _hitbox.X = (int)_position.X;
                    _hitbox.Y = (int)_position.Y;
                }
                else
                {
                    _returnTimer = 0;
                    _ballState = BallState.Return;
                    _returnDistance = new Vector2(_window.Width / 2, _window.Height / 2) - new Vector2(_hitbox.Center.X, _hitbox.Center.Y);
                    _returnSpeed = _returnDistance.Length() / 40;
                    _returnDistance.Normalize();
                    _velocity = _returnDistance * _returnSpeed;
                }
            }
            else if (_ballState == BallState.Return)
            {
                _position += _velocity;
                _hitbox.X = (int)_position.X;
                _hitbox.Y = (int)_position.Y;

                if (_hitbox.Intersects(new Rectangle(_window.Width / 2, _window.Height / 2, 1, 1)))
                {
                    _position = new Vector2((_window.Width / 2) - (_hitbox.Width / 2), (_window.Height / 2) - (_hitbox.Height / 2));
                    _hitbox.X = (int)_position.X;
                    _hitbox.Y = (int)_position.Y;
                    _ballState = BallState.Ready;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_glowTexture, _glowRect, Color.White * 0.2f);
            spriteBatch.Draw(_texture, _hitbox, Color);
        }
        public Rectangle Hitbox
        {
            get { return _hitbox; }
        }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
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
        public BallState State
        {
            get { return _ballState; }
            set { _ballState = value; }
        }
        public void Stop()
        {
            _ballState = BallState.Stopped;
            _returnTarget = _position + new Vector2(0, 250);
        }
        public bool CanStart
        {
            get { return _canStart; }
            set { _canStart = value; }
        }
        public void ChangeStartSpeed(int speed)
        {
            _startSpeed = speed;
        }
    }
}
