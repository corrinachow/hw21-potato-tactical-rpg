using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalResources
{
    // Color palette
    public static Color Team1Color { get; } = new Color(0.3529412f, 0.5686275f, 0.7333333f);
    public static Color Team2Color { get; } = new Color(0.8117647f, 0.2745098f, 0.3098039f);

    // Warrior MoveSet sprites
    public static Sprite WarriorSwordAttackSprite { get; } = null;
    public static Sprite WarriorDefendSprite { get; } = null;
    public static Sprite WarriorChargeSprite { get; } = null;
    public static Sprite WarriorThrowSprite { get; } = null;

    // White Mage MoveSet sprites
    public static Sprite WhiteMageMagicArrowSprite { get; } = null;
    public static Sprite WhiteMageHealingSprite { get; } = null;
    public static Sprite WhiteMageBarrierSprite { get; } = null;
    public static Sprite WhiteMageManaWardSprite { get; } = null;

    // Black Mage MoveSet sprites
    public static Sprite BlackMageMagicMissileSprite { get; } = null;
    public static Sprite BlackMageIceAttackSprite { get; } = null;
    public static Sprite BlackMageLightningAttackSprite { get; } = null;
    public static Sprite BlackMageFireAttackSprite { get; } = null;
}
