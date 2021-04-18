using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Движение.Entites
{
    public interface ICharacter
    {
        void Move();

        void ResetMove();

        int Health { get; set; }

        void PlayAnimation(Graphics g, int posX, int posY, int size);

        
        void SetAnimationConfiguration(int currentAnimation);
        
    }
}
