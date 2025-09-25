using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Monogame___Breakout_
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        KeyboardState keyboardState;
        Paddle paddle;
        Ball ball;
        List<Brick> bricks;
        Texture2D paddleTexture1;
        Texture2D brickTexture1;
        Texture2D ballTexture;

        Rectangle window;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();

            window = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            bricks = new List<Brick>();

            base.Initialize();

            paddle = new Paddle(paddleTexture1, window);
            ball = new Ball(ballTexture, window, paddle);

            for (int i = 0; i < 5; i++)
            {
                int xLocation = 10;
                int yLocation = 50 * i + 10;
                for (int j = 0; j < 10; j++)
                {
                    int width = window.Width / 10 - 10;
                    bricks.Add(new Brick(new Rectangle(xLocation, yLocation, width, 40), brickTexture1));
                    xLocation += 99;
                }
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            paddleTexture1 = Content.Load<Texture2D>("Breakout/Images/Paddles/crossroads_paddle");
            brickTexture1 = Content.Load<Texture2D>("Breakout/Images/Bricks/crossroads_plat");
            ballTexture = Content.Load<Texture2D>("Breakout/Images/Ball/ball");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();

            paddle.Update(keyboardState);
            ball.Update(keyboardState);
            for (int i = 0; i < bricks.Count; i++)
            {
                if (ball.Hitbox.Intersects(bricks[i].Hitbox))
                {
                    if (ball.PreviousTop - bricks[i].Hitbox.Bottom < 0 && ball.PreviousBottom > bricks[i].Hitbox.Top)
                    {
                        ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
                        bricks.RemoveAt(i);
                        break;
                    }
                    else
                    {
                        ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                        bricks.RemoveAt(i);
                        break;
                    }
                }
            }
            if (keyboardState.IsKeyDown(Keys.E))
            {
                ball.ChangeSpeed(20);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            _spriteBatch.Begin();

            paddle.Draw(_spriteBatch);
            ball.Draw(_spriteBatch);
            for (int i = 0; i < bricks.Count; i++)
            {
                bricks[i].Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
