using System;
using System.Collections;
using System.Collections.Generic;
using SimpleCommands;
using UnityEngine;

public abstract class BaseCommandSuggestionDisplay : MonoBehaviour
{
    public delegate void OnSelectedCommandSuggestionDelegate(SCCommand command);
    public static OnSelectedCommandSuggestionDelegate SelectedCommandSuggestionEvent;

    /// <summary>
    /// List of all the suggested commands that can be picked from for displaying.
    /// </summary>
    protected List<SCCommand> _SuggestedCommandList = new List<SCCommand>();

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
    public void ToggleVisible()
    {
        _IsVisible = !IsVisible;

        OnVisibleToggle(IsVisible);
    }

    /// <summary>
    /// Invoke on visibilty toggle.
    /// </summary>
    /// <param name="isVisible">True if toggled to visible.</param>
    protected abstract void OnVisibleToggle(bool isVisible);

    /// <summary>
    /// Display the suggested commands.
    /// </summary>
    /// <param name="outputMessage">Suggestions as instances of command objects.</param>
    public virtual void SetSuggestedCommands(List<SCCommand> suggestedCommands)
    {
        _SuggestedCommandList.Clear();
        _SuggestedCommandList.AddRange(suggestedCommands);
    }

    /// <summary>
    /// Trigger the public selected command suggestion event: <see cref="SelectedCommandSuggestionEvent"/>.
    /// </summary>
    /// <param name="val">The selected command from available suggestions.</param>
    protected virtual void TriggerSelectedCommandSuggestion(SCCommand command)
    {
        if (SelectedCommandSuggestionEvent != null)
            SelectedCommandSuggestionEvent(command);
    }
}
