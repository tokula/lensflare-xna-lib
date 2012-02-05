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

using LensflareGameFramework;
using Util;
using Camera;
using FarseerPhysics.Dynamics;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;

namespace Test2D {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game2D : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        ProceduralTextureBuilder proceduralTexture;

        SmoothCamera2 camera = new SmoothCamera2();

        public Engine engine;
        public World world;

        SpriteFont defaultFont;

        Random random = new Random();

        public Game2D() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            proceduralTexture = new ProceduralTextureBuilder(this.GraphicsDevice);

            engine = new Engine(this);
            engine.Initialize();

            Window.Title = "XNA 2D Test";

            this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 1000/60);
            this.IsFixedTimeStep = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            engine.Load();

            defaultFont = Content.Load<SpriteFont>("defaultFont");

            world = new World(new Vector2(0, 100));

            Vector2 screenCenter = new Vector2(engine.ScreenWidth / 2, engine.ScreenHeight / 2);
            camera.PositionScreen = screenCenter;
            camera.Size = new Vector2(800, 600);

            Entity.Add(new GroundEntity(this));
            Entity.Add(new TestEntity(this, new Vector2(100, 50), 10, 1));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected void ProcessInput(GameTime gameTime) {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 mousePosition = Input.MousePosition;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                this.Exit();
            }

            float movementBoost = 1.0f;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift)) {
                movementBoost = 50.0f;
            }

            float debugValueSpeed = 1.0f * movementBoost * elapsedSeconds; ;
            float cameraMovementSpeed = 1.0f * movementBoost * elapsedSeconds;

            if (Input.MousePressing(Input.MouseButton.LeftButton)) {
                Entity.Add(new TestEntity(this, mousePosition, random.Next(10, 20), 1.0f));
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (IsActive) {
                float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

                ProcessInput(gameTime);

                camera.Update(gameTime);
                world.Step(elapsedSeconds);
                engine.Update2D(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            Vector2 screenCenter = new Vector2(engine.ScreenWidth / 2, engine.ScreenHeight / 2);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            engine.Draw2D(gameTime);

            spriteBatch.Begin();

            Entity.DrawAll();

            //camera:
            Primitive2.DrawRect(spriteBatch, camera.PositionScreen - camera.Size * 0.5f, camera.Size, Color.White, false); //camera frame

            //hud:
            Vector2 mousePos = Input.MousePosition;
            
            /*for(int i=0; i<10000; ++i) {
                Vector2 mousePosOffset = mousePos + new Vector2((float)random.NextDouble() * 100, (float)random.NextDouble() * 100);
                Primitive2D.DrawCircle(spriteBatch, mousePosOffset, random.Next(4,8), Color.Yellow, false);
            }*/

            String fpsString = "FPS: " + engine.Fps;
            Vector2 fpsStringSize = defaultFont.MeasureString(fpsString);
            spriteBatch.DrawString(defaultFont, fpsString, new Vector2(engine.ScreenWidth - fpsStringSize.X - 8, 0), Color.Blue);

            Primitive2.DrawCircle(spriteBatch, mousePos, 4, Color.Blue, false);
            Primitive2.DrawCircle(spriteBatch, mousePos, 5, Color.DarkBlue, false);
            Primitive2.DrawCircle(spriteBatch, mousePos, 6, Color.Blue, false);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}