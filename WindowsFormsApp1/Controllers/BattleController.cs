using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Движение.Entites;
using Движение.Controllers;
using Движение.Models;

namespace Движение.Controllers
{
    class BattleController
    {
        
        public void BattleSceneDraw(Entites.Hero player, OrangeMonster[] monsters, Image background, Graphics graphics)
        {
            graphics.DrawImageUnscaled(background, new Point(0, 0));
            
        }
    }
}
