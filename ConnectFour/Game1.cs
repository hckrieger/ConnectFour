using ConnectFour.Scenes;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConnectFour
{
    public class Game1 : ExtendedGame
    {
        public const string MainScene = "Scene_Main";
        public const string TitleScreen = "TitleScreen";
        public Game1()
        {
            IsMouseVisible = true;

            windowSize = new Point(700, 725);
            worldSize = new Point(700, 725);

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            GameStateManager.AddGameState(MainScene, new MainScene(worldSize));
            GameStateManager.AddGameState(TitleScreen, new TitleScreen(worldSize));

            GameStateManager.SwitchTo(TitleScreen);
            // TODO: use this.Content to load your game content here
        }
    }
}
