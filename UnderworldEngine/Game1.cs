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

        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;

        internal static Camera Camera;
        internal static ContentManager DefaultContent;
        internal static GraphicsDeviceManager DefaultGraphics;
        internal static GraphicsDevice DefaultGraphicsDevice;
        internal static AudioManager audioManager;
        internal static Interpreter interpreter;
        internal static ControllerManager controller1;
        internal static IGameConsole console;

        GridMap gridMap;
        GameObjectModel gom;
        KeyboardState mLastKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Game1.FileStream = new FileStream("log.txt", FileMode.Truncate);
            Game1.Debug = new StreamWriter(Game1.FileStream);

            Game1.audioManager = new AudioManager();

            // global access to rarely changing elements
            Game1.DefaultContent = Content;
            Game1.DefaultGraphics = graphics;

            // global access to controllers
            Game1.controller1 = new ControllerManager(PlayerIndex.One);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Game1.DefaultGraphicsDevice = GraphicsDevice;
            graphics.SynchronizeWithVerticalRetrace = false;

            // global access to camera
            Game1.Camera = new Camera();
            Game1.Camera.SetFarPlaneDistance(10000);
            Game1.Camera.LookAt(5, 0, 5);

            // console stuff
            GameConsole.Initialize(this, "Consolas", Color.Black, Color.White, 0.8f, 10);
            Game1.console = (IGameConsole)Services.GetService(typeof(IGameConsole));

            console.BindCommandHandler("hello", delegate(GameTime time, string args)
            {
                console.Log(args);
            }
            , ' ');

            mLastKeyboardState = Keyboard.GetState();

            // interpreter
            Game1.interpreter = new Interpreter();
            //set up surprise
            interpreter.run("run test.rs");

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

            // TODO: use this.Content to load your game content here
            //gridMap = new GridMap(20, 20);
            gom = new GameObjectModel("Models/testmap2");
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
            FpsCounter(gameTime);

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

            base.Update(gameTime);
        }

        float deltaFPSTime = 0; //time since last FPS update in seconds
        int numOfFrames = 0;
        double FPS = 0;  
        private void FpsCounter(GameTime gameTime)
        {
            // Show FPS
            // The time since Update was called last
            float elapsed = (float)gameTime.ElapsedRealTime.TotalSeconds;

            float fps = 1 / elapsed;
            deltaFPSTime += elapsed;
            if (deltaFPSTime > 1) {

                Window.Title = "I am running at  <" + fps.ToString() + "> FPS";
                deltaFPSTime -= 1;
            };
            if (gameTime.TotalGameTime.Milliseconds == 0) {
                FPS = numOfFrames;
                numOfFrames = 0;
            }  
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
            //gridMap.Draw();
            gom.Draw();

            VertexDeclaration vd = GraphicsDevice.VertexDeclaration;

            SpriteFont sf = Game1.DefaultContent.Load<SpriteFont>("Consolas");
            numOfFrames++;
            /*
            spriteBatch.Begin();
            spriteBatch.DrawString(sf, FPS.ToString(), new Vector2(0, 0), Color.Yellow);
            spriteBatch.End();
             */

            GraphicsDevice.VertexDeclaration = vd;

            base.Draw(gameTime);
        }
    }
}
