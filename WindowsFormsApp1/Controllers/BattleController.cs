using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Движение.Entites;
using Движение.Controllers;
using Движение.Models;

namespace Движение.Controllers
{
    class BattleController
    {
        List<ICharacter> characters;
        public void BattleSceneDraw(Hero player, ICharacter[] monsters, Image background, Graphics graphics, ref bool battleFlag)
        {
            graphics.DrawImageUnscaled(background, new Point(0, 0));
            monsters[0].PlayAnimation(graphics, background.Width * 5 / 6, background.Height * 2 / 3, 170);
            player.PlayAnimation()
        }
    }
}
