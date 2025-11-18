using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace eindProject.Input
{
    internal class MouseReader : IInputReader
    {

        // TODO: make mousereader work
        private Vector2 previousPosition;
        private bool hasPrevious = false;

        // Returns a direction vector based on mouse movement delta.
        public Vector2 ReadInput()
        {
            MouseState state = Mouse.GetState();
            var current = new Vector2(state.X, state.Y);

            if (!hasPrevious)
            {
                previousPosition = current;
                hasPrevious = true;
                return Vector2.Zero;
            }

            var delta = current - previousPosition;
            previousPosition = current;

            if (delta == Vector2.Zero)
                return Vector2.Zero;

            // Only return direction, not magnitude
            delta.Normalize();
            return delta;
        }
    }
}
