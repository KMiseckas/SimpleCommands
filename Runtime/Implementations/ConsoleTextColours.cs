using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ConsoleTextColoursSO", order = 1)]
public class ConsoleTextColours : ScriptableObject
{
    [SerializeField]
    private Color32 _CommandColor, _TypeColor, _DescriptionColor, _WarningColor, _ErrorColor, _DetailPrefixColour, _TargetColour, _SuccessColour;

    public string CommandColorHex => ColorUtility.ToHtmlStringRGBA(_CommandColor);
    public string TypeColorHex => ColorUtility.ToHtmlStringRGBA(_TypeColor);
    public string DescriptionColorHex => ColorUtility.ToHtmlStringRGBA(_DescriptionColor);
    public string WarningColorHex => ColorUtility.ToHtmlStringRGBA(_WarningColor);
    public string ErrorColorHex => ColorUtility.ToHtmlStringRGBA(_ErrorColor);
    public string DetailPrefixColourHex => ColorUtility.ToHtmlStringRGBA(_DetailPrefixColour);
    public string TargetColourHex => ColorUtility.ToHtmlStringRGBA(_TargetColour);
    public string SuccessColourHex => ColorUtility.ToHtmlStringRGBA(_SuccessColour);
}
