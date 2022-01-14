using ConnectFour.Scenes;
using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectFour.GameObjects
{
    class BoardSection : SpriteGameObject
    {
        public Checker Contains { get; set; }


        bool checkOut;

        
        public BoardSection() : base("connect4@2x1", .9f, 0)
        {
            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Contains != null && !checkOut)
            {
                if (Contains.LocalPosition.Y >= this.GlobalPosition.Y)
                {
                    Contains.DropChecker = false;
                    Contains.LocalPosition = this.GlobalPosition - new Vector2(0, 29.5f);

                    MainScene.CheckWin = true;
                    checkOut = true;
                }
                
            }
        }

        MainScene MainScene
        {
            get { return (MainScene)ExtendedGame.GameStateManager.GetGameState(Game1.MainScene); }
        }

        public override void Reset()
        {
            base.Reset();
            checkOut = false;
            Contains = null;
        }
    }
}
