using SimpleCommands.Attributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.IO;

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

    protected internal override void OnVisibleToggle(bool isVisible)
    {
        _OutputUITop.gameObject.SetActive(isVisible);
    }

    public override void Output(string outputMessage)
    {
        _TextField.text += "\n";
        _TextField.text += outputMessage;

        AdjustView();
    }

    private void AdjustView()
    {
        //Expand content based on text overflow.
        Vector2 currentSize = _ScrollRect.content.sizeDelta;
        currentSize.y = _TextField.preferredHeight;

        _ScrollRect.content.sizeDelta = currentSize;

        //Move to the bottom of the scroll view when new text is displayed.
        _ScrollRect.verticalNormalizedPosition = 0;
    }

    [SCCommand("scc_print", "Writes the console outputs into a .txt file.")]
    public void PrintToTextFile()
    {
        string printTime = System.DateTime.Now.ToString("dd-MM-yy_HH-mm-ss");

        string outputPath = Application.dataPath + "/SCCOutput" + printTime +".txt";

        if(!File.Exists(outputPath))
        {
            try
            {
                File.WriteAllText(outputPath, _TextField.text);
            }
            catch(System.Exception exception)
            {
                Output("PRINT ERROR: " + exception.Message);
                Debug.LogError(exception);
            }
        }
    }
}
