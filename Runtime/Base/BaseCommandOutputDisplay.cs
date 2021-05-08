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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base implementation for a command output display component. Extend this class to create a custom display for command console output.
/// </summary>
public abstract class BaseCommandOutputDisplay : MonoBehaviour
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
    public void ToggleVisible()
    {
        _IsVisible = !IsVisible;

        OnVisibleToggle(IsVisible);
    }

    /// <summary>
    /// Invoke on visibilty toggle.
    /// </summary>
    /// <param name="isVisible">True if toggled to visible.</param>
    protected internal abstract void OnVisibleToggle(bool isVisible);

    /// <summary>
    /// Output a string message to the display.
    /// </summary>
    /// <param name="outputMessage">String message.</param>
    public abstract void Output(string outputMessage);
}
