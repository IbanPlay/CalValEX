using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CalValEX.Boi.BaseClasses
{
    public class BoiPlayer : BoiEntity, IDamageable, IColliding
    {
        public static float InteractionRadius = 5;

        public void ProcessControls()
        {
            return;
        }
    }
}