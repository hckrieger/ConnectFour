using ConnectFour.Scenes;
using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectFour.GameObjects
{
    class Checker : SpriteGameObject
    {
        public bool DropChecker { get; set; } = false;


        public Checker() : base("connect4@2x1", .3f, 1)
        {
            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (DropChecker)
                velocity.Y = 1750;
            else
                velocity.Y = 0;


        }

        public override void Reset()
        {
            base.Reset();
            LocalPosition = new Vector2(-100, -100);
            Active = false;
        }

    }
}
