using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalValEX.Projectiles.Pets.LightPets
{
    public class HeatPet : FlyingPet
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cinder Spirit");
            Main.projFrames[projectile.type] = 4; //frames
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true; //this is needed for a light pet.
        }

        public override void SafeSetDefaults() //SafeSetDefaults!!!
        {
            projectile.width = 40;
            projectile.height = 24;
            projectile.ignoreWater = true;
            /* you don't need to set these anymore!
            projectile.penetrate = -1;
            projectile.netImportant = true;
            projectile.timeLeft *= 5;
            projectile.friendly = true;
            projectile.tileCollide = false;
            */
            facingLeft = true; //is the sprite facing left? if so, put this to true. if its facing to right keep it false.
            spinRotation = false; //should it spin? if that's the case, set to true. else, leave it false.
            shouldFlip = true; //should the sprite flip? set true if it should, false if it shouldnt
            usesAura = false; //does this pet use an aura?
            usesGlowmask = true; //does this pet use a glowmask?
            auraUsesGlowmask = false; //does the aura use a glowmask?
        }
        public override void SetUpAuraAndGlowmask()
        {
     	    glowmaskTexture = "Projectiles/Pets/LightPets/HeatPet_Glow";
            //glowmaskTexture = CalValEX.month == 12 "ExtraTextures/ChristmasPets/HeatPetGlow" ? : "Projectiles/Pets/LightPets/HeatPet_Glow";
        }

        public override void SetUpFlyingPet()
        {
            distance[0] = 1840f; //teleport distance
            distance[1] = 560f; //faster speed distance
            speed = 12f;
            inertia = 60f;
            animationSpeed = 4; //how fast the animation should play
            spinRotationSpeedMult = 0f; //rotation speed multiplier, keep it positive for it to spin in the right direction
            offSetX = -11f * -Main.player[projectile.owner].direction; //this is needed so it's always behind the player.
            offSetY = -21f; //how much higher from the center the pet should float
        }

        //you usualy don't have to use the lower two unless you want the pet to have an aura, glowmask
        //or if you want the pet to emit light

        public override void SetUpLight() //for when the pet emmits light
        {
            shouldLightUp = true; //should the pet glow? true if it should, false if it shouldn't
            RGB = new Vector3(253, 19, 43); //should only go up to 255 and as low as 0
            intensity = 0.70f; //how intense the light should be. do not go over 2f or else the light will be too strong
            abyssLightLevel = 2; //for abyss light level to work shouldLightUp must be set up true.
        }

        public override void SafeAI(Player player)
        {
            CalValEXPlayer modPlayer = player.GetModPlayer<CalValEXPlayer>();

            if (player.dead)
                modPlayer.mHeat = false;
            if (modPlayer.mHeat)
                projectile.timeLeft = 2;

            /* THIS CODE ONLY RUNS AFTER THE MAIN CODE RAN.
             * for custom behaviour, you can check if the projectile is walking or not via projectile.localAI[1]
             * you should make new custom behaviour with numbers higher than 0, or less than 0
             * the next few lines is an example on how to implement this
             *
             * switch ((int)projectile.localAI[1])
             * {
             *     case -1:
             *         break;
             *     case 1:
             *         break;
             * }
             *
             * 0 is already in use.
             * 0 = flying
             *
             * you can still use this, changing thing inside (however it's not recomended unless you want to add custom behaviour to this)
             */
        }
    }
}