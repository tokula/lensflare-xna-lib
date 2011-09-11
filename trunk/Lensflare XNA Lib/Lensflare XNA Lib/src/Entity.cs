using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LensflareGameFramework {
	public abstract class Entity {
        static LinkedList<Entity> all = new LinkedList<Entity>();
        static LinkedList<Entity> toRemove = new LinkedList<Entity>();

        protected abstract void Dispose();
		public abstract void Update(GameTime gameTime);
		public abstract void Draw();

        public static LinkedList<Entity> All {
            get { return all; }
        }

        public static void Add(Entity entity) {
            all.AddLast(entity);
        }

        public static void Remove(Entity entity) {
            toRemove.AddLast(entity);
        }

        public static void UpdateAll(GameTime gameTime) {
            foreach (Entity entity in Entity.toRemove) {
                while (Entity.all.Contains(entity)) {
                    Entity.all.Remove(entity);
                }
                entity.Dispose();
            }
            Entity.toRemove.Clear();

            foreach (Entity entity in Entity.all) {
                entity.Update(gameTime);
            }
        }

        public static void DrawAll() {
            foreach (Entity entity in Entity.all) {
                entity.Draw();
            }
        }
	}
}
