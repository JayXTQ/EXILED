// -----------------------------------------------------------------------
// <copyright file="UpdateScp244.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Scp244
{
#pragma warning disable SA1313
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection.Emit;

    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    using HarmonyLib;

    using InventorySystem;
    using InventorySystem.Items.Usables.Scp244;
    using InventorySystem.Searching;

    using Mirror;

    using NorthwoodLib.Pools;

    using UnityEngine;

    using static HarmonyLib.AccessTools;
    /// <summary>
    /// Patches <see cref="Scp244DeployablePickup"/> to add missing event handler to the <see cref="Scp244DeployablePickup"/>.
    /// </summary>
    [HarmonyPatch(typeof(Scp244DeployablePickup), nameof(Scp244DeployablePickup.UpdateRange))]
    internal static class UpdateScp244
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label returnFalse = generator.DefineLabel();
            Label continueProcessing = generator.DefineLabel();
            Label normalProcessing = generator.DefineLabel();
#pragma warning disable SA1118 // Parameter should not span multiple lines

            int offset = 2;
            int index = newInstructions.FindIndex(instruction => instruction.Calls(PropertyGetter(typeof(Scp244DeployablePickup), nameof(Scp244DeployablePickup.State)))) + offset;

            int continueOffset = 0;
            int continueIndex = newInstructions.FindLastIndex(instruction => instruction.Calls(PropertyGetter(typeof(Scp244DeployablePickup), nameof(Scp244DeployablePickup.State)))) + continueOffset;

            /*
             *                 new(OpCodes.Ldstr, "Scp244DeployablePickup logic for transpiler"),
                new(OpCodes.Call, Method(typeof(Log), nameof(Log.Info), new[] { typeof(string) })),
                */

            // FYI this gets called A LOT, and I mean A LOT. UpdateRange might be a bad idea for an event catch but.. I'll defer to Nao or Joker.
            // However, it seems to be functional, I guess.
            newInstructions.InsertRange(index, new[]
            {
                // Load arg 0 (No param, instance of object) EStack[Scp244DeployablePickup Instance]
                new(OpCodes.Ldarg_0),

                // Load the field within the instance, since get; set; we can use PropertyGetter to get state. EStack[State]
                new(OpCodes.Callvirt, PropertyGetter(typeof(Scp244DeployablePickup), nameof(Scp244DeployablePickup.State))),

                // What to compare again (Idle State) EStack[State, 0]
                new(OpCodes.Ldc_I4_0),

                // If they are not equal, we do not do our logic and we skip nw logic EStack[]
                new(OpCodes.Bne_Un, continueProcessing),

                // Used for instance call EStack[Scp244DeployablePickup Instance] --------------------
                new(OpCodes.Ldarg_0),

                // Load the field within the instance, since no get; set; we can use Field. EStack[transform]
                new(OpCodes.Call, PropertyGetter(typeof(Scp244DeployablePickup), nameof(Scp244DeployablePickup.transform))),

                // Load the field within the instance, since no get; set; we can use Field. EStack[Transform.up]
                new(OpCodes.Callvirt, PropertyGetter(typeof(UnityEngine.Transform), nameof(Transform.up))),

                // Load the field within the instance, since no get; set; we can use Field. EStack[Transform.up, Vector3.up]
                new(OpCodes.Call, PropertyGetter(typeof(Vector3), nameof(Vector3.up))),

                // Load the field within the instance, since no get; set; we can use Field. EStack[Vector3.Dot]
                new(OpCodes.Call, Method(typeof(Vector3), nameof(Vector3.Dot), new[] { typeof(Vector3), typeof(Vector3) })),

                // Second parameter EStack[Vector3.Dot, Scp244DeployablePickup Instance] -----------
                new(OpCodes.Ldarg_0),

                // Load our activation dot EStack[Vector3.Dot, _activationDot]
                new(OpCodes.Ldfld, Field(typeof(Scp244DeployablePickup), nameof(Scp244DeployablePickup._activationDot))),

                // Verify if dot product < activation EStack[result]
                new(OpCodes.Clt),

                // If the dot product is less than we jump, otherwise continue EStack[]
                new(OpCodes.Brfalse, continueProcessing),

                // Load the Scp244DeployablePickup instance EStack[Scp244DeployablePickup Instance]
                new(OpCodes.Ldarg_0),

                // Load our true since it is a bool anyway, EStack[Scp244DeployablePickup Instance, 1]
                new(OpCodes.Ldc_I4_1, continueProcessing),

                // Pass all 2 variables to OpeningScp244EventArgs New Object, get a new object in return EStack[OpeningScp244EventArgs Instance]
                new(OpCodes.Newobj, GetDeclaredConstructors(typeof(OpeningScp244EventArgs))[0]),

                // Copy it for later use again EStack[OpeningScp244EventArgs Instance, PickingUpScp244EventArgs Instance]
                new(OpCodes.Dup),

                // Call Method on Instance EStack[OpeningScp244EventArgs Instance] (pops off so that's why we needed to dup)
                new(OpCodes.Call, Method(typeof(Handlers.Scp244), nameof(Handlers.Scp244.OnOpeningScp244))),

                // Call its instance field (get; set; so property getter instead of field) EStack[IsAllowed]
                new(OpCodes.Callvirt, PropertyGetter(typeof(PickingUpScp244EventArgs), nameof(PickingUpScp244EventArgs.IsAllowed))),

                // If isAllowed = 1, jump to continue route, otherwise, false return occurs below EStack[]
                new(OpCodes.Brfalse, continueProcessing),

                // Load the Scp244DeployablePickup instance EStack[Scp244DeployablePickup Instance] ------
                new(OpCodes.Ldarg_0),

                // Load with Scp244State.Active EStack[Scp244DeployablePickup Instance, Active]
                new(OpCodes.Ldc_I4_1),

                // Load the field within the instance, since get; set; we can use PropertySetter to set state. EStack[]
                new(OpCodes.Callvirt, PropertySetter(typeof(Scp244DeployablePickup), nameof(Scp244DeployablePickup.State))),

                // Load the Scp244DeployablePickup instance EStack[Scp244DeployablePickup Instance]
                new(OpCodes.Ldarg_0),

                // Load the field within the instance, since get; set; we can use PropertyGetter to get state. EStack[_lifeTime]
                new(OpCodes.Ldfld, Field(typeof(Scp244DeployablePickup), nameof(Scp244DeployablePickup._lifeTime))),

                // Load the field within the instance, since get; set; we can use PropertyGetter to get state. EStack[]
                new(OpCodes.Callvirt, Method(typeof(Stopwatch), nameof(Stopwatch.Restart))),

                // We finished our if logic, now we will continue on with normal logic
                new(OpCodes.Br, continueProcessing),

                // False Route
                new CodeInstruction(OpCodes.Nop).WithLabels(returnFalse),
                new(OpCodes.Ret),

                // Good route of is allowed being true 
                new CodeInstruction(OpCodes.Nop).WithLabels(continueProcessing),
            });

            // Jumping over original NW logic.
            newInstructions.InsertRange(continueIndex, new[]
            {
                new CodeInstruction(OpCodes.Nop).WithLabels(normalProcessing),
            });

            for (int z = 0; z < newInstructions.Count; z++)
            {
                yield return newInstructions[z];
            }
            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
