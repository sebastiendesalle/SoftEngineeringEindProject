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
        private Vector2 position;
        private Vector2 velocity; // Speed + Direction

        private IInputReader inputReader;
        private List<Rectangle> obstacles; // walls etc.
        private Rectangle bounds; // screen bounds

        // Physics Config
        private float moveSpeed = 200f;
        private float jumpStrength = 600f; // jumpheight
        private float gravity = 1000f; // fall speed

        private float scale;
        private Vector2 spriteSize;
        private bool isGrounded; // check if we are standing on something

        public Hero(Texture2D texture, IInputReader inputReader, List<Rectangle> obstacles, Rectangle bounds, Vector2 startPos, float scale = 5f)
        {
            this.heroTexture = texture;
            this.inputReader = inputReader;
            this.obstacles = obstacles;
            this.bounds = bounds;
            this.scale = scale;
            this.position = startPos;

            // use sprite coords
            animation = new Animation();
            animation.AddFrame(new AnimationFrame(new Rectangle(0, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(64, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(128, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(192, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(256, 64, 64, 64)));

            var src = animation.CurrentFrame.SourceRectangle;
            spriteSize = new Vector2(src.Width * scale, src.Height * scale);
        }

        // Helper to get the current physics box
        private Rectangle CollisionBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)spriteSize.X, (int)spriteSize.Y);
            }
        }

        public void Update(GameTime gameTime)
        {
            Move(gameTime);
            animation.Update(gameTime);
        }

        private void Move(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 input = inputReader.ReadInput();

            // walk on x axis
            velocity.X = input.X * moveSpeed;

            // jump logic
            if (isGrounded && input.Y > 0)
            {
                velocity.Y = -jumpStrength; // negative Y means up
                isGrounded = false;
            }

            // set gravity
            velocity.Y += gravity * delta;

            // x movement and collision
            position.X += velocity.X * delta;

            foreach (var obstacle in obstacles)
            {
                if (CollisionBox.Intersects(obstacle))
                {
                    if (velocity.X > 0) // Moving Right
                        position.X = obstacle.Left - spriteSize.X;
                    else if (velocity.X < 0) // Moving Left
                        position.X = obstacle.Right;
                }
            }

            // y movement and collision
            position.Y += velocity.Y * delta;
            isGrounded = false;

            foreach (var obstacle in obstacles)
            {
                if (CollisionBox.Intersects(obstacle))
                {
                    if (velocity.Y > 0) // to floor
                    {
                        position.Y = obstacle.Top - spriteSize.Y;
                        velocity.Y = 0;
                        isGrounded = true;
                    }
                    else if (velocity.Y < 0) // to ceiling
                    {
                        position.Y = obstacle.Bottom;
                        velocity.Y = 0;
                    }
                }
            }

            // window bounds
            float minX = bounds.X;
            float minY = bounds.Y;
            float maxX = bounds.X + bounds.Width - spriteSize.X;
            float maxY = bounds.Y + bounds.Height - spriteSize.Y;

            position.X = MathHelper.Clamp(position.X, minX, maxX);
            position.Y = MathHelper.Clamp(position.Y, minY, maxY);

            // Check if we hit the floor of the screen
            if (position.Y >= maxY)
            {
                position.Y = maxY;
                velocity.Y = 0;
                isGrounded = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                heroTexture,
                position,
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