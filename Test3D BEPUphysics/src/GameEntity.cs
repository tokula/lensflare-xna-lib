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
        public Texture2D Texture { get; set; }
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

            if (Game.engine.Effect is BasicEffect) {
                BasicEffect basicEffect = (BasicEffect)Game.engine.Effect;
                basicEffect.World = worldMatrix;
                basicEffect.Projection = Game.camera.ProjectionMatrix;
                basicEffect.View = Game.camera.ViewMatrix;
                basicEffect.VertexColorEnabled = false;
                basicEffect.LightingEnabled = true;
                basicEffect.TextureEnabled = true;
                basicEffect.Texture = Texture;

                const float ambientComponent = 0.3f;
                basicEffect.AmbientLightColor = new Vector3(ambientComponent, ambientComponent, ambientComponent);
            } else {
                /*
                effect.Parameters["WorldViewProjection"].SetValue(worldMatrix * Game.camera.ViewMatrix * Game.camera.ProjectionMatrix);
                effect.Parameters["xTexture"].SetValue(Texture);
                effect.Parameters["World"].SetValue(worldMatrix);
                effect.Parameters["LightsWorldViewProjection"].SetValue(worldMatrix * Game.engine.lightsViewProjectionMatrix);
                */
            }

            foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                pass.Apply();
                shape.Draw();
            }
        }
    }
}
