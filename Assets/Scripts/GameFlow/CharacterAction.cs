using System;
using UnityEngine;

public class CharacterAction
{
    public string ActionName { get; set; }

    public Sprite ActionIcon { get; set; }

    public bool IsAvailable { get; set; }

    public GameTarget[] Targets { get; set; }

    public Action<GameObject, int> OnInvoke { get; set; }
}