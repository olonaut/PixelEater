using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PixelEater
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //Basic Stuff
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random rng = new Random();

        //Values
        private Vector2 playersize = new Vector2(40, 40);
        private Color playercolor = Color.Red;
        private Vector2 pixelsize = new Vector2(30, 30);
        private Color pixelcolor = Color.Black;

        //Textures
        Texture2D player;
        Texture2D pixel;

        //Positions
        Vector2 playerposition;
        Vector2 pixelposition;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            playerposition = new Vector2(graphics.GraphicsDevice.Viewport.Width/2,graphics.GraphicsDevice.Viewport.Height/2);
            pixelposition = new Vector2(rng.Next(0,graphics.GraphicsDevice.Viewport.Width), rng.Next(0, graphics.GraphicsDevice.Viewport.Height));


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Textures
            player = new Texture2D(graphics.GraphicsDevice, (int)playersize.X, (int)playersize.Y);
            Color[] playerdata = new Color[(int)playersize.X * (int)playersize.Y];
            for (int i = 0; i < playerdata.Length; i++) playerdata[i] = playercolor;
            player.SetData(playerdata);

            pixel = new Texture2D(graphics.GraphicsDevice, (int)pixelsize.X, (int)pixelsize.Y);
            Color[] pixeldata = new Color[(int)pixelsize.X * (int)pixelsize.Y];
            for (int i = 0; i < pixeldata.Length; i++) pixeldata[i] = pixelcolor;
            pixel.SetData(pixeldata);


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(player, playerposition);
            spriteBatch.Draw(pixel, pixelposition);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
