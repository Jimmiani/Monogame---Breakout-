using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace Monogame___Breakout_
{
    enum Screen
    {
        Intro,
        Game,
        End
    }
    enum GameState
    {
        Crossroads,
        Greenpath,
        City,
        Sanctum,
        Palace,
        Abyss
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        KeyboardState keyboardState;
        Screen screen;
        GameState gameState;
        Paddle paddle;
        Ball ball;
        int ballStartSpeed;
        List<Brick> bricks1, bricks2, bricks3, bricks4, bricks5, bricks6;
        Texture2D paddleTexture1, paddleTexture2, paddleTexture3, paddleTexture4, paddleTexture5, paddleTexture6;
        Texture2D brickTexture1, brickTexture2, brickTexture3, brickTexture4, brickTexture5, brickTexture6;
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
            bricks1 = new List<Brick>();
            bricks2 = new List<Brick>();
            bricks3 = new List<Brick>();
            bricks4 = new List<Brick>();
            bricks5 = new List<Brick>();
            bricks6 = new List<Brick>();
            ballStartSpeed = 10;

            screen = Screen.Game;
            gameState = GameState.Crossroads;

            base.Initialize();

            paddle = new Paddle(paddleTexture1, window);
            ball = new Ball(ballTexture, window, paddle);

            for (int i = 0; i < 8; i++)
            {
                int width = 116;
                int height = 50;
                int x = (width + 8) * i + 8;
                int y = 5;
                bricks5.Add(new Brick(new Rectangle(x, y, width, height), brickTexture5));
            }
            for (int i = 0; i < 8; i++)
            {
                int width = 116;
                int height = 50;
                int x = (width + 8) * i + 8;
                int y = 70;
                bricks4.Add(new Brick(new Rectangle(x, y, width, height), brickTexture4));
            }
            for (int i = 0; i < 8; i++)
            {
                int width = 116;
                int height = 50;
                int x = (width + 8) * i + 8;
                int y = 135;
                bricks3.Add(new Brick(new Rectangle(x, y, width, height), brickTexture3));
            }
            for (int i = 0; i < 8; i++)
            {
                int width = 116;
                int height = 50;
                int x = (width + 8) * i + 8;
                int y = 190;
                bricks2.Add(new Brick(new Rectangle(x, y, width, height), brickTexture2));
            }
            for (int i = 0; i < 8; i++)
            {
                int width = 116;
                int height = 50;
                int x = (width + 8) * i + 8;
                int y = 255;
                bricks1.Add(new Brick(new Rectangle(x, y, width, height), brickTexture1));
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Paddles

            paddleTexture1 = Content.Load<Texture2D>("Breakout/Images/Paddles/crossroads_paddle");
            paddleTexture2 = Content.Load<Texture2D>("Breakout/Images/Paddles/greenpath_paddle");
            paddleTexture3 = Content.Load<Texture2D>("Breakout/Images/Paddles/city_paddle");
            paddleTexture4 = Content.Load<Texture2D>("Breakout/Images/Paddles/sanctum_paddle");
            paddleTexture5 = Content.Load<Texture2D>("Breakout/Images/Paddles/palace_paddle");
            paddleTexture6 = Content.Load<Texture2D>("Breakout/Images/Paddles/abyss_paddle");

            // Bricks

            brickTexture1 = Content.Load<Texture2D>("Breakout/Images/Bricks/crossroads_plat");
            brickTexture2 = Content.Load<Texture2D>("Breakout/Images/Bricks/greenpath_plat");
            brickTexture3 = Content.Load<Texture2D>("Breakout/Images/Bricks/city_plat");
            brickTexture4 = Content.Load<Texture2D>("Breakout/Images/Bricks/sanctum_plat");
            brickTexture5 = Content.Load<Texture2D>("Breakout/Images/Bricks/palace_plat");
            brickTexture6 = Content.Load<Texture2D>("Breakout/Images/Bricks/abyss_plat");


            // Ball
            ballTexture = Content.Load<Texture2D>("Breakout/Images/Ball/ball");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();
            Debug.WriteLine(ballStartSpeed);

            if (screen == Screen.Game)
            {
                // Crossroads

                if (gameState == GameState.Crossroads)
                {
                    paddle.Update(keyboardState);
                    ball.Update(gameTime, keyboardState, ballStartSpeed);
                    for (int i = 0; i < bricks1.Count; i++)
                    {
                        if (ball.Hitbox.Intersects(bricks1[i].Hitbox))
                        {
                            // Collision Checking

                            if (ball.PreviousTop - bricks1[i].Hitbox.Bottom < 0 && ball.PreviousBottom > bricks1[i].Hitbox.Top)
                            {
                                ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
                                bricks1.RemoveAt(i);
                                if (bricks1.Count <= 0)
                                {
                                    ball.Stop();
                                }
                                break;
                            }
                            else
                            {
                                ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                                bricks1.RemoveAt(i);
                                if (bricks1.Count <= 0)
                                {
                                    ball.Stop();
                                }
                                break;
                            }
                        }
                    }

                    // Collision Checking for rows behind current row

                    if (ball.Hitbox.Intersects(new Rectangle(0, 0, window.Width, bricks2[0].Hitbox.Bottom)))
                    {
                        ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                    }

                    // Go to next state

                    if (ball.State == BallState.Ready && bricks1.Count <= 0)
                    {
                        ballStartSpeed = 15;
                        gameState = GameState.Greenpath;
                    }
                }

                // Greenpath

                if (gameState == GameState.Greenpath)
                {
                    paddle.Update(keyboardState);
                    ball.Update(gameTime, keyboardState, ballStartSpeed);
                    for (int i = 0; i < bricks2.Count; i++)
                    {
                        if (ball.Hitbox.Intersects(bricks2[i].Hitbox))
                        {
                            if (ball.PreviousTop - bricks2[i].Hitbox.Bottom < 0 && ball.PreviousBottom > bricks2[i].Hitbox.Top)
                            {
                                ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
                                bricks2.RemoveAt(i);
                                break;
                            }
                            else
                            {
                                ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                                bricks2.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            _spriteBatch.Begin();

            paddle.Draw(_spriteBatch);
            ball.Draw(_spriteBatch);
            for (int i = 0; i < bricks5.Count; i++)
            {
                bricks5[i].Draw(_spriteBatch);
            }
            for (int i = 0; i < bricks4.Count; i++)
            {
                bricks4[i].Draw(_spriteBatch);
            }
            for (int i = 0; i < bricks3.Count; i++)
            {
                bricks3[i].Draw(_spriteBatch);
            }
            for (int i = 0; i < bricks2.Count; i++)
            {
                bricks2[i].Draw(_spriteBatch);
            }
            for (int i = 0; i < bricks1.Count; i++)
            {
                bricks1[i].Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
