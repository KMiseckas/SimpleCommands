using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CommandOutputDisplay : BaseCommandOutputDisplay
{
    [FormerlySerializedAs("output_console_top_view_ui")]
    [SerializeField]
    private GameObject _OutputUITop;

    [FormerlySerializedAs("text_field_gameobject")]
    [SerializeField]
    private Text _TextField;

    protected internal override void OnVisibleToggle(bool isVisible)
    {
        _OutputUITop.gameObject.SetActive(isVisible);
    }

    public override void Output(string outputMessage)
    {
        _TextField.text += "\n";
        _TextField.text += outputMessage;
    }
}
