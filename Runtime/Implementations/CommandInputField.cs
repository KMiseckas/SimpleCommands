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
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class CommandInputField : BaseCommandInputDisplay
{
    [FormerlySerializedAs("input_field_top_view_object")]
    [SerializeField]
    private GameObject _InputUITop;

    [FormerlySerializedAs("input_text_field_gameobject")]
    [SerializeField]
    private InputField _InputField;

    protected internal override void OnVisibleToggle(bool isVisible)
    {
        _InputUITop.gameObject.SetActive(isVisible);

        if(!isVisible)
        {
            _InputField.text = "";
        }
    }

    protected internal override string GetInputString()
    {
        return _InputField.text;
    }

    protected internal override void OverrideInputString(string inputOverride)
    {
        _InputField.text = inputOverride;
    }
}
