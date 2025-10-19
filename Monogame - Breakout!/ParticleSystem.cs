using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame___Breakout_
{
    public enum EmitterShape
    {
        Point,
        Rectangle
    }
    public class ParticleSystem
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        public Rectangle EmitterBoundary { get; set; }
        private List<Particle> _particles;
        private List<Texture2D> _textures;
        private EmitterShape _emitterShape;
        private float _spawnTimer;
        private float _velocityAngle;

        public Color Color { get; set; } = Color.White;
        public bool ColorChange { get; set; } = false;
        public bool CanSpawn { get; set; } = true;
        public float SpawnRate { get; set; } = 1.0f;
        public int SpawnAmount { get; set; } = 1;
        public float MinYVelocity { get; set; } = -1f;
        public float MaxYVelocity { get; set; } = 1f;
        public float MinXVelocity { get; set; } = -1f;
        public float MaxXVelocity { get; set; } = 1f;
        public bool HighVelocityMode { get; set; } = false;

        public float MinAngularVelocity { get; set; } = -0.02f;
        public float MaxAngularVelocity { get; set; } = 0.02f;

        public float MinTTL { get; set; } = 120f;
        public float MaxTTL { get; set; } = 240f;

        public float MinSize {  get; set; } = 0.5f;
        public float MaxSize { get; set; } = 2f;
        public float MaxOpacity { get; set; } = 1f;
        public bool FadeIn { get; set; } = true;

        public bool OnlyMoveUp { get; set; } = false;

        public ParticleSystem(List<Texture2D> textures, Vector2 location, EmitterShape shape)
        {
            EmitterLocation = location;
            _textures = textures;
            _particles = new List<Particle>();
            random = new Random();
            _emitterShape = shape;
            _spawnTimer = 0;
            _velocityAngle = MathHelper.TwoPi;
        }

        public ParticleSystem(List<Texture2D> textures, Rectangle boundary, EmitterShape shape)
        {
            EmitterBoundary = boundary;
            _textures = textures;
            _particles = new List<Particle>();
            random = new Random();
            _emitterShape = shape;
            _spawnTimer = 0;
            _velocityAngle = MathHelper.TwoPi;
        }

        public void Update(GameTime gameTime)
        {
            if (CanSpawn)
            {
                _spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_spawnTimer >= SpawnRate)
                {
                    _spawnTimer -= SpawnRate;
                    for (int i = 0; i < SpawnAmount; i++)
                    {
                        _particles.Add(GenerateNewParticle());
                    }
                }
            }
            

            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].Update(FadeIn);
                if (_particles[i].Color != Color && ColorChange)
                {
                    _particles[i].Color = Color;
                }
                if (_particles[i].TTL <= 0)
                {
                    _particles.RemoveAt(i);
                    i--;
                }
            }
        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = _textures[random.Next(_textures.Count)];
            Vector2 position = EmitterLocation;

            if (_emitterShape == EmitterShape.Rectangle)
            {
                position = new Vector2(RandomRange(EmitterBoundary.Left, EmitterBoundary.Right), RandomRange(EmitterBoundary.Top, EmitterBoundary.Bottom));
            }


            float vx, vy; // Velocity Components
            if (OnlyMoveUp)
            {
                vx = 0f;
                vy = RandomRange(-Math.Abs(MinYVelocity), -Math.Abs(MaxYVelocity));
            }
            else
            {
                vx = RandomRange(MinXVelocity, MaxXVelocity);
                vy = RandomRange(MinYVelocity, MaxYVelocity);
            }
            _velocityAngle = (float)random.NextDouble() * MathHelper.TwoPi;
            Vector2 velocity = new Vector2(vx, vy);
            if (HighVelocityMode)
            {
                velocity = new Vector2(vx * (float)Math.Cos(_velocityAngle), vy * (float)(Math.Sin(_velocityAngle)));
            }
            float angle = 0;
            float angularVelocity = RandomRange(MinAngularVelocity, MaxAngularVelocity);
            Color color = Color;
            float size = RandomRange(MinSize, MaxSize);
            float ttl = RandomRange(MinTTL, MaxTTL);
            float opacity = MaxOpacity;

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl, opacity);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < _particles.Count; index++)
            {
                _particles[index].Draw(spriteBatch);
            }
        }
        private float RandomRange(float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }
        public void SetVelocity(float xMin, float xMax, float yMin, float yMax)
        {
            MinXVelocity = xMin;
            MinYVelocity = yMin;
            MaxXVelocity = xMax;
            MaxYVelocity = yMax;
        }
        public void SetLifespan(float min, float max)
        {
            MaxTTL = max * 60;
            MinTTL = min * 60;
        }
        public void SetSize(float min, float max)
        {
            MaxSize = max;
            MinSize = min;
        }
        public void SetAngularVelocity(float min, float max)
        {
            MaxAngularVelocity = max / 60;
            MinAngularVelocity = min / 60;
        }
        public void SetSpawnInfo(float rate, int amount)
        {
            SpawnRate = rate;
            SpawnAmount = amount;
        }
    }
}
