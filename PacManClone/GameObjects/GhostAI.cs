using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PacManClone.GameObjects
{
    public class GhostAI
    {
        private GhostObject ghost;
        private Vector2 targetPosition;
        static Random random = new Random();

        public GhostAI(GhostObject Ghost)
        {
            this.ghost = Ghost;
        }

        public MovementDirection GetFutureDirection()
        {
            // First determine the target tile that the ghosts are trying to get to, based on a number of factors:
            if (ghost.Status == GhostStatus.EyesOnly)
            { // Ghost has been eaten, and needs to head back to base, to the "heal spot".
                targetPosition = MapObject.HealTile.Position;
            }
            else if (ghost.Status == GhostStatus.Scared)
            { // Ghosts are scared.  Generate a random tile and head to there.

                targetPosition = MapObject.TileGrid[random.Next(0, Globals.MAP_NUM_OF_VERTICAL_TILES - 1), random.Next(0, Globals.MAP_NUM_OF_HORIZONTAL_TILES - 1)].Position;
            }
            else if (ghost.Status == GhostStatus.LeavingBase)
            { // Time to leave the base, just go north. The status will change once you pass the gate.
                targetPosition = MapObject.TileGrid[0, Globals.MAP_NUM_OF_HORIZONTAL_TILES / 2].Position;
            }
            else
            { // Ghosts are chasing
                if (ghost.color == Color.Red) // Red chases pacman
                    targetPosition = PacmanObject.CurrentTile.Position;
                else if (ghost.color == Color.Pink) // Pink chases 4 tiles ahead of pacman
                {
                    targetPosition = PacmanObject.CurrentTile.Position; 
                    targetPosition.Y -= (PacmanObject.currentDirection == MovementDirection.Up ? Globals.MAP_TILE_PIXEL_SIZE * 4 : 0.0f);
                    targetPosition.Y += (PacmanObject.currentDirection == MovementDirection.Down ? Globals.MAP_TILE_PIXEL_SIZE * 4 : 0.0f);
                    targetPosition.X -= (PacmanObject.currentDirection == MovementDirection.Left ? Globals.MAP_TILE_PIXEL_SIZE * 4 : 0.0f);
                    targetPosition.X += (PacmanObject.currentDirection == MovementDirection.Right ? Globals.MAP_TILE_PIXEL_SIZE * 4 : 0.0f);
                }
                else if (ghost.color == Color.Cyan) // Get point 2 tiles in front of pacman. Then get red ghost position. 
                //Draw line from red ghost to pos in front of pacman and mirror it out across to the other side to find targetTile.
                {
                    // Get 2 tiles in front of pacman
                    targetPosition = PacmanObject.CurrentTile.Position;
                    targetPosition.Y -= (PacmanObject.currentDirection == MovementDirection.Up ? Globals.MAP_TILE_PIXEL_SIZE * 2 : 0.0f);
                    targetPosition.Y += (PacmanObject.currentDirection == MovementDirection.Down ? Globals.MAP_TILE_PIXEL_SIZE * 2 : 0.0f);
                    targetPosition.X -= (PacmanObject.currentDirection == MovementDirection.Left ? Globals.MAP_TILE_PIXEL_SIZE * 2 : 0.0f);
                    targetPosition.X += (PacmanObject.currentDirection == MovementDirection.Right ? Globals.MAP_TILE_PIXEL_SIZE * 2 : 0.0f);

                    // Mirror out distance from red ghost to position above
                    targetPosition.Y += targetPosition.Y - GhostManager.Ghosts[0].Position.Y;
                    targetPosition.X += targetPosition.X - GhostManager.Ghosts[0].Position.X;
                }
                else if (ghost.color == Color.Orange) // Orange chases pacman if 7.5xTILE_SIZE distance from pacman, otherwise, chases scatter point
                {
                    if (Helpers.GetDistanceBetween(ghost.Position, PacmanObject.Position) >= 7.5f * Globals.MAP_TILE_PIXEL_SIZE)
                        targetPosition = PacmanObject.CurrentTile.Position;
                    else
                        targetPosition = Globals.SCATTER_PT_ORANGE;
                }
            }
          

            // Look at the future tile based on the ghosts future direction
            TileObject futureTile = ghost.CurrentTile.GetFutureTile(ghost.futureDirection);

            // Consider all tiles surrounding the futureTile that ARE NOT a wall. Also, make sure you don't
            // end up reversing direction. Calculate the distance target position from each.
            float upDistance = float.MaxValue, leftDistance = float.MaxValue, downDistance = float.MaxValue, rightDistance = float.MaxValue;
            if (!futureTile.TileUp.IsWall && ghost.currentDirection != MovementDirection.Down && (ghost.Status == GhostStatus.LeavingBase || ghost.Status == GhostStatus.EyesOnly ? true : !futureTile.TileUp.IsGate)) 
                upDistance = Helpers.GetDistanceBetween(futureTile.TileUp.Position, targetPosition);
            if (!futureTile.TileLeft.IsWall && ghost.currentDirection != MovementDirection.Right && (ghost.Status == GhostStatus.LeavingBase || ghost.Status == GhostStatus.EyesOnly ? true : !futureTile.TileLeft.IsGate)) 
                leftDistance = Helpers.GetDistanceBetween(futureTile.TileLeft.Position, targetPosition);
            if (!futureTile.TileDown.IsWall && ghost.currentDirection != MovementDirection.Up && (ghost.Status == GhostStatus.LeavingBase || ghost.Status == GhostStatus.EyesOnly ? true : !futureTile.TileDown.IsGate)) 
                downDistance = Helpers.GetDistanceBetween(futureTile.TileDown.Position, targetPosition);
            if (!futureTile.TileRight.IsWall && ghost.currentDirection != MovementDirection.Left && (ghost.Status == GhostStatus.LeavingBase || ghost.Status == GhostStatus.EyesOnly ? true : !futureTile.TileRight.IsGate)) 
                rightDistance = Helpers.GetDistanceBetween(futureTile.TileRight.Position, targetPosition);

            // Move in the direction closest to pacman. 
            if (upDistance <= leftDistance && upDistance <= downDistance && upDistance <= rightDistance)
                return MovementDirection.Up;
            else if (leftDistance <= downDistance && leftDistance <= rightDistance)
                return MovementDirection.Left;
            else if (downDistance <= rightDistance)
                return MovementDirection.Down;
            else
                return MovementDirection.Right;
        }

    }
}
