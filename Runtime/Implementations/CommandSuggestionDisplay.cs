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
    /// <summary>
    /// The root of the suggestion UI panel.
    /// </summary>
    [FormerlySerializedAs("suggestion_console_top_view_ui")]
    [SerializeField]
    private RectTransform _SuggestionUIRoot;

    /// <summary>
    /// The reference to the ScrollRect object.
    /// </summary>
    [FormerlySerializedAs("scroll_bar_object")]
    [SerializeField]
    private ScrollRect _ScrollRect;

    /// <summary>
    /// The max number of auto-complete suggestions that the suggestions panel should display.
    /// </summary>
    [FormerlySerializedAs("suggestion_console_top_view_ui")]
    [SerializeField]
    private int _MaxSuggestionsToDisplay = 20;

    /// <summary>
    /// The template object to use when creating an autocomplete suggestion clickable area.
    /// </summary>
    [SerializeField]
    private RectTransform _SuggestionButtonTemplate;

    /// <summary>
    /// List of all the buttons that have been created for suggestions display.
    /// </summary>
    private List<RectTransform> _SuggestionButtonList = new List<RectTransform>();

    /// <summary>
    /// Is the suggestion panel currently empty of any autocomplete suggestions.
    /// </summary>
    private bool _IsSuggestionPanelEmpty = false;

    /// <summary>
    /// The max size in pixels of the suggestion panel.
    /// </summary>
    [SerializeField]
    private float _MaxSuggetionPanelHeight = 175;


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
            suggestionBtn.SetParent(_ScrollRect.content);

            Text commandKeyBox = suggestionBtn.Find("Command Key").GetComponent<Text>();
            Text commandDescBox = suggestionBtn.Find("Command Desc").GetComponent<Text>();

            SCCommand command = _SuggestedCommandList[i];

            commandKeyBox.text = command.CommandKey;

            for (int j = 0; j < command.ParamInfo.Length; j++)
            {
                commandKeyBox.text += " <" + command.ParamInfo[j].Type.Name + ">";

                if(command.ParamInfo[j].IsOptional)
                {
                    commandKeyBox.text += "(opt)";
                }
            }

            commandDescBox.text = command.CommandDesc;

            suggestionBtn.localScale = new Vector3(1, 1, 1);
            suggestionBtn.anchoredPosition = new Vector2(0, -_SuggestionButtonList.Count * _SuggestionButtonTemplate.rect.height);

            //Bind the on click event to the method that will fire of our selection event.
            suggestionBtn.GetComponent<Button>().onClick.AddListener(() => TriggerSelectedCommandSuggestion(command));

            _SuggestionButtonList.Add(suggestionBtn);
        }

        Vector2 currentSize = _ScrollRect.content.sizeDelta;
        currentSize.y = _SuggestionButtonList.Count * _SuggestionButtonTemplate.rect.height;

        _ScrollRect.content.sizeDelta = currentSize;

        Vector2 scrollRectSize = _ScrollRect.GetComponent<RectTransform>().sizeDelta;
        scrollRectSize.y = Math.Min(currentSize.y, _MaxSuggetionPanelHeight);

        _ScrollRect.GetComponent<RectTransform>().sizeDelta = scrollRectSize;

        //Move to the top of the scroll view when new text is displayed.
        _ScrollRect.verticalNormalizedPosition = 1;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void SetSuggestedCommands(List<SCCommand> suggestedCommands)
    {
        base.SetSuggestedCommands(suggestedCommands);

        if (_SuggestedCommandList.Count > 0)
        {
            _IsSuggestionPanelEmpty = false;

            _SuggestionUIRoot.gameObject.SetActive(true);

            DisplaySuggestions();
        }
        else if(!_IsSuggestionPanelEmpty)
        {
            _IsSuggestionPanelEmpty = true;

            _SuggestionUIRoot.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnVisibilityChanged(bool isVisible)
    {
        _IsSuggestionPanelEmpty = _SuggestedCommandList.Count <= 0;

        _SuggestionUIRoot.gameObject.SetActive(!_IsSuggestionPanelEmpty);
    }
}
