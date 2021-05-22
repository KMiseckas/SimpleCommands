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
