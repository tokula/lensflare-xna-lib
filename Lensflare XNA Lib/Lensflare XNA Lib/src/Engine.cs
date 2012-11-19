using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Util;

namespace LensflareGameFramework {
    public class Engine {        
        RenderTarget2D renderTarget;
        Random random = new Random(); //TODO: needed?
        Vector3 debugVector = Vector3.Zero;

        public Game Game { get; protected set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; protected set; }
        public Effect Effect { get; set; }
        private Texture2D ShadowMap { get; set; } //TODO: evtl nicht nötig

        public float ambientPower = 0.2f;
        public float lightPower = 1.0f;
        public Vector3 lightDirection = new Vector3(1.0f, -1.0f, 1.0f);
        public Vector3 lightPos = new Vector3(0, 10.0f, 0);
        public Matrix lightsViewProjectionMatrix;

        protected float timeSinceLastFpsUpdate;
        protected int framesSinceLastFpsUpdate;
        public float Fps { get; protected set; }
        public float FpsUpdateDelay { get; set; }

        public bool MouseCursorCentering { get; set; }

        public Viewport Viewport { get { return Game.GraphicsDevice.Viewport; } }

        //TODO: auch set, außerdem umbenennen in viewport statt screen
        /*
        public int ViewportWidth { get { return Game.GraphicsDevice.Viewport.Width; } }
        public int ViewportHeight { get { return Game.GraphicsDevice.Viewport.Height; }  }
        public Vector2 ViewportSize { get { Viewport vp = Game.GraphicsDevice.Viewport; return new Vector2(vp.Width, vp.Height); } }
        public Vector2 ViewportCenter { get { Viewport vp = Game.GraphicsDevice.Viewport; return new Vector2(vp.Width * 0.5f, vp.Height * 0.5f); } }*/

        //TODO: Constructor(), Initialize(), Load() evtl. zusammenfassen

        public Engine(Game game) {
            Game = game;
            FpsUpdateDelay = 0.5f;

            GraphicsDeviceManager = new GraphicsDeviceManager(Game);
        }

        public void Initialize() {
            Input.Update();
        }

        public void Load() {
            Primitive2.Init(Game.GraphicsDevice);

            //GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            //GraphicsDevice.PresentationParameters.MultiSampleQuality = 8;
            //GraphicsDevice.PresentationParameters.MultiSampleType = MultiSampleType.EightSamples;
            //GraphicsDevice.RenderState.MultiSampleAntiAlias = true;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            rasterizerState.MultiSampleAntiAlias = true;
            Game.GraphicsDevice.RasterizerState = rasterizerState;
            //Game.GraphicsDevice.BlendState = BlendState.Additive; //TODO: texturen mit alpha 0.5 sind trotzdem nicht transparent

            //effect = new BasicEffect(GraphicsDevice);
            //Effect = Game.Content.Load<Effect>("TestEffect");

            //PresentationParameters pp = game.GraphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(Game.GraphicsDevice, 1024 * 4, 1024 * 4, true, Game.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

            CenterMouseCursor();
        }

        public void Update2D(GameTime gameTime) {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Entity.UpdateAll(gameTime);
            if (MouseCursorCentering) {
                CenterMouseCursor();
            }
            Input.Update();
        }

        public void Update3D(GameTime gameTime) {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdateLight();
            Entity.UpdateAll(gameTime);
            if (MouseCursorCentering) {
                CenterMouseCursor();
            }
            Input.Update();
        }

        protected void UpdateLight() {
            //Matrix lightsView = Matrix.CreateLookAt(lightPos, lightDirection, new Vector3(0, 1, 0));
            //Matrix lightsView = camera.view;
            Matrix lightsView = Matrix.CreateLookAt(lightPos, lightPos + Vector3.Normalize(lightDirection), new Vector3(0, 1, 0));

            float fov = MathHelper.PiOver2;
            //float fov = MathHelper.Pi * 0.1f;
            Matrix lightsProjection = Matrix.CreatePerspectiveFieldOfView(fov, 1.0f, 0.1f, 1000.0f);

            //Matrix lightsProjection;
            //Matrix.CreateOrthographic(100, 100, 1.0f, 100.0f, out lightsProjection);
            lightsViewProjectionMatrix = lightsView * lightsProjection;

            //lightArrow.position = debugVector;
            //lightArrow.direction = /*Vector3.Normalize(new Vector3(-1, -1, -1));*/camera.rotationMatrix.Forward;
        }

        public void Draw2D(GameTime gameTime) {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastFpsUpdate += elapsedSeconds;
            framesSinceLastFpsUpdate += 1;
            if (timeSinceLastFpsUpdate >= FpsUpdateDelay) {
                timeSinceLastFpsUpdate -= FpsUpdateDelay;
                Fps = (int)(framesSinceLastFpsUpdate / FpsUpdateDelay);
                framesSinceLastFpsUpdate = 0;
            }
        }

        public void Draw3D(GameTime gameTime) {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastFpsUpdate += elapsedSeconds;
            framesSinceLastFpsUpdate += 1;
            if (timeSinceLastFpsUpdate >= FpsUpdateDelay) {
                timeSinceLastFpsUpdate -= FpsUpdateDelay;
                Fps = (int)(framesSinceLastFpsUpdate / FpsUpdateDelay);
                framesSinceLastFpsUpdate = 0;
            }

            Game.GraphicsDevice.SetRenderTarget(renderTarget);

            Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            //GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            DrawScene("ShadowMap");

            Game.GraphicsDevice.SetRenderTarget(null);
            ShadowMap = (Texture2D)renderTarget;

            Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            //GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1.0f, 0);
            DrawScene("ShadowedScene");

            ShadowMap = null;
        }

        protected void DrawScene(String technique) {
            Effect.CurrentTechnique = Effect.Techniques[technique];
            Effect.Parameters["LightPos"].SetValue(lightPos);
            Effect.Parameters["LightPower"].SetValue(lightPower);
            Effect.Parameters["Ambient"].SetValue(ambientPower);
            Effect.Parameters["shadowMap"].SetValue(ShadowMap);

            Entity.DrawAll();
        }

        protected void CenterMouseCursor() {
            Viewport vp = Viewport;
            Mouse.SetPosition(vp.Width / 2, vp.Height / 2);
        }

        public static void DirectionToRotationMatrix(ref Vector3 direction, out Matrix m) {
            m = Matrix.Identity;
            m.Forward = direction;
            m.Right = Vector3.Normalize(Vector3.Cross(m.Forward, Vector3.Up));
            m.Up = Vector3.Normalize(Vector3.Cross(m.Right, m.Forward));
        }
    }
}
