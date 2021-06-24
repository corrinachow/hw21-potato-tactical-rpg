using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatMenuController : MonoBehaviour
{
    private const float BUTTON_MARGIN = 2f;
    
    public GameObject mainActionTarget;
    public GameObject secondaryActionsTarget;
    public GameObject cameraActionsTarget;
    
    public GameObject miniButtonPrefab;
    public GameObject mainButtonPrefab;
    
    public int secondaryActionsCount = 3;
    public int cameraActionsCount = 2;

    public bool IsDebug = false;

    private ActionButton mainActionButton;
    private ActionButton[] secondaryActionButtons;
    private ActionButton[] cameraActionButtons;

    public MenuItem MainItem { get; set; }
    public MenuItem[] SecondaryActions { get; set; }
    public MenuItem[] CameraActions { get; set; }

    public void Refresh()
    {
        ConfigureButton(MainItem, mainActionButton);

        for (var i = 0; i < Math.Max(secondaryActionsCount, cameraActionsCount); i++)
        {
            var secondaryAction = SafelyGetItem(i, SecondaryActions);
            var secondaryButton = SafelyGetItem(i, secondaryActionButtons);
            ConfigureButton(secondaryAction, secondaryButton);
            
            var cameraAction = SafelyGetItem(i, CameraActions);
            var cameraButton = SafelyGetItem(i, cameraActionButtons);
            ConfigureButton(cameraAction, cameraButton);
        }
    }

    private T SafelyGetItem<T>(int index, IReadOnlyList<T> items)
    {
        if (items == null || items.Count <= index)
        {
            return default;
        }

        return items[index];
    }
    
    private void ConfigureButton(MenuItem item, ActionButton button)
    {
        if (button == null)
        {
            return;
        }

        if (item == null)
        {
            button.Reset();
        }
        else
        {
            button.SetupButton(item);
        }
    }

    private void Start()
    {
        if (IsDebug)
        {
            MainItem = new MenuItem
            {
                Name = "WarriorAttack",
                Label = "Warrior attack",
                // Icon = GameObject.Find("Sword").GetComponent<SpriteRenderer>().sprite,
                Disabled = false,
                Hidden = false,
                Team = Team.Team1,
                OnClick = () => Debug.Log("Warrior attacking")
            };

            SecondaryActions = new[]
            {
                new MenuItem
                {
                    Name = "WarriorCharge",
                    Label = "Warrior Charge",
                    // Icon = GameObject.Find("Charge").GetComponent<SpriteRenderer>().sprite,
                    Disabled = false,
                    Hidden = false,
                    Team = Team.Team2,
                    OnClick = () => Debug.Log("Warrior charge")
                },
                new MenuItem
                {
                    Name = "WarriorShield",
                    Label = "Warrior Shield",
                    // Icon = GameObject.Find("Shield").GetComponent<SpriteRenderer>().sprite,
                    Disabled = false,
                    Hidden = true,
                    Team = Team.Team1,
                    OnClick = () => Debug.Log("Warrior shield (hidden)"),
                },
                new MenuItem
                {
                    Name = "WarriorThrow",
                    Label = "Warrior throw",
                    // Icon = GameObject.Find("Axe").GetComponent<SpriteRenderer>().sprite,
                    Disabled = true,
                    Hidden = false,
                    Team = Team.Team1,
                    OnClick = () => Debug.Log("Warrior axe throw (disabled)")
                }
            };
        }
        
        mainActionButton = CreateMainActionButton();
        secondaryActionButtons = CreateSecondaryActionButtons(secondaryActionsCount);
        cameraActionButtons = CreateCameraActionButtons(cameraActionsCount);
    }

    private ActionButton CreateMainActionButton()
    {
        var mainButtonEl = Instantiate(this.mainButtonPrefab, mainActionTarget.transform);
        
        var button = mainButtonEl.GetComponent<Button>();
        var image = mainButtonEl.GetComponentsInChildren<Image>()[1];
        
        var actionButton = new ActionButton(button, image);
        ConfigureButton(MainItem, actionButton);

        return actionButton;
    }

    private ActionButton[] CreateSecondaryActionButtons(int count)
    {
        var buttons = new ActionButton[count];
        
        for (var i = 0; i < count; i++)
        {
            var buttonEl = Instantiate(this.miniButtonPrefab, secondaryActionsTarget.transform);

            var buttonTransform = buttonEl.GetComponent<RectTransform>();
            var buttonHeight = buttonTransform.rect.height;
            
            buttonTransform.pivot = new Vector2(0.5f, 0);
            buttonTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, (buttonHeight + BUTTON_MARGIN) * i, buttonHeight);

            var button = buttonEl.GetComponent<Button>();
            var image = buttonEl.GetComponentsInChildren<Image>()[1];

            buttons[i] = new ActionButton(button, image);
            
            var secondaryAction = SafelyGetItem(i, SecondaryActions);
            ConfigureButton(secondaryAction, buttons[i]);
        }

        return buttons;
    }
    
    private ActionButton[] CreateCameraActionButtons(int count)
    {
        var buttons = new ActionButton[count];
        
        for (var i = 0; i < count; i++)
        {
            var buttonEl = Instantiate(this.miniButtonPrefab, cameraActionsTarget.transform);

            var buttonTransform = buttonEl.GetComponent<RectTransform>();
            var buttonWidth = buttonTransform.rect.width;
            
            buttonTransform.pivot = new Vector2(1, 0.5f);
            buttonTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, (buttonWidth + BUTTON_MARGIN) * i, buttonWidth);

            var button = buttonEl.GetComponent<Button>();
            var image = buttonEl.GetComponentsInChildren<Image>()[1];

            buttons[i] = new ActionButton(button, image);

            var secondaryAction = SafelyGetItem(i, CameraActions);
            ConfigureButton(secondaryAction, buttons[i]);
        }

        return buttons;
    }

    private class ActionButton
    {
        private readonly Button button;
        private readonly Image icon;

        public ActionButton(Button button, Image icon)
        {
            this.button = button;
            this.icon = icon;
        }

        public void SetupButton(MenuItem menuItem)
        {
            var teamColor = menuItem.Team == Team.Team1 ? GlobalResources.Team1Color : GlobalResources.Team2Color;
            var hoverEffect = new Color(0.8f, 0.8f, 0.8f, 1f);

            button.gameObject.SetActive(!menuItem.Hidden);
            button.interactable = !menuItem.Disabled;
            button.transition = Selectable.Transition.ColorTint;
            button.colors = new ColorBlock
            {
                normalColor = teamColor,
                highlightedColor = teamColor * hoverEffect,
                disabledColor = new Color(0.3f, 0.3f, 0.3f, 0.6f),
                fadeDuration = 0.1f,
                colorMultiplier = 1.0f,
                pressedColor = teamColor,
                selectedColor = teamColor * hoverEffect,
            };

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(menuItem.InvokeClick);

            icon.sprite = menuItem.Icon;
            icon.preserveAspect = true;
            icon.color = menuItem.Disabled ? new Color(1f, 1f, 1f, 0.6f) : Color.white;
        }

        public void Reset()
        {
            button.gameObject.SetActive(false);
            button.interactable = false;
            button.colors = new ColorBlock();
            button.onClick.RemoveAllListeners();

            icon.sprite = null;
        }
    }
}
