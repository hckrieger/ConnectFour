using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectFour.Scenes
{
    class TitleScreen : GameState
    {
        TextGameObject title = new TextGameObject("title", 1f, Color.White, TextGameObject.Alignment.Center);
        TextGameObject press = new TextGameObject("restartPrompt", 1f, Color.White, TextGameObject.Alignment.Center);
        TextGameObject credit = new TextGameObject("credit", 1f, Color.White, TextGameObject.Alignment.Center);

        float timer, startTimer;

        public TitleScreen(Point screenSize)
        {
            ExtendedGame.BackgroundColor = Color.Black;

            title.LocalPosition = new Vector2(screenSize.X / 2, 75);
            title.Text = "Connect Four";
            gameObjects.AddChild(title);

            press.LocalPosition = new Vector2(screenSize.X / 2, 350);
            press.Text = "Press space to play";
            gameObjects.AddChild(press);

            credit.LocalPosition = new Vector2(screenSize.X / 2, screenSize.Y - 125);
            credit.Text = "  programmed by Hunter Krieger\n\n" +
                          "property of Milton Bradley/Hasbro";
            gameObjects.AddChild(credit);

            timer = .666f;
            startTimer = timer;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0)
            {
                if (press.Visible)
                    press.Visible = false;
                else
                    press.Visible = true;

                timer = startTimer;
            }

        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if (inputHelper.KeyPressed(Keys.Space))
            {
                ExtendedGame.BackgroundColor = Color.DarkGray;
                ExtendedGame.GameStateManager.SwitchTo(Game1.MainScene);
            }

        }
    }
}
