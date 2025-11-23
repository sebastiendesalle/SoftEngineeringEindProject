using eindProject.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace eindProject
{
    internal class Hero : IGameObject
    {
        private Texture2D heroTexture;
        private Animation animation;

        private IInputReader inputReader;
        private PhysicsComponent physics;
        private float moveSpeed = 200f;

        private float scale;
        private Vector2 spriteSize;
        private Vector2 drawOffset;

        public Hero(Texture2D texture, IInputReader inputReader, List<Rectangle> obstacles, Rectangle bounds, Vector2 startPos, float scale = 5f)
        {
            this.heroTexture = texture;
            this.inputReader = inputReader;
            this.scale = scale;


            // use sprite coords
            // TODO: fix hitbox (too big)
            animation = new Animation();
            animation.AddFrame(new AnimationFrame(new Rectangle(0, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(64, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(128, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(192, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(256, 64, 64, 64)));

            // calc hitbox size
            float hitBoxWidth = 30 * scale;
            float hitBoxHeight = 50 * scale;
            spriteSize = new Vector2(hitBoxWidth, hitBoxHeight); // physics size

            // calc offset
            var src = animation.CurrentFrame.SourceRectangle;
            float visualWidth = src.Width * scale;
            float visualHeight = src.Height * scale;

            drawOffset = new Vector2(
                (visualWidth - hitBoxWidth) / 2f, // center X
                visualHeight - hitBoxHeight // center Y
            );
            physics = new PhysicsComponent(startPos, obstacles, bounds);
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // get input
            Vector2 input = inputReader.ReadInput();

            // send input to physics
            physics.ApplyMovement(input, moveSpeed, delta);

            // update physics
            physics.Update(delta, (int)spriteSize.X, (int)spriteSize.Y);
            animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawPosition = physics.Position - drawOffset;

            spriteBatch.Draw(
                heroTexture,
                drawPosition,
                animation.CurrentFrame.SourceRectangle,
                Color.White,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}