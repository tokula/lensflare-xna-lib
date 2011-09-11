using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LensflareGameFramework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EngineTest {
    abstract class GameEntity : Entity {
        protected Game1 game;
        protected Texture2D texture;

        public Texture2D Texture { get { return texture; } set { texture = value; } }

        public GameEntity(Game1 game, Texture2D texture) {
            this.game = game;
			this.texture = texture;
		}

        protected override void Dispose() {
            game = null;
            texture = null;
        }

        protected void DrawShape(Matrix worldMatrix, Shape3D.Shape shape) {
            /*
            effect.World = physicsBox.WorldTransform;
            effect.VertexColorEnabled = false;
            effect.LightingEnabled = true;
            effect.TextureEnabled = true;
            effect.Texture = texture;
            */

            Effect effect = game.engine.effect;
            effect.Parameters["xWorldViewProjection"].SetValue(worldMatrix * game.camera.ViewMatrix * game.camera.ProjectionMatrix);
            effect.Parameters["xTexture"].SetValue(texture);
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xLightsWorldViewProjection"].SetValue(worldMatrix * game.engine.lightsViewProjectionMatrix);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                pass.Apply();
                shape.Draw();
            }
        }
    }
}
