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

        public Brick(Rectangle hitbox, Texture2D texture)
        {
            _hitbox = hitbox;
            _texture = texture;
        }

        public void Update()
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _hitbox, Color.White);
        }
        public Rectangle Hitbox
        {
            get { return _hitbox; }
        }
    }
}
