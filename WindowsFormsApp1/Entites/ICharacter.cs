using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Движение.Entites
{
    interface ICharacter
    {
        public void Move()
        {
            throw new NotImplementedException();
        }

        public void ResetMove()
        {
            throw new NotImplementedException();
        }

        public void PlayAnimation(Graphics g)
        {
            throw new NotImplementedException();
        }

        public void SetAnimationConfiguration(int currentAnimation)
        {
            throw new NotImplementedException();
        }
    }
}
