using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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



        Song crossroadsMusic, greenpathMusic, cityMusic, sanctumMusic, palaceMusic, abyssMusic;
        SoundEffect abyssAmbience, abyssRoar, abyssScreenCover, ballNormalReturn, ballDarkReturn, ballShine, brickDamage1, brickDamage2, brickDeath, brickDeflect, paddleBounce;
        SoundEffectInstance abyssAmbienceInstance, ballShineInstance;



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
            ballStartSpeed = 8;

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

            // Audio-----------------------------------------------------------------------

            // Music

            crossroadsMusic = Content.Load<Song>("Breakout/Audio/Music/Crossroads/crossraods_music");
            greenpathMusic = Content.Load<Song>("Breakout/Audio/Music/Greenpath/greenpath_music");
            cityMusic = Content.Load<Song>("Breakout/Audio/Music/City/city_music");
            sanctumMusic = Content.Load<Song>("Breakout/Audio/Music/Sanctum/sanctum_music");
            palaceMusic = Content.Load<Song>("Breakout/Audio/Music/Palace/palace_music");
            abyssMusic = Content.Load<Song>("Breakout/Audio/Music/Abyss/abyss_music");

            // Sound Effects

            abyssAmbience = Content.Load<SoundEffect>("Breakout/Audio/Music/Abyss/abyss_ambience");
            abyssAmbienceInstance = abyssAmbience.CreateInstance();
            abyssRoar = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Return/Dark/abyss_roar");
            abyssScreenCover = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Return/Dark/abyss_screen_cover");
            ballNormalReturn = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Return/Normal/ball_return");
            ballDarkReturn = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Return/Dark/ball_return_abyss");
            ballShine = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Shine/ball_shine");
            ballShineInstance = ballShine.CreateInstance();
            brickDamage1 = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Bricks/Damage/brick_damage_1");
            brickDamage2 = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Bricks/Damage/brick_damage_2");
            brickDeath = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Bricks/Break/brick_death");
            brickDeflect = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Bricks/Deflect/brick_deflect");
            paddleBounce = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Paddle/paddle_bounce");

            // Images----------------------------------------------------------------------

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

            if (screen == Screen.Game)
            {
                // Crossroads

                if (gameState == GameState.Crossroads)
                {
                    if (MediaPlayer.State == MediaState.Stopped)
                    {
                        MediaPlayer.Play(crossroadsMusic);
                    }
                    paddle.Update(keyboardState);
                    ball.Update(gameTime, keyboardState, ballStartSpeed, paddleBounce);
                    for (int i = 0; i < bricks1.Count; i++)
                    {
                        bricks1[i].Update();
                        if (ball.Hitbox.Intersects(bricks1[i].Hitbox))
                        {
                            // Collision Checking

                            if (ball.PreviousTop - bricks1[i].Hitbox.Bottom < 0 && ball.PreviousBottom > bricks1[i].Hitbox.Top)
                            {
                                ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
                                bricks1[i].Health -= 5;
                                if (bricks1[i].Health == 10)
                                {
                                    brickDamage1.Play();
                                }
                                else if (bricks1[i].Health == 5)
                                {
                                    brickDamage2.Play();
                                }
                                else if (bricks1[i].Health == 0)
                                {
                                    brickDeath.Play();
                                    bricks1.RemoveAt(i);
                                }
                                if (bricks1.Count <= 0)
                                {
                                    ball.Stop();
                                    ballNormalReturn.Play();
                                }
                                break;
                            }
                            else
                            {
                                ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                                bricks1[i].Health -= 5;
                                if (bricks1[i].Health == 10)
                                {
                                    brickDamage1.Play();
                                }
                                else if (bricks1[i].Health == 5)
                                {
                                    brickDamage2.Play();
                                }
                                else if (bricks1[i].Health == 0)
                                {
                                    brickDeath.Play();
                                    bricks1.RemoveAt(i);
                                }
                                if (bricks1.Count <= 0)
                                {
                                    ball.Stop();
                                    ballNormalReturn.Play();
                                    MediaPlayer.Volume = 0.1f;
                                }
                                break;
                            }
                        }
                    }

                    // Collision Checking for rows behind current row

                    if (ball.Hitbox.Intersects(new Rectangle(0, 0, window.Width, bricks2[0].Hitbox.Bottom)))
                    {
                        ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                        brickDeflect.Play();
                    }

                    // Go to next state

                    if (ball.State == BallState.Ready && bricks1.Count <= 0)
                    {
                        ballStartSpeed = 10;
                        gameState = GameState.Greenpath;
                        MediaPlayer.Play(greenpathMusic);
                        MediaPlayer.Volume = 1;
                    }
                }

                // Greenpath

                if (gameState == GameState.Greenpath)
                {
                    if (MediaPlayer.State == MediaState.Stopped)
                    {
                        MediaPlayer.Play(greenpathMusic);
                    }
                    paddle.Update(keyboardState);
                    ball.Update(gameTime, keyboardState, ballStartSpeed, paddleBounce);
                    for (int i = 0; i < bricks2.Count; i++)
                    {
                        bricks2[i].Update();
                        if (ball.Hitbox.Intersects(bricks2[i].Hitbox))
                        {
                            // Collision Checking

                            if (ball.PreviousTop - bricks2[i].Hitbox.Bottom < 0 && ball.PreviousBottom > bricks2[i].Hitbox.Top)
                            {
                                ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
                                bricks2[i].Health -= 5;
                                if (bricks2[i].Health == 10)
                                {
                                    brickDamage1.Play();
                                }
                                else if (bricks2[i].Health == 5)
                                {
                                    brickDamage2.Play();
                                }
                                else if (bricks2[i].Health == 0)
                                {
                                    brickDeath.Play();
                                    bricks2.RemoveAt(i);
                                }
                                if (bricks2.Count <= 0)
                                {
                                    ball.Stop();
                                    ballNormalReturn.Play();
                                }
                                break;
                            }
                            else
                            {
                                ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                                bricks2[i].Health -= 5;
                                if (bricks2[i].Health == 10)
                                {
                                    brickDamage1.Play();
                                }
                                else if (bricks2[i].Health == 5)
                                {
                                    brickDamage2.Play();
                                }
                                else if (bricks2[i].Health == 0)
                                {
                                    brickDeath.Play();
                                    bricks2.RemoveAt(i);
                                }
                                if (bricks2.Count <= 0)
                                {
                                    ball.Stop();
                                    ballNormalReturn.Play();
                                    MediaPlayer.Volume = 0.1f;
                                }
                                break;
                            }
                        }
                    }

                    // Collision Checking for rows behind current row

                    if (ball.Hitbox.Intersects(new Rectangle(0, 0, window.Width, bricks3[0].Hitbox.Bottom)))
                    {
                        ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                        brickDeflect.Play();
                    }

                    // Go to next state

                    if (ball.State == BallState.Ready && bricks2.Count <= 0)
                    {
                        ballStartSpeed = 12;
                        gameState = GameState.City;
                        MediaPlayer.Play(cityMusic);
                        MediaPlayer.Volume = 1;
                    }
                }

                // City

                if (gameState == GameState.Greenpath)
                {
                    if (MediaPlayer.State == MediaState.Stopped)
                    {
                        MediaPlayer.Play(greenpathMusic);
                    }
                    paddle.Update(keyboardState);
                    ball.Update(gameTime, keyboardState, ballStartSpeed, paddleBounce);
                    for (int i = 0; i < bricks2.Count; i++)
                    {
                        bricks2[i].Update();
                        if (ball.Hitbox.Intersects(bricks2[i].Hitbox))
                        {
                            // Collision Checking

                            if (ball.PreviousTop - bricks2[i].Hitbox.Bottom < 0 && ball.PreviousBottom > bricks2[i].Hitbox.Top)
                            {
                                ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
                                bricks2[i].Health -= 5;
                                if (bricks2[i].Health == 10)
                                {
                                    brickDamage1.Play();
                                }
                                else if (bricks2[i].Health == 5)
                                {
                                    brickDamage2.Play();
                                }
                                else if (bricks2[i].Health == 0)
                                {
                                    brickDeath.Play();
                                    bricks2.RemoveAt(i);
                                }
                                if (bricks2.Count <= 0)
                                {
                                    ball.Stop();
                                    ballNormalReturn.Play();
                                }
                                break;
                            }
                            else
                            {
                                ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                                bricks2[i].Health -= 5;
                                if (bricks2[i].Health == 10)
                                {
                                    brickDamage1.Play();
                                }
                                else if (bricks2[i].Health == 5)
                                {
                                    brickDamage2.Play();
                                }
                                else if (bricks2[i].Health == 0)
                                {
                                    brickDeath.Play();
                                    bricks2.RemoveAt(i);
                                }
                                if (bricks2.Count <= 0)
                                {
                                    ball.Stop();
                                    ballNormalReturn.Play();
                                    MediaPlayer.Volume = 0.1f;
                                }
                                break;
                            }
                        }
                    }

                    // Collision Checking for rows behind current row

                    if (ball.Hitbox.Intersects(new Rectangle(0, 0, window.Width, bricks3[0].Hitbox.Bottom)))
                    {
                        ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
                        brickDeflect.Play();
                    }

                    // Go to next state

                    if (ball.State == BallState.Ready && bricks2.Count <= 0)
                    {
                        ballStartSpeed = 12;
                        gameState = GameState.City;
                        MediaPlayer.Play(cityMusic);
                        MediaPlayer.Volume = 1;
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
