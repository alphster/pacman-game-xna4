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
    public class GhostObject
    {
        private Texture2D sprite;
		private Texture2D spriteBlueGhost;
		private Texture2D spriteNormalGhost;
        private float spriteScale;
        private int spriteIndex;
        private int spriteTimeElapsed;
        public MovementDirection currentDirection;
        public MovementDirection futureDirection;
        private Vector2 center;
        private float distanceTraveled;
        private float currentSpeed;
        public Color color;
        private Texture2D leftEyeTexture;
        private Texture2D rightEyeTexture;
        private Vector2 leftEyeOffset;
        private Vector2 rightEyeOffset;
        private Vector2 eyeCenter;
        private float eyeRotation;
        private float eyeSpriteScale;

        public GhostAI ai;

        public Vector2 Position;
        public TileObject CurrentTile;

        private int statusTimeLeft;
        private int inbaseTimeLeft;
		private GhostStatus status;
		public GhostStatus Status
		{
			get
			{
				return status;
			}
			set
			{
                switch (value)
                {
                    case GhostStatus.Chase:
                        currentSpeed = Globals.GHOST_SPEED;
                        sprite = spriteNormalGhost;
                        status = value;
                        break;
                    case GhostStatus.Scared:
                        if (status != GhostStatus.EyesOnly && status != GhostStatus.LeavingBase)
                        {
                            currentSpeed = (currentSpeed == 0 ? 0 : Globals.GHOST_SPEED / 2.0f);
                            sprite = spriteBlueGhost;
                            statusTimeLeft = Globals.GHOST_SCARED_TIMER; // Use this to countdown when Scared ghosts reset back to normal
                            status = value;
                        }
                        break;
                       
                    case GhostStatus.EyesOnly:
                        currentSpeed = Globals.GHOST_SPEED;
                        status = value;
                        break;
                    case GhostStatus.InsideBase:
                        sprite = spriteNormalGhost;
                        currentSpeed = 0.0f;
                        status = value;
                        break;
                    case GhostStatus.LeavingBase:
                        currentSpeed = Globals.GHOST_SPEED;
                        status = value;
                        break;
                }
			}
		}

        public GhostObject(ContentManager Content, Color color, TileObject startTile, int inBaseTimeLeft)
        {
            List<Texture2D> ghostSprites = new List<Texture2D>();

            leftEyeTexture = Content.Load<Texture2D>("Sprites\\Ghost\\Eyeball");
            rightEyeTexture = Content.Load<Texture2D>("Sprites\\Ghost\\Eyeball");

			spriteNormalGhost = Content.Load<Texture2D>("Sprites\\Ghost\\ghost");
			spriteBlueGhost = Content.Load<Texture2D>("Sprites\\Ghost\\blue_ghost");

			sprite = spriteNormalGhost;

            this.CurrentTile = startTile;
            this.Position = this.CurrentTile.Position;
			this.Status = GhostStatus.InsideBase;

            this.color = color;
            this.center = new Vector2(sprite.Height / 2, sprite.Height / 2);

            this.spriteScale = (float)Globals.GHOST_IMAGE_SIZE / (float)sprite.Height;
            this.eyeSpriteScale = (float)Globals.GHOST_EYES_IMAGE_SIZE / (float)leftEyeTexture.Height;

            this.leftEyeOffset.Y = -1 * (float)(sprite.Height * .15) * spriteScale;
            this.leftEyeOffset.X = -1 * (float)(sprite.Width / 2 * .18) * spriteScale;
            this.rightEyeOffset.Y = -1 * (float)(sprite.Height * .15) * spriteScale;
            this.rightEyeOffset.X = (float)(sprite.Width / 2 * .18) * spriteScale;
            this.eyeCenter = new Vector2(leftEyeTexture.Width / 2, leftEyeTexture.Height / 2);
            this.eyeRotation = (float)Math.Atan2(0, 1);

            spriteIndex = 0;
            spriteTimeElapsed = 0;
            currentDirection = MovementDirection.Up;
            distanceTraveled = 0.0f;

            this.inbaseTimeLeft = inBaseTimeLeft;

            ai = new GhostAI(this);
        }

        public void Update(GameTime gameTime)
        {
            if (Status == GhostStatus.InsideBase)
            {
                inbaseTimeLeft -= gameTime.ElapsedGameTime.Milliseconds;
                if (inbaseTimeLeft <= 0)
                    Status = GhostStatus.LeavingBase;
            }
            if (Status == GhostStatus.Scared)
            {
                statusTimeLeft -= gameTime.ElapsedGameTime.Milliseconds;
                if (statusTimeLeft <= 0)
                    Status = (inbaseTimeLeft > 0 ? GhostStatus.InsideBase : GhostStatus.Chase);
            }
            else if (Status == GhostStatus.EyesOnly)
            {
                if (CurrentTile == MapObject.HealTile)
                {
                    Status = GhostStatus.LeavingBase;
                    currentDirection = MovementDirection.Up;
                    futureDirection = MovementDirection.Up;
                }
            }
            else if (Status == GhostStatus.LeavingBase && CurrentTile.IsGate)
            {
                Status = GhostStatus.Chase;
                currentDirection = MovementDirection.Up;
                futureDirection = MovementDirection.Up;
            }

            // Does ghost have the "opportunity" to change direction (He needs to have travelled a full tile)
            if ((distanceTraveled >= Globals.MAP_TILE_PIXEL_SIZE || distanceTraveled == 0.0f) && Status != GhostStatus.InsideBase)
            {
                distanceTraveled %= Globals.MAP_TILE_PIXEL_SIZE;

                // Change ghosts direction if he's allowed to go that way
                if (MapObject.DirectionAllowed(CurrentTile, futureDirection, Status == GhostStatus.LeavingBase || Status == GhostStatus.EyesOnly))
                {
                    ChangeDirection(futureDirection);
                }
                // See if ghost is running into a wall, stop him if he is.
                if (!MapObject.DirectionAllowed(CurrentTile, currentDirection, Status == GhostStatus.LeavingBase || Status == GhostStatus.EyesOnly))
                {
                    currentSpeed = 0.0f;
                }

                futureDirection = ai.GetFutureDirection();
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

            // Update current tile that ghost is in
            CurrentTile = MapObject.UpdateCurrentTile(Position, center, CurrentTile);
        }

        public void ChangeDirection(MovementDirection direction)
        {
            this.currentDirection = direction;
            this.Position = this.CurrentTile.Position;
            this.distanceTraveled = 0.0f;
            switch (direction)
            {
                case MovementDirection.Up:
                    eyeRotation = (float)Math.Atan2(0, -1);
                    break;
                case MovementDirection.Down:
                    eyeRotation = (float)Math.Atan2(0, 1);
                    break;
                case MovementDirection.Left:
                    eyeRotation = (float)Math.Atan2(1, 0);
                    break;
                case MovementDirection.Right:
                    eyeRotation = (float)Math.Atan2(-1, 0);
                    break;

            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteTimeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (spriteTimeElapsed > Globals.GHOST_SPRITE_ANIM_SPEED)
            {
                spriteTimeElapsed -= Globals.GHOST_SPRITE_ANIM_SPEED;
                spriteIndex = (spriteIndex + 1) % 2;
            }

            if (Status != GhostStatus.EyesOnly)
            {
                spriteBatch.Draw(sprite,
                    Position,
                    new Rectangle(spriteIndex * 290, 0, 290, 290),
                    (Status == GhostStatus.Scared ? Color.White : color),
                    0.0f,
                    center,
                    spriteScale,
                    SpriteEffects.None, 0);
            }

			if (Status != GhostStatus.Scared)
			{
				spriteBatch.Draw(leftEyeTexture,
					Position + leftEyeOffset,
					null,
					Color.White,
					eyeRotation,
					eyeCenter,
					eyeSpriteScale,
					SpriteEffects.None,
					0);

				spriteBatch.Draw(rightEyeTexture,
					Position + rightEyeOffset,
					null,
					Color.White,
					eyeRotation,
					eyeCenter,
					eyeSpriteScale,
					SpriteEffects.None,
					0);
			}
        }
    }

	public enum GhostStatus
	{
		Chase, // Chase pacman
        Scatter, // Chase corner tile
		Scared, // pacman ate a big food
		EyesOnly, // pacman killed ghost
        InsideBase, // ghost just wanders inside base
        LeavingBase // ghost is heading towards base exit 
	}

}
