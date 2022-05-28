using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CalValEX.Boi.BaseClasses
{
    public class BoiEntity
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public List<BoiItem> Inventory;

        public bool DestroyOnRoomEdgeContact => false;
        public void EdgeContactDestroy() { };

        public virtual void OnSpawn() { }
        public virtual void Update() { }
    }

    public class BoiRoom
    {

    }

    public class BoiItem
    {
        Entity Owner;

        public void UpdateEffect() { }
    }
}
