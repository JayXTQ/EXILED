// -----------------------------------------------------------------------
// <copyright file="SelectingRespawnTeamEventArgs.cs" company="ExMod Team">
// Copyright (c) ExMod Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs.Server
{
    using Interfaces;
    using PlayerRoles;

    /// <summary>
    /// Contains all information before selecting the team to respawn next.
    /// </summary>
    public class SelectingRespawnTeamEventArgs : IExiledEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectingRespawnTeamEventArgs"/> class.
        /// </summary>
        /// <param name="team">The <see cref="Team"/> used as the starting value for this event.</param>
        public SelectingRespawnTeamEventArgs(Team team)
        {
            Team = team;
        }

        /// <summary>
        /// Gets or sets <see cref="Team"/> that represents the team chosen to spawn.
        /// </summary>
        public Team Team { get; set; }
    }
}