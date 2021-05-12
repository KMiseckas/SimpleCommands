// MIT License 
//
// Copyright (c) 2021 Klaudijus Miseckas 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions: 
//
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
// SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The abstract input display that is used for input text rendering.
/// </summary>
public abstract class BaseCommandInputDisplay : MonoBehaviour
{
    public delegate void OnInputChangedDelegate(string val);
    public static OnInputChangedDelegate InputChangedDelegate;

    /// <summary>
    /// Is the display visible on screen.
    /// </summary>
    private bool _IsVisible = false;

    /// <summary>
    /// Get whether the display is visible on the screen.
    /// </summary>
    public bool IsVisible => _IsVisible;

    private void Awake()
    {
        BindOnInputChangedToAction(new Action<string>(OnTextChanged));
    }

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

    protected void OnTextChanged(string val)
    {
        if (InputChangedDelegate != null)
            InputChangedDelegate(val);
    }

    /// <summary>
    /// Bind the change of the input to the action that should be fired on that change.
    /// </summary>
    /// <param name="triggerOnTextChanged">The action to fire when the input field is changed.</param>
    protected abstract void BindOnInputChangedToAction(Action<string> triggerOnTextChanged);
}
