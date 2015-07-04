using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PacManClone.GameObjects
{
    public static class GhostManager
    {
        public static List<GhostObject> Ghosts;

        public static void LoadContent(ContentManager Content)
        {
            Ghosts = new List<GhostObject>();
            Ghosts.Add(new GhostObject(Content, Color.Red, MapObject.GetTile('1'), Globals.RED_TIME_INBASE));    // TODO Later on, add extra parameter here defining ghost behaviour
            Ghosts.Add(new GhostObject(Content, Color.Pink, MapObject.GetTile('2'), Globals.PINK_TIME_INBASE));
            Ghosts.Add(new GhostObject(Content, Color.Cyan, MapObject.GetTile('3'), Globals.CYAN_TIME_INBASE));
            Ghosts.Add(new GhostObject(Content, Color.Orange, MapObject.GetTile('4'), Globals.ORANGE_TIME_INBASE));
        }

        public static void Update(GameTime gameTime)
        {
            foreach (GhostObject ghost in Ghosts)
                ghost.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (GhostObject ghost in Ghosts)
                ghost.Draw(spriteBatch, gameTime);
        }

        public static void SetStatus(GhostStatus status)
        {
            foreach (GhostObject ghost in Ghosts)
            {
                ghost.Status = status;
            }
        }

        public static void CheckForCollisions(TileObject tile, out bool pacmanKilled)
        {
            pacmanKilled = false;

            foreach (GhostObject ghost in Ghosts)
            {
                if (tile == ghost.CurrentTile)
                {
                    if (ghost.Status == GhostStatus.Scared)
                    {
                        ghost.Status = GhostStatus.EyesOnly;
                    }
                    else if (ghost.Status != GhostStatus.EyesOnly)
                    {
                        pacmanKilled = true;
                    }
                }
            }
        }
    }
}
