using System;
using LensflareGameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Util;
using Shape3;
using System.Collections.Generic;
using BEPUphysics.MathExtensions;
using BEPUphysics;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Camera;

namespace EngineTest {
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	class Game3D : Microsoft.Xna.Framework.Game {
		public SpriteBatch spriteBatch;
        public TextureBuilder textureBuilder;
        public FunctionPlotter fp;
        bool showFunctionPlots = false;

        public Engine engine;
        public Space space;

        SoundEffect[] noteSound;

        public Texture2D ProceduralTexture { get; protected set; }

        public SmoothCamera3 camera;

        Random random = new Random();

        Vector3 debugVector = new Vector3(0.4f, -1, 0.4f);
        SpriteFont defaultFont;

        ArrowEntity lightArrow;

        Dictionary<IntVector2, Tile> tiles = new Dictionary<IntVector2, Tile>();

        Creator creator;
        bool freeCamera = false;
        bool planeGround = true;

		public Game3D() {
			Content.RootDirectory = "Content";

            engine = new Engine(this);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
            engine.Initialize();
            engine.MouseCursorCentering = true;

            engine.GraphicsDeviceManager.PreferMultiSampling = true;
            engine.GraphicsDeviceManager.ApplyResolution(1024, 768, false);

            textureBuilder = new TextureBuilder(this.GraphicsDevice);

            Window.Title = "XNA 3D Graphics with BEPUphysics";

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);

            engine.Load();
            engine.Effect = new BasicEffect(GraphicsDevice); //Content.Load<Effect>("DefaultEffect");

			camera = new SmoothCamera3();
            camera.AspectRatio = (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height;
			camera.Position = new Vector3(0, 1, 10);
			camera.Rotation = new Vector3(0, 0, 0);

            fp = new FunctionPlotter(spriteBatch, new ColoredFunction[] { 
                new ColoredFunction(Color.Cyan, x => MathProcedural.Gain(0.2f, x)),
                new ColoredFunction(Color.Red, x => MathProcedural.Pulse(0.1f, 0.9f, x)),
                new ColoredFunction(Color.Yellow, x => MathProcedural.SmoothStep(0.1f, 0.9f, x)),
                new ColoredFunction(Color.Violet, x => MathProcedural.SmoothStep(0.1f, 0.9f, MathProcedural.Mod(x,1.0f)/1.0f)),
            });
            fp.PosOnScreen = engine.Viewport.GetCenter() - fp.Size*fp.Scale*0.5f;

            noteSound = new SoundEffect[] {
                Content.Load<SoundEffect>("note_c"),
                Content.Load<SoundEffect>("note_d"),
                Content.Load<SoundEffect>("note_ds"),
                Content.Load<SoundEffect>("note_f"),
                Content.Load<SoundEffect>("note_g"),
                Content.Load<SoundEffect>("note_gs"),
                Content.Load<SoundEffect>("note_as"),
            };

            defaultFont = Content.Load<SpriteFont>("defaultFont");

            space = new Space();
            if (planeGround) {
                space.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);
            }

            ProceduralTexture = textureBuilder.Test(1024, 1024);

            Texture2D groundTexture = Content.Load<Texture2D>("ground");
            //Texture2D groundTexture = proceduralTexture.Sphere(512, new Color(1.0f, 0.5f, 0.0f, 0.5f));
            if (planeGround) {
                float groundThickness = 0.05f;
                Entity.Add(new BoxEntity(this, new Vector3(0, -groundThickness, 0), new Vector3(5, groundThickness, 5), -1, groundTexture));
            } else {
                float planetRadius = 8.0f;
                Entity.Add(new ForceEntity(this, Vector3.Zero, planetRadius, -1, groundTexture, 20.0f));
            }

            //Entity.Add(new BoxEntity(this, new Vector3(0, -groundThickness, 0), new Vector3(5, groundThickness, 5), -1, engine.ColorToTexture(Color.Gray)));
            //Entity.Add(new BoxEntity(this, new Vector3(0, 5, 0), new Vector3(1, 1, 1) * 0.5f, 1, Content.Load<Texture2D>("metallkreis")));
            //Entity.Add(new SphereEntity(this, new Vector3(0, 5, 0), 0.5f, 1, Content.Load<Texture2D>("metallkreis")));
            lightArrow = new ArrowEntity(this, new Vector3(0, 0, 0), new Vector3(1, 1, 1), true, textureBuilder.TextureFromColor(new Color(1.0f, 0.5f, 0.0f, 0.5f)));
            Entity.Add(lightArrow);
            //Entity.Add(new GroundEntity(this, new Vector3(0, 0, 0), new Vector3(1, 1, 1) * 0.5f, Content.Load<Texture2D>("tits")));
            creator = new Creator(this, new Vector3(0, 2, 0), 0.5f, 1, Content.Load<Texture2D>("metallkreis"));
            Entity.Add(creator);
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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                this.Exit();
            }

