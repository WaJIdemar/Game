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
        int MaxHealth { get; set; }
        int Size { get; set; }
        int posX { get; set; }
        int posY { get; set; }
        Image SpriteForBattle { get; set; }
        Image SpriteFace { get; set; }
        int AttackPower { get; set; }
        void PlayAnimation(Graphics g, int posX, int posY, int size);


        void SetAnimationConfiguration(int currentAnimation);

    }
}
