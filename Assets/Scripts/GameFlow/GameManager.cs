using System;
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
    
    private bool isMoving = false;
    private bool moveSelectionMade = false;

    private CharacterAction[] activeCharacterActions;
    private CharacterAction activeAction;
    private CharacterAction moveAction;

    private SmartCamera smartCamera;
    private CombatMenuController combatMenuController;
    private SideMenuController sideMenuController;
    
    private void Start()
    {
        AssetInitializer.InitializeAssets();

        combatMenuController = GameObject.Find("CombatMenu").GetComponent<CombatMenuController>();
        Assert.IsNotNull(combatMenuController, "combatMenuController != null");
        
        sideMenuController = GameObject.Find("SideMenu").GetComponent<SideMenuController>();
        Assert.IsNotNull(sideMenuController, "sideMenuController != null");

        smartCamera = GameObject.Find("Main Camera").GetComponent<SmartCamera>();
        Assert.IsNotNull(smartCamera, "smartCamera != null");

        moveAction = new CharacterAction
        {
            ActionName = "Move",
            ActionIcon = GlobalResources.MoveSprite,
            Targets = null,
            IsAvailable = true,
            ImmediateInvoke = this.MoveCharacter,
        };

        playableCharacters = DiscoverCharacters();
        roundIndex = 0;
        
        StartRound();
    }

    private void FixedUpdate()
    {
        if (!isMoving || moveSelectionMade)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("MouseButtonDown detected");
            
            var mousePosition = Input.mousePosition;
            
            var activePlayer = turnOrder.Peek();
            activePlayer.GetComponent<Player>().TargetPoint(mousePosition);
            moveSelectionMade = true;
            
            RefreshCombatMenu();
        } else if (Input.touchCount > 0)
        {
            Debug.Log("Touch action detected");
            
            var touchPosition = Input.touches[0].position;
            var activePlayer = turnOrder.Peek();
            activePlayer.GetComponent<Player>().TargetPoint(touchPosition);
            moveSelectionMade = true;
            
            RefreshCombatMenu();
        }
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
        smartCamera.FocusOn(turnOrder.Peek().gameObject);
        smartCamera.SetToOverviewCamera();

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
        activeAction = null;
        isMoving = false;
        moveSelectionMade = false;
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

    private void MoveCharacter()
    {
        Debug.Log("Initiating movement");
        activeAction = moveAction;
        
        var activeChar = turnOrder.Peek();
        smartCamera.FocusOn(activeChar.gameObject);
        smartCamera.SetToOverviewCamera();
            
        var player = activeChar.GetComponent<Player>();
        player.DisplayMoveCircle();
        
        isMoving = true;
        moveSelectionMade = false;
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
        if (hideMenu || activeCharacterActions == null || activeCharacterActions.Length == 0)
        {
            combatMenuController.MainItem = null;
            combatMenuController.SecondaryActions = null;
            
            combatMenuController.Refresh();
            return;
        }

        var activePlayer = turnOrder.Peek();
        
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
                    Team = activePlayer.Team,
                    OnClick = () =>
                    {
                        Debug.Log($"Main action invoked: {mainAction.ActionName}");

                        activeAction = mainAction;
                        mainAction?.ImmediateInvoke();
                        RefreshCombatMenu();
                    }
                };

                combatMenuController.MainItem = menuItem;
            }

            combatMenuController.SecondaryActions = activeCharacterActions.Skip(1).Append(moveAction).Select(action =>
            {
                var menuItem = new MenuItem()
                {
                    Name = action.ActionName,
                    Label = action.ActionName,
                    Disabled = !action.IsAvailable,
                    Hidden = false,
                    Icon = action.ActionIcon,
                    Team = activePlayer.Team,
                    OnClick = () =>
                    {
                        Debug.Log("Invoking secondary action: " + action.ActionName);
                        
                        activeAction = action;
                        action?.ImmediateInvoke();
                        RefreshCombatMenu();
                    }
                };

                return menuItem;
            }).ToArray();
        }
        else if (activeAction.Targets != null)
        {
            var cancelMenuItem = new MenuItem
            {
                Name = "Cancel",
                Label = $"Cancel {activeAction.ActionName}",
                Disabled = false,
                Hidden = false,
                Icon = GlobalResources.CancelSprite,
                Team = activePlayer.Team,
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
                    Team = target.Team,
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
        else
        {
            var cancelMenuItem = new MenuItem
            {
                Name = "Cancel",
                Label = $"Cancel {activeAction.ActionName}",
                Disabled = false,
                Hidden = false,
                Icon = GlobalResources.CancelSprite,
                Team = activePlayer.Team,
                OnClick = () =>
                {
                    Debug.Log($"Cancelling active action ({activeAction.ActionName})");
                    
                    activeAction = null;
                    isMoving = false;
                    moveSelectionMade = false;
                    
                    RefreshCombatMenu();
                }
            };
            combatMenuController.MainItem = cancelMenuItem;

            combatMenuController.SecondaryActions = new[]
            {
                new MenuItem
                {
                    Name = "Confirm",
                    Label = "Confirm move",
                    Disabled = !moveSelectionMade,
                    Hidden = false,
                    Icon = GlobalResources.ConfirmSprite,
                    Team = activePlayer.Team,
                    OnClick = () =>
                    {
                        activePlayer.GetComponent<Player>().MoveToDestination();
                        EndAction();
                    }
                }
            };
        }


        combatMenuController.CameraActions = new[]
        {
            new MenuItem
            {
                Name = "Focus/Unfocus camera",
                Label = "Whatever...",
                Disabled = false,
                Hidden = false,
                Icon = GlobalResources.TargetSprite,
                Team = activePlayer.Team,
                OnClick = () =>
                {
                    smartCamera.FocusOn(activePlayer.gameObject);
                    smartCamera.SetToOverviewCamera();
                }
            }
        };
            
        combatMenuController.Refresh();
    }
}
