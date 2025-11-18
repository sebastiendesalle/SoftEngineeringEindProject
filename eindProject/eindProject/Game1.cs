using eindProject.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace eindProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D heroTexture;
        private Texture2D pixelTexture; // For debugging walls
        private Hero hero;

        private List<Rectangle> _obstacles;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            heroTexture = Content.Load<Texture2D>("GoblinKingSpriteSheet");

            // create white pixel texture to draw walls
            pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            pixelTexture.SetData(new Color[] { Color.White });

            // make and place obstacles
            _obstacles = new List<Rectangle>();
            _obstacles.Add(new Rectangle(0, 400, 800, 50));   // Main Floor
            _obstacles.Add(new Rectangle(200, 300, 200, 30)); // Floating Platform
            _obstacles.Add(new Rectangle(500, 200, 50, 200)); // Tall Wall
            //_obstacles.Add(new Rectangle(500, 200, 20, 40)); // TODO: see why this isn't being added

            // input and bounds
            var inputReader = new KeyboardReader();
            var viewport = GraphicsDevice.Viewport;
            var playArea = new Rectangle(0, 0, viewport.Width, viewport.Height);

            // create hero
            hero = new Hero(
                heroTexture,
                inputReader,
                _obstacles,
                playArea,
                new Vector2(50, 50), // Start Position
                scale: 2f //hero scale
            );
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            hero.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // draw the obstacles so they aren't inviz
            foreach (var rect in _obstacles)
            {
                spriteBatch.Draw(pixelTexture, rect, Color.Black);
            }

            hero.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}