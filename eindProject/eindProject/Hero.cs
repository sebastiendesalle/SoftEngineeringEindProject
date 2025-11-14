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
        

        public Hero(Texture2D texture)
        {
            this.heroTexture = texture;
            animation = new Animation();
            animation.AddFrame(new AnimationFrame(new Rectangle(0, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(64, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(128, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(192, 64, 64, 64)));
            animation.AddFrame(new AnimationFrame(new Rectangle(256, 64, 64, 64)));
            speed = new Vector2(1, 1);
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
                5.0f, // change size here
                SpriteEffects.None,
                0f
            );
        }

        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
            Move();
        }

        private void Move()
        {
            position += speed;
            if (position.X > 800 - 280 || position.X < 0)
            {
                speed.X *= -1;
            }
            if (position.Y > 480 - 320 || position.Y < 0)
            {
                speed.Y *= -1;
            }
        }
    }
}
