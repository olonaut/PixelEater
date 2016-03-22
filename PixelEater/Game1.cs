using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
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
        Random rng;
        KeyboardState keystate;
        GamePadState padstate;


        //Control locks
        bool dpadlock, wasd_keylock, rtsticklock;

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

        //"Featues"
        int score;

        //Fonts
        SpriteFont font;

        //Sounds
        SoundEffect eat;

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
            rng = new Random();

            playerposition = new Vector2(graphics.GraphicsDevice.Viewport.Width/2,graphics.GraphicsDevice.Viewport.Height/2);
            pixelposition = new Vector2(rng.Next(0,graphics.GraphicsDevice.Viewport.Width - (int)pixelsize.X), rng.Next(0, graphics.GraphicsDevice.Viewport.Height - (int)pixelsize.Y));

            score = 0;

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

            font = Content.Load<SpriteFont>("score");

            eat = Content.Load<SoundEffect>("coin");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            pixel.Dispose();
            player.Dispose();
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

            //Getting keystate & padstate
            keystate = Keyboard.GetState();
            padstate = GamePad.GetState(PlayerIndex.One);


            //TODO block Controlls of all other devices when one device is pressed.
            //Controlls (Keyboard)
            if(dpadlock == false && rtsticklock == false) { 
                if (keystate.IsKeyDown(Keys.W) || keystate.IsKeyDown(Keys.Up)) playerposition.Y -= 10;
                if (keystate.IsKeyDown(Keys.S) || keystate.IsKeyDown(Keys.Down)) playerposition.Y += 10;
                if (keystate.IsKeyDown(Keys.A) || keystate.IsKeyDown(Keys.Left)) playerposition.X -= 10;
                if (keystate.IsKeyDown(Keys.D) || keystate.IsKeyDown(Keys.Right)) playerposition.X += 10;
                //lock
                if (keystate.IsKeyDown(Keys.W) || keystate.IsKeyDown(Keys.Up) || keystate.IsKeyDown(Keys.S) || keystate.IsKeyDown(Keys.Down) || keystate.IsKeyDown(Keys.A) || keystate.IsKeyDown(Keys.Left) || keystate.IsKeyDown(Keys.D) || keystate.IsKeyDown(Keys.Right)) wasd_keylock = true ;
                else wasd_keylock = false;
            }

            //Controlls (Gamepad)
            //DPAD
            if(wasd_keylock == false && rtsticklock == false) { 
                if (padstate.DPad.Up == ButtonState.Pressed) playerposition.Y -= 10;
                if (padstate.DPad.Down == ButtonState.Pressed) playerposition.Y += 10;
                if (padstate.DPad.Left == ButtonState.Pressed) playerposition.X -= 10;
                if (padstate.DPad.Right == ButtonState.Pressed) playerposition.X += 10;
                //lock
                if (padstate.DPad.Up == ButtonState.Pressed || padstate.DPad.Down == ButtonState.Pressed || padstate.DPad.Left == ButtonState.Pressed || padstate.DPad.Right == ButtonState.Pressed) dpadlock = true;
                else dpadlock = false;
            }
            //Right Thumb-Stick
            if(wasd_keylock == false && dpadlock == false) { 
                playerposition.X += (int)(padstate.ThumbSticks.Right.X * 10);
                playerposition.Y -= (int)(padstate.ThumbSticks.Right.Y * 10);
                //lock
                if ((int)(padstate.ThumbSticks.Right.X * 10) != 0 || (int)(padstate.ThumbSticks.Right.Y * 10) != 0) rtsticklock = true;
                else rtsticklock = false;
            }

            //Collisions (Walls)
            if (playerposition.X < 0) playerposition.X = 0;
            if (playerposition.X + playersize.X > graphics.GraphicsDevice.Viewport.Width) playerposition.X = graphics.GraphicsDevice.Viewport.Width - playersize.X;
            if (playerposition.Y < 0) playerposition.Y = 0;
            if (playerposition.Y + playersize.Y > graphics.GraphicsDevice.Viewport.Height) playerposition.Y = graphics.GraphicsDevice.Viewport.Height - playersize.Y;

            //If player Touches Pixel
            if (playerposition.X + playersize.X >= pixelposition.X && playerposition.X <= pixelposition.X + pixelsize.X && playerposition.Y + playersize.Y >= pixelposition.Y && playerposition.Y <= pixelposition.Y + pixelsize.Y) pixelreset();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Draw blue Screen
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw stuff on screen ontop of blue screen.
            spriteBatch.Begin(SpriteSortMode.Deferred);
            spriteBatch.Draw(player, playerposition);
            spriteBatch.Draw(pixel, pixelposition);
            spriteBatch.DrawString(font, "score: " + score, new Vector2(0, graphics.GraphicsDevice.Viewport.Height - font.MeasureString("score: " + score).Y), Color.Black);
            spriteBatch.End();
            
            //Draw. Just Draw.
            base.Draw(gameTime);
        }

        public void pixelreset()
        {
            score += 1;
            pixelposition = new Vector2(rng.Next(0, graphics.GraphicsDevice.Viewport.Width-(int)pixelsize.X), rng.Next(0, graphics.GraphicsDevice.Viewport.Height-(int)pixelsize.Y));
            eat.Play();
        }

    }
}
