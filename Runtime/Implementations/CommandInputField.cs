using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
