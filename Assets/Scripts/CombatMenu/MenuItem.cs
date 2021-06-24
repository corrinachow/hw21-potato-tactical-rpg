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

    public Button.ButtonClickedEvent OnClick = new Button.ButtonClickedEvent();

    public void InvokeClick()
    {
        this.OnClick.Invoke();
    }
}
