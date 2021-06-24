using System;
using UnityEngine;

public struct CharacterAction
{
    public string ActionName { get; set; }

    public Sprite ActionIcon { get; set; }

    public GameTarget[] Targets { get; set; }

    public Action<GameObject> OnInvoke { get; set; }
}