﻿using CalValEX.Items.Tiles.Banners;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalValEX.Items.Critters;

namespace CalValEX.NPCs.Critters
{
    /// <summary>
    /// This file shows off a critter npc. The unique thing about critters is how you can catch them with a bug net.
    /// The important bits are: Main.npcCatchable, npc.catchItem, and item.makeNPC
    /// We will also show off adding an item to an existing RecipeGroup (see ExampleMod.AddRecipeGroups)
    /// </summary>
    internal class SandTurtle : ModNPC
    {
        int heal = 0;
        private bool shellin;
        private bool shellout;
        public override bool Autoload(ref string name)
        {
            IL.Terraria.Wiring.HitWireSingle += HookStatue;
            return base.Autoload(ref name);
        }

        /// <summary>
        /// Change the following code sequence in Wiring.HitWireSingle
        /// num12 = Utils.SelectRandom(Main.rand, new short[5]
        /// {
        /// 	359,
        /// 	359,
        /// 	359,
        /// 	359,
        /// 	360,
        /// });
        ///
        /// to
        ///
        /// var arr = new short[5]
        /// {
        /// 	359,
        /// 	359,
        /// 	359,
        /// 	359,
        /// 	360,
        /// }
        /// arr = arr.ToList().Add(id).ToArray();
        /// num12 = Utils.SelectRandom(Main.rand, arr);
        ///
        /// </summary>
        /// <param name="il"></param>
        private void HookStatue(ILContext il)
        {
            // obtain a cursor positioned before the first instruction of the method
            // the cursor is used for navigating and modifying the il
            var c = new ILCursor(il);

            // the exact location for this hook is very complex to search for due to the hook instructions not being unique, and buried deep in control flow
            // switch statements are sometimes compiled to if-else chains, and debug builds litter the code with no-ops and redundant locals

            // in general you want to search using structure and function rather than numerical constants which may change across different versions or compile settings
            // using local variable indices is almost always a bad idea

            // we can search for
            // switch (*)
            //   case 56:
            //     Utils.SelectRandom *

            // in general you'd want to look for a specific switch variable, or perhaps the containing switch (type) { case 105:
            // but the generated IL is really variable and hard to match in this case

            // we'll just use the fact that there are no other switch statements with case 56, followed by a SelectRandom

            ILLabel[] targets = null;
            while (c.TryGotoNext(i => i.MatchSwitch(out targets)))
            {
                // some optimising compilers generate a sub so that all the switch cases start at 0
                // ldc.i4.s 51
                // sub
                // switch
                int offset = 0;
                if (c.Prev.MatchSub() && c.Prev.Previous.MatchLdcI4(out offset))
                {
                    ;
                }

                // get the label for case 56: if it exists
                int case56Index = 56 - offset;
                if (case56Index < 0 || case56Index >= targets.Length || !(targets[case56Index] is ILLabel target))
                {
                    continue;
                }

                // move the cursor to case 56:
                c.GotoLabel(target);
                // there's lots of extra checks we could add here to make sure we're at the right spot, such as not encountering any branching instructions
                c.GotoNext(i => i.MatchCall(typeof(Utils), nameof(Utils.SelectRandom)));

                // goto next positions us before the instruction we searched for, so we can insert our array modifying code right here
                c.EmitDelegate<Func<short[], short[]>>(arr =>
                {
                    // resize the array and add our custom snail
                    Array.Resize(ref arr, arr.Length + 1);
                    arr[arr.Length - 1] = (short)npc.type;
                    return arr;
                });

                // hook applied successfully
                return;
            }

            // couldn't find the right place to insert
            throw new Exception("Hook location not found, switch(*) { case 56: ...");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sand Turtle");
            Main.npcFrameCount[npc.type] = 6;
            Main.npcCatchable[npc.type] = true;
        }

