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
        Draw();
    }

    public void Populate(IEnumerable<CharacterInfo> characterInfos)
    {
        this.charactersInfo.Clear();
        foreach (var charInfo in characterInfos)
        {
            this.charactersInfo.Add(charInfo);
        }

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
        containerImg.color = characterInfo.Team == Team.Team1 ? GlobalResources.Team1Color : GlobalResources.Team2Color;

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
