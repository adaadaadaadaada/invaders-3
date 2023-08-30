using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace invaders
{
    internal class Player
    {
        public TransformComponent transform;
        public CollisionComponent collision;

        double shootInterval = 0.3;
        double lastShootTime;

        public Texture playerImage;

        public Player(Vector2 startPos, float speed, int size, Texture playerImage)
        {
            transform = new TransformComponent(startPos, new Vector2(0, 0), speed);
            collision = new CollisionComponent(new Vector2(size, size));

            lastShootTime = -shootInterval;
            this.playerImage = playerImage;
        }

        public bool Update()
        {
            float deltaTime = Raylib.GetFrameTime();
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                transform.position.X -= transform.speed.X * deltaTime;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                transform.position.X += transform.speed.X * deltaTime;
            }

            bool shoot = false;

            if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
            {
                double timeNow = Raylib.GetTime();
                double timeSinceLastShot = timeNow - lastShootTime;
                if (timeSinceLastShot >= shootInterval)
                {
                    Console.WriteLine("Player shoots!");
                    lastShootTime = timeNow;
                    shoot = true;
                }
            }
            return shoot;
        }

        public void Draw()
        {
            Raylib.DrawTextureEx(playerImage, transform.position, 0, collision.size.X / playerImage.width, Raylib.WHITE);
        }
    }
}
