using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PacManClone.GameObjects
{
    public class TileObject
    {
        ContentManager Content;

        public Texture2D Sprite;
        private TileObject[,] tileGrid;
        public char StartingInputChar;

        public float Rotation;
        public Vector2 Position;
        public Vector2 Center;
        public float Scale;
        public bool IsWall = false;
        public bool IsGate = false;

        public TileObject TileLeft { get { return tileGrid[Row, (Col == 0 ? Globals.MAP_NUM_OF_HORIZONTAL_TILES - 1 : Col - 1)]; } }
        public TileObject TileRight { get { return tileGrid[Row, (Col == Globals.MAP_NUM_OF_HORIZONTAL_TILES - 1 ? 0 : Col + 1)]; } }
        public TileObject TileUp { get { return tileGrid[Row - 1, Col]; } }
        public TileObject TileDown { get { return tileGrid[Row + 1, Col]; } }

        public int Row;
        public int Col;

        private TileType type;
        public TileType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                switch (value)
                {
                    case TileType.BigDot:
                        IsWall = false;
                        IsGate = false;
                        Sprite = Content.Load<Texture2D>("Sprites\\Map\\large_dot");
                        Rotation = 0.0f;
                        break;
                    case TileType.BottomLeftCorner:
                        Sprite = Content.Load<Texture2D>("Sprites\\Map\\wall_corner");
                        IsWall = true;
                        IsGate = false;
                        Rotation = (float)Math.Atan2(-1, 0);
                        break;
                    case TileType.BottomRightCorner:
                        Sprite = Content.Load<Texture2D>("Sprites\\Map\\wall_corner");
                        IsWall = true;
                        IsGate = false;
                        Rotation = (float)Math.Atan2(0, -1);
                        break;
                    /*case TileType.Cherry:
                        Sprite = Content.Load<Texture2D>("Sprites\\Map\\small_dot");
                        IsWall = false;
                        IsGate = false;
                        Rotation = 0.0f;
                        break;*/
                    case TileType.Empty:
                        Sprite = Content.Load<Texture2D>("Sprites\\Map\\empty");
                        IsWall = false;
                        IsGate = false;
                        Rotation = 0.0f;
                        break;
                    case TileType.Gate:
                        Sprite = Content.Load<Texture2D>("Sprites\\Map\\small_dot");
                        IsWall = false;
                        IsGate = true;
                        Rotation = 0.0f;
                        break;
                    case TileType.HorizontalWall:
                        Sprite = Content.Load<Texture2D>("Sprites\\Map\\wall_straight");
                        IsWall = true;
                        IsGate = false;
                        Rotation = 0.0f;
                        break;
                    case TileType.SmallDot:
                        Sprite = Content.Load<Texture2D>("Sprites\\Map\\small_dot");
                        IsWall = false;
                        IsGate = false;
                        Rotation = 0.0f;
                        break;
                    case TileType.TopLeftCorner:
                        Sprite = Content.Load<Texture2D>("Sprites\\Map\\wall_corner");
                        IsWall = true;
                        IsGate = false;
                        Rotation = 0.0f;
                        break;
                    case TileType.TopRightCorner:
                        Sprite = Content.Load<Texture2D>("Sprites\\Map\\wall_corner");
                        IsWall = true;
                        IsGate = false;
                        Rotation = (float)Math.Atan2(1, 0);
                        break;
                    case TileType.VerticalWall:
                        Sprite = Content.Load<Texture2D>("Sprites\\Map\\wall_straight");
                        IsWall = true;
                        IsGate = false;
                        Rotation = (float)Math.Atan2(1, 0);
                        break;
                }
            }
        }

        public TileObject(ContentManager Content, TileObject[,] tileGrid, int row, int col, char inputChar)
        {
            this.tileGrid = tileGrid;
            this.StartingInputChar = inputChar;
            Row = row;
            Col = col;
            Position.X = col * Globals.MAP_TILE_PIXEL_SIZE + Globals.MAP_X_OFFSET;
            Position.Y = row * Globals.MAP_TILE_PIXEL_SIZE + Globals.MAP_Y_OFFSET;
            this.Content = Content;

            switch (inputChar)
            {
                case 'o':
                    Type = TileType.BigDot;
                    break;
                case '<':
                    Type = TileType.BottomLeftCorner;
                    break;
                case '>':
                    Type = TileType.BottomRightCorner;
                    break;
                /*case '.':
                    Type = TileType.Cherry;
                    break;*/
                case 'g':
                    Type = TileType.Gate;
                    break;
                case '-':
                    Type = TileType.HorizontalWall;
                    break;
                case '.':
                    Type = TileType.SmallDot;
                    break;
                case '/':
                    Type = TileType.TopLeftCorner;
                    break;
                case '\\':
                    Type = TileType.TopRightCorner;
                    break;
                case '|':
                    Type = TileType.VerticalWall;
                    break;
                default:
                    Type = TileType.Empty;
                    break;
            }

            Scale = (float)Globals.MAP_TILE_PIXEL_SIZE / (float)Sprite.Height;
            Center = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
        }

        public TileObject GetFutureTile(MovementDirection direction)
        {
            if (direction == MovementDirection.Up)
                return TileUp;
            else if (direction == MovementDirection.Left)
                return TileLeft;
            else if (direction == MovementDirection.Down)
                return TileDown;
            else if (direction == MovementDirection.Right)
                return TileRight;
            return null;
        }
    }
}
