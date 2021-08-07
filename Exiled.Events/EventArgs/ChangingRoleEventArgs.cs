// -----------------------------------------------------------------------
// <copyright file="ChangingRoleEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Exiled.API.Features;

    /// <summary>
    /// Contains all information before a player's <see cref="RoleType"/> changes.
    /// </summary>
    public class ChangingRoleEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangingRoleEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="newRole"><inheritdoc cref="NewRole"/></param>
        /// <param name="shouldPreservePosition"><inheritdoc cref="Lite"/></param>
        /// <param name="isEscaped"><inheritdoc cref="IsEscaped"/></param>
        public ChangingRoleEventArgs(Player player, RoleType newRole, bool shouldPreservePosition, bool isEscaped)
        {
            Player = player;
            NewRole = newRole;
            Items = InventorySystem.Configs.StartingInventories.DefinedInventories.ContainsKey(newRole) ? InventorySystem.Configs.StartingInventories.DefinedInventories[newRole].Items.ToList() : new List<ItemType>();
            Lite = shouldPreservePosition;
            IsEscaped = isEscaped;
        }

        /// <summary>
        /// Gets the player whose <see cref="RoleType"/> is changing.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the new player's role.
        /// </summary>
        public RoleType NewRole { get; set; }

        /// <summary>
        /// Gets base items that the player will receive. (Changing this will overwrite their current inventory if Lite is true!).
        /// </summary>
        public List<ItemType> Items { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the player escaped or not.
        /// </summary>
        public bool IsEscaped { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the position and items has to be preserved after changing the role.
        /// </summary>
        public bool Lite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event can continue.
        /// </summary>
        public bool IsAllowed { get; set; } = true;
    }
}
