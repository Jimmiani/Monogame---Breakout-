using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Monogame___Breakout_
{
    internal class CollisionManager
    {
        private Ball _ball;
        private List<Brick> _bricks;
        private Paddle _paddle;
        private int _deflectHeight;
        private Camera2D _camera;

        // Audio
        private SoundEffect _brickDamage1, _brickDamage2, _brickDeath, _brickDeflect, _paddleBounce;

        public CollisionManager(Ball ball, List<Brick> bricks, Paddle paddle, Camera2D camera, SoundEffect brickDamage1, SoundEffect brickDamage2, SoundEffect brickDeath, SoundEffect brickDeflect, SoundEffect paddleBounce)
        {
            _ball = ball;
            _bricks = bricks;
            _paddle = paddle;
            _camera = camera;
            _brickDamage1 = brickDamage1;
            _brickDamage2 = brickDamage2;
            _brickDeath = brickDeath;
            _brickDeflect = brickDeflect;
            _paddleBounce = paddleBounce;
            _deflectHeight = _bricks[0].Hitbox.Bottom - 65;
        }

        public void Update()
        {
            CheckBrickCollisions();
            CheckPaddleCollisions();
        }

        public void CheckPaddleCollisions()
        {
            if (_ball.Hitbox.Intersects(_paddle.Hitbox))
            {
                if (_ball.Hitbox.Right > _paddle.Hitbox.Right && _ball.Hitbox.Left < _paddle.Hitbox.Right && _ball.PreviousBottom - _paddle.Hitbox.Top > 0)
                {
                    _ball.Position = new Vector2(_paddle.Hitbox.Right, _ball.Position.Y);
                }
                else if (_ball.Hitbox.Left < _paddle.Hitbox.Left && _ball.Hitbox.Right > _paddle.Hitbox.Left && _ball.PreviousBottom - _paddle.Hitbox.Top > 0)
                {
                    _ball.Position = new Vector2(_paddle.Hitbox.Left - _ball.Hitbox.Width, _ball.Position.Y);
                }
                else
                {
                    _ball.Position = new Vector2(_ball.Position.X, _paddle.Hitbox.Top - _ball.Hitbox.Height);

                    float paddleCenter = _paddle.Hitbox.Center.X;
                    float ballCenter = _ball.Hitbox.Center.X;
                    float centerDistance = (ballCenter - paddleCenter) / (_paddle.Hitbox.Width / 2);
                    float speed = _ball.Velocity.Length();
                    float maxBounceAngle = MathHelper.ToRadians(45);
                    float bounceAngle = centerDistance * maxBounceAngle;
                    float angle = MathHelper.PiOver2 + bounceAngle;
                    _ball.Velocity = new Vector2((float)Math.Cos(angle), -(float)Math.Sin(angle)) * speed;
                    _paddleBounce.Play(0.5f, 0, 0);
                }
            }
        }

        public void CheckBrickCollisions()
        {
            for (int i = 0; i < _bricks.Count; i++)
            {
                if (_ball.Hitbox.Intersects(_bricks[i].Hitbox))
                {
                    // Collision Checking

                    if (_ball.PreviousTop - _bricks[i].Hitbox.Bottom < 0 && _ball.PreviousBottom > _bricks[i].Hitbox.Top)
                    {
                        _ball.Velocity = new Vector2(-_ball.Velocity.X, _ball.Velocity.Y);
                        _bricks[i].Health -= 5;
                        if (_bricks[i].Health == 10)
                        {
                            _brickDamage1.Play();
                            _camera.Shake(3, 0.3f, true);
                        }
                        else if (_bricks[i].Health == 5)
                        {
                            _brickDamage2.Play();
                            _camera.Shake(6, 0.3f, true);
                        }
                        else if (_bricks[i].Health == 0)
                        {
                            _brickDeath.Play();
                            _bricks.RemoveAt(i);
                            _camera.Shake(10, 0.3f, true);
                        }
                        break;
                    }
                    else
                    {
                        _ball.Velocity = new Vector2(_ball.Velocity.X, -_ball.Velocity.Y);
                        _bricks[i].Health -= 5;
                        if (_bricks[i].Health == 10)
                        {
                            _brickDamage1.Play();
                            _camera.Shake(3, 0.3f, true);
                        }
                        else if (_bricks[i].Health == 5)
                        {
                            _brickDamage2.Play();
                            _camera.Shake(6, 0.3f, true);
                        }
                        else if (_bricks[i].Health == 0)
                        {
                            _brickDeath.Play();
                            _bricks.RemoveAt(i);
                            _camera.Shake(10, 0.3f, true);
                        }
                        break;
                    }
                }
            }

            // Bricks beyond breakable bricks

            if (_ball.Hitbox.Top < _deflectHeight)
            {
                _ball.Position = new Vector2(_ball.Position.X, _deflectHeight);
                _ball.Velocity = new Vector2(_ball.Velocity.X, -_ball.Velocity.Y);
                _brickDeflect.Play();
            }
        }
        public void SetDeflectHeight(int height)
        {
            _deflectHeight = height;
        }
        public void SetActiveBricks(List<Brick> bricks)
        {
            _bricks = bricks;
        }
    }
}
