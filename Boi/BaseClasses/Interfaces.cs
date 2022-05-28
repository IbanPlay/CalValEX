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

    public class BoiPlayer : BoiEntity
    {
        public static float InteractionRadius = 5;

        public void ProcessControls()
        {
            return;
        }
    }

    public enum Factions
    {
        ally,
        enemy
    }

    /// <summary>
    /// Used for any entity that can deal damage, such as bullets or enemies with contact damage.
    /// </summary>
    public interface IDamageDealer
    {
        /// <summary>
        /// What faction is this entity able to deal damage to?
        /// </summary>
        public Factions[] hostileTo
        {
            get;
        }

        /// <summary>
        /// Checks if for any given active hurtbox, this entity collides with it.
        /// </summary>
        /// <param name="hurtbox">The hurtbox of the entity you're checking if you're hittign</param>
        /// <returns>Wether or not a collision happened</returns>
        public bool HitCheck(Rectangle hurtbox);

        /// <summary>
        /// Returns the amount of damage this should deal
        /// You can use this function to add extra on-hit effects as well.
        /// Do not reduce the target's health in this method though. This is handled automatically
        /// </summary>
        /// <returns>The damage dealt</returns>
        public float DealDamage(BoiEntity target);
    }

    /// <summary>
    /// Used for any entity that can take damage.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// The health of the entity
        /// </summary>
        public float Health 
        {
            get;        
        }

        /// <summary>
        /// What faction does this entity belong to?
        /// </summary>
        public Factions Faction
        {
            get;
        }

        /// <summary>
        /// Gets the hurtbox of the entity
        /// </summary>
        public Rectangle Hurtbox
        {
            get;
        }

        /// <summary>
        /// Can the entity be hit currently?
        /// </summary>
        public bool Vulnerable => true;

        /// <summary>
        /// Called when the entity gets hit. Use for any on hit effects.
        /// </summary>
        public void TakeHit(float damageTaken) { }

        /// <summary>
        /// Called when the entity hits zero health.
        /// Return true if the entity's health should be checked again after this method is called. Use this if you want ressurection effects / Death prevention.
        /// </summary>
        public bool Die() { return false; }
    }

    /// <summary>
    /// Any entity that can prevent movement
    /// The hitbox should be convex. If you need concave shapes , use more convex objects
    /// </summary>
    public interface ICollidable
    {
        /// <summary>
        /// This should represent the longest distance a point can be from the center of this object and still collide with it.
        /// E.G, a round rock of radius 1 would return 1. A square tile would return the value of one half its diagonal, since the distance between the center and the corner of a square is the longest distance
        /// </summary>
        public float SimulationDistance
        {
            get;
        }

        /// <summary>
        /// Returns the distance between the object's surface and a potentially colliding entity.
        /// Example : This would return 0.5 if the object is a sphere of radius 1, and the potentially colliding entity was 1.5 units of distance from the spheres center.
        /// </summary>
        /// <param name="position">The position of the potentially colliding entity</param>
        /// <returns></returns>
        public float MovementCheck(Vector2 position);


        /// <summary>
        /// What happens on a collision with an entity. If you want to hurt them on contact though, it would be wiser to implement an IDamageDealer interface to the object.
        /// </summary>
        /// <param name="collider">What entity collided with this object</param>
        public void OnCollide(BoiEntity collider) { }

    }

    /// <summary>
    /// Represents an entity that can collide with collidable objects.
    /// </summary>
    public interface IColliding
    {
        /// <summary>
        /// How big the radius of collision is for this entity.
        /// </summary>
        public float CollisionCircleRadius
        {
            get;
        }

        /// <summary>
        /// If the collision of this object is currently on. If the entity just never collides, please simply don't implement this interface uwu
        /// </summary>
        public bool CanCollide
        {
            get;
        }

        /// <summary>
        /// What happens on a collision.
        /// </summary>
        public void OnCollide(BoiEntity collider) { }
    }

    /// <summary>
    /// Represents an object the player can interact with
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// How big the radius of collision is for this interactable.
        /// </summary>
        public float CollisionCircleRadius
        {
            get;
        }

        /// <summary>
        /// Can the player interact with this?
        /// Useful if you want certain items to only be pickable up with a keybind.
        /// </summary>
        public bool CanBeInteractedWith => true;

        /// <summary>
        /// What happens when the item gets interacted with
        /// Return true to kill the interactable after the itneraction
        /// </summary>
        /// <param name="player">The player that interacted with the entity</param>
        public bool Interact(BoiPlayer player);
    }

    /// <summary>
    /// Represents an entity that can be drawn
    /// </summary>
    public interface IDrawable
    {

        /// <summary>
        /// The layer this should be drawn at. 
        /// Higher number = gets drawn above everything else on a lower layer.
        /// </summary>
        public int Layer => 0;

        public void Draw(SpriteBatch spriteBatch, Vector2 offset, float scale);
    }
}