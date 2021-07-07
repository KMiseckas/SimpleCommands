// Copyright (c) 2021 Klaudijus Miseckas. All Rights Reserved

using UnityEngine;
using UnityEngine.UI;

namespace SimpleCommands.Runtime.Implementations
{
   public class SuggestionButton : MonoBehaviour
    {
        [SerializeField]
        private Text _CommandKeyText;

        [SerializeField]
        private Text _CommandDescText;

        public Text CommandKeyText => _CommandKeyText;

        public Text CommandDescText => _CommandDescText;
    }
}