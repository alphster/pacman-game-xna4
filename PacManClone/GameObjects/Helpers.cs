using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PacManClone.GameObjects
{
    public static class Helpers
    {
        public static Vector2 WrapObjectAtScreenBorder(Vector2 position)
        {
            if (position.X > Globals.WINDOW_WIDTH + 20)
                position.X = -20;
            else if (position.X < -20)
                position.X = Globals.WINDOW_WIDTH + 20;

            if (position.Y > Globals.WINDOW_HEIGHT + 20)
                position.Y = -20;
            else if (position.Y < -20)
                position.Y = Globals.WINDOW_HEIGHT + 20;

            return position;
        }

        public static float GetDistanceBetween(Vector2 p, Vector2 q)
        {
            double a = p.X - q.X;
            double b = p.Y - q.Y;
            double distance = Math.Sqrt(a * a + b * b);
            return (float)distance;
        }
    }

    public enum MovementDirection
    {
        Up,
        Down,
        Left,
        Right,
        None
    }
}
