using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace PacManClone.GameObjects
{
    public static class PacmanObject
    {
        private static Texture2D sprite;
        private static float spriteScale;
        private static int spriteTimeElapsed;
        private static int spriteIndex;
        public static MovementDirection currentDirection;
        private static MovementDirection futureDirection;
        private static Vector2 center;
        private static float rotation;
        private static float distanceTraveled;
        private static float currentSpeed;

        public static Vector2 Position;
        public static TileObject CurrentTile;

        public static void LoadContent(ContentManager Content, TileObject startTile)
        {
            sprite = Content.Load<Texture2D>("Sprites\\Pacman\\pacman");
            CurrentTile = startTile;
            center = new Vector2(sprite.Height / 2, sprite.Height / 2);
            spriteScale = (float)Globals.PACMAN_IMAGE_SIZE / (float)sprite.Height;
            spriteTimeElapsed = 0;
            spriteIndex = 4;
            currentDirection = MovementDirection.Left;
            futureDirection = MovementDirection.None;
            rotation = (float)Math.Atan2(1, 0);
            distanceTraveled = 0.0f;
            currentSpeed = 0.0f;
            Position = CurrentTile.Position;
        }

        public static void Update(KeyboardState keyboardState)
        {
            SetFutureDirection(keyboardState); // Looks at keyboardstate and sets the futureDirection for pacman to possibly turn
            
            // If pacman has travelled a tile, do stuff
            if (distanceTraveled >= Globals.MAP_TILE_PIXEL_SIZE || distanceTraveled == 0.0f)
            {
                // Sees if pacman ate something, updates scoreboard.
                CheckCollisionsWithMapElements(); 

                // Reset the distance traveled by subracting a tile
                distanceTraveled %= Globals.MAP_TILE_PIXEL_SIZE;
                
                // Change Pacmans direction if he's allowed to go that way
                if (MapObject.DirectionAllowed(CurrentTile, futureDirection))
                {
                    ChangeDirection(futureDirection);
                    currentSpeed = Globals.PACMAN_SPEED; // reset speed in case he stopped
                }
                // See if Pacman is running into a wall, stop him if he is.
                if (!MapObject.DirectionAllowed(CurrentTile, currentDirection))
                {
                    currentSpeed = 0.0f;
                }
            }

            switch (currentDirection)
            {
                case MovementDirection.Up:
                    Position.Y = Position.Y - currentSpeed;
                    break;
                case MovementDirection.Down:
                    Position.Y = Position.Y + currentSpeed;
                    break;
                case MovementDirection.Left:
                    Position.X = Position.X - currentSpeed;
                    break;
                case MovementDirection.Right:
                    Position.X = Position.X + currentSpeed;
                    break;
            }

            distanceTraveled += currentSpeed;

            // Update pacmans current tile
            CurrentTile = MapObject.UpdateCurrentTile(Position, center, CurrentTile);
        }

        private static void ChangeDirection(MovementDirection direction)
        {
            // First, center pacman back on the tile
            Position = CurrentTile.Position;
            distanceTraveled = 0.0f;

            currentDirection = direction;
            switch (direction)
            {
                case MovementDirection.Up:
                    rotation = (float)Math.Atan2(0, 1);
                    break;
                case MovementDirection.Down:
                    rotation = (float)Math.Atan2(0, -1);
                    break;
                case MovementDirection.Left:
                    rotation = (float)Math.Atan2(-1, 0);
                    break;
                case MovementDirection.Right:
                    rotation = (float)Math.Atan2(1, 0);
                    break;
            }
        }

        private static void CheckCollisionsWithMapElements()
        {
            switch (PacmanObject.CurrentTile.Type)
            {
                case TileType.BigDot:
                    GhostManager.SetStatus(GhostStatus.Scared);
                    //ScoreBoardObject.Score = ScoreBoardObject.Score + 2;
                    break;
                case TileType.SmallDot:
                    //ScoreBoardObject.Score++;
                    break;
            }
            PacmanObject.CurrentTile.Type = TileType.Empty; // Clear tiles as you move through them
        }

        private static void SetFutureDirection(KeyboardState keyboardState)
        {
            // Record user input - pacman remember which direction you picked,
            // but that doesn't mean he's allowed to turn immediately.
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                futureDirection = MovementDirection.Up;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                futureDirection = MovementDirection.Down;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                futureDirection = MovementDirection.Left;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                futureDirection = MovementDirection.Right;
            }
        }

        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
            spriteTimeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (spriteTimeElapsed > Globals.PACMAN_SPRITE_ANIM_SPEED)
            {
                spriteTimeElapsed -= Globals.PACMAN_SPRITE_ANIM_SPEED;
                spriteIndex--;
                if (spriteIndex < -3)
                {
                    spriteIndex = 4;
                }
            }
            

			spriteBatch.Draw(sprite,
				Position,
				new Rectangle(Math.Abs(spriteIndex) * 290, 0, 290, 290),
				Color.White,
				rotation,
				center,
				spriteScale,
				SpriteEffects.None, 0);
		}
    }
}
