using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LensflareGameFramework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Test2D {
    abstract class GameEntity : Entity {
        public Game2D Game { get; protected set; }

        public GameEntity(Game2D game) {
            Game = game;
        }

        protected override void Dispose() {
            Game = null;
        }
    }
}
