using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public static class GlobalResources
{
    // Color palette
    public static Color Team1Color { get; } = new Color(0.3529412f, 0.5686275f, 0.7333333f);
    public static Color Team2Color { get; } = new Color(0.8117647f, 0.2745098f, 0.3098039f);

    // Warrior MoveSet sprites
    public static Sprite WarriorSwordAttackSprite { get; set; } = null;
    public static Sprite WarriorDefendSprite { get; set; } = null;
    public static Sprite WarriorChargeSprite { get; set; } = null;
    public static Sprite WarriorThrowSprite { get; set; } = null;

    // White Mage MoveSet sprites
    public static Sprite WhiteMageMagicArrowSprite { get; set; } = null;
    public static Sprite WhiteMageHealingSprite { get; set; } = null;
    public static Sprite WhiteMageBarrierSprite { get; set; } = null;
    public static Sprite WhiteMageManaWardSprite { get; set; } = null;

    // Black Mage MoveSet sprites
    public static Sprite BlackMageMagicMissileSprite { get; set; } = null;
    public static Sprite BlackMageIceAttackSprite { get; set; } = null;
    public static Sprite BlackMageLightningAttackSprite { get; set; } = null;
    public static Sprite BlackMageFireAttackSprite { get; set; } = null;

    
}
