using System;
using System.Collections;
using System.Collections.Generic;
using SimpleCommands;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// A default implementation of a suggestions display for the SimpleCommands project.
/// </summary>
public class CommandSuggestionDisplay : BaseCommandSuggestionDisplay
{
    [FormerlySerializedAs("suggestion_console_top_view_ui")]
    [SerializeField]
    private RectTransform _SuggestionUITop;

    [FormerlySerializedAs("suggestion_console_top_view_ui")]
    [SerializeField]
    private int _MaxSuggestionsToDisplay = 5;

    [SerializeField]
    private RectTransform _SuggestionButtonTemplate;

    /// <summary>
    /// List of all the buttons that have been created for suggestions display.
    /// </summary>
    private List<RectTransform> _SuggestionButtonList = new List<RectTransform>();

    /// <summary>
    /// Display the suggestions onto the UI for the user to pick out from.
    /// </summary>
    protected virtual void DisplaySuggestions()
    {
        for (int i = 0; i < _SuggestionButtonList.Count; i++)
        {
            Destroy(_SuggestionButtonList[i].gameObject);
        }

        _SuggestionButtonList.Clear();

        int displayAmount = Math.Min(_MaxSuggestionsToDisplay, _SuggestedCommandList.Count);

        for (int i = 0; i < displayAmount; i++)
        {
            RectTransform suggestionBtn = Instantiate(_SuggestionButtonTemplate);
            suggestionBtn.gameObject.SetActive(true);
            suggestionBtn.SetParent(_SuggestionUITop);

            Text commandKeyBox = suggestionBtn.Find("Command Key").GetComponent<Text>();
            Text commandDescBox = suggestionBtn.Find("Command Desc").GetComponent<Text>();

            SCCommand command = _SuggestedCommandList[i];

            commandKeyBox.text = command.CommandKey;

            for (int j = 0; j < command.ParamInfo.Length; j++)
            {
                commandKeyBox.text += command.ParamInfo[j].Type;

                if(command.ParamInfo[i].IsOptional)
                {
                    commandKeyBox.text += "(opt)";
                }
            }

            commandDescBox.text = command.CommandDesc;

            suggestionBtn.localScale = new Vector3(1, 1, 1);
            suggestionBtn.anchoredPosition = new Vector2(0, _SuggestionButtonList.Count * _SuggestionButtonTemplate.rect.height);

            //Bind the on click event to the method that will fire of our selection event.
            suggestionBtn.GetComponent<Button>().onClick.AddListener(() => TriggerSelectedCommandSuggestion(command));

            _SuggestionButtonList.Add(suggestionBtn);
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void SetSuggestedCommands(List<SCCommand> suggestedCommands)
    {
        base.SetSuggestedCommands(suggestedCommands);

        DisplaySuggestions();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnVisibleToggle(bool isVisible)
    {
        _SuggestionUITop.gameObject.SetActive(isVisible);
    }
}
