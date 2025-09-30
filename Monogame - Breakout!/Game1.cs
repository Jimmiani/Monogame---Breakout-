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
        AbyssCutscene,
        Abyss
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        CollisionManager collisionManager;

        ParticleSystem smokeSystem, essenceSystem, ballSystem, dotSystem;
        List<Texture2D> smokeParticles, essenceParticles, dotParticles;

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

        Texture2D crossroadsBackground;
        Texture2D screenFader, vignette;


        float abyssTimer;
        bool tendril1Up, tendril2Up, hasScreenFill, hasRoared;
        VoidTendril tendril1, tendril2;
        List<Texture2D> tendril1UpTextures, tendril2UpTextures, tendril1DownTextures, tendril2DownTextures;
        SoundEffect tendrilEffect;

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

            smokeParticles = new List<Texture2D>();
            essenceParticles = new List<Texture2D>();
            dotParticles = new List<Texture2D>();

            window = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            bricks1 = new List<Brick>();
            bricks2 = new List<Brick>();
            bricks3 = new List<Brick>();
            bricks4 = new List<Brick>();
            bricks5 = new List<Brick>();
            bricks6 = new List<Brick>();
            ballStartSpeed = 8;

            abyssTimer = 0;
            tendril1Up = false;
            tendril2Up = false;
            tendril1UpTextures = new List<Texture2D>();
            tendril1DownTextures = new List<Texture2D>();
            tendril2UpTextures = new List<Texture2D>();
            tendril2DownTextures = new List<Texture2D>();

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
                bricks6.Add(new Brick(new Rectangle(x, y, width, height), brickTexture6));
            }
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
            
            collisionManager = new CollisionManager(ball, bricks1, paddle, brickDamage1, brickDamage2, brickDeath, brickDeflect, paddleBounce);
            collisionManager.SetDeflectHeight(bricks2[0].Hitbox.Bottom);

            smokeSystem = new ParticleSystem(smokeParticles, new Rectangle(-150, 900, 1300, 150), EmitterShape.Rectangle);
            essenceSystem = new ParticleSystem(essenceParticles, new Rectangle(0, 0, 1000, 800), EmitterShape.Rectangle);
            ballSystem = new ParticleSystem(dotParticles, ball.Hitbox, EmitterShape.Rectangle);
            dotSystem = new ParticleSystem(dotParticles, new Rectangle(0, 650, 1000, 150), EmitterShape.Rectangle);

            tendril1 = new VoidTendril(tendril1UpTextures, tendril1DownTextures, tendrilEffect);
            tendril2 = new VoidTendril(tendril2UpTextures, tendril2DownTextures, tendrilEffect);

            smokeSystem.SetVelocity(0, 0, -0.4f, -0.5f);
            smokeSystem.SetSize(2, 2.5f);
            smokeSystem.SetAngularVelocity(-0.25f, 0.25f);
            smokeSystem.SetSpawnInfo(0.4f, 2);
            smokeSystem.SetLifespan(6, 6);
            smokeSystem.MaxOpacity = 0;

            essenceSystem.SetSpawnInfo(3f, 1);
            essenceSystem.SetVelocity(0, 0, 0, 0);
            essenceSystem.SetLifespan(9, 10);
            essenceSystem.MaxOpacity = 0.4f;
            essenceSystem.SetSize(2, 4);
            essenceSystem.SetAngularVelocity(0.15f, 0.2f);
            essenceSystem.Color = Color.DarkSlateBlue;

            ballSystem.SetVelocity(-0.1f, 0.1f, -0.1f, 0.1f);
            ballSystem.SetSize(0.7f, 1);
            ballSystem.SetSpawnInfo(0.05f, 1);
            ballSystem.SetLifespan(0.5f, 1);

            dotSystem.SetVelocity(0, 0, -0.2f, -0.3f);
            dotSystem.SetSize(0.5f, 1);
            dotSystem.SetSpawnInfo(0.6f, 1);
            dotSystem.SetLifespan(3, 4);
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
            abyssRoar = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Abyss/abyss_roar");
            abyssScreenCover = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Abyss/abyss_screen_cover");
            ballNormalReturn = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Return/Normal/ball_return");
            ballDarkReturn = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Return/Dark/ball_return_abyss");
            ballShine = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Shine/ball_shine");
            ballShineInstance = ballShine.CreateInstance();
            brickDamage1 = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Bricks/Damage/brick_damage_1");
            brickDamage2 = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Bricks/Damage/brick_damage_2");
            brickDeath = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Bricks/Break/brick_death");
            brickDeflect = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Bricks/Deflect/brick_deflect");
            paddleBounce = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Paddle/paddle_bounce");
            tendrilEffect = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Abyss/radiance_tentacles_whip_up");

            // Images----------------------------------------------------------------------

            // Particles

            for (int i = 1; i <= 5; i++)
                smokeParticles.Add(Content.Load<Texture2D>("Breakout/Images/Particles/Smoke/abyss_smoke_0" + i));
            for (int i = 1; i <= 3; i++)
                essenceParticles.Add(Content.Load<Texture2D>("Breakout/Images/Particles/Essence/dream_particle_" + i));
            dotParticles.Add(Content.Load<Texture2D>("Breakout/Images/Particles/Ball/particle_01"));
            screenFader = Content.Load<Texture2D>("Breakout/Images/Particles/Fader/black_fader");
            vignette = Content.Load<Texture2D>("Breakout/Images/Particles/Vignette/vignette");

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

            // Tendrils
            
            for (int i = 0; i <= 8; i++)
                tendril1UpTextures.Add(Content.Load<Texture2D>("Breakout/Images/Tendrils/Left Tendril/Tendril Up 20/Tendril5 Up_00" + i));
            for (int i = 0; i <= 8; i++)
                tendril2UpTextures.Add(Content.Load<Texture2D>("Breakout/Images/Tendrils/Right Tendril/Tendril Up 20/Tendril1 Up_00" + i));
            for (int i = 0; i <= 4; i++)
                tendril1DownTextures.Add(Content.Load<Texture2D>("Breakout/Images/Tendrils/Left Tendril/Tendril Down 20/Tendril5 Down_00" + i));
            for (int i = 0; i <= 4; i++)
                tendril2DownTextures.Add(Content.Load<Texture2D>("Breakout/Images/Tendrils/Right Tendril/Tendril Down 20/Tendril1 Down_00" + i));

            // Backgrounds

            crossroadsBackground = Content.Load<Texture2D>("Breakout/Images/Backgrounds/crossroads_background");
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
                essenceSystem.Update(gameTime);
                smokeSystem.Update(gameTime);
                ballSystem.Update(gameTime);
                dotSystem.Update(gameTime);
                ballSystem.EmitterBoundary = ball.Hitbox;

                // Crossroads

                if (gameState == GameState.Crossroads)
                {

                    essenceSystem.Update(gameTime);

                    for (int i = 0; i < bricks1.Count; i++)
                    {
                        bricks1[i].Update();
                    }
                    paddle.Update(keyboardState);
                    ball.Update(gameTime, keyboardState, ballStartSpeed);

                    collisionManager.Update();

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        bricks1.Clear();
                    }

                    // Go to next state

                    if (bricks1.Count <= 0 && ball.State == BallState.Moving)
                    {
                        ball.Stop();
                        ballNormalReturn.Play();
                        MediaPlayer.Volume = 0.1f;
                    }
                    
                    if (ball.State == BallState.Ready && bricks1.Count <= 0)
                    {
                        essenceSystem.Color = Color.DarkGreen;
                        smokeSystem.MaxOpacity = 0.7f;
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

                else if (gameState == GameState.Greenpath)
                {
                    for (int i = 0; i < bricks2.Count; i++)
                    {
                        bricks2[i].Update();
                    }
                    paddle.Update(keyboardState);
                    ball.Update(gameTime, keyboardState, ballStartSpeed);

                    collisionManager.Update();

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        bricks2.Clear();
                    }

                    // Go to next state

                    if (bricks2.Count <= 0 && ball.State == BallState.Moving)
                    {
                        ball.Stop();
                        ballNormalReturn.Play();
                        MediaPlayer.Volume = 0.1f;
                    }

                    if (ball.State == BallState.Ready && bricks2.Count <= 0)
                    {
                        essenceSystem.Color = Color.MidnightBlue;
                        smokeSystem.SetSpawnInfo(0.4f, 3);
                        smokeSystem.SetLifespan(8, 8);
                        

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

                else if (gameState == GameState.City)
                {
                    for (int i = 0; i < bricks3.Count; i++)
                    {
                        bricks3[i].Update();
                    }
                    paddle.Update(keyboardState);
                    ball.Update(gameTime, keyboardState, ballStartSpeed);

                    collisionManager.Update();

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        bricks3.Clear();
                    }

                    // Go to next state

                    if (bricks3.Count <= 0 && ball.State == BallState.Moving)
                    {
                        ball.Stop();
                        ballNormalReturn.Play();
                        MediaPlayer.Volume = 0.1f;
                    }

                    if (ball.State == BallState.Ready && bricks3.Count <= 0)
                    {
                        essenceSystem.Color = Color.Plum;
                        smokeSystem.SetSpawnInfo(0.2f, 5);

                        ballStartSpeed = 14;
                        collisionManager.SetDeflectHeight(bricks5[0].Hitbox.Bottom);
                        collisionManager.SetActiveBricks(bricks4);
                        gameState = GameState.Sanctum;
                        currentMusic = sanctumMusic;
                        MediaPlayer.Play(currentMusic);
                        MediaPlayer.Volume = 1;
                    }
                }

                // Sanctum

                else if (gameState == GameState.Sanctum)
                {
                    for (int i = 0; i < bricks4.Count; i++)
                    {
                        bricks4[i].Update();
                    }
                    paddle.Update(keyboardState);
                    ball.Update(gameTime, keyboardState, ballStartSpeed);

                    collisionManager.Update();

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        bricks4.Clear();
                    }

                    // Go to next state

                    if (bricks4.Count <= 0 && ball.State == BallState.Moving)
                    {
                        ball.Stop();
                        ballNormalReturn.Play();
                        MediaPlayer.Volume = 0.1f;
                    }

                    if (ball.State == BallState.Ready && bricks4.Count <= 0)
                    {
                        essenceSystem.Color = Color.White;
                        dotSystem.SetSpawnInfo(0.2f, 1);

                        ballStartSpeed = 16;
                        collisionManager.SetDeflectHeight(0);
                        collisionManager.SetActiveBricks(bricks5);
                        gameState = GameState.Palace;
                        currentMusic = palaceMusic;
                        MediaPlayer.Play(currentMusic);
                        MediaPlayer.Volume = 1;
                    }
                }

                // Palace

                else if (gameState == GameState.Palace)
                {
                    for (int i = 0; i < bricks5.Count; i++)
                    {
                        bricks5[i].Update();
                    }

                    collisionManager.Update();
                    paddle.Update(keyboardState);
                    ball.Update(gameTime, keyboardState, ballStartSpeed);

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        bricks5.Clear();
                    }

                    // Go to next state

                    if (bricks5.Count <= 0 && ball.State == BallState.Moving)
                    {
                        ball.Stop();
                        ballDarkReturn.Play();
                        MediaPlayer.Volume = 0.1f;
                    }

                    if (ball.State == BallState.Ready && bricks5.Count <= 0)
                    {
                        gameState = GameState.AbyssCutscene;
                        MediaPlayer.Volume = 0;
                        ball.CanStart = false;
                        paddle.CanMove = false;
                    }
                }

                // Abyss Cutscene

                else if (gameState == GameState.AbyssCutscene)
                {
                    tendril1.Update(gameTime);
                    tendril2.Update(gameTime);
                    abyssTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (abyssTimer >= 4 && !tendril1Up)
                    {
                        tendril1.SetLocation(paddle.Hitbox.X - 100, paddle.Hitbox.Y - 158);
                        tendril1.Up();
                        tendril1Up = true;
                    }
                    if (abyssTimer >= 5 && !tendril2Up)
                    {
                        tendril2.SetLocation(paddle.Hitbox.Right - 110, paddle.Hitbox.Y - 155);
                        tendril2.Up();
                        tendril2Up = true;
                    }
                    if (abyssTimer >= 6 && !hasScreenFill)
                    {
                        // screen fill

                        smokeSystem.SetSpawnInfo(0.06f, 10);
                        smokeSystem.SetVelocity(0, 0, -15, -25);
                        smokeSystem.MaxOpacity = 1;
                        smokeSystem.SetLifespan(1.5f, 1.5f);
                        smokeSystem.SetSize(3, 4);

                        abyssScreenCover.Play();
                        hasScreenFill = true;
                    }
                    if (abyssTimer > 7)
                    {
                        smokeSystem.SetSpawnInfo(0.1f, 4);
                        smokeSystem.SetVelocity(0, 0, -0.4f, -0.5f);
                        essenceSystem.Color = Color.Black;
                        abyssRoar.Play();
                        gameState = GameState.Abyss;
                        collisionManager.SetActiveBricks(bricks6);
                        currentMusic = abyssMusic;
                        MediaPlayer.Play(abyssMusic);
                        MediaPlayer.Volume = 1;
                        ball.CanStart = true;
                        paddle.CanMove = true;
                    }
                }

                else if (gameState == GameState.Abyss)
                {

                    for (int i = 0; i < bricks6.Count; i++)
                    {
                        bricks6[i].Update();
                    }
                    paddle.Update(keyboardState);
                    ball.Update(gameTime, keyboardState, ballStartSpeed);

                    collisionManager.Update();

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        bricks6.Clear();
                    }

                    // Go to next state

                    if (bricks6.Count <= 0 && ball.State == BallState.Moving)
                    {
                        ball.Stop();
                        ballDarkReturn.Play();
                        MediaPlayer.Volume = 0.1f;
                    }

                    if (ball.State == BallState.Ready && bricks6.Count <= 0)
                    {
                        ballStartSpeed = 16;
                        collisionManager.SetActiveBricks(bricks6);
                        gameState = GameState.Abyss;
                        currentMusic = abyssMusic;
                        MediaPlayer.Play(currentMusic);
                        MediaPlayer.Volume = 1;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (gameState != GameState.Abyss)
                GraphicsDevice.Clear(Color.Gray);
            else
                GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.Draw(crossroadsBackground, Vector2.Zero, Color.White * 0.9f);

            essenceSystem.Draw(_spriteBatch);

            paddle.Draw(_spriteBatch);

            dotSystem.Draw(_spriteBatch);
            ballSystem.Draw(_spriteBatch);

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
            if (gameState == GameState.Abyss)
            {
                for (int i = 0; i < bricks6.Count; i++)
                {
                    bricks6[i].Draw(_spriteBatch);
                }
            }

            tendril1.Draw(_spriteBatch);
            tendril2.Draw(_spriteBatch);

            smokeSystem.Draw(_spriteBatch);
            _spriteBatch.Draw(screenFader, new Rectangle(-800, 640, 2600, 400), Color.White);
            _spriteBatch.Draw(vignette, new Rectangle(-6000, -3000, 13000, 6800), Color.White * 0.9f);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
