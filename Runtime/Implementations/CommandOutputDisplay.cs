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

using SimpleCommands.Attributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System;

/// <summary>
/// Base implementation for a default output display. Does not have to be used but contains majority 
/// of required functionality for base use of the console system. Can be extended or completely
/// ignored over a custom solution. The system practises flexibility to allow the user to implement 
/// their own display as long as it is extending the class of <see cref="BaseCommandOutputDisplay"/>
/// </summary>
public class CommandOutputDisplay : BaseCommandOutputDisplay
{
    [FormerlySerializedAs("output_console_top_view_ui")]
    [SerializeField]
    private GameObject _OutputUITop;

    [FormerlySerializedAs("scroll_bar_object")]
    [SerializeField]
    private ScrollRect _ScrollRect;

    [FormerlySerializedAs("text_field_gameobject")]
    [SerializeField]
    private Text _TextField;

    [SerializeField]
    private bool _TimePrefix = true;

    [SerializeField]
    private bool _DatePrefix = false;

    [SerializeField]
    private bool _LineNumberPrefix = false;

    [SerializeField]
    private bool _InOutPrefix = false;

    /// <summary>
    /// Saved log history.
    /// </summary>
    private string _Log;

    /// <summary>
    /// Number of log entries made.
    /// </summary>
    private int _LineCount = 0;

    /// <summary>
    /// Map of colours markup to their output types where the color markup is a hex value in a color tag as `<color=#fffff>`.
    /// </summary>
    private Dictionary<OutputType, string> _StringColorHexMarkup = new Dictionary<OutputType, string>();

    [SerializeField]
    private ConsoleTextColours _ConsoleColours;

    protected virtual void Awake()
    {
        string tag = "<color=#";

        _StringColorHexMarkup.Add(OutputType.NONE, tag + ColorUtility.ToHtmlStringRGBA(_TextField.color) + ">");
        _StringColorHexMarkup.Add(OutputType.INFO, tag + _ConsoleColours.InfoColourHex + ">");
        _StringColorHexMarkup.Add(OutputType.WARNING, tag + _ConsoleColours.WarningColorHex + ">");
        _StringColorHexMarkup.Add(OutputType.ERROR, tag + _ConsoleColours.ErrorColorHex + ">");
        _StringColorHexMarkup.Add(OutputType.SUCCESS, tag + _ConsoleColours.SuccessColourHex + ">");
        _StringColorHexMarkup.Add(OutputType.FROM_INPUT, tag + _ConsoleColours.InputColourHex + ">");
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnVisibilityChanged(bool isVisible)
    {
        _OutputUITop.gameObject.SetActive(isVisible);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void Output(string outputMessage, OutputType outputType = OutputType.NONE)
    {
        string endColorMarkup = "</color>";

        if (!_StringColorHexMarkup.TryGetValue(outputType, out string outputTypeColorMarkup))
        {
            outputTypeColorMarkup = tag + ColorUtility.ToHtmlStringRGBA(_TextField.color) + ">";
        }

        string detailPrefix = GetDetailPrefix(outputType.Equals(OutputType.FROM_INPUT));

        _Log += "\n" + detailPrefix + outputMessage;
        _TextField.text += "\n" + $"<color=#{_ConsoleColours.DetailPrefixColourHex}>{detailPrefix}</color>" + outputTypeColorMarkup + outputMessage + endColorMarkup;

        AdjustView();

        _LineCount++;
    }

    /// <summary>
    /// Get the correct prefix to use for the ouput message.
    /// </summary>
    /// <param name="isOuputFromInput">Is the output the same as the input that has been received.</param>
    /// <returns>String which is to be used as the prefix for the main output message.</returns>
    protected virtual string GetDetailPrefix(bool isOuputFromInput)
    {
        string prefix = "";

        if (_LineNumberPrefix)
            prefix += $"[{_LineCount}]";
        if (_DatePrefix)
            prefix += $"[{DateTime.Now.ToString("yyyy-MM-dd")}]";
        if (_TimePrefix)
            prefix += $"[{DateTime.Now.ToString("HH:mm:ss")}]";
        if (_InOutPrefix)
        {
            if (isOuputFromInput)
                prefix += "[IN]";
            else
                prefix += "[OUT]";
        }

        return prefix + ": ";
    }

    /// <summary>
    /// Adjust the view to fit and display the newly entered command output.
    /// </summary>
    private void AdjustView()
    {
        //Expand content based on text overflow.
        Vector2 currentSize = _ScrollRect.content.sizeDelta;
        currentSize.y = _TextField.preferredHeight;

        _ScrollRect.content.sizeDelta = currentSize;

        //Move to the bottom of the scroll view when new text is displayed.
        _ScrollRect.verticalNormalizedPosition = 0;
    }

    /// <summary>
    /// Functionality that allows the console output to be printed to a text file within the project package.
    /// </summary>
    [SCCommand("sc_print", "Writes the console outputs into a .txt file.")]
    public void PrintToTextFile()
    {
        string outputPath = GetPrintOutputPath();

        if (!File.Exists(outputPath))
        {
            try
            {
                File.WriteAllText(outputPath, _Log);
            }
            catch (System.Exception exception)
            {
                Output(exception.Message);
                Debug.LogError(exception);
            }
        }
    }

    /// <summary>
    /// Get the path to where the print file should be saved.
    /// </summary>
    /// <returns>String as path.</returns>
    protected virtual string GetPrintOutputPath()
    {
        string printTime = System.DateTime.Now.ToString("dd-MM-yy_HH-mm-ss");
        string outputPath = Application.dataPath + "/SCOutput" + printTime +".txt";

        return outputPath;
    }
}
