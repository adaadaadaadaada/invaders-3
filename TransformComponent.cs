using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace invaders
{
    /// <summary>
    /// Luo paikan, nopeuden, ja suunnan joita objektit voi käyttää
    /// </summary>
    internal class TransformComponent
    {
        public Vector2 position;
        public Vector2 speed;
        public Vector2 direction;
        public TransformComponent(Vector2 position, Vector2 direction, float speed)
        {
            this.position = position;
            this.speed = new Vector2(speed, speed);
            this.direction = direction;
        }
    }
}
