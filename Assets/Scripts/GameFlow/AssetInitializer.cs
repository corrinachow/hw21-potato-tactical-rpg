using UnityEngine;
using UnityEngine.Assertions;

public static class AssetInitializer
{
    public static void InitializeAssets()
    {
        var assetLoader = GameObject.Find("AssetLoader").transform;
        Assert.IsNotNull(assetLoader);
        
        // Camera assets
        GlobalResources.TargetSprite = InitializeSpriteAsset("Icons/Camera/Target", assetLoader);
        
        // Move asset
        GlobalResources.MoveSprite = InitializeSpriteAsset("Icons/Action/Move", assetLoader);
        
        // Confirm action assets
        GlobalResources.CancelSprite = InitializeSpriteAsset("Icons/Action/Cancel", assetLoader);
        GlobalResources.ConfirmSprite = InitializeSpriteAsset("Icons/Action/Confirm", assetLoader);
        
        // Warrior assets
        GlobalResources.WarriorSwordAttackSprite = InitializeSpriteAsset("Icons/Warrior/Sword", assetLoader);
        GlobalResources.WarriorChargeSprite = InitializeSpriteAsset("Icons/Warrior/Charge", assetLoader);
        GlobalResources.WarriorDefendSprite = InitializeSpriteAsset("Icons/Warrior/Shield", assetLoader);
        GlobalResources.WarriorThrowSprite = InitializeSpriteAsset("Icons/Warrior/Axe", assetLoader);
        
        // White mage assets
        GlobalResources.WhiteMageBarrierSprite = InitializeSpriteAsset("Icons/WhiteMage/Barrier", assetLoader);
        GlobalResources.WhiteMageHealingSprite = InitializeSpriteAsset("Icons/WhiteMage/Healing", assetLoader);
        GlobalResources.WhiteMageMagicArrowSprite = InitializeSpriteAsset("Icons/WhiteMage/MagicBow", assetLoader);
        GlobalResources.WhiteMageManaWardSprite = InitializeSpriteAsset("Icons/WhiteMage/MagicWard", assetLoader);
        
        // Black mage assets
        GlobalResources.BlackMageMagicMissileSprite =
            InitializeSpriteAsset("Icons/BlackMage/MagicMissile", assetLoader);
        GlobalResources.BlackMageFireAttackSprite = InitializeSpriteAsset("Icons/BlackMage/Fireball", assetLoader);
        GlobalResources.BlackMageIceAttackSprite = InitializeSpriteAsset("Icons/BlackMage/Iceball", assetLoader);
        GlobalResources.BlackMageLightningAttackSprite =
            InitializeSpriteAsset("Icons/BlackMage/LightningBall", assetLoader);
    }

    private static Sprite InitializeSpriteAsset(string prefabPath, Transform parent)
    {
        var gameObject = GameObject.Instantiate(Resources.Load<GameObject>(prefabPath), parent);
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        var sprite = spriteRenderer.sprite;
        Assert.IsNotNull(sprite);

        return sprite;
    }
}
