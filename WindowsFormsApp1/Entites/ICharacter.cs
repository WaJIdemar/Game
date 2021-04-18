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


        void PlayAnimation(Graphics g);


        void SetAnimationConfiguration(int currentAnimation);
        
    }
}
