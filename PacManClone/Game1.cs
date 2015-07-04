using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using PacManClone.GameObjects;

namespace PacManClone
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Matrix WindowScale;
        KeyboardState previousKeyboardState = Keyboard.GetState();

        bool pacmanKilled = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = Convert.ToInt32(Globals.WINDOW_HEIGHT * Globals.WINDOW_SCALE);
            graphics.PreferredBackBufferWidth = Convert.ToInt32(Globals.WINDOW_WIDTH * Globals.WINDOW_SCALE);
            Content.RootDirectory = "Content";
            WindowScale = Matrix.CreateScale(Globals.WINDOW_SCALE);
            //Window.AllowUserResizing = true;
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            // Make changes to handle the new window size.
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Flag that causes the game to restart
            pacmanKilled = false;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Initialize and load map and scoreboard
            MapObject.LoadContent(Content);
            ScoreBoardObject.LoadContent(Content);

            // Load pacman and set starting position
            PacmanObject.LoadContent(Content, MapObject.GetTile('S'));

            // Initialize the ghosts
            GhostManager.LoadContent(Content);


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyboardState = Keyboard.GetState();

            // Update Pacman position and check for collisions with map elements
            PacmanObject.Update(keyboardState);

            // Update Ghosts position and checks for collisions with pacman
            GhostManager.Update(gameTime);

            // Check for collisions with pacman
            GhostManager.CheckForCollisions(PacmanObject.CurrentTile, out pacmanKilled);
            if (pacmanKilled) LoadContent();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, WindowScale);

            MapObject.Draw(spriteBatch);
			ScoreBoardObject.Draw(spriteBatch);
            PacmanObject.Draw(spriteBatch, gameTime);
            GhostManager.Draw(spriteBatch, gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
