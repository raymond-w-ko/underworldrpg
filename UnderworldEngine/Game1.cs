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

using UnderworldEngine.GraphicsEngine;
using UnderworldEngine.Game;
using UnderworldEngine.Audio;
namespace UnderworldEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Game1.Camera = new Camera();
            Game1.DefaultContent = Content;
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
            Game1.Camera.CalculateAspectRatio(GraphicsDevice.Viewport);
            Game1.Camera.MoveTo(200, 65, 200);
            Game1.Camera.LookAt(0, 0, 0);
            Game1.Camera.SetFovDegrees(45);
            Game1.Camera.SetFarPlaneDistance(1000);
            basicEffectManager = new BasicEffectManager(GraphicsDevice);
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
            ground = new GameObjectModel("Models/ground");

            level2 = new GameObjectModel("Models/ground");
            level2.Position = new Vector3(0, 50, 0);

            ship = new GameObjectModel("Models/ship");
            ship.Position = new Vector3(0, 20,0);
            ship.Scale(.05f);
            ship.ApplyRotationY(270.0f - 27.5f);
            ship.OffsetBy(0, -5, 0);

            interceptor = new GameObjectModel("Models/ship");
            interceptor.Position = new Vector3(100, 20, 100);
            interceptor.Scale(.005f);
            interceptor.ApplyRotationY(270.0f - 27.5f);

            //Audio loading
            Game1.audioManager.AddSound("Music");
            Game1.audioManager.PlaySound("Hello");
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

            level2.Draw();

            ship.Draw();
            interceptor.Draw();

            base.Draw(gameTime);
        }
    }
}
