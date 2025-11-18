using eindProject.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;

namespace eindProject
{
    internal class Hero : IGameObject
    {
        private Texture2D heroTexture;
        Animation animation;
        private Vector2 position;
        private float moveSpeed; // pixels per second
        private IInputReader inputReader;
        private Rectangle bounds; // play area
        private float scale;
        private Vector2 spriteSize;

        public Hero(Texture2D texture, IInputReader inputReader, Rectangle bounds, float moveSpeed = 200f, float scale = 5f)
        {
            // use sprite frames
            this.heroTexture = texture;
            this.inputReader = inputReader;
            this.bounds = bounds;
            this.moveSpeed = moveSpeed;
            this.scale = scale;
            animation = new Animation();
            // initial position
            position = new Vector2(1, 1);

            //frames
            animation.AddFrame(new AnimationFrame(new Rectangle(0, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(64, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(128, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(192, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(256, 64, 64, 64)));

            var src = animation.CurrentFrame.SourceRectangle;
            spriteSize = new Vector2(src.Width * scale, src.Height * scale);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw the sprite
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


        public void Update(GameTime gameTime)
        {
            Move(gameTime);
            animation.Update(gameTime);
        }

        private void Move(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // read input
            Vector2 input = inputReader.ReadInput();
            if (input != Vector2.Zero && input.LengthSquared() > 1f)
            {
                input.Normalize();
            }

            //time-based movement, better than fps based movement ofc
            Vector2 movement = input * moveSpeed * delta;
            position += movement;

            //bounds
            float minX = bounds.X;
            float minY = bounds.Y;
            float maxX = bounds.X + bounds.Width - spriteSize.X;
            float maxY = bounds.Y + bounds.Height - spriteSize.Y;

            // stop hero from moving further
            position.X = MathHelper.Clamp(position.X, minX, maxX);
            position.Y = MathHelper.Clamp(position.Y, minY, maxY);
        }
    }
}
