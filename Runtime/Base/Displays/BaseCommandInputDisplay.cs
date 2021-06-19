namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// The abstract input display that is used for input text rendering.
    /// </summary>
    public abstract class BaseCommandInputDisplay : Display
    {
        public delegate void OnInputChangedDelegate(string val);
        public static OnInputChangedDelegate InputChangedEvent;

        /// <summary>
        /// Get the currently visible string in the input field text display.
        /// </summary>
        /// <returns>String in the input field.</returns>
        protected internal abstract string GetInputString();

        /// <summary>
        /// Force the override of the visible string in the input field text display to the provided string.
        /// </summary>
        /// <param name="inputOverride">String to force show in the input field.</param>
        protected internal abstract void OverrideInputString(string inputOverride);

        /// <summary>
        /// Focus the input field to allow input.
        /// </summary>
        protected internal abstract void Focus();

        /// <summary>
        /// Trigger the public text changed event: <see cref="InputChangedEvent"/>.
        /// </summary>
        /// <param name="val">Value to which the text changed to.</param>
        protected void TriggerTextChanged(string val)
        {
            if (InputChangedEvent != null)
                InputChangedEvent(val);
        }
    }
}