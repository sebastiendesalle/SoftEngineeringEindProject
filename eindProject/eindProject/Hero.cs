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
        private Vector2 speed;
        private IInputReader inputReader;
        

        public Hero(Texture2D texture, IInputReader inputReader)
        {
            // use sprite frames
            this.heroTexture = texture;
            this.inputReader = inputReader;
            animation = new Animation();
            position = new Vector2(1, 1);
            speed = new Vector2(2, 2);
            animation.AddFrame(new AnimationFrame(new Rectangle(0, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(64, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(128, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(192, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(256, 64, 64, 64)));
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
                5.0f, // change size here
                SpriteEffects.None,
                0f
            );
        }


        public void Update(GameTime gameTime)
        {
            Move();
            animation.Update(gameTime);
        }

        private void Move()
        {
            // read input
            Vector2 input = inputReader.ReadInput();
            if (input != Vector2.Zero && input.LengthSquared() > 1f)
            {
                input.Normalize();
            }

            // make movement using speed per axis
            Vector2 movement = new Vector2(input.X * speed.X, input.Y * speed.Y);
            position += movement;
            //bounds
            const float minX = 0f;
            const float minY = 0f;
            const float maxX = 800 - 280;
            const float maxY = 480 - 320;

            // stop hero from moving further
            position.X = MathHelper.Clamp(position.X, minX, maxX);
            position.Y = MathHelper.Clamp(position.Y, minY, maxY);
        }
    }
}
