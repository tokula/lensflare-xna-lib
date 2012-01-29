using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Util;

namespace LensflareGameFramework {
    //TODO: in eigene datei verschieben (oder löschen)
    struct MyOwnVertexFormat : IVertexType {
        public Vector3 position;
        private Vector2 texCoord;
        private Vector3 normal;

        public readonly static VertexDeclaration vertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * (3 + 2), VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
        );

        public VertexDeclaration VertexDeclaration { get { return vertexDeclaration; } }

        public MyOwnVertexFormat(Vector3 position, Vector3 normal, Vector2 texCoord) {
            this.position = position;
            this.texCoord = texCoord;
            this.normal = normal;
        }
    }

    //TODO: projekt als lib

    //TODO: FPS counter

    public class Engine {
        //TODO: properties
        
        RenderTarget2D renderTarget;
        Random random = new Random();
        Vector3 debugVector = Vector3.Zero;

        public Game Game { get; protected set; }
        public Effect Effect { get; protected set; }
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

        //TODO: auch set
        public int ScreenWidth { get { return Game.GraphicsDevice.Viewport.Width; } }
        public int ScreenHeight { get { return Game.GraphicsDevice.Viewport.Height; }  }

        //TODO: Constructor(), Initialize(), Load() evtl. zusammenfassen

        //TODO: verschieben zu einer zentralen stelle
        public static int GetEnumLength<T>() {
            return Enum.GetNames(typeof(T)).Length;
        }

        public Engine(Game game) {
            Game = game;
            FpsUpdateDelay = 0.5f;
        }

        public void Initialize() {
            CenterMouseCursor();
            Input.Update();
        }

        public void Load() {
            Primitive2D.Init(Game.GraphicsDevice);

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
            Effect = Game.Content.Load<Effect>("TestEffect");

            //PresentationParameters pp = game.GraphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(Game.GraphicsDevice, 1024 * 4, 1024 * 4, true, Game.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
        }

        public void Update2D(GameTime gameTime) {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Entity.UpdateAll(gameTime);
            Input.Update();
        }

        public void Update3D(GameTime gameTime) {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdateLight();
            Entity.UpdateAll(gameTime);
            CenterMouseCursor();
            Input.Update();
        }

        private void UpdateLight() {
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

        private void DrawScene(String technique) {
            Effect.CurrentTechnique = Effect.Techniques[technique];
            Effect.Parameters["LightPos"].SetValue(lightPos);
            Effect.Parameters["LightPower"].SetValue(lightPower);
            Effect.Parameters["Ambient"].SetValue(ambientPower);
            Effect.Parameters["shadowMap"].SetValue(ShadowMap);

            Entity.DrawAll();
        }

        private void CenterMouseCursor() {
            Mouse.SetPosition(ScreenWidth / 2, ScreenHeight / 2);
        }

        public static void DirectionToRotationMatrix(ref Vector3 direction, out Matrix m) {
            m = Matrix.Identity;
            m.Forward = direction;
            m.Right = Vector3.Normalize(Vector3.Cross(m.Forward, Vector3.Up));
            m.Up = Vector3.Normalize(Vector3.Cross(m.Right, m.Forward));
        }
    }
}
