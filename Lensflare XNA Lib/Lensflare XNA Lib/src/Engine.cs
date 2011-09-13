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

    public class Engine {
        //TODO: properties

        protected Game game;

        public Effect effect;

        RenderTarget2D renderTarget;
        public Texture2D shadowMap;

        Random random = new Random();

        Vector3 debugVector = Vector3.Zero;

        public float ambientPower = 0.2f;
        public float lightPower = 1.0f;
        public Vector3 lightDirection = new Vector3(1.0f, -1.0f, 1.0f);
        public Vector3 lightPos = new Vector3(0, 10.0f, 0);
        public Matrix lightsViewProjectionMatrix;

        //TODO: auch set
        public int ScreenWidth { get { return game.GraphicsDevice.Viewport.Width; } }
        public int ScreenHeight { get { return game.GraphicsDevice.Viewport.Height; }  }

        //TODO: Constructor(), Initialize(), Load() evtl. zusammenfassen

        public Engine(Game game) {
            this.game = game;
        }

        public void Initialize() {
            CenterMouseCursor();
            Input.Update();
        }

        public void Load() {
            Primitive2D.Init(game.GraphicsDevice);

            //GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            //GraphicsDevice.PresentationParameters.MultiSampleQuality = 8;
            //GraphicsDevice.PresentationParameters.MultiSampleType = MultiSampleType.EightSamples;
            //GraphicsDevice.RenderState.MultiSampleAntiAlias = true;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            rasterizerState.MultiSampleAntiAlias = true;
            game.GraphicsDevice.RasterizerState = rasterizerState;

            //effect = new BasicEffect(GraphicsDevice);
            effect = game.Content.Load<Effect>("TestEffect");

            //PresentationParameters pp = game.GraphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(game.GraphicsDevice, 1024 * 4, 1024 * 4, true, game.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
        }

        public void Update(GameTime gameTime) {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateLight();

            Entity.UpdateAll(gameTime);

            CenterMouseCursor();
            Input.Update();
        }

        private void UpdateLight() {
            //Matrix lightsView = Matrix.CreateLookAt(lightPos, lightDirection, new Vector3(0, 1, 0));
            //Matrix lightsView = camera.view;
            Matrix lightsView = Matrix.CreateLookAt(debugVector, new Vector3(0.1f, 0, 0), new Vector3(0, 1, 0));

            //float fov = MathHelper.PiOver2;
            //float fov = MathHelper.Pi * 0.1f;
            //Matrix lightsProjection = Matrix.CreatePerspectiveFieldOfView(fov, 1.0f, 1.0f, 100000.0f);

            Matrix lightsProjection;
            Matrix.CreateOrthographic(100, 100, 1.0f, 100.0f, out lightsProjection);
            lightsViewProjectionMatrix = lightsView * lightsProjection;

            //lightArrow.position = debugVector;
            //lightArrow.direction = /*Vector3.Normalize(new Vector3(-1, -1, -1));*/camera.rotationMatrix.Forward;
        }

        public void Draw() {
            game.GraphicsDevice.SetRenderTarget(renderTarget);

            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            //GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            DrawScene("ShadowMap");

            game.GraphicsDevice.SetRenderTarget(null);
            shadowMap = (Texture2D)renderTarget;

            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            //GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1.0f, 0);
            DrawScene("ShadowedScene");

            shadowMap = null;
        }

        private void DrawScene(String technique) {
            effect.CurrentTechnique = effect.Techniques[technique];
            effect.Parameters["xLightPos"].SetValue(lightPos);
            effect.Parameters["xLightPower"].SetValue(lightPower);
            effect.Parameters["xAmbient"].SetValue(ambientPower);
            effect.Parameters["xShadowMap"].SetValue(shadowMap);

            Entity.DrawAll();
        }

        private void CenterMouseCursor() {
            Mouse.SetPosition(ScreenWidth / 2, ScreenHeight / 2);
        }

        public static String VectorToString(Vector3 v) {
            return "(" + v.X.ToString("0.00") + " | " + v.Y.ToString("0.00") + " | " + v.Z.ToString("0.00") + ")";
        }

        public Entity GetEntityFromRay(Vector3 position, Vector3 direction) {
            Entity entityHit = null;
            /* TODO: ray ohne physics
            float min = float.PositiveInfinity;
            Ray ray = new Ray(position, direction);
            foreach (Entity entity in Entity.All) {
                if (entity is BoxEntity) {
                    BoxEntity entityTest = ((BoxEntity)entity);
                    RayHit rayHit;
                    entityTest.physicsBox.CollisionInformation.RayCast(ray, float.PositiveInfinity, out rayHit);
                    float dist = rayHit.T;
                    if (dist > 0.0f) {
                        if (dist < min) {
                            min = dist;
                            entityHit = entity;
                        }
                    }
                }
            }*/
            return entityHit;
        }

        public Texture2D ColorToTexture(Color color) {
            Texture2D texture = new Texture2D(game.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { color });
            return texture;
        }

        public static void DirectionToRotationMatrix(ref Vector3 direction, out Matrix m) {
            m = Matrix.Identity;
            m.Forward = direction;
            m.Right = Vector3.Normalize(Vector3.Cross(m.Forward, Vector3.Up));
            m.Up = Vector3.Normalize(Vector3.Cross(m.Right, m.Forward));
        }

    }
}
