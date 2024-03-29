﻿using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BluishFramework
{
    /// <summary>
    /// Provides logic to entities which have the relevant <see cref="Component"/>'s
    /// </summary>
    public class System
    {
        /// <summary>
        /// The <see cref="Type"/>'s of <see cref="Component"/>'s that entities must have to be registered to this <see cref="System"/>
        /// </summary>
        protected Type[] RequiredComponents { get; private set; } 
        /// <summary>
        /// The <see cref="World"/> that this <see cref="System"/> operates from
        /// </summary>
        protected World World { get; private set; }
        private protected HashSet<Entity> RegisteredEntities { get; private set; }

        public System(World world, params Type[] requiredComponents)
        {
            RegisteredEntities = new HashSet<Entity>();
            RequiredComponents = requiredComponents;
            World = world;
        }

        /// <summary>
        /// Unregisters an entity from this <see cref="System"/>
        /// </summary>
        public void UnregisterEntity(Entity entity)
        {
            RegisteredEntities.Remove(entity);
        }

        /// <summary>
        /// Evaluates <paramref name="entity"/>'s components and adds it to this <see cref="UpdateSystem"/> if it meets the signature,
        /// Or removes it from this <see cref="UpdateSystem"/> if the entity was registered but no longer matches the signature
        /// </summary>
        public void UpdateEntityRegistration(Entity entity)
        {
            bool matches = Matches(entity);

            if (RegisteredEntities.Contains(entity))
            {
                if (!matches)
                    RegisteredEntities.Remove(entity);
            }
            else
            {
                if (matches)
                    RegisteredEntities.Add(entity);
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the entity matches this <see cref="UpdateSystem"/>'s signature, <c>false</c> otherwise
        /// </summary>
        private bool Matches(Entity entity)
        {
            return World.GetComponents(entity).HasComponents(RequiredComponents);
        }
    }
}