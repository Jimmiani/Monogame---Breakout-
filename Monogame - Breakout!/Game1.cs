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

        CollisionManager collisionManager;

        Song crossroadsMusic, greenpathMusic, cityMusic, sanctumMusic, palaceMusic, abyssMusic, currentMusic;
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

            currentMusic = crossroadsMusic;
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
                int y = 200;
                bricks2.Add(new Brick(new Rectangle(x, y, width, height), brickTexture2));
            }
            for (int i = 0; i < 8; i++)
            {
                int width = 116;
                int height = 50;
                int x = (width + 8) * i + 8;
                int y = 265;
                bricks1.Add(new Brick(new Rectangle(x, y, width, height), brickTexture1));
            }
            
            collisionManager = new CollisionManager(ball, bricks1, paddle, ballNormalReturn, brickDamage1, brickDamage2, brickDeath, brickDeflect, paddleBounce);
            collisionManager.SetDeflectHeight(bricks2[0].Hitbox.Bottom);
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
                if (MediaPlayer.State == MediaState.Stopped)
                {
                    MediaPlayer.Play(currentMusic);
                }

                // Crossroads

                if (gameState == GameState.Crossroads)
                {
                    for (int i = 0; i < bricks1.Count; i++)
                    {
                        bricks1[i].Update();
                    }
                    paddle.Update(keyboardState);
                    ball.Update(gameTime, keyboardState, ballStartSpeed);

                    collisionManager.Update();

                    // Go to next state

                    if (bricks1.Count <= 0 && ball.State == BallState.Moving)
                    {
                        ball.Stop(ballNormalReturn);
                        MediaPlayer.Volume = 0.1f;
                    }
                    
                    if (ball.State == BallState.Ready && bricks1.Count <= 0)
                    {
                        ballStartSpeed = 10;
                        collisionManager.SetDeflectHeight(bricks3[0].Hitbox.Bottom);
                        collisionManager.SetActiveBricks(bricks2);
                        gameState = GameState.Greenpath;
                        currentMusic = greenpathMusic;
                        MediaPlayer.Play(currentMusic);
                        MediaPlayer.Volume = 1;
                    }
                }

                // Greenpath

                if (gameState == GameState.Greenpath)
                {
                    for (int i = 0; i < bricks2.Count; i++)
                    {
                        bricks2[i].Update();
                    }
                    paddle.Update(keyboardState);
                    ball.Update(gameTime, keyboardState, ballStartSpeed);

                    collisionManager.Update();

                    // Go to next state

                    if (bricks2.Count <= 0 && ball.State == BallState.Moving)
                    {
                        ball.Stop(ballNormalReturn);
                        MediaPlayer.Volume = 0.1f;
                    }

                    if (ball.State == BallState.Ready && bricks2.Count <= 0)
                    {
                        ballStartSpeed = 12;
                        collisionManager.SetDeflectHeight(bricks4[0].Hitbox.Bottom);
                        collisionManager.SetActiveBricks(bricks3);
                        gameState = GameState.City;
                        currentMusic = cityMusic;
                        MediaPlayer.Play(currentMusic);
                        MediaPlayer.Volume = 1;
                    }
                }

                // City

                
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
