using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CalValEX.Boi.BaseClasses
{
	public class BoiHandler
	{
		public static Vector2 playingField = new Vector2(500, 500);
		public List<BoiRoom> Map;
		public List<BoiEntity> Entities;

		public void Run()
        {
			//Process the players movement and actions


			//For each entity
			//-Run their update function (and the UpdateEffect functions of their inventory slots
			//-If they either are not an IColliding, or are one but have CanCollide set to false, update their position based on their current velocity. If they aren't either, add them to a list
			//-After updating their position, check if they are still within the bounds of the playing field. If not, either clamp their position to it, or kill them (depends on if their DestroyOnRoomEdgeContact variable is set to true or not) (if it is, call the EdgeContactDestroy method)

			//-If IDamageable & Vulnerable is true, store them in a list. Additioanlly, get a dictionnary that pairs them with their hurtbox
			//-If IDamageDealer, store them in a list
			//-If IInteractable, store them in a list
			//-You get the idea. Do that for all, but the Idrawable

			//For each IDamageDealer run the HitCheck function on all IDamageable hurtboxes that can are of a faction that they are hostile to. 
			//If the HitCheck returns true, run the DealDamage function to get the damage dealt by them.
			//Call the DamageTaken function of any of the IDamageable entity that were hit
			//If their health is now under zero, run the Die function, then clear them from the Entities list, unless the function returns true. In which case, check if their health is under zero again (until either they have more than zero hp, or the function returns false)

			//For each entity that is IColliding and has CanCollide set to true (as listed above)
			//Check for all IColliders, and grab their SimulationDistance.
			//If the simulation distance + the CollisionCircleRadius of the IColliding is higher than the distance between the two entities, thats awesome, don't even do anything about it and go to the next one
			//If it ISNT, call the MovementCheck function and provide the position of the entity with their current velocity added
			//If the distance obtained from the MovementCheck function added to the CollisionCircleRadius is lower than the distance between the two entities, call both their onCollide function, and cancel the movement of the IColliding

			//If all the collisions checks passed, update their position based on their current velocity. Do the same checks about the room position and everything

			//For each IInteractable, check if the player is in their InteractionRadius and if they have CanBeInteracted to true. If it is the case, simply just run their Interact() function.
			//Kill the interactable if the Interact function returns true


			//Clear all the lists created earlier
		}

		public void Draw()
        {
			//Draw bg
			//Draw doors and such
			//For each entity in Entities, if its an IDrawable, store them in new lists, separated on their Layer
			//Then for all these lists, draw the IDrawables with the proper scale and offset.
        }
	}
}