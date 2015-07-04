using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PacManClone.GameObjects
{
    public static class MapObject
    {
        public static TileObject[,] TileGrid; // used to draw sprites
        public static TileObject HealTile;

        public static void LoadContent(ContentManager Content)
        {
            // Initialize Tile and Collision Grid
            TileGrid = new TileObject[Globals.MAP_NUM_OF_VERTICAL_TILES, Globals.MAP_NUM_OF_HORIZONTAL_TILES];

            // Read ASCII map. Populate Tile and Collision grid
            TextReader reader = new StreamReader("map.txt", Encoding.ASCII);
            string inputString = reader.ReadToEnd().Replace("\r", "").Replace("\n", "");
             
            char input;
            int count = 0;
            for (int row = 0; row < Globals.MAP_NUM_OF_VERTICAL_TILES; row++)
            {
                for (int col = 0; col < Globals.MAP_NUM_OF_HORIZONTAL_TILES; col++)
                {
                    input = inputString[count++];
                    TileGrid[row, col] = new TileObject(Content, TileGrid, row, col, input);
                }
            }

            HealTile = GetTile('H');
        }

        internal static void Draw(SpriteBatch spriteBatch)
        {
			for (int row = 0; row < Globals.MAP_NUM_OF_VERTICAL_TILES; row++)
			{
				for (int col = 0; col < Globals.MAP_NUM_OF_HORIZONTAL_TILES; col++)
				{
					spriteBatch.Draw(TileGrid[row, col].Sprite,
						TileGrid[row, col].Position,
						null,
						Color.White,
						TileGrid[row, col].Rotation,
						TileGrid[row, col].Center,
						TileGrid[row, col].Scale,
						SpriteEffects.None,
						0);
				}
			}
        }

        public static TileObject GetTile(char input)
        {
            foreach (TileObject t in TileGrid)
            {
                if (t.StartingInputChar == input)
                    return t;
            }
            return null;
        }

        public static bool DirectionAllowed(TileObject CurrentTile, MovementDirection direction)
        {
            return DirectionAllowed(CurrentTile, direction, false);
        }

        public static bool DirectionAllowed(TileObject CurrentTile, MovementDirection direction, bool walkThroughGate)
        {
            // Check to see if we can move in the direction specified.
            try
            {
                switch (direction)
                {
                    case MovementDirection.Up:
                        return !(CurrentTile.TileUp.IsWall || (walkThroughGate ? false : CurrentTile.TileUp.IsGate));
                    case MovementDirection.Down:
                        return !(CurrentTile.TileDown.IsWall || (walkThroughGate ? false : CurrentTile.TileDown.IsGate));
                    case MovementDirection.Left:
                        return !(CurrentTile.TileLeft.IsWall || (walkThroughGate ? false : CurrentTile.TileLeft.IsGate));
                    case MovementDirection.Right:
                        return !(CurrentTile.TileRight.IsWall || (walkThroughGate ? false : CurrentTile.TileRight.IsGate));
                }
            }
            catch (Exception) // If we are here, then pacman or ghost is trying to warp to the other side of the map
            {
                return true;
            }
            return false;	
        }

        internal static TileObject UpdateCurrentTile(Vector2 Position, Vector2 Center, TileObject CurrentTile)
        {
            float diffx = CurrentTile.Position.X - Position.X;
            float diffy = CurrentTile.Position.Y - Position.Y;

            if (diffx > (float)Globals.MAP_TILE_PIXEL_SIZE / 2.0f)
                CurrentTile = CurrentTile.TileLeft;
            else if (diffx < -1.0f * (float)Globals.MAP_TILE_PIXEL_SIZE / 2.0f)
                CurrentTile = CurrentTile.TileRight;
            else if (diffy > (float)Globals.MAP_TILE_PIXEL_SIZE / 2.0f)
                CurrentTile = CurrentTile.TileUp;
            else if (diffy < -1.0f * (float)Globals.MAP_TILE_PIXEL_SIZE / 2.0f)
                CurrentTile = CurrentTile.TileDown;

            return CurrentTile;
        }
	}

    public enum TileType
    {
        VerticalWall,
        HorizontalWall,
        TopLeftCorner,
        TopRightCorner,
        BottomLeftCorner,
        BottomRightCorner,
        SmallDot,
        BigDot,
        Cherry,
        Gate,
        Empty,
    };
}
