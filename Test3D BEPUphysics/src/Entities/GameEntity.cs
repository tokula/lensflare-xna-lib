using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LensflareGameFramework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EngineTest {
    abstract class GameEntity : Entity { //TODO: evtl nach engine verschieben (mögliches problem: Game1 referenz)
        protected List<BEPUphysics.Entities.Entity> physicsEntities = new List<BEPUphysics.Entities.Entity>();

        public Game3D Game { get; private set; }
        virtual public Texture2D Texture { get; set; }
        virtual public Texture2D BumpMapTexture { get; set; }
        public List<BEPUphysics.Entities.Entity> PhysicsEntities { get { return physicsEntities; } }

        public GameEntity(Game3D game, Texture2D texture) {
            Game = game;
			this.Texture = texture;
		}

        protected override void Dispose() {
            foreach (var e in physicsEntities) {
                e.CollisionInformation.Events.RemoveAllEvents();
                Game.space.Remove(e);
            }
            physicsEntities.Clear();
            physicsEntities = null;
            Game = null;
            Texture = null;
        }

        protected void DrawShape(Matrix worldMatrix, Shape3.Shape shape) {
            Effect effect = Game.engine.Effect;

            const float ambientComponent = 0.3f;

            if (Game.engine.Effect is BasicEffect) {
                BasicEffect basicEffect = (BasicEffect)Game.engine.Effect;
                basicEffect.World = worldMatrix;
                basicEffect.Projection = Game.camera.ProjectionMatrix;
                basicEffect.View = Game.camera.ViewMatrix;
                basicEffect.VertexColorEnabled = false;
                basicEffect.LightingEnabled = true;
                basicEffect.TextureEnabled = true;
                basicEffect.Texture = Texture;
                
                basicEffect.AmbientLightColor = new Vector3(ambientComponent, ambientComponent, ambientComponent);
            } else {
                /*
                effect.Parameters["WorldViewProjection"].SetValue(worldMatrix * Game.camera.ViewMatrix * Game.camera.ProjectionMatrix);
                effect.Parameters["xTexture"].SetValue(Texture);
                effect.Parameters["World"].SetValue(worldMatrix);
                effect.Parameters["LightsWorldViewProjection"].SetValue(worldMatrix * Game.engine.lightsViewProjectionMatrix);
                */

                effect.Parameters["World"].SetValue(worldMatrix);
                effect.Parameters["View"].SetValue(Game.camera.ViewMatrix);
                effect.Parameters["Projection"].SetValue(Game.camera.ProjectionMatrix);
                //effect.Parameters["ViewProjection"].SetValue(Game.camera.ViewMatrix * Game.camera.ProjectionMatrix);
                effect.Parameters["ColorTexture"].SetValue(Texture);
                if (BumpMapTexture != null) {
                    effect.Parameters["BumpMapTexture"].SetValue(BumpMapTexture);
                }
                effect.Parameters["HasBumpMapTexture"].SetValue(BumpMapTexture != null);


                effect.Parameters["LightPos"].SetValue(Game.engine.lightPos);
                effect.Parameters["LightPower"].SetValue(1.0f);
                effect.Parameters["Ambient"].SetValue(ambientComponent);
            }

            foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                pass.Apply();
                shape.Draw();
            }
        }
    }
}
