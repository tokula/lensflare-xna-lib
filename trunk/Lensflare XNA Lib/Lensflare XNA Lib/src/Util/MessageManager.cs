using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Util {
    public class MessageManager {
        LinkedList<String> messages = new LinkedList<String>();
        float currentTimeToLiveForMessage;

        public SpriteBatch SpriteBatch { get; set; }
        public SpriteFont Font { get; set; }
        public Vector2 DrawPosition { get; set; }
        public Color TextColor { get; set; }
        public float TimeToLive { get; set; }
        public int MessageLimit { get; set; }

        public int MessageCount { get { return messages.Count;  } }
        public String CurrentMessage { get { return messages.Count > 0 ? messages.First() : null; } }
        
        public MessageManager(SpriteBatch spriteBatch, SpriteFont font, Vector2 drawPosition, Color textColor, float timeToLive) {
            SpriteBatch = spriteBatch;
            Font = font;
            DrawPosition = drawPosition;
            TextColor = textColor;
            TimeToLive = timeToLive;
            MessageLimit = 1;
        }

        public void AddMessage(String message) {
            if (messages.Count == 0) {
                ResetViewTime();
            } else {
                while (messages.Count >= MessageLimit) {
                    messages.RemoveFirst();
                    ResetViewTime();
                }
            }
            messages.AddLast(message);
        }

        public void ClearMessages() {
            messages.Clear();
        }

        private void ResetViewTime() {
            currentTimeToLiveForMessage = TimeToLive;
        }

        public void Update(GameTime gameTime) {
            if (messages.Count > 0) {
                float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
                currentTimeToLiveForMessage -= elapsedSeconds;
                if (currentTimeToLiveForMessage < 0) {
                    currentTimeToLiveForMessage = 0;

                    messages.RemoveFirst();
                    if (messages.Count > 0) {

                        ResetViewTime();
                    }
                }
            }
        }

        public void Draw() {
            if (currentTimeToLiveForMessage > 0) {
                float alphaFactor = currentTimeToLiveForMessage / TimeToLive;
                Color color = TextColor * alphaFactor * 4.0f;
                if (messages.Count > 0) {
                    SpriteBatch.DrawString(Font, messages.First(), DrawPosition, color);
                }
            }
        }
    }
}