            float movementBoost = 1.0f;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift)) {
                movementBoost = 50.0f;
            }

            float debugValueSpeed = 1.0f * movementBoost * elapsedSeconds;
            float cameraMovementAcc = 50.0f * movementBoost * elapsedSeconds;
            float cameraRotationSpeed = 1.5f * elapsedSeconds;
            float mouseRotationSpeed = 0.1f * elapsedSeconds;
            bool debugValueHasChanged = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Left)) {
                debugVector.X -= debugValueSpeed;
                debugValueHasChanged = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) {
                debugVector.X += debugValueSpeed;
                debugValueHasChanged = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) {
                debugVector.Y += debugValueSpeed;
                debugValueHasChanged = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) {
                debugVector.Y -= debugValueSpeed;
                debugValueHasChanged = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.PageUp)) {
                debugVector.Z += debugValueSpeed;
                debugValueHasChanged = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.PageDown)) {
                debugVector.Z -= debugValueSpeed;
                debugValueHasChanged = true;
            }
            if (debugValueHasChanged) {
                Console.WriteLine("debugVector: " + VectorToString(debugVector));
            }

            Vector3 rotation = Vector3.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.Q)) {
                rotation.Y += cameraRotationSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E)) {
                rotation.Y -= cameraRotationSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R)) {
                rotation.X += cameraRotationSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F)) {
                rotation.X -= cameraRotationSpeed;
            }
            rotation.X -= Input.MouseDeltaY * mouseRotationSpeed;
            rotation.Y -= Input.MouseDeltaX * mouseRotationSpeed;
            camera.Rotation += rotation;

            Matrix cameraRotationMatrix = camera.CalcRotationMatrix();

            if (freeCamera) {
                //camera controls:
                if (Keyboard.GetState().IsKeyDown(Keys.S)) {
                    camera.Velocity -= cameraRotationMatrix.Forward * cameraMovementAcc;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W)) {
                    camera.Velocity += cameraRotationMatrix.Forward * cameraMovementAcc;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                    camera.Velocity += cameraRotationMatrix.Left * cameraMovementAcc;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D)) {
                    camera.Velocity += cameraRotationMatrix.Right * cameraMovementAcc;
                }
            } else {
                float creatorMovementSpeed = 30.0f * elapsedSeconds;
                Vector3 sideAxis = Vector3.Cross(cameraRotationMatrix.Right, Vector3.Up);
                if (Keyboard.GetState().IsKeyDown(Keys.S)) {
                    creator.AngularVelocity += cameraRotationMatrix.Right * creatorMovementSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W)) {
                    creator.AngularVelocity += cameraRotationMatrix.Left * creatorMovementSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                    creator.AngularVelocity += sideAxis * creatorMovementSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D)) {
                    creator.AngularVelocity -= sideAxis * creatorMovementSpeed;
                }
                camera.Position = creator.Position - cameraRotationMatrix.Forward * 10;
            }

            if (Input.KeyboardPressed(Keys.K) || Keyboard.GetState().IsKeyDown(Keys.L)) {
                //Entity.Add(new BoxEntity(this, new Vector3(0, 2, 0), new Vector3(1, 1, 1) * 0.5f, 1, textureBuilder.Test(1024, 1024)));
                Entity.Add(new TestBoxEntity(this, new Vector3(0, 2, 0), new Vector3(1, 1, 1) * 0.5f, 1));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.O)) {
                Entity.Add(new SphereEntity(this, new Vector3(0, 2, 0), 0.5f, 1, Content.Load<Texture2D>("metallkreis")));
            }

            if (Input.KeyboardPressed(Keys.T)) {
                ProceduralTexture = textureBuilder.Test(1024, 1024);
            }

            if (Input.KeyboardPressed(Keys.X)) {
                BoxEntity et = new BoxEntity(this, camera.Position, new Vector3(1, 1, 1) * 0.1f, 0.3f, Content.Load<Texture2D>("alufolie"));
                et.PhysicsEntities[0].LinearVelocity = cameraRotationMatrix.Forward * 10.0f;
                Entity.Add(et);
            }
            if (Input.KeyboardPressed(Keys.C)) {
                BoxEntity et = new BoxEntity(this, camera.Position, new Vector3(1, 1, 1) * 0.1f, 10.0f, Content.Load<Texture2D>("propeller"));
                et.PhysicsEntities[0].LinearVelocity = cameraRotationMatrix.Forward * 10.0f;
                Entity.Add(et);
            }
            if (Input.KeyboardPressing(Keys.V)) {
                int colorMin = 150;
                int colorMax = 255;
                Color color = new Color(random.Next(colorMin, colorMax), random.Next(colorMin, colorMax), random.Next(colorMin, colorMax) * 0);
                float size = 0.02f + 0.1f * (float)random.NextDouble();
                var et = new BoxEntity(this, new Vector3(13, 0, 0), new Vector3(1, 1, 1) * size, size, textureBuilder.TextureFromColor(color));
                //var et = new ForceEntity(this, camera.Position, 0.1f, 0.5f, engine.ColorToTexture(color), 0.001f);
                et.PhysicsEntities[0].LinearVelocity = Vector3.Forward * 10.0f;
                Entity.Add(et);
            }

            float forceEntityRadius = 0.25f;
            if (Input.MousePressed(Input.MouseButton.RightButton)) {
                if (freeCamera) {
                    Entity entityHit = GetEntityFromRay(camera.Position, cameraRotationMatrix.Forward);

                    if (entityHit != null) {
                        if (entityHit is BoxEntity || entityHit is SphereEntity || entityHit is ForceEntity) {
                            Entity.Remove(entityHit);
                        } else if (entityHit is Tile) {
                            Tile tile = (Tile)entityHit;
                            Entity.Add(new ForceEntity(this, tile.PhysicsEntities[0].Position + new Vector3(0, Tile.Size.Y + forceEntityRadius, 0), forceEntityRadius, -1, textureBuilder.TextureFromColor(Color.DarkRed), -1.5f));
                        }
                    }
                }
            }
            if (Input.MousePressed(Input.MouseButton.LeftButton)) {
                if (freeCamera) {
                    RayHit rayHit;
                    Entity entityHit = GetEntityFromRay(camera.Position, cameraRotationMatrix.Forward, out rayHit);
                    if (entityHit != null) {
                        if (entityHit is BoxEntity) {
                            SplitBox((BoxEntity)entityHit);
                        } else if(entityHit is Tile) {
                            var tile = (Tile)entityHit;
                            Entity.Add(new ForceEntity(this, tile.PhysicsEntities[0].Position + new Vector3(0, Tile.Size.Y + forceEntityRadius, 0), forceEntityRadius, -1, textureBuilder.TextureFromColor(Color.Green), 1.5f));
                        } else if (entityHit is ForceEntity) {
                            var e = (ForceEntity)entityHit;
                            Vector3 hitPos = rayHit.Location;
                            Vector3 n = Vector3.Normalize(hitPos - e.Position);
                            Entity.Add(new ForceEntity(this, hitPos + n * forceEntityRadius, forceEntityRadius, -1, textureBuilder.TextureFromColor(Color.Green), 0.5f));
                        }
                    }
                }
            }
            if (Input.KeyboardPressed(Keys.D1)) {
                noteSound[0].Play();
            }
            if (Input.KeyboardPressed(Keys.D2)) {
                noteSound[1].Play();
            }
            if (Input.KeyboardPressed(Keys.D3)) {
                noteSound[2].Play();
            }
            if (Input.KeyboardPressed(Keys.D4)) {
                noteSound[3].Play();
            }
            if (Input.KeyboardPressed(Keys.D5)) {
                noteSound[4].Play();
            }
            if (Input.KeyboardPressed(Keys.D6)) {
                noteSound[5].Play();
            }
            if (Input.KeyboardPressed(Keys.D7)) {
                noteSound[6].Play();
            }
            if (Input.KeyboardPressed(Keys.D0)) {
                noteSound[random.Next(7)].Play();
            }

            if (Input.KeyboardPressed(Keys.G) || Input.MousePressed(Input.MouseButton.MiddleButton)) {
                freeCamera = !freeCamera;
            }

            if (Input.KeyboardPressed(Keys.P)) {
                showFunctionPlots = !showFunctionPlots;
            }
        }

        protected void SplitBox(BoxEntity box) {
            var e = box.PhysicsEntities[0];
            if (e is BEPUphysics.Entities.Prefabs.Box) {
                var physicsBox = (BEPUphysics.Entities.Prefabs.Box)e;
                if (physicsBox.IsDynamic) {
                    float t = physicsBox.Length / 4;
                    if (t > 0.01f) {
                        Entity.Remove(box);

                        Vector3 size = new Vector3(physicsBox.Length / 4, physicsBox.Height / 4, physicsBox.Width / 4);

                        Matrix eWorldMatrix = physicsBox.WorldTransform;
                        Matrix3X3 eom = physicsBox.OrientationMatrix;
                        float mass = physicsBox.Mass / 8;

                        BEPUphysics.Entities.Prefabs.Box tpb;

                        BoxEntity eFrontUpRight = new BoxEntity(this, new Vector3(0, 0, 0), size, mass, box.Texture);
                        tpb = (BEPUphysics.Entities.Prefabs.Box)eFrontUpRight.PhysicsEntities[0];
                        tpb.WorldTransform = eWorldMatrix;
                        tpb.Position += eom.Up * t + eom.Right * t + eom.Forward * t;
                        tpb.LinearVelocity = physicsBox.LinearVelocity;
                        Entity.Add(eFrontUpRight);

                        BoxEntity eFrontUpLeft = new BoxEntity(this, new Vector3(0, 0, 0), size, mass, box.Texture);
                        tpb = (BEPUphysics.Entities.Prefabs.Box)eFrontUpLeft.PhysicsEntities[0];
                        tpb.WorldTransform = eWorldMatrix;
                        tpb.Position += eom.Up * t + eom.Left * t + eom.Forward * t;
                        tpb.LinearVelocity = physicsBox.LinearVelocity;
                        Entity.Add(eFrontUpLeft);

                        BoxEntity eFrontDownRight = new BoxEntity(this, new Vector3(0, 0, 0), size, mass, box.Texture);
                        tpb = (BEPUphysics.Entities.Prefabs.Box)eFrontDownRight.PhysicsEntities[0];
                        tpb.WorldTransform = eWorldMatrix;
                        tpb.Position += eom.Down * t + eom.Right * t + eom.Forward * t;
                        tpb.LinearVelocity = physicsBox.LinearVelocity;
                        Entity.Add(eFrontDownRight);

                        BoxEntity eFrontDownLeft = new BoxEntity(this, new Vector3(0, 0, 0), size, mass, box.Texture);
                        tpb = (BEPUphysics.Entities.Prefabs.Box)eFrontDownLeft.PhysicsEntities[0];
                        tpb.WorldTransform = eWorldMatrix;
                        tpb.Position += eom.Down * t + eom.Left * t + eom.Forward * t;
                        tpb.LinearVelocity = physicsBox.LinearVelocity;
                        Entity.Add(eFrontDownLeft);

                        BoxEntity eBackUpRight = new BoxEntity(this, new Vector3(0, 0, 0), size, mass, box.Texture);
                        tpb = (BEPUphysics.Entities.Prefabs.Box)eBackUpRight.PhysicsEntities[0];
                        tpb.WorldTransform = eWorldMatrix;
                        tpb.Position += eom.Up * t + eom.Right * t + eom.Backward * t;
                        tpb.LinearVelocity = physicsBox.LinearVelocity;
                        Entity.Add(eBackUpRight);

                        BoxEntity eBackUpLeft = new BoxEntity(this, new Vector3(0, 0, 0), size, mass, box.Texture);
                        tpb = (BEPUphysics.Entities.Prefabs.Box)eBackUpLeft.PhysicsEntities[0];
                        tpb.WorldTransform = eWorldMatrix;
                        tpb.Position += eom.Up * t + eom.Left * t + eom.Backward * t;
                        tpb.LinearVelocity = physicsBox.LinearVelocity;
                        Entity.Add(eBackUpLeft);

                        BoxEntity eBackDownRight = new BoxEntity(this, new Vector3(0, 0, 0), size, mass, box.Texture);
                        tpb = (BEPUphysics.Entities.Prefabs.Box)eBackDownRight.PhysicsEntities[0];
                        tpb.WorldTransform = eWorldMatrix;
                        tpb.Position += eom.Down * t + eom.Right * t + eom.Backward * t;
                        tpb.LinearVelocity = physicsBox.LinearVelocity;
                        Entity.Add(eBackDownRight);

                        BoxEntity eBackDownLeft = new BoxEntity(this, new Vector3(0, 0, 0), size, mass, box.Texture);
                        tpb = (BEPUphysics.Entities.Prefabs.Box)eBackDownLeft.PhysicsEntities[0];
                        tpb.WorldTransform = eWorldMatrix;
                        tpb.Position += eom.Down * t + eom.Left * t + eom.Backward * t;
                        tpb.LinearVelocity = physicsBox.LinearVelocity;
                        Entity.Add(eBackDownLeft);

                        tpb = null;
                    }
                }
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

                bool createTiles = false;
                if (createTiles) {
                    float tileExistenceRadiusF = 1.25f;
                    int tileExistenceRadiusI = (int)(tileExistenceRadiusF + 3);
                    Vector3 creatorPos = creator.Position;
                    IntVector2 creatorTilePos = new IntVector2((int)creatorPos.X, (int)creatorPos.Z);
                    //float offsetX = creatorPos.X - creatorTilePos.X * Tile.Size.X * 2;
                    //float offsetZ = creatorPos.Z - creatorTilePos.Y * Tile.Size.Z * 2;
                    for (int x = -tileExistenceRadiusI; x <= tileExistenceRadiusI; ++x) {
                        for (int z = -tileExistenceRadiusI; z <= tileExistenceRadiusI; ++z) {
                            IntVector2 pickedPoint = new IntVector2(x + creatorTilePos.X, z + creatorTilePos.Y);
                            float xDist = (creatorPos.X - pickedPoint.X * Tile.Size.X * 2);
                            float zDist = (creatorPos.Z - pickedPoint.Y * Tile.Size.Z * 2);
                            float dist = (float)Math.Sqrt(xDist * xDist + zDist * zDist);
                            if (dist <= tileExistenceRadiusF) {
                                if (!tiles.ContainsKey(pickedPoint)) {
                                    int colorMin = 150;
                                    int colorMax = 255;
                                    Color color = new Color(random.Next(colorMin, colorMax), random.Next(colorMin, colorMax), random.Next(colorMin, colorMax) * 0);
                                    Tile tile = new Tile(this, new Vector3(pickedPoint.X * Tile.Size.X * 2, 1, pickedPoint.Y * Tile.Size.Z * 2), textureBuilder.TextureFromColor(color), noteSound[random.Next(noteSound.Length)]);
                                    Entity.Add(tile);
                                    tiles.Add(pickedPoint, tile);
                                    //noteSound[random.Next(noteSound.Length)].Play(0.5f, 0.0f, 0.0f);
                                }
                            }
                        }
                    }
                }

                //engine.lightPos = debugVector;
                //engine.lightDirection = new Vector3(0.4f, -1, 0.4f);
                Vector3 lightDirection = debugVector;
                lightDirection.Normalize();
                engine.lightDirection = lightDirection;
                engine.lightPos = -engine.lightDirection;

                lightArrow.position = engine.lightPos;
                lightArrow.direction = Vector3.Normalize(engine.lightDirection);

                camera.Update(gameTime);
                space.Update(elapsedSeconds);
                engine.Update3D(gameTime);
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
            //reset wegen SpriteBatch:
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            engine.Draw3D(gameTime);

            //hud:
            Vector2 screenCenter = engine.Viewport.GetCenter();
            spriteBatch.Begin();
            spriteBatch.DrawString(defaultFont, "debugVector: " + VectorToString(debugVector), new Vector2(4, 0), Color.Blue);

            if (showFunctionPlots) {
                fp.Draw();
            }

            String fpsString = "FPS: " + engine.Fps;
            Vector2 fpsStringSize = defaultFont.MeasureString(fpsString);
            spriteBatch.DrawString(defaultFont, fpsString, new Vector2(engine.Viewport.Width - fpsStringSize.X - 8, 0), Color.Blue);
            if (freeCamera) {
                Primitive2.DrawCircle(spriteBatch, screenCenter, 4, Color.Blue, false, 1.0f);
                Primitive2.DrawCircle(spriteBatch, screenCenter, 5, Color.DarkBlue, false, 1.0f);
                Primitive2.DrawCircle(spriteBatch, screenCenter, 6, Color.Blue, false, 1.0f);
            }
            spriteBatch.End();

			base.Draw(gameTime);
		}

        public void HandleCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair) {
            //TODO: ...
        }

        public static String VectorToString(Vector3 v) {
            return "(" + v.X.ToString("0.00") + " | " + v.Y.ToString("0.00") + " | " + v.Z.ToString("0.00") + ")";
        }

        public static Entity GetEntityFromRay(Vector3 position, Vector3 direction, out RayHit rayHit) {
            Entity entityHit = null;
            float minDist = float.PositiveInfinity;
            rayHit = new RayHit();
            rayHit.T = minDist;
            Ray ray = new Ray(position, direction);
            foreach (Entity entity in Entity.All) {
                if(entity is GameEntity) { //TODO: nicht nötig sobald GameEntity in Entity integirert ist
                    GameEntity ge = (GameEntity)entity;
                    foreach(var physicsEntity in ge.PhysicsEntities) {
                        RayHit trh;
                        physicsEntity.CollisionInformation.RayCast(ray, float.PositiveInfinity, out trh);
                        float dist = trh.T;
                        if (dist > 0.0f) {
                            if (dist < minDist) {
                                minDist = dist;
                                rayHit = trh;
                                entityHit = entity;
                            }
                        }
                    }
                }
            }
            return entityHit;
        }

        //TODO: Entity from mouse cursor on screen

        public static Entity GetEntityFromRay(Vector3 position, Vector3 direction) {
            RayHit rayHit;
            return GetEntityFromRay(position, direction, out rayHit);
        }
	}
}
