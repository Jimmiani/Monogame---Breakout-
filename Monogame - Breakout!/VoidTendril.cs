using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Diagnostics;

namespace Monogame___Breakout_
{
    enum TendrilState
    {
        Waiting,
        Up,
        Down,
        Gone
    }
    internal class VoidTendril
    {
        private TendrilState _tendrilState;
        private float _animTimer;
        private int _currentFrame;
        private List<Texture2D> _upAnim;
        private List<Texture2D> _downAnim;
        private List<Texture2D> _currentAnim;
        private Rectangle _hitbox;
        private SoundEffect _tendrilEffect;

        public VoidTendril(List<Texture2D> upAnim, List<Texture2D> downAnim, SoundEffect tendrilEffect)
        {
            _upAnim = upAnim;
            _downAnim = downAnim;
            _tendrilEffect = tendrilEffect;

            _hitbox = new Rectangle(-1000, 1000, 257, 693);
            _currentFrame = 0;
            _animTimer = 0;
            _tendrilState = TendrilState.Waiting;
            _currentAnim = _upAnim;
        }

        public void Update(GameTime gameTime)
        {
            if (_tendrilState == TendrilState.Up)
            {
                _animTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnim = _upAnim;

                if (_animTimer >= 1.0 / 20.0)
                {
                    _currentFrame++;
                    _animTimer = 0;
                    if (_currentFrame >= _upAnim.Count)
                    {
                        _currentFrame = _upAnim.Count - 1;
                    }
                }
            }
            else if (_tendrilState == TendrilState.Down)
            {
                _animTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                _currentAnim = _downAnim;

                if (_animTimer >= 1.0 / 20.0)
                {
                    _currentFrame++;
                    _animTimer = 0;
                    if (_currentFrame >= _downAnim.Count)
                    {
                        _currentFrame = _downAnim.Count;
                        _tendrilState = TendrilState.Gone;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_currentAnim[_currentFrame], _hitbox, Color.White);
        }

        public void Up()
        {
            _tendrilState = TendrilState.Up;
            _tendrilEffect.Play(0.7f, 0, 0);
        }
        public void Down()
        {
            _tendrilState = TendrilState.Down;
        }
        public void SetLocation(int x, int y)
        {
            _hitbox = new Rectangle(x, y, _hitbox.Width, _hitbox.Height);
        }
    }
}
