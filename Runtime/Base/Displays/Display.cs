using UnityEngine;

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Base class for the display implementation.
    /// </summary>
    public abstract class Display : MonoBehaviour
    {
        /// <summary>
        /// Is the display visible on screen.
        /// </summary>
        private bool _IsVisible = false;

        /// <summary>
        /// Get whether the display is visible on the screen.
        /// </summary>
        public bool IsVisible => _IsVisible;

        /// <summary>
        /// Toggle the display visibility.
        /// </summary>
        public void SetVisible(bool isVisible)
        {
            _IsVisible = isVisible;

            OnVisibilityChanged(IsVisible);
        }

        /// <summary>
        /// Invoke on visibilty property change.
        /// </summary>
        /// <param name="isVisible">True if visibility has change to visible.</param>
        protected abstract void OnVisibilityChanged(bool isVisible);
    }
}