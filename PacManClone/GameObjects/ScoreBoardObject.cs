using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PacManClone.GameObjects
{
	public static class ScoreBoardObject
	{
        public static int Score = 0;
        private static Vector2 ScoreLocation = new Vector2(Globals.SCORE_LOCATION_X, Globals.SCORE_LOCATION_Y);
        static SpriteFont font;

		public static void LoadContent(ContentManager Content)
		{
			font = Content.Load<SpriteFont>("DefaultFont");
		}

		public static void Draw(SpriteBatch spritebatch)
		{
			spritebatch.DrawString(font, "Score: " + Score.ToString(), ScoreLocation, Color.White);
		}
	}

}
