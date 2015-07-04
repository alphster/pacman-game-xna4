using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PacManClone
{
    public static class Globals
    {
        // Set the scale for the actual window (sets visible resolution)
        public const float WINDOW_SCALE = 0.625f;
        //public const float WINDOW_SCALE = 0.3f;

        // Some details about the map and the tiles in the map
        public static int MAP_NUM_OF_HORIZONTAL_TILES = 28;
        public static int MAP_NUM_OF_VERTICAL_TILES = 36;
        public static int MAP_X_OFFSET = Convert.ToInt32(MAP_TILE_PIXEL_SIZE / 2);
        public static int MAP_Y_OFFSET = Convert.ToInt32(MAP_TILE_PIXEL_SIZE / 2);

        // Window Resolution
        public const int MAP_TILE_PIXEL_SIZE = 32;
        public static int WINDOW_HEIGHT = MAP_TILE_PIXEL_SIZE * MAP_NUM_OF_VERTICAL_TILES;
        public static int WINDOW_WIDTH = MAP_TILE_PIXEL_SIZE * MAP_NUM_OF_HORIZONTAL_TILES;

        // Scoreboard
        public static int SCORE_LOCATION_X = MAP_TILE_PIXEL_SIZE / 2;
        public static int SCORE_LOCATION_Y = MAP_TILE_PIXEL_SIZE * 30;

        //Sizes of sprites (in pixels)
        public static int PACMAN_IMAGE_SIZE = Convert.ToInt32(MAP_TILE_PIXEL_SIZE * 1.5);
        public static int GHOST_IMAGE_SIZE = Convert.ToInt32(MAP_TILE_PIXEL_SIZE * 1.5);
        public static int GHOST_EYES_IMAGE_SIZE = Convert.ToInt32(MAP_TILE_PIXEL_SIZE / 2);

        // Animation speeds (milliseconds per image)
        public static int PACMAN_SPRITE_ANIM_SPEED = 25; 
        public static int GHOST_SPRITE_ANIM_SPEED = 250;

        // movement speed
        public const float PACMAN_SPEED = 4f;
        public const float GHOST_SPEED = 4f;

        // Timers
        public static int GHOST_SCARED_TIMER = 5000;

        // Scatter points
        public static Vector2 SCATTER_PT_RED = new Vector2(Globals.MAP_TILE_PIXEL_SIZE * 25, Globals.MAP_TILE_PIXEL_SIZE * -3);
        public static Vector2 SCATTER_PT_PINK = new Vector2(Globals.MAP_TILE_PIXEL_SIZE * 2, Globals.MAP_TILE_PIXEL_SIZE * -3);
        public static Vector2 SCATTER_PT_CYAN = new Vector2(Globals.MAP_TILE_PIXEL_SIZE * 28, Globals.MAP_TILE_PIXEL_SIZE * 35);
        public static Vector2 SCATTER_PT_ORANGE = new Vector2(Globals.MAP_TILE_PIXEL_SIZE * 0, Globals.MAP_TILE_PIXEL_SIZE * 35);

        // Time before ghosts leave the base
        public static int RED_TIME_INBASE = 0;
        public static int PINK_TIME_INBASE = 3000;
        public static int CYAN_TIME_INBASE = 6000;
        public static int ORANGE_TIME_INBASE = 9000;

        // Ghost AI
        // We will roll random numbers up to this number.
        public const int AI_ROLL_NUMBER = 1000;
        // Percentage chance that ghost will change direction to chase pacman.
        public const float CHANCE_TO_ATTACK = .1f;
        // Percentage chance that ghost will change randomly to any direction.
        public const float CHANCE_TO_WANDER = .01f;

        public static Random Random = new Random((int)DateTime.Now.Ticks);
    }
}
