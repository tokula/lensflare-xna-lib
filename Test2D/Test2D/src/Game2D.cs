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
        SpriteFont defaultFont;
        Texture2D circleTexture;
        public Texture2D lineTexture;

        public SpriteBatch SpriteBatch          { get; protected set; }
        public TextureBuilder TextureBuilder    { get; protected set; }
        public Engine Engine                    { get; protected set; }
        public World World                      { get; protected set; }
        public KeyValueManager KeyValueManager  { get; protected set; }
        public SmoothCamera2 Camera             { get; protected set; }
        public Random Random                    { get; protected set; }
        public LayerManager LayerManager        { get; protected set; }
        public WormEntity Worm                  { get; protected set; }

        public Game2D() {
            Content.RootDirectory = "Content";

            Camera = new SmoothCamera2();
            Random = new Random();
            LayerManager = new LayerManager(EnumExtension.GetLength<MainLayer>());

            Engine = new Engine(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            Engine.Initialize();
            Engine.GraphicsDeviceManager.ApplyResolution(1024, 768, false);

            TextureBuilder = new TextureBuilder(this.GraphicsDevice);

            Window.Title = "XNA 2D Test";

            //this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 1000/60);
            this.IsFixedTimeStep = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Engine.Load();

            defaultFont = Content.Load<SpriteFont>("defaultFont");
            Texture2D platesTexture = Content.Load<Texture2D>("plates7");
            Texture2D scratchedTexture = Content.Load<Texture2D>("scratched2");
            circleTexture = Content.Load<Texture2D>("circle2");
            Texture2D masontyStoneOnyxBlue = Content.Load<Texture2D>("Masonry.Stone.Onyx.Blue");
            lineTexture = Content.Load<Texture2D>("line_c");

            KeyValueManager = new KeyValueManager(new MessageManager(SpriteBatch, defaultFont, new Vector2(8, 8), Color.Yellow, 2));

            Keys gravityKey = Keys.G;
            KeyValueManager.SetValueForKey(gravityKey, 100);
            KeyValueManager.SetValueStepForKey(gravityKey, 10);

            Keys zoomKey = Keys.Z;
            KeyValueManager.SetValueForKey(zoomKey, 1.0f);
            KeyValueManager.SetValueStepForKey(zoomKey, 0.05f);

            Keys frustumKey = Keys.F;
            KeyValueManager.SetValueForKey(frustumKey, 1.0f);
            KeyValueManager.SetValueStepForKey(frustumKey, 0.05f);

            World = new World(Vector2.Zero);

            Camera.PositionScreen = Engine.Viewport.GetCenter();
            //Camera.ViewSize = new Vector2(700, 500);
            Camera.ViewSize = Engine.Viewport.GetSize() * KeyValueManager.ValueForKey(Keys.F);

            /*
            ParallaxEntity parallaxEntity = new ParallaxEntity(this);
            parallaxEntity.texture1 = platesTexture;
            parallaxEntity.texture2 = scratchedTexture;
            Entity.Add(parallaxEntity);
            */

            //Entity.Add(new WorldFrameEntity(this, Vector2.Zero, new Vector2(1000, 500)));
            Entity.Add(new GroundEntity(this, masontyStoneOnyxBlue));
            //Entity.Add(new TestEntity(this, new Vector2(100, 50), 10, 1));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            //Unload any non ContentManager content here
        }

        protected void ProcessInput(GameTime gameTime) {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 mouseWorldPosition = Camera.Unproject(Input.MousePosition);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                this.Exit();
            }

            DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            if (Input.KeyboardPressed(Keys.F7)) {
                IntVector2 viewSize;
                bool switchToFullscreen = !Engine.GraphicsDeviceManager.IsFullScreen;
                if(switchToFullscreen) {
                    viewSize = new IntVector2(displayMode.Width, displayMode.Height);
                } else {
                    viewSize = new IntVector2(1024, 768);
                }
                Engine.GraphicsDeviceManager.ApplyResolution(viewSize.X, viewSize.Y, switchToFullscreen);
                //Camera.ViewSize = viewSize.Vector2;
                Camera.PositionScreen = viewSize * 0.5f;
            }
            Camera.ViewSize = Engine.Viewport.GetSize() * KeyValueManager.ValueForKey(Keys.F);

            float movementBoost = 1.0f;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift)) {
                movementBoost = 10.0f;
            }

            float cameraMovementSpeedKeys = 3000.0f * movementBoost * elapsedSeconds;
            float cameraMovementSpeedMouse = 100.0f * movementBoost * elapsedSeconds;

            if (Input.MousePressing(Input.MouseButton.LeftButton)) {
                TestEntity testEntity = new TestEntity(this, mouseWorldPosition, Random.Next(30, 60), 1.0f);
                testEntity.texture = circleTexture;
                Entity.Add(testEntity);
            }

            if (Input.KeyboardPressed(Keys.P)) {
                if (Worm == null) {
                    Worm = new WormEntity(this, mouseWorldPosition);
                    Worm.texture = circleTexture;
                    Entity.Add(Worm);
                }
            }

            if (Input.KeyboardPressed(Keys.Space)) {
                if (Worm != null) {
                    Worm.ToggleRope();
                }
            }

            Vector2 arrowKeysVector = Vector2.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.Left)) {
                arrowKeysVector.X -= 1.0f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) {
                arrowKeysVector.X += 1.0f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) {
                arrowKeysVector.Y -= 1.0f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) {
                arrowKeysVector.Y += 1.0f;
            }

            bool arrowKeyScrolling = false;
            if (arrowKeyScrolling) {
                Camera.Velocity += arrowKeysVector * cameraMovementSpeedKeys;
            } else {
                if (Worm != null) {
                    float accelerationFactor = 1000000.0f;
                    Worm.AccelerateBy(arrowKeysVector * accelerationFactor);
                }
            }

            if (Input.MousePressing(Input.MouseButton.MiddleButton)) {
                Camera.PositionWorld -= Input.MouseDelta;
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

                KeyValueManager.Update(gameTime);

                World.Gravity.Y = KeyValueManager.ValueForKey(Keys.G);
                Camera.Zoom = KeyValueManager.ValueForKey(Keys.Z);

                Camera.Update(gameTime);
                World.Step(elapsedSeconds);
                Engine.Update2D(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Engine.Draw2D(gameTime);

            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            Entity.DrawAll();

            //camera:
            Primitive2.DrawRect(SpriteBatch, new IntVector2(Camera.PositionScreen) - Camera.ViewSize * 0.5f - Vector2.One * 1.001f, Camera.ViewSize + Vector2.One * 2 * 1.001f, Color.White, false, LayerManager.Depth((int)MainLayer.Hud)); //camera frame

            //hud:
            Vector2 mousePos = Input.MousePosition;
            
            String fpsString = "FPS: " + Engine.Fps;
            Vector2 fpsStringSize = defaultFont.MeasureString(fpsString);
            SpriteBatch.DrawString(defaultFont, fpsString, new Vector2(Engine.Viewport.Width - fpsStringSize.X - 8, 0), Color.Blue); //TODO: layerDepth

            KeyValueManager.Draw();

            float mouseLayerDepth = LayerManager.Depth((int)MainLayer.MouseCursor);
            Primitive2.DrawCircle(SpriteBatch, mousePos, 4, Color.Blue, false, mouseLayerDepth);
            Primitive2.DrawCircle(SpriteBatch, mousePos, 5, Color.DarkBlue, false, mouseLayerDepth);
            Primitive2.DrawCircle(SpriteBatch, mousePos, 6, Color.Blue, false, mouseLayerDepth);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
