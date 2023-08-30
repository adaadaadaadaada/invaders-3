using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Raylib_CsLo;

namespace invaders
{
    internal class Bullet
    {
        public bool active;
        public TransformComponent transform;
        public CollisionComponent collision;
        public float angle = 0f;
        public float anglespeed = 1f;
        

        public Bullet(Vector2 startPosition, Vector2 direction, float speed, int size)
        {
            active = true;

            this.transform = new TransformComponent(startPosition, direction, speed);
            this.collision = new CollisionComponent(new Vector2(size, size));
        }

        public void Reset(Vector2 startPosition, Vector2 direction, float speed, int size)
        {
            active = true;

            transform = new TransformComponent(startPosition, direction, speed);
            collision = new CollisionComponent(new Vector2(size, size));

            angle = 0f;
        }

        public void Update()
        {
            Matrix4x4 rotation = Matrix4x4.CreateRotationZ(angle); // pyörivä ammus
            Vector2 rotated = Vector2.Transform(transform.direction, rotation);

            transform.position += rotated * transform.speed * Raylib.GetFrameTime();
            
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                angle -= anglespeed * Raylib.GetFrameTime();
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                angle += anglespeed * Raylib.GetFrameTime();
            }
        }

        public void Draw()
        {
            Raylib.DrawRectangleV(transform.position, collision.size, Raylib.RED);
        }
    }
}
