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

using UnderworldEngine.GraphicsEngine;
using UnderworldEngine.Game;
using UnderworldEngine.Audio;
using System.IO;

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
        internal static AudioManager audioManager;
        BasicEffectManager basicEffectManager;

        GameObjectModel ground;
        GameObjectModel level2;
        GameObjectModel ship;
        GameObjectModel interceptor;

        UnderworldEngine.Game.Quad quad;
        VertexDeclaration quadVertexDecl;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Game1.FileStream = new FileStream("log.txt", FileMode.OpenOrCreate);
            Game1.Debug = new StreamWriter(Game1.FileStream);

            

            Game1.audioManager = new AudioManager();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //AllocConsole();
            Console.WriteLine("Hello, World!");
            Console.WriteLine(Vector3.Backward);

            Game1.Camera = new Camera(GraphicsDevice.Viewport);
            Game1.Camera.CalculateAspectRatio(GraphicsDevice.Viewport);
            Game1.Camera.MoveTo(10, 10, 10);
            Game1.Camera.LookAt(0, 0, 0);
            Game1.Camera.SetFovDegrees(45);
            Game1.Camera.SetFarPlaneDistance(1000);

            Game1.DefaultContent = Content;

            basicEffectManager = new BasicEffectManager(GraphicsDevice);

            quad = new UnderworldEngine.Game.Quad(Vector3.Zero, Vector3.Up, Vector3.Forward, 10, 10);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        Texture2D texture;
        BasicEffect quadEffect;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ground = new GameObjectModel("Models/ground");
            ground.IsVisible = false;

            level2 = new GameObjectModel("Models/ground");
            level2.Position = new Vector3(0, 50, 0);
            level2.IsVisible = false;

            ship = new GameObjectModel("Models/ship");
            ship.Position = new Vector3(0, 20, 0);
            ship.Scale(.05f);
            ship.ApplyRotationY(270.0f - 27.5f);
            ship.OffsetBy(0, -5, 0);
            ship.IsVisible = false;

            interceptor = new GameObjectModel("Models/ship");
            interceptor.Position = new Vector3(100, 20, 100);
            interceptor.Scale(.005f);
            interceptor.ApplyRotationY(270.0f - 27.5f);
            interceptor.IsVisible = false;

            texture = Game1.DefaultContent.Load<Texture2D>("Textures/ground");
            quadEffect = new BasicEffect(graphics.GraphicsDevice, null);
            quadEffect.EnableDefaultLighting();
            quadEffect.World = Matrix.Identity;
            quadEffect.View = Game1.Camera.ViewMatrix;
            quadEffect.Projection = Game1.Camera.ProjectionMatrix;
            quadEffect.TextureEnabled = true;
            quadEffect.Texture = texture;

            quadVertexDecl = new VertexDeclaration(graphics.GraphicsDevice,
                VertexPositionNormalTexture.VertexElements);

            //Audio loading
            Game1.audioManager.AddSoundLibrary("Music");
            Game1.audioManager.PlaySound("Hello");
            Game1.audioManager.Volume = 1.0f;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            FreeConsole();
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
            ground.Draw();

            //level2.Draw();

            ship.Draw();
            interceptor.Draw();

            GraphicsDevice.VertexDeclaration = quadVertexDecl;
            quadEffect.Begin();
            foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes) {
                pass.Begin();

                GraphicsDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList,
                    quad.Vertices, 0, 4,
                    quad.Indexes, 0, 2);

                pass.End();
            }

            quadEffect.End();


            base.Draw(gameTime);
        }
    }
}
