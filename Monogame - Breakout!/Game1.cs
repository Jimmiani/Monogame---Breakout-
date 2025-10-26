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
        Win
    }
    enum GameState
    {
        Crossroads,
        Greenpath,
        City,
        Sanctum,
        Palace,
        AbyssCutscene,
        Abyss,
        AbyssEnding1,
        AbyssEnding2,
        AbyssEnding3
    }
    enum AbyssState
    {
        Bubble,
        Tendril1Up,
        Tendril2Up,
        AbyssCover,
        Roar,
        Shine,
        Shake,
        LightCover,
        LightFade
    }
    enum LoseState
    {
        Lose,
        TextAppear1,
        TextAppear2,
        TextAppear3,
        Replay
    }
    enum WinState
    {
        TextAppear1,
        TextAppear2,
        TextAppear3,
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        CollisionManager collisionManager;
        Camera2D camera;

        ParticleSystem smokeSystem, essenceSystem, ballSystem, dotSystem;
        List<Texture2D> smokeParticles, essenceParticles, dotParticles;
        SpriteFont titleFont, gameFont;

        Song crossroadsMusic, greenpathMusic, cityMusic, sanctumMusic, palaceMusic, abyssMusic, currentMusic;
        SoundEffect abyssAmbience, abyssRoar, abyssScreenCover, ballNormalReturn, ballDarkReturn, ballShine, brickDamage1, brickDamage2, brickDeath, brickDeflect, paddleBounce, screenRumbleEffect, ballLongShine, ballEntrance1, ballEntrance2, laserPrepare, laserBurst;
        SoundEffect lightExplodeLoop, finalLightDisappear, finalHit, finalLightExplode, longLose, textAppear, ballAppear, winEffect, titleSong, titleAmbienceEffect;
        SoundEffectInstance abyssAmbienceInstance, rumbleInstance, longShineInstance, explodeInstance, titleMusic, titleAmbience;



        KeyboardState keyboardState;
        Screen screen;
        GameState gameState;
        AbyssState abyssState;
        LoseState loseState;
        WinState winState;
        Paddle paddle;
        Ball ball;
        List<Brick> bricks1, bricks2, bricks3, bricks4, bricks5, bricks6;
        Texture2D paddleTexture1, paddleTexture2, paddleTexture3, paddleTexture4, paddleTexture5, paddleTexture6;
        Texture2D brickTexture1, brickTexture2, brickTexture3, brickTexture4, brickTexture5, brickTexture6;
        Texture2D ballTexture, ballGlowTexture;

        Texture2D titleBackground, crossroadsBackground, greenpathBackground, cityBackground, sanctumBackground, palaceBackground, abyssBackground, currentBackground, blackBackground;
        Texture2D screenFader, vignette, shineTexture;
        Rectangle faderRect, shineRect;
        Color shineColor, blackBackgroundColor, textColor;


        float abyssTimer, loseTimer, winTimer;
        bool showText1, showText2, showText3, hasChangedLoseStates;
        string text1, text2, text3;
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

            camera = new Camera2D(GraphicsDevice.Viewport);

            smokeParticles = new List<Texture2D>();
            essenceParticles = new List<Texture2D>();
            dotParticles = new List<Texture2D>();

            window = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            faderRect = new Rectangle(-1200, 630, 3400, 400);
            shineRect = new Rectangle(-2000, 0, 4, 4);
            shineColor = Color.White;
            blackBackgroundColor = Color.Black * 0;
            bricks1 = new List<Brick>();
            bricks2 = new List<Brick>();
            bricks3 = new List<Brick>();
            bricks4 = new List<Brick>();
            bricks5 = new List<Brick>();
            bricks6 = new List<Brick>();

            abyssTimer = 0;
            loseTimer = 0;
            winTimer = 0;
            tendril1UpTextures = new List<Texture2D>();
            tendril1DownTextures = new List<Texture2D>();
            tendril2UpTextures = new List<Texture2D>();
            tendril2DownTextures = new List<Texture2D>();

            screen = Screen.Intro;
            gameState = GameState.Crossroads;
            abyssState = AbyssState.Bubble;
            loseState = LoseState.Lose;
            winState = WinState.TextAppear1;

            showText1 = false;
            showText2 = false;
            showText3 = false;

            text1 = "You Lost";
            text2 = "Taken by the Abyss";
            text3 = "Press 'R' to play again";
            textColor = Color.White;

            hasChangedLoseStates = false;

            base.Initialize();

            currentBackground = titleBackground;
            currentMusic = crossroadsMusic;
            rumbleInstance.IsLooped = true;
            paddle = new Paddle(paddleTexture1, window);
            ball = new Ball(ballTexture, window, ballGlowTexture);


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
            
            collisionManager = new CollisionManager(ball, bricks1, paddle, camera, brickDamage1, brickDamage2, brickDeath, brickDeflect, paddleBounce);
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
            smokeSystem.SetLifespan(7, 7);
            smokeSystem.MaxOpacity = 1;

            smokeSystem.SetDefaults(Color.White, false, true, 0.4f, 2, 0, 0, -0.4f, -0.5f, false, -0.25f, 0.25f, 7, 7, 2, 2.5f, 0, true);

            essenceSystem.SetSpawnInfo(3f, 1);
            essenceSystem.SetVelocity(0, 0, 0, 0);
            essenceSystem.SetLifespan(9, 10);
            essenceSystem.MaxOpacity = 0.3f;
            essenceSystem.SetSize(2, 4);
            essenceSystem.SetAngularVelocity(-0.2f, 0.2f);
            essenceSystem.Color = Color.DarkSlateBlue;
            essenceSystem.ColorChange = true;

            essenceSystem.SetDefaults(Color.DarkSlateBlue, true, true, 3, 1, 0, 0, 0, 0, false, -0.2f, 0.2f, 9, 10, 2, 4, 0.3f, true);

            ballSystem.SetVelocity(-0.1f, 0.1f, -0.1f, 0.1f);
            ballSystem.SetSize(0.7f, 1);
            ballSystem.SetSpawnInfo(0.05f, 1);
            ballSystem.SetLifespan(0.5f, 1);
            ballSystem.Color = Color.White;
            ballSystem.ColorChange = true;

            ballSystem.SetDefaults(Color.White, false, true, 0.05f, 1, -0.1f, 0.1f, -0.1f, 0.1f, false, 0, 0, 0.5f, 1, 0.7f, 1, 1, true);

            dotSystem.SetVelocity(0, 0, -0.2f, -0.3f);
            dotSystem.SetSize(0.5f, 1);
            dotSystem.SetSpawnInfo(0.6f, 1);
            dotSystem.SetLifespan(3, 4);
            dotSystem.Color = Color.Black;

            dotSystem.SetDefaults(Color.Black, false, true, 0.6f, 1, 0, 0, -0.2f, -0.3f, false, 0, 0, 3, 4, 0.5f, 1, 1, true);
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
            abyssMusic = Content.Load<Song>("Breakout/Audio/Music/Abyss/dark_descent");
            titleSong = Content.Load<SoundEffect>("Breakout/Audio/Music/Title/titleMusic");
            titleMusic = titleSong.CreateInstance();

            // Sound Effects

            abyssAmbience = Content.Load<SoundEffect>("Breakout/Audio/Music/Abyss/abyss_ambience");
            abyssAmbienceInstance = abyssAmbience.CreateInstance();
            abyssRoar = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Abyss/abyss_roar");
            abyssScreenCover = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Abyss/abyss_screen_cover");
            ballNormalReturn = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Return/Normal/ball_return");
            ballDarkReturn = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Return/Dark/ball_return_abyss");
            ballShine = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Shine/ball_shine");
            brickDamage1 = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Bricks/Damage/brick_damage_1");
            brickDamage2 = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Bricks/Damage/brick_damage_2");
            brickDeath = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Bricks/Break/brick_death");
            brickDeflect = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Bricks/Deflect/brick_deflect");
            paddleBounce = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Paddle/paddle_bounce");
            tendrilEffect = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Abyss/radiance_tentacles_whip_up");
            ballLongShine = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Shine/radiance_challenge");
            longShineInstance = ballLongShine.CreateInstance();
            screenRumbleEffect = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Shine/misc_rumble_loop");
            rumbleInstance = screenRumbleEffect.CreateInstance();
            ballEntrance1 = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Shine/mage_lord_onscreen_appear");
            ballEntrance2 = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Shine/radiance_scream_long");
            laserPrepare = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Shine/radiance_laser_prepare");
            laserBurst = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Shine/radiance_laser_burst");
            lightExplodeLoop = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Win/explode_loop");
            explodeInstance = lightExplodeLoop.CreateInstance();
            finalLightExplode = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Win/explode");
            finalHit = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Win/knock_down");
            finalLightDisappear = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Win/final_light_disappear");
            longLose = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Lose/lose_long");
            textAppear = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Lose/text_appear");
            ballAppear = Content.Load<SoundEffect>("Breakout/Audio/Sound EFfects/Ball/Appear/dream_enter_pt_2");
            winEffect = Content.Load<SoundEffect>("Breakout/Audio/Sound Effects/Ball/Win/win_effect");

            // Images----------------------------------------------------------------------

            // Particles

            for (int i = 1; i <= 5; i++)
                smokeParticles.Add(Content.Load<Texture2D>("Breakout/Images/Particles/Smoke/abyss_smoke_0" + i));
            for (int i = 1; i <= 3; i++)
                essenceParticles.Add(Content.Load<Texture2D>("Breakout/Images/Particles/Essence/dream_particle_" + i));
            dotParticles.Add(Content.Load<Texture2D>("Breakout/Images/Particles/Ball/particle_01"));
            screenFader = Content.Load<Texture2D>("Breakout/Images/Particles/Fader/black_fader");
            vignette = Content.Load<Texture2D>("Breakout/Images/Particles/Vignette/credits vignette");
            shineTexture = Content.Load<Texture2D>("Breakout/Images/Particles/Fader/credits flash_round");

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
            ballGlowTexture = Content.Load<Texture2D>("Breakout/Images/Particles/Fader/white_light");

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

            titleBackground = Content.Load<Texture2D>("Breakout/Images/Backgrounds/Voidheart_menu_BG");
            crossroadsBackground = Content.Load<Texture2D>("Breakout/Images/Backgrounds/crossroads_background");
            greenpathBackground = Content.Load<Texture2D>("Breakout/Images/Backgrounds/greenpath_background");
            cityBackground = Content.Load<Texture2D>("Breakout/Images/Backgrounds/city_background");
            sanctumBackground = Content.Load<Texture2D>("Breakout/Images/Backgrounds/sanctum_background");
            palaceBackground = Content.Load<Texture2D>("Breakout/Images/Backgrounds/palace_background");
            abyssBackground = Content.Load<Texture2D>("Breakout/Images/Backgrounds/abyss_background");
            blackBackground = Content.Load<Texture2D>("Breakout/Images/Backgrounds/white_square");

            // Fonts----------------------------------------------------------------------

            titleFont = Content.Load<SpriteFont>("Breakout/Font/titleFont");
            gameFont = Content.Load<SpriteFont>("Breakout/Font/gameFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();

            if (screen == Screen.Intro)
            {
                if (titleMusic.State == SoundState.Stopped)
                    titleMusic.Play();
                smokeSystem.Update(gameTime);
            }

            else if (screen == Screen.Game)
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

                paddle.Update(keyboardState);
                ball.Update(gameTime, keyboardState);

                collisionManager.Update();
                camera.Update(gameTime);


                if (Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    ballSystem.RestoreDefaults();
                }

                // Lose

                if (ball.Hitbox.Top > window.Height)
                {
                    abyssAmbienceInstance.Stop();

                    if (!hasChangedLoseStates)
                    {
                        loseState = LoseState.Lose;
                        hasChangedLoseStates = true;
                    }

                    loseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (MediaPlayer.Volume > 0)
                    {
                        blackBackgroundColor = Color.Black * (loseTimer / 3);
                        MediaPlayer.Volume = 1 - (loseTimer / 3);
                    }

                    if (loseState == LoseState.Lose)
                    {
                        longLose.Play();
                        loseState = LoseState.TextAppear1;
                        loseTimer = 0;

                        text1 = "You Lost";
                        text2 = "Taken by the Abyss";
                        text3 = "Press 'R' to play again";

                        textColor = Color.White;
                    }

                    else if (loseState == LoseState.TextAppear1)
                    {
                        if (loseTimer > 7.5f)
                        {
                            textAppear.Play();
                            showText1 = true;
                            loseState = LoseState.TextAppear2;
                            loseTimer = 0;
                        }
                    }

                    else if (loseState == LoseState.TextAppear2)
                    {
                        if (loseTimer > 3)
                        {
                            textAppear.Play();
                            showText2 = true;
                            loseState = LoseState.TextAppear3;
                            loseTimer = 0;
                        }
                    }

                    else if (loseState == LoseState.TextAppear3)
                    {
                        if (loseTimer > 4)
                        {
                            showText3 = true;
                            if (keyboardState.IsKeyDown(Keys.R))
                            {
                                // Particles

                                smokeSystem.RestoreDefaults();
                                essenceSystem.RestoreDefaults();
                                ballSystem.RestoreDefaults();
                                dotSystem.RestoreDefaults();

                                // Music

                                currentMusic = crossroadsMusic;
                                MediaPlayer.Play(currentMusic);
                                longLose.Play();

                                // Text

                                showText1 = false;
                                showText2 = false;
                                showText3 = false;

                                // Images

                                currentBackground = crossroadsBackground;
                                paddle.SetPaddle(paddleTexture1);
                                ball.Color = Color.White;

                                // Bricks

                                bricks1.Clear();
                                bricks2.Clear();
                                bricks3.Clear();
                                bricks4.Clear();
                                bricks5.Clear();
                                bricks6.Clear();

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

                                // States

                                gameState = GameState.Crossroads;
                                abyssState = AbyssState.Bubble;
                                ball.CanStart = false;
                                ball.State = BallState.Ready;
                                loseState = LoseState.Replay;
                                collisionManager.SetActiveBricks(bricks1);
                                collisionManager.SetDeflectHeight(bricks2[0].Hitbox.Bottom);
                                loseTimer = 0;
                            }
                        }
                    }
                    else if (loseState == LoseState.Replay)
                    {
                        blackBackgroundColor = Color.Black * (1 - (loseTimer / 3));
                        if (MediaPlayer.Volume < 1)
                            MediaPlayer.Volume = 0 + (loseTimer / 3);

                        if (loseTimer > 4)
                        {
                            ball.Position = new Vector2(485, 385);
                            ball.CanStart = true;
                            ball.ChangeStartSpeed(8);
                            loseState = LoseState.Lose;
                            loseTimer = 0;
                            ballAppear.Play();
                            camera.Shake(4, 1, true);
                        }
                    }
                }

                // Crossroads

                if (gameState == GameState.Crossroads)
                {
                    for (int i = 0; i < bricks1.Count; i++)
                    {
                        bricks1[i].Update();
                    }

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
                        essenceSystem.Color = Color.LightGreen;
                        smokeSystem.MaxOpacity = 0.7f;
                        ball.ChangeStartSpeed(10);
                        collisionManager.SetDeflectHeight(bricks3[0].Hitbox.Bottom);
                        collisionManager.SetActiveBricks(bricks2);
                        gameState = GameState.Greenpath;
                        currentMusic = greenpathMusic;
                        MediaPlayer.Play(currentMusic);
                        MediaPlayer.Volume = 1;
                        currentBackground = greenpathBackground;
                        paddle.SetPaddle(paddleTexture2);

                        ballSystem.Color = Color.LightGray;
                        ball.Color = Color.LightGray;
                    }
                }

                // Greenpath

                else if (gameState == GameState.Greenpath)
                {
                    for (int i = 0; i < bricks2.Count; i++)
                    {
                        bricks2[i].Update();
                    }

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
                        

                        ball.ChangeStartSpeed(12);
                        collisionManager.SetDeflectHeight(bricks4[0].Hitbox.Bottom);
                        collisionManager.SetActiveBricks(bricks3);
                        gameState = GameState.City;
                        currentMusic = cityMusic;
                        MediaPlayer.Play(currentMusic);
                        MediaPlayer.Volume = 1;
                        currentBackground = cityBackground;
                        paddle.SetPaddle(paddleTexture3);

                        ballSystem.Color = Color.DarkGray;
                        ball.Color = Color.DarkGray;
                    }
                }

                // City

                else if (gameState == GameState.City)
                {
                    for (int i = 0; i < bricks3.Count; i++)
                    {
                        bricks3[i].Update();
                    }

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

                        ball.ChangeStartSpeed(14);
                        collisionManager.SetDeflectHeight(bricks5[0].Hitbox.Bottom);
                        collisionManager.SetActiveBricks(bricks4);
                        gameState = GameState.Sanctum;
                        currentMusic = sanctumMusic;
                        MediaPlayer.Play(currentMusic);
                        MediaPlayer.Volume = 1;
                        currentBackground = sanctumBackground;
                        paddle.SetPaddle(paddleTexture4);

                        ballSystem.Color = Color.Gray;
                        ball.Color = Color.Gray;
                    }
                }

                // Sanctum

                else if (gameState == GameState.Sanctum)
                {
                    for (int i = 0; i < bricks4.Count; i++)
                    {
                        bricks4[i].Update();
                    }

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

                        ball.ChangeStartSpeed(16);
                        collisionManager.SetDeflectHeight(0);
                        collisionManager.SetActiveBricks(bricks5);
                        gameState = GameState.Palace;
                        currentMusic = palaceMusic;
                        MediaPlayer.Play(currentMusic);
                        MediaPlayer.Volume = 1;
                        currentBackground = palaceBackground;
                        paddle.SetPaddle(paddleTexture5);

                        ballSystem.Color = Color.Black;
                        ball.Color = Color.Black;
                    }
                }

                // Palace

                else if (gameState == GameState.Palace)
                {
                    for (int i = 0; i < bricks5.Count; i++)
                    {
                        bricks5[i].Update();
                    }

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
                        ball.ChangeStartSpeed(18);
                    }
                }

                // Abyss Cutscene

                else if (gameState == GameState.AbyssCutscene)
                {
                    tendril1.Update(gameTime);
                    tendril2.Update(gameTime);
                    abyssTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (abyssState == AbyssState.Bubble)
                    {
                        if (abyssTimer > 3)
                        {
                            dotSystem.SetVelocity(0, 0, -5, -8);
                            dotSystem.SetSpawnInfo(0.03f, 1);
                            dotSystem.SetLifespan(1, 1.5f);
                            dotSystem.EmitterBoundary = new Rectangle(paddle.Hitbox.X - 100, paddle.Hitbox.Bottom + 50, paddle.Hitbox.Width + 200, 20);
                            rumbleInstance.Volume = 1;
                            rumbleInstance.Play();
                            camera.Shake(5, 1, true);
                        }
                        if (abyssTimer > 7)
                        {
                            abyssState = AbyssState.Tendril1Up;
                            abyssTimer = 0;
                        }
                    }
                    else if (abyssState == AbyssState.Tendril1Up)
                    {
                        if (rumbleInstance.Volume > 0.1)
                            rumbleInstance.Volume = 1 - abyssTimer;
                        else
                            rumbleInstance.Stop();
                        if (abyssTimer >= 1.5)
                        {
                            dotSystem.EmitterBoundary = new Rectangle(0, 650, 1000, 150);
                            dotSystem.SetVelocity(0, 0, -0.2f, -0.3f);
                            dotSystem.SetSpawnInfo(0.6f, 1);
                            dotSystem.SetLifespan(3, 4);

                            tendril1.SetLocation(paddle.Hitbox.X - 100, paddle.Hitbox.Y - 158);
                            tendril1.Up();
                            abyssState = AbyssState.Tendril2Up;
                            abyssTimer = 0;
                        }
                    }
                    else if (abyssState == AbyssState.Tendril2Up)
                    {
                        if (abyssTimer >= 1)
                        {
                            tendril2.SetLocation(paddle.Hitbox.Right - 110, paddle.Hitbox.Y - 155);
                            tendril2.Up();
                            abyssState = AbyssState.AbyssCover;
                            abyssTimer = 0;
                        }
                    }
                    else if (abyssState == AbyssState.AbyssCover)
                    {
                        if (abyssTimer >= 1)
                        {
                            camera.Shake(4, 2, false);
                            abyssScreenCover.Play(0.7f, 0, 0);

                            smokeSystem.SetSpawnInfo(0.05f, 5);
                            smokeSystem.SetVelocity(0, 0, -15, -20);
                            smokeSystem.MaxOpacity = 1;
                            smokeSystem.SetLifespan(1.5f, 1.5f);
                            smokeSystem.SetSize(3, 4);

                            dotSystem.SetVelocity(0, 0, -20, -25);
                            dotSystem.SetSpawnInfo(0.05f, 5);
                            dotSystem.SetLifespan(3, 4);
                            dotSystem.SetSize(0.7f, 1.3f);
                            dotSystem.FadeIn = false;

                            abyssState = AbyssState.Roar;
                            abyssTimer = 0;
                        }
                    }
                    else if (abyssState == AbyssState.Roar)
                    {
                        if (abyssTimer >= 1)
                        {
                            smokeSystem.SetVelocity(0, 0, -0.4f, -0.5f);
                            smokeSystem.SetSize(2, 2.5f);
                            smokeSystem.SetAngularVelocity(-0.25f, 0.25f);
                            smokeSystem.SetSpawnInfo(0.15f, 8);
                            smokeSystem.SetLifespan(7, 7);

                            dotSystem.SetVelocity(0, 0, -0.2f, -0.3f);
                            dotSystem.SetSize(0.5f, 1);
                            dotSystem.SetSpawnInfo(0.6f, 1);
                            dotSystem.SetLifespan(3, 4);
                            dotSystem.FadeIn = true;

                            essenceSystem.Color = Color.Black;

                            currentBackground = abyssBackground;
                            paddle.SetPaddle(brickTexture6);


                            abyssRoar.Play(0.7f, 0, 0);
                            abyssAmbienceInstance.IsLooped = true;
                            abyssAmbienceInstance.Play();
                            abyssAmbienceInstance.Volume = 0.5f;
                            currentMusic = abyssMusic;

                            ball.Glow = false;

                            abyssState = AbyssState.Shine;
                            abyssTimer = 0;
                            blackBackgroundColor = Color.Black;
                        }
                    }
                    else if (abyssState == AbyssState.Shine)
                    {
                        blackBackgroundColor = Color.Black * (1 - (abyssTimer / 3));
                        if (abyssTimer >= 6)
                        {
                            longShineInstance.Play();
                            abyssState = AbyssState.Shake;
                            abyssTimer = 0;
                            shineColor = Color.White * 0;
                            ballSystem.ColorChange = false;

                            shineRect.Width = 1;
                            shineRect.Height = 1;

                            shineRect.X = ball.Hitbox.Center.X - shineRect.Width / 2;
                            shineRect.Y = ball.Hitbox.Center.Y - shineRect.Height / 2;
                        }
                    }
                    else if (abyssState == AbyssState.Shake)
                    {
                        if (abyssTimer >= 4)
                        {
                            float color = abyssTimer / 12;
                            shineColor = Color.White * (color * color);
                            shineRect.X = ball.Hitbox.Center.X - shineRect.Width / 2;
                            shineRect.Y = ball.Hitbox.Center.Y - shineRect.Height / 2;
                            shineRect.Inflate(1, 1);

                            camera.Shake(6, 0.5f, true);
                            rumbleInstance.Play();

                            ballSystem.HighVelocityMode = true;
                            ballSystem.SetVelocity(3, 4, 3, 4);
                            ballSystem.SetLifespan(2, 3);
                        }
                        if (abyssTimer >= 7.1)
                        {
                            camera.Shake(40, 5, true);
                            ballEntrance1.Play();
                            ballEntrance2.Play();
                            abyssState = AbyssState.LightCover;
                            abyssTimer = 0;
                            ballSystem.Color = Color.White;
                            ball.Color = Color.White;
                            tendril1.Down();
                            tendril2.Down();
                            paddle.CanMove = true;
                            ball.Glow = true;
                            laserPrepare.Play();

                            ballSystem.FadeIn = false;
                            ballSystem.SetVelocity(10, 10, 10, 10);
                            ballSystem.SetSpawnInfo(0.05f, 4);
                        }
                    }
                    else if (abyssState == AbyssState.LightCover)
                    {
                        if (abyssTimer >= 4.5)
                        {
                            float color = abyssTimer / 4.6f;
                            shineColor = Color.White * (color * color);
                            shineRect.X = ball.Hitbox.Center.X - shineRect.Width / 2;
                            shineRect.Y = ball.Hitbox.Center.Y - shineRect.Height / 2;

                            shineRect.Inflate(110, 110);
                            rumbleInstance.Stop(true);
                        }
                        if (abyssTimer >= 4.7)
                        {
                            abyssState = AbyssState.LightFade;
                            abyssTimer = 0;

                            ballSystem.SetVelocity(-0.1f, 0.1f, -0.1f, 0.1f);
                            ballSystem.SetSpawnInfo(0.05f, 1);
                            ballSystem.SetLifespan(0.5f, 1);
                            ballSystem.ColorChange = true;
                            ballSystem.FadeIn = true;

                            longShineInstance.Stop();
                            MediaPlayer.Play(abyssMusic);
                            MediaPlayer.Volume = 1;
                        }
                    }
                    else if (abyssState == AbyssState.LightFade)
                    {
                        if (abyssTimer > 3)
                        {
                            shineColor = shineColor * 0.9f;
                        }
                        if (abyssTimer > 3.5)
                        {
                            abyssTimer = 0;
                            gameState = GameState.Abyss;
                            ball.CanStart = true;
                            collisionManager.SetActiveBricks(bricks6);
                            shineColor = Color.White * 0;
                            shineRect.Width = 1;
                            shineRect.Height = 1;

                            shineRect.X = ball.Hitbox.Center.X - shineRect.Width / 2;
                            shineRect.Y = ball.Hitbox.Center.Y - shineRect.Height / 2;
                        }
                    }
                }

                // Abyss

                else if (gameState == GameState.Abyss)
                {
                    for (int i = 0; i < bricks6.Count; i++)
                    {
                        bricks6[i].Update();
                    }

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        bricks6.Clear();
                    }

                    // Go to next state

                    if (bricks6.Count <= 0 && ball.State == BallState.Moving)
                    {
                        ball.Win();
                        MediaPlayer.Volume = 0;
                        finalHit.Play();
                    }

                    if (ball.State == BallState.Win && bricks6.Count <= 0)
                    {
                        gameState = GameState.AbyssEnding1;
                        abyssAmbienceInstance.Stop();
                    }
                }
                else if (gameState == GameState.AbyssEnding1)
                {
                    abyssTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (abyssTimer > 4)
                    {
                        camera.Shake(8, 2, false);
                        if (abyssTimer < 5)
                            explodeInstance.Play();
                        ballSystem.FadeIn = false;
                        ballSystem.SetVelocity(5, 5, 5, 5);
                        ballSystem.SetSpawnInfo(0.05f, 2);
                        ballSystem.SetLifespan(10, 10);
                    }
                    if (explodeInstance.State == SoundState.Stopped && abyssTimer > 5)
                    {
                        finalLightExplode.Play();
                        explodeInstance.Stop();
                        gameState = GameState.AbyssEnding2;
                        abyssTimer = 0;

                        ballSystem.SetVelocity(9, 9, 9, 9);

                        shineRect.Width = 1;
                        shineRect.Height = 1;

                        shineRect.X = ball.Hitbox.Center.X - shineRect.Width / 2;
                        shineRect.Y = ball.Hitbox.Center.Y - shineRect.Height / 2;

                        camera.Shake(40, 4, true);
                    }
                }
                else if (gameState == GameState.AbyssEnding2)
                {
                    abyssTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    float color = abyssTimer / 2.5f;
                    shineColor = Color.White * (color * color);

                    shineRect.X = window.Center.X - shineRect.Width / 2;
                    shineRect.Y = window.Center.Y - shineRect.Height / 2;
                    shineRect.Inflate(10, 10);

                    if (abyssTimer > 5)
                    {
                        finalLightDisappear.Play();
                        gameState = GameState.AbyssEnding3;
                        abyssTimer = 0;

                        text1 = "You Won";
                        text2 = "Conquered the Abyss";
                        text3 = "You can be at peace now";

                        textColor = Color.Black;
                    }
                }
                else if (gameState == GameState.AbyssEnding3)
                {
                    abyssTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (abyssTimer > 4)
                    {
                        screen = Screen.Win;
                        winEffect.Play();
                    }
                }
            }
            else if (screen == Screen.Win)
            {
                winTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (winState == WinState.TextAppear1)
                {
                    if (winTimer > 3)
                    {
                        textAppear.Play();
                        showText1 = true;
                        winTimer = 0;
                        winState = WinState.TextAppear2;
                    }
                }
                else if (winState == WinState.TextAppear2)
                {
                    if (winTimer > 3)
                    {
                        textAppear.Play();
                        showText2 = true;
                        winTimer = 0;
                        winState = WinState.TextAppear3;
                    }
                }
                else if (winState == WinState.TextAppear3)
                {
                    if (winTimer > 2)
                    {
                        showText3 = true;
                        winTimer = 0;
                    }
                }
            }

                base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (screen == Screen.Intro)
            {
                _spriteBatch.Begin();

                _spriteBatch.Draw(currentBackground, new Rectangle(-100, 0, 1200, 900), Color.White);
                smokeSystem.Draw(_spriteBatch);
                _spriteBatch.Draw(screenFader, faderRect, Color.White);
                _spriteBatch.Draw(vignette, new Rectangle(-7000, -5000, 15000, 10800), Color.White * 0.7f);

                _spriteBatch.End();
            }

            else if (screen != Screen.Intro)
            {
                _spriteBatch.Begin(transformMatrix: camera.Transform);

                _spriteBatch.Draw(currentBackground, new Rectangle(-50, -40, 1100, 880), Color.White);

                essenceSystem.Draw(_spriteBatch);

                ballSystem.Draw(_spriteBatch);
                ball.Draw(_spriteBatch);
                paddle.Draw(_spriteBatch);

                dotSystem.Draw(_spriteBatch);

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
                if (gameState == GameState.Abyss || abyssState == AbyssState.LightFade)
                {
                    for (int i = 0; i < bricks6.Count; i++)
                    {
                        bricks6[i].Draw(_spriteBatch);
                    }
                }

                tendril1.Draw(_spriteBatch);
                tendril2.Draw(_spriteBatch);

                smokeSystem.Draw(_spriteBatch);
                _spriteBatch.Draw(screenFader, faderRect, Color.White);
                _spriteBatch.Draw(vignette, new Rectangle(-7000, -5000, 15000, 10800), Color.White * 0.7f);
                _spriteBatch.Draw(shineTexture, shineRect, shineColor);
                _spriteBatch.Draw(blackBackground, new Rectangle(-500, -500, 2000, 2000), blackBackgroundColor);

                if (showText1)
                {
                    _spriteBatch.DrawString(titleFont, text1, new Vector2(190, 150), textColor);
                    _spriteBatch.DrawString(titleFont, text1, new Vector2(193, 150), textColor * 0.5f);
                }
                if (showText2)
                {
                    _spriteBatch.DrawString(gameFont, text2, new Vector2(368, 290), textColor);
                }
                if (showText3)
                {
                    _spriteBatch.DrawString(gameFont, text3, new Vector2(350, 500), textColor);
                }

                _spriteBatch.End();
            }


            base.Draw(gameTime);
        }
    }
}
