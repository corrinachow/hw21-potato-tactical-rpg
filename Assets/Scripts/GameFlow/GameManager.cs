using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    private Character[] playableCharacters;
    private List<Character> turn;
    
    private CombatMenuController combatMenuController;
    private SideMenuController sideMenuController;
    
    private void Start()
    {
        AssetInitializer.InitializeAssets();

        combatMenuController = GameObject.Find("CombatMenu").GetComponent<CombatMenuController>();
        Assert.IsNotNull(combatMenuController);
        
        sideMenuController = GameObject.Find("SideMenu").GetComponent<SideMenuController>();
        Assert.IsNotNull(sideMenuController);

        // Need to attach Character script component to each character
        // playableCharacters = DiscoverCharacters();
        //
        // StartRound();
    }

    private Character[] DiscoverCharacters()
    {
        var blueTeam = GameObject.FindGameObjectsWithTag("BlueTeam");
        var redTeam = GameObject.FindGameObjectsWithTag("RedTeam");

        var characters = blueTeam.Concat(redTeam).Select(go =>
        {
            var character = go.GetComponentInChildren<Character>();
            Assert.IsNotNull(character, $"Element {go.name} has no Character script attached");
            
            return character;
        }).ToArray();
        
        var sorted = characters
            .OrderByDescending(c => c.Speed)
            .ThenBy(c => (int)c.GetTeam());

        return sorted.ToArray();
    }

    private void StartRound()
    {
        turn = new List<Character>(playableCharacters);
        RefreshSideMenu();
    }

    private void RefreshSideMenu()
    {
        var characterInfo = new CharacterInfo[turn.Count];

        for (int i = 0; i < turn.Count; i++)
        {
            var character = turn[i];
            var charInfo = new CharacterInfo
            {
                CurrentHP = character.CurrentHealth,
                TotalHP = character.TotalHealth,
                Picture = character.GetSprite(),
                Team = character.GetTeam(),
            };

            characterInfo[i] = charInfo;
        }
        
        this.sideMenuController.Populate(characterInfo);
    }
}
