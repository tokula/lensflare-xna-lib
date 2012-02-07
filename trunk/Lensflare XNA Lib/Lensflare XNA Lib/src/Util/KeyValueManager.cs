using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Util {
    public class KeyValueManager {
        SpriteBatch spriteBatch;
        SpriteFont font;
        Vector2 drawPosition;
        Color textColor;

        const int keyIndexBegin = (int)Keys.A;
        const int keyIndexEnd = (int)Keys.Z;
        const int valueCount = keyIndexEnd - keyIndexBegin + 1;

        float[] values = new float[valueCount];
        float[] valueSteps = new float[valueCount];

        const Keys keyIncrease = Keys.PageUp;
        const Keys keyDecrease = Keys.PageDown;

        const float timeToLiveForMessages = 2.0f;
        float currentTimeToLiveForMessage;
        Keys currentKeyForMessage = Keys.None;

        public KeyValueManager(SpriteBatch spriteBatch, SpriteFont font, Vector2 drawPosition, Color textColor) {
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.drawPosition = drawPosition;
            this.textColor = textColor;

            for (int i = 0; i < valueCount; ++i) {
                valueSteps[i] = 1.0f;
            }
        }

        private void ValueChangedForKey(Keys key) {
            currentTimeToLiveForMessage = timeToLiveForMessages;
            currentKeyForMessage = key;
        }

        public float ValueForKey(Keys key) {
            int keyIndex = (int)key;
            if (keyIndex >= keyIndexBegin && keyIndex <= keyIndexEnd) {
                return values[keyIndex - keyIndexBegin];
            } else {
                return 0;
            }
        }

        public void SetValueForKey(Keys key, float value) {
            int keyIndex = (int)key;
            if (keyIndex >= keyIndexBegin && keyIndex <= keyIndexEnd) {
                values[keyIndex - keyIndexBegin] = value;
                ValueChangedForKey(key);
            }
        }

        public void SetValueStepForKey(Keys key, float value) {
            int keyIndex = (int)key;
            if (keyIndex >= keyIndexBegin && keyIndex <= keyIndexEnd) {
                valueSteps[keyIndex - keyIndexBegin] = value;
            }
        }

        public void Update(GameTime gameTime) {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboardState = Keyboard.GetState();
            for (int i = keyIndexBegin; i <= keyIndexEnd; ++i) {
                Keys key = (Keys)i;
                if (keyboardState.IsKeyDown(key)) {
                    int valueIndex = i - keyIndexBegin;
                    if (keyboardState.IsKeyDown(keyIncrease)) {
                        values[valueIndex] += valueSteps[valueIndex] * elapsedSeconds;
                        ValueChangedForKey(key);
                    } else if (keyboardState.IsKeyDown(keyDecrease)) {
                        values[valueIndex] -= valueSteps[valueIndex] * elapsedSeconds;
                        ValueChangedForKey(key);
                    }

                    int mouseWheelDelta = Input.MouseWheelDelta;
                    if (mouseWheelDelta != 0) {
                        values[valueIndex] += mouseWheelDelta * valueSteps[valueIndex] * elapsedSeconds;
                        ValueChangedForKey(key);
                    }
                }
            }

            currentTimeToLiveForMessage -= elapsedSeconds;
            if (currentTimeToLiveForMessage < 0) {
                currentTimeToLiveForMessage = 0;
            }
        }

        public void Draw() {
            if (currentTimeToLiveForMessage > 0) {
                float alphaFactor = currentTimeToLiveForMessage / timeToLiveForMessages;
                Color color = textColor * alphaFactor * 4.0f;
                String valueString = String.Format("{0:0.00}", values[(int)currentKeyForMessage - keyIndexBegin]);
                spriteBatch.DrawString(font, "" + currentKeyForMessage + " = " + valueString, drawPosition, color);
            }
        }
    }
}
