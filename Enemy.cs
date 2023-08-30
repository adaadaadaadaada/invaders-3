using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace invaders
{
    internal class Enemy
    {
        public TransformComponent transform;
        public CollisionComponent collision;
        public bool active;
        public int scoreValue;

        double shootInterval = 0.3;
        double lastShootTime;

        public Texture enemyImage;
        public Enemy(Vector2 startPos, Vector2 direction, float speed, int size, int score, Texture enemyImage)
        {
            this.enemyImage = enemyImage;
            transform = new TransformComponent(startPos, direction, speed);
            collision = new CollisionComponent(new Vector2(size, size));
            active = true;
            scoreValue = score;

            lastShootTime = -shootInterval;
        }

        internal void Update()
        {
            if (active)
            {
                float deltaTime = Raylib.GetFrameTime();
                transform.position += transform.direction * transform.speed * deltaTime;
            }
        }

        internal void Draw()
        {
            if (active)
            {
                Raylib.DrawTextureEx(enemyImage, transform.position, 0, collision.size.X / enemyImage.width, Raylib.WHITE);
            }
        }

    }
}
