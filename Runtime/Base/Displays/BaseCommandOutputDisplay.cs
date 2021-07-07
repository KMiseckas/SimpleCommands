// Copyright (c) 2021 Klaudijus Miseckas. All Rights Reserved

using UnityEngine;

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Base implementation for a command output display component. Extend this class to create a custom display for command console output.
    /// </summary>
    public abstract class BaseCommandOutputDisplay : Display
    {
        /// <summary>
        /// Output a string message to the display.
        /// </summary>
        /// <param name="outputMessage">String message.</param>
        public virtual void Output(string outputMessage, OutputType outputType = OutputType.NONE)
        {
            if (OutputType.ERROR.Equals(outputType)) Debug.LogError(outputMessage);
            else if (OutputType.WARNING.Equals(outputType)) Debug.LogWarning(outputMessage);
            else if (OutputType.INFO.Equals(outputType)) Debug.Log(outputMessage);
        }
    }
}