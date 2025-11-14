using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eindProject
{
    internal class Hero : IGameObject
    {
        private Texture2D heroTexture;
        private Rectangle deelRectangle;

        private int moveOn_X = 0;

        public Hero(Texture2D texture)
        {
            this.heroTexture = texture;
            deelRectangle = new Rectangle(moveOn_X, 64, 64, 64);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                heroTexture,
                new Vector2(0, 0),
                deelRectangle,
                Color.White,
                0f,
                Vector2.Zero,
                5.0f, // change size here
                SpriteEffects.None,
                0f
            );
        }

        public void Update()
        {
            moveOn_X += 64;
            if (moveOn_X > 384)
            {
                moveOn_X = 0;
            }
            deelRectangle.X = moveOn_X;
        }
    }
}
