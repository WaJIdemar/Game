using System;
using System.Collections.Generic;
using System.Drawing;
using Движение.Entites;
using Движение.Controllers;
using Движение.Models;
using System.IO;

namespace Движение.Controllers
{
    public static class BattleController
    {
        public static void BattleSceneDraw(Hero player, IEnumerable<ICharacter> monsters, Image background, Graphics graphics, ref bool battleFlag)
        {
            //graphics.DrawImage(background, new Point(0, 0));
            //.PlayAnimation(graphics, background.Width * 5 / 6, background.Height * 2 / 3, 170);
            //player.PlayAnimation(graphics, background.Width * 1 / 3, background.Height * 2 / 3, 30);
        }
    }
}
