// Copyright (c) 2021 Klaudijus Miseckas. All Rights Reserved

using System.Collections;
using SimpleCommands.Runtime.Base;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SimpleCommands.Runtime.Implementations
{

    /// <summary>
    /// Base implementation for a default input display. Does not have to be used but contains majority 
    /// of required functionality for base use of the console system. Can be extended or completely
    /// ignored over a custom solution. The system practises flexibility to allow the user to implement 
    /// their own display as long as it is extending the class of <see cref="BaseCommandInputDisplay"/>
    /// </summary>
    public class CommandInputDisplay : BaseCommandInputDisplay
    {
        /// <summary>
        /// The top level parent (hierarchy object). This object will be set visible or invisible to hide or unhide the console component.
        /// </summary>
        [FormerlySerializedAs("input_field_top_view_object")]
        [SerializeField]
        private GameObject _InputUITop;

        /// <summary>
        /// The input field component where the text will be input into and rendered.
        /// </summary>
        [FormerlySerializedAs("input_text_field_gameobject")]
        [SerializeField]
        private InputField _InputField;

        private void Awake()
        {
            _InputField.onValueChanged.AddListener(TriggerTextChanged);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnVisibilityChanged(bool isVisible)
        {
            _InputUITop.gameObject.SetActive(isVisible);

            if (!isVisible)
                _InputField.text = "";
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected internal override string GetInputString()
        {
            return _InputField.text;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected internal override void OverrideInputString(string inputOverride)
        {
            _InputField.text = inputOverride;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected internal override void Focus()
        {
            StartCoroutine(FocusOnTextField());
        }

        /// <summary>
        /// Coroutine that focuses the input onto the input field and at the ed of the frame moves the caret to the end of the text input 
        /// field and removes the highlight from the whole input text.
        /// </summary>
        private IEnumerator FocusOnTextField()
        {
            _InputField.ActivateInputField();

            yield return new WaitForEndOfFrame();

            _InputField.caretPosition = _InputField.text.Length;
            _InputField.ForceLabelUpdate();
        }
    }
}