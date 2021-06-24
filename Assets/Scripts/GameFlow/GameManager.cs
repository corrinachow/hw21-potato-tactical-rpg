using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    private const int ACTIONS_PER_TURN = 3;
    
    private Character[] playableCharacters;
    private Queue<Character> turnOrder;
    
    private int roundIndex = 0;
    private int turnActions = 0;
    private bool hideMenu = false;
    
    private CharacterAction[] activeCharacterActions;
    private CharacterAction activeAction;
    
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
        roundIndex = 0;
        
        StartRound();
    }

    private Character[] DiscoverCharacters()
    {
        var blueTeam = GameObject.FindGameObjectsWithTag("BlueTeam");
        var redTeam = GameObject.FindGameObjectsWithTag("RedTeam");

        var characters = blueTeam.Concat(redTeam).Select(go =>
        {
            var character = go.GetComponent<Character>();
            return character;
        }).Where(c => c != null).ToArray();
        
        var sorted = characters
            .OrderByDescending(c => c.Speed)
            .ThenBy(c => (int)c.Team);

        return sorted.ToArray();
    }

    private void StartRound()
    {
        turnOrder = new Queue<Character>(playableCharacters);
        StartTurn();
    }

    private void EndRound()
    {
        roundIndex++;
        StartRound();
    }

    private void StartTurn()
    {
        turnActions = 0;
        
        RefreshSideMenu();
        StartAction();
    }

    private void EndTurn()
    {
        turnOrder.Dequeue();

        if (turnOrder.Count == 0)
        {
            EndRound();
        }
        else
        {
            StartTurn();
        }
    }

    private void StartAction()
    {
        var activePlayer = turnOrder.Peek();
        activeCharacterActions = activePlayer.GetActions(roundIndex);
        
        RefreshCombatMenu();
    }

    private void EndAction()
    {
        // Maybe wait for animation to conclude?
        activeAction = null;
        turnActions++;
        
        RefreshSideMenu();

        if (turnActions >= ACTIONS_PER_TURN)
        {
            EndTurn();
        }
        else
        {
            StartAction();
        }
    }

    private void RefreshSideMenu()
    {
        var characterInfo = new CharacterInfo[turnOrder.Count];

        for (int i = 0; i < turnOrder.Count; i++)
        {
            var character = turnOrder.ElementAt(i);
            var charInfo = new CharacterInfo
            {
                CurrentHP = character.CurrentHealth,
                TotalHP = character.TotalHealth,
                Team = character.Team,
                Picture = character.GetSprite(),
            };

            characterInfo[i] = charInfo;
        }
        
        this.sideMenuController.Populate(characterInfo);
    }

    private void RefreshCombatMenu()
    {
        if (hideMenu)
        {
            combatMenuController.MainItem = null;
            combatMenuController.SecondaryActions = null;
        }
        
        if (activeAction == null)
        {
            var mainAction = activeCharacterActions.ElementAtOrDefault(0);
            if (mainAction != null)
            {
                var menuItem = new MenuItem
                {
                    Name = mainAction.ActionName,
                    Label = mainAction.ActionName,
                    Disabled = !mainAction.IsAvailable,
                    Hidden = false,
                    Icon = mainAction.ActionIcon,
                    OnClick = () =>
                    {
                        Debug.Log($"Main action invoked: {mainAction.ActionName}");

                        activeAction = mainAction;
                        RefreshCombatMenu();
                    }
                };

                combatMenuController.MainItem = menuItem;
            }

            combatMenuController.SecondaryActions = activeCharacterActions.Skip(1).Select(action =>
            {
                var menuItem = new MenuItem()
                {
                    Name = action.ActionName,
                    Label = action.ActionName,
                    Disabled = !action.IsAvailable,
                    Hidden = false,
                    Icon = action.ActionIcon,
                    OnClick = () =>
                    {
                        Debug.Log("Invoking secondary action: " + action.ActionName);
                        
                        activeAction = action;
                        RefreshCombatMenu();
                    }
                };

                return menuItem;
            }).ToArray();
        }
        else
        {
            var cancelMenuItem = new MenuItem
            {
                Name = "Cancel",
                Label = $"Cancel {activeAction.ActionName}",
                Disabled = false,
                Hidden = false,
                Icon = GlobalResources.CancelSprite,
                OnClick = () =>
                {
                    Debug.Log($"Cancelling active action ({activeAction.ActionName})");
                    
                    activeAction = null;
                    RefreshCombatMenu();
                }
            };
            combatMenuController.MainItem = cancelMenuItem;

            combatMenuController.SecondaryActions = activeAction.Targets.Select(target =>
            {
                var menuItem = new MenuItem
                {
                    Name = $"target_{target.TargetName}",
                    Label = $"Target {target.TargetName}",
                    Disabled = !target.IsAvailable,
                    Hidden = false,
                    Icon = target.GameObject.GetComponent<Character>().GetSprite(),
                    OnClick = () =>
                    {
                        Debug.Log($"Picked target: {target.TargetName}");
                        
                        activeAction.OnInvoke(target.GameObject, roundIndex);
                        EndAction();
                    }
                };

                return menuItem;
            }).ToArray();
        }
        
        combatMenuController.Refresh();
    }
}
