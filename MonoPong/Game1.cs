using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch ballSprite;
        SpriteBatch playerPaddleSprite;
        SpriteBatch AIPaddleSprite;
        Texture2D ballTexture;
        Texture2D paddleTexture;
        Vector2 ballPosition;
        Vector2 playerPaddlePosition;
        Vector2 AIPaddlePosition;
        int direction;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ballPosition = new Vector2(0, 100);
            playerPaddlePosition = new Vector2(10, 20);
            AIPaddlePosition = new Vector2(750, 70);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ballTexture = new Texture2D(this.GraphicsDevice, 100, 100);
            Color[] ballColorData = new Color[100 * 100];
            for (int i = 0; i < 10000; i++)
            {
                ballColorData[i] = Color.White;
            }   
            ballTexture.SetData<Color>(ballColorData);

            paddleTexture = new Texture2D(this.GraphicsDevice, 20, 100);
            Color[] paddleColorData = new Color[20 * 100];
            for (int i = 0; i < 2000; i++)
            {
                paddleColorData[i] = Color.White;
            }
            paddleTexture.SetData<Color>(paddleColorData);

            base.Initialize();

            direction = 1;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            ballSprite = new SpriteBatch(GraphicsDevice);
            playerPaddleSprite = new SpriteBatch(GraphicsDevice);
            AIPaddleSprite = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (direction == 1)
                ballPosition.X += 1;
            else
                ballPosition.X -= 1;
            if (ballPosition.X > this.GraphicsDevice.Viewport.Width-100)
                direction = 0;
            if (ballPosition.X < 0)
                direction = 1;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            ballSprite.Begin();
            ballSprite.Draw(ballTexture, ballPosition, Color.White);
            ballSprite.End();
            playerPaddleSprite.Begin();
            playerPaddleSprite.Draw(paddleTexture, playerPaddlePosition, Color.White);
            playerPaddleSprite.End();
            AIPaddleSprite.Begin();
            AIPaddleSprite.Draw(paddleTexture, AIPaddlePosition, Color.White);
            AIPaddleSprite.End();
            base.Draw(gameTime);
        }
    }
}
