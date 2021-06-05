using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuggestionButton : MonoBehaviour
{
    [SerializeField]
    private Text _CommandKeyText;

    [SerializeField]
    private Text _CommandDescText;

    public Text CommandKeyText => _CommandKeyText;

    public Text CommandDescText => _CommandDescText;
}
