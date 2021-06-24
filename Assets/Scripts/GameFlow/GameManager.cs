using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    private PlayableCharacter[] playableCharacters;
    
    private CombatMenuController combatMenuController;
    private SideMenuController sideMenuController;
    
    private void Start()
    {
        AssetInitializer.InitializeAssets();

        combatMenuController = GameObject.Find("CombatMenu").GetComponent<CombatMenuController>();
        Assert.IsNotNull(combatMenuController);
        
        sideMenuController = GameObject.Find("SideMenu").GetComponent<SideMenuController>();
        Assert.IsNotNull(sideMenuController);

        playableCharacters = DiscoverCharacters();
    }

    private PlayableCharacter[] DiscoverCharacters()
    {
        // TODO: Get character stats and sort by a given stat.
        return GameObject.FindObjectsOfType<PlayableCharacter>();
    }
}
