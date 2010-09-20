using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.Game
{
    interface IControllable
    {
        /// <summary>
        /// Smoothly translates an entity to a certain location
        /// in the gaming grid. Basically shows normal walking,
        /// or running motion, and the action is not an
        /// instantaneous telport.
        /// </summary>
        /// <returns></returns>
        bool AutoWalkTo(Vector3 position);

        /// <summary>
        /// Responds to direction given by gamepad controller/keyboard.
        /// Used only on explore mode maps where free movement is allowed.
        /// </summary>
        /// <param name="dir"></param>
        void WalkTo(EntityDirection dir);

        /// <summary>
        /// Instantaneously teleports entity to a certain location
        /// on the grid.
        /// </summary>
        /// <returns></returns>
        bool MoveTo(Vector3 position);

        /// <summary>
        /// Tells an entity to orient themselves to a certain direction
        /// (i.e. face toward a cardinal direction, so you can see their
        /// face or back)
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        bool FaceDirection(EntityDirection dir);
    }
}
