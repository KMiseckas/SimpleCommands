using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCommandOutputDisplay : MonoBehaviour
{
    private bool _IsVisible = false;

    public bool IsVisible => _IsVisible;

    public void ToggleVisible()
    {
        _IsVisible = !IsVisible;

        OnVisibleToggle(IsVisible);
    }

    protected internal abstract void OnVisibleToggle(bool isVisible);

    public abstract void Output(string outputMessage);
}
