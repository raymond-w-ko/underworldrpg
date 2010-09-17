using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using UnderworldEngine.Graphics;
using UnderworldEngine.Game;
using UnderworldEngine.Audio;
using System.IO;
using UnderworldEngine.Scripting;
using UnderworldEngine.IO;

namespace UnderworldEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();
        public static FileStream FileStream;
        public static StreamWriter Debug;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private MouseMenu _mouseMenu;

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

        // Renderables
        GameObjectModel gom;
        KeyboardState mLastKeyboardState;
        Grid map;
        Menu _testMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // create a debug log text file
            Game1.FileStream = new FileStream("log.txt", FileMode.Truncate);
            Game1.Debug = new StreamWriter(Game1.FileStream);

            Game1.audioManager = new AudioManager();

            // global access to rarely changing elements
            // graphics
            Game1.DefaultContent = Content;
            Game1.DefaultGraphics = graphics;

            // global access to controllers
            Game1.controller1 = new ControllerManager(PlayerIndex.One);
            Game1.kb = new KeyboardManager();
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
            Game1.Camera.SetFarPlaneDistance(10000);
            Game1.Camera.LookAt(5, 0, 5);

            // console stuff
            GameConsole.Initialize(this, "Consolas", Color.Black, Color.White, 0.8f, 10);
            Game1.console = (IGameConsole)Services.GetService(typeof(IGameConsole));

            mLastKeyboardState = Keyboard.GetState();

            // interpreter
            Game1.interpreter = new Interpreter();
            //set up surprise
            Game1.interpreter.run("run test");

            // mouse menu
            this.IsMouseVisible = true;
            Color backgroundColor = new Color(255, 240, 209);
            Color borderColor = new Color(54, 30, 26);
            Color textColor = new Color(17, 11, 0);
            _mouseMenu = new MouseMenu("WascoSans18", textColor, backgroundColor, 1.0f,
                borderColor, 200, 500);
            _mouseMenu.AddEntry(Menu.MenuEntryType.Label, "Test");
            
            _testMenu = new Menu("WascoSans18", textColor, backgroundColor, .90f,
                borderColor, DefaultGraphicsDevice.Viewport.Width - 20,
                (DefaultGraphicsDevice.Viewport.Height / 4) - 20,
                10, 10);
            _testMenu.AddEntry(Menu.MenuEntryType.Label,
                "To be, or not to be: that is the question: " +
                "Whether 'tis nobler in the mind to suffer the slings and arrows of outrageous fortune, " +
                "or to take arms against a sea of troubles, and by opposing end them? " +
                "To die, to sleep; To sleep: perchance to dream: ay, there's the rub; " +
                "For in that sleep of death what dreams may come when we have shuffled off this mortal coil, " +
                "Must give us pause: there's the respect that makes calamity of so long life;" +
                "For who would bear the whips and scorns of time,"
                );

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
            //gridMap = new GridMap(20, 20);
            gom = new GameObjectModel("Models/testmap2");
            gom.Scale(12.0f * gom.BoundingBox.FindScaleToUnitFactor());
            gom.OffsetBy(6, 0, 6);
            console.Log((12.0f * gom.BoundingBox.FindScaleToUnitFactor()).ToString());

            map = new Grid(12, 12);
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
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                this.Exit();
            }

            // TODO: Add your update logic here
            Game1.Camera.Update();

            //Console stuff
            KeyboardState kb = Keyboard.GetState();

            if (kb[Keys.Escape] == KeyState.Down)
                this.Exit();

            if (!console.IsOpen && kb[Keys.OemTilde] == KeyState.Down && mLastKeyboardState[Keys.OemTilde] == KeyState.Up)
                console.Open(Keys.OemTilde);

            mLastKeyboardState = kb;

            //controller stuff
            Game1.controller1.UpdateInput();
            Game1.kb.UpdateInput();

            _mouseMenu.Update(gameTime);
            fps.Update(gameTime);

            //Game1.DefaultGraphicsDevice.Viewport.Project();
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
            // Draw 3D here
            //gridMap.Draw();
            gom.Draw();
            map.Draw();

            
            
            // Draw 2D Sprites Here
            spriteBatch.Begin();
            // Queue Sprites Here
            fps.Draw();
            spriteBatch.End();
            spriteBatch.ResetFor3d();
            // Mouse Menu
            _mouseMenu.Draw();
            _testMenu.Draw();
            base.Draw(gameTime);
        }
    }
}
