using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using UnderworldEngine.Graphics;
using UnderworldEngine.Audio;
using UnderworldEngine.Scripting;
using UnderworldEngine.IO;
using UnderworldEngine.GameState;

namespace UnderworldEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private static FileStream _fileStream;
        public static StreamWriter Debug;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        // Graphics Globals
        internal static Camera Camera = null;
        internal static GraphicsDeviceManager DefaultGraphics = null;
        internal static GraphicsDevice DefaultGraphicsDevice = null;
        internal static SpriteBatch DefaultSpriteBatch = null;
        // Audio Globals
        internal static AudioManager audioManager;
        // Content Globals
        internal static ContentManager DefaultContent = null;
        // Components Globals
        internal static Interpreter interpreter;
        internal static ControllerManager controller1;
        internal static KeyboardManager kb;
        internal static IGameConsole console;
        internal static FpsCounter fps;
        internal static ScreenManager screenManager;
        internal static bool exit = false;

        public Game1()
        {
            this.IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // create a debug log text file
            Game1._fileStream = new FileStream("log.txt", FileMode.Create);
            Game1.Debug = new StreamWriter(Game1._fileStream);

            Game1.audioManager = new AudioManager();

            // global access to rarely changing elements
            // graphics
            Game1.DefaultContent = Content;
            Game1.DefaultGraphics = graphics;

            // global access to controllers
            Game1.controller1 = new ControllerManager(PlayerIndex.One);
            Game1.kb = new KeyboardManager();

            //global access to Screen Manager
            Game1.screenManager = new ScreenManager();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Not initialized yet in constructor
            Game1.DefaultGraphicsDevice = GraphicsDevice;

            // global access to camera
            Game1.Camera = new Camera();

            // console stuff
            GameConsole.Initialize(this, "WascoSans10", Color.Black, Color.White, 0.8f, 10);
            Game1.console = (IGameConsole)Services.GetService(typeof(IGameConsole));

            // interpreter
            Game1.interpreter = new Interpreter();
            Game1.interpreter.run("run startup");

            /*
            _testDialogue = new DialogueManager(
                DefaultGraphicsDevice.Viewport.Width - 40, (DefaultGraphicsDevice.Viewport.Height / 4) - 20,
                20, 530);
            _testDialogue.AddLines(
                "To be, or not to be: that is the question: " +
                "Whether 'tis nobler in the mind to suffer the slings and arrows of outrageous fortune, " +
                "or to take arms against a sea of troubles, and by opposing end them? " +
                "To die, to sleep; To sleep: perchance to dream: ay, there's the rub; " +
                "For in that sleep of death what dreams may come when we have shuffled off this mortal coil, " +
                "Must give us pause: there's the respect that makes calamity of so long life; " +
                "For who would bear the whips and scorns of time, "
                );
            */

            //add screens to screenManager
            Game1.screenManager.AddScreen("blank", new BlankScreen());

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
            Game1.DefaultSpriteBatch = spriteBatch;
            
            // FPS Counter
            fps = new FpsCounter();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Game1.Debug.Close();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>\
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (exit)
                this.Exit();

            // TODO: Add your update logic here
            Game1.Camera.Update(gameTime);

            //controller stuff
            Game1.controller1.UpdateInput();
            Game1.kb.UpdateInput();

            Game1.screenManager.Update(gameTime);

            fps.Update(gameTime);

            //update interpreter
            Game1.interpreter.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear the Display
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            Game1.screenManager.Draw(gameTime);
            
            // Draw 2D Sprites Here
            spriteBatch.Begin();

            // Queue Sprites Here
            fps.Draw();
            spriteBatch.End();

            // End 2D Draw
            spriteBatch.ResetFor3d();

            base.Draw(gameTime);
        }
    }
}
