using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem
{
    public string Name { get; set; }

    public string Label { get; set; }

    public Sprite Icon { get; set; }

    public bool Disabled { get; set; } = false;

    public bool Hidden { get; set; } = false;

    public Team Team { get; set; }

    public Action OnClick;

    public void InvokeClick()
    {
        OnClick?.Invoke();
    }
}