        public override void SetDefaults()
        {
            //npc.width = 56;
            //npc.height = 26;
            //npc.aiStyle = 67;
            //npc.damage = 0;
            //npc.defense = 0;
            //npc.lifeMax = 2000;

            //npc.noGravity = true;
            //npc.catchItem = 2007;

            npc.CloneDefaults(NPCID.Squirrel);
            npc.catchItem = (short)ItemType<SandTurtleItem>();
            npc.lavaImmune = false;
            //npc.aiStyle = 0;
            npc.friendly = true; // We have to add this and CanBeHitByItem/CanBeHitByProjectile because of reasons.
            aiType = NPCID.Squirrel;
            animationType = -1;
            npc.npcSlots = 0.25f;
            npc.lifeMax = 300;
            npc.GivenName = Main.rand.NextFloat() < 0.00014f ? "Debrina" : "Sand Turtle";
            banner = npc.type;
            bannerItem = ItemType<Items.Tiles.Banners.SandTurtleBanner>();
            npc.HitSound = SoundID.NPCHit50;
            npc.DeathSound = SoundID.NPCDeath54;
        }
        float valax = 0;

        public override void AI()
        { 
            npc.spriteDirection = npc.direction;
            if (npc.localAI[0] != 4)
            {
                valax = Math.Abs(npc.velocity.X);
                npc.defense = 0;
                npc.dontTakeDamageFromHostiles = false;
                if (npc.velocity.X != 0 && (npc.velocity.X > 1 || npc.velocity.X < -1))
                {
                    npc.velocity.X = (npc.velocity.X / valax) * 1;
                }
                //shellout = false;
                //shellin = false;
            }
            Mod clamMod = ModLoader.GetMod("CalamityMod");
            if (npc.life <= npc.lifeMax * 0.5f && npc.localAI[0] != 4)
            {
                npc.localAI[0] = 4;
            }
            if (npc.localAI[0] == 4)
            {
                heal++;
                /*shellin = true;
                if (shellin == true && heal == 5)
                {
                    shellin = false;
                }
                if (npc.life == npc.lifeMax * 0.9f)
                {
                    shellout = true;
                }*/
                npc.velocity.X = 0;
                npc.defense = 99;
                npc.dontTakeDamageFromHostiles = true;
                if (heal == 8)
                {
                    npc.life += 1;
                    heal = 0;
                }

                if (npc.life >= npc.lifeMax)
                {
                    npc.localAI[0] = 1;
                }
            }
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode && !CalValEXConfig.Instance.CritterSpawns && spawnInfo.player.ZoneUndergroundDesert)
            {
                if (spawnInfo.playerSafe)
                {
                    return SpawnCondition.DesertCave.Chance * 0.025f;
                }
                else
                {
                    return SpawnCondition.DesertCave.Chance * 0.05f;
                }
            }
            return 0f;
        }
        public override void FindFrame(int frameHeight) //9 total frames
        {
            npc.frameCounter += 1.0;
            /*if (shellin || shellout)
            {
                npc.frame.Y = 4 * frameHeight;
            }
            else */
            if (npc.localAI[0] == 4)
            {
                npc.frame.Y = 5 * frameHeight;
            }
            else if (npc.velocity.X == 0)
            {
                npc.frame.Y = 0 * frameHeight;
            }
            else
            {
                if (npc.frameCounter > 4.0)
                {
                    npc.frame.Y = npc.frame.Y + frameHeight;
                    npc.frameCounter = 0.0;
                }
                if (npc.frame.Y >= frameHeight * 4)
                {
                    npc.frame.Y = 1;
                }
            }
            /*npc.frameCounter += 1.0;
            if (shellin || shellout)
            {
                npc.frameCounter = 4.0;
                npc.frame.Y = npc.frame.Y + frameHeight;
            }
            else if (npc.localAI[0] == 4 && !shellout && !shellin)
            {
                npc.frameCounter = 5.0;
                npc.frame.Y = npc.frame.Y + frameHeight;
            }
            else if (npc.velocity.X != 0)
            {
                if (npc.frameCounter > 2.0)
                {
                    npc.frameCounter = 1.0;
                    npc.frame.Y = npc.frame.Y + frameHeight;
                }
            }
            else
            {
                if (npc.frameCounter > 0)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = npc.frame.Y + frameHeight;
                }
            }*/
        }

        public override void OnCatchNPC(Player player, Item item)
        {
            item.stack = 1;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SandTurtle"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SandTurtle2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SandTurtle3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SandTurtle4"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SandTurtle5"), 1f);
            }
        }
    }
}