﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LensflareGameFramework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EngineTest {
    abstract class GameEntity : Entity { //TODO: evtl nach engine verschieben (mögliches problem: Game1 referenz)
        public Game1 Game { get; private set; }
        public Texture2D Texture { get; set; }

        public GameEntity(Game1 game, Texture2D texture) {
            Game = game;
			this.Texture = texture;
		}

        protected override void Dispose() {
            Game = null;
            Texture = null;
        }

        protected void DrawShape(Matrix worldMatrix, Shape3D.Shape shape) {
            /*
            effect.World = physicsBox.WorldTransform;
            effect.VertexColorEnabled = false;
            effect.LightingEnabled = true;
            effect.TextureEnabled = true;
            effect.Texture = texture;
            */

            Effect effect = Game.engine.effect;
            effect.Parameters["xWorldViewProjection"].SetValue(worldMatrix * Game.camera.ViewMatrix * Game.camera.ProjectionMatrix);
            effect.Parameters["xTexture"].SetValue(Texture);
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xLightsWorldViewProjection"].SetValue(worldMatrix * Game.engine.lightsViewProjectionMatrix);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                pass.Apply();
                shape.Draw();
            }
        }
    }
}