using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class SideMenuController : MonoBehaviour
{
    private float CHAR_INFO_PADDING = 5.0f;
    
    private readonly List<CharacterInfo> charactersInfo = new List<CharacterInfo>(6);

    public GameObject target;
    public GameObject prefab;

    private void Start()
    {
        charactersInfo.Add(new CharacterInfo
        {
            CurrentHP = 1,
            TotalHP = 100,
            Team = Team.Team1,
            Picture = GameObject.Find("Player0")?.GetComponent<SpriteRenderer>()?.sprite,
        });
        
        charactersInfo.Add(new CharacterInfo
        {
            CurrentHP = 60,
            TotalHP = 60,
            Team = Team.Team2,
            Picture = GameObject.Find("Player1")?.GetComponent<SpriteRenderer>()?.sprite,
        });
        
        Draw();
    }

    public void Push(CharacterInfo characterInfo)
    {
        charactersInfo.Add(characterInfo);
        Draw();
    }

    public void Pop()
    {
        charactersInfo.RemoveAt(0);
        Draw();
    }

    public void UpdatePlayerInfos()
    {
        Draw();
    }

    private void Draw()
    {
        var children = this.target.GetComponentsInChildren<Transform>(true).Where(t => t.name != target.name).ToArray();
        var lastIndex = -1;

        for (var i = 0; i < children.Length; i++)
        {
            lastIndex = i;
            var characterTransform = children[i].gameObject;
            var characterInfo = charactersInfo.ElementAtOrDefault(i);

            RefreshPlayerGameObject(characterTransform, characterInfo);
        }
        
        for (var i = lastIndex + 1; i < charactersInfo.Count; i++)
        {
            var characterInfo = charactersInfo[i];
            var characterTransform = Instantiate(prefab, target.transform);
            
            var rectTransform = characterTransform.GetComponent<RectTransform>();
            var characterInfoHeight = rectTransform.rect.height;

            var y = i * (characterInfoHeight + CHAR_INFO_PADDING) + CHAR_INFO_PADDING;
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, y, rectTransform.rect.height);

            RefreshPlayerGameObject(characterTransform.gameObject, characterInfo);
        }
    }

    private void RefreshPlayerGameObject(GameObject playerGameObject, CharacterInfo characterInfo)
    {
        if (characterInfo == null)
        {
            playerGameObject.SetActive(false);
            return;
        }

        var pictureContainer = playerGameObject.transform.Find("PictureContainer");
        var containerImg = pictureContainer.GetComponent<Image>();
        containerImg.color = characterInfo.Team == Team.Team1 ? Color.blue : Color.red;

        var picture = pictureContainer.Find("Picture");
        picture.GetComponent<Image>().sprite = characterInfo.Picture;

        var hpBar = playerGameObject.transform.Find("HPBar");
        var hpBarWidth = hpBar.GetComponent<RectTransform>().rect.width;
        
        var hpText = hpBar.Find("HPText");
        hpText.GetComponent<TextMeshProUGUI>().text = $"{characterInfo.CurrentHP}/{characterInfo.TotalHP}";

        var currentHPBar = hpBar.Find("CurrentHP");
        var currentHPRect = currentHPBar.GetComponent<RectTransform>();
        var hpPercentage = Math.Max(0.15f, (float) characterInfo.CurrentHP / characterInfo.TotalHP);
        currentHPRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpPercentage * hpBarWidth);

        playerGameObject.SetActive(true);
    }

}
