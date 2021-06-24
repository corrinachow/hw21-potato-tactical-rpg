using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SideMenuController : MonoBehaviour
{
    private const int MAX_PLACEHOLDERS = 6;
    private float CHAR_INFO_PADDING = 5.0f;

    private readonly List<CharacterInfo> charactersInfo = new List<CharacterInfo>(MAX_PLACEHOLDERS);
    private GameObject[] placeholders;

    public GameObject target;
    public GameObject prefab;

    private void Start()
    {
        placeholders = CreatePlaceholders();
    }

    private GameObject[] CreatePlaceholders()
    {
        var collection = new GameObject[MAX_PLACEHOLDERS];
        
        for (var i = 0; i < MAX_PLACEHOLDERS; i++)
        {
            var placeholder = Instantiate(prefab, target.transform);
            var rectTransform = placeholder.GetComponent<RectTransform>();
            
            var characterInfoHeight = rectTransform.rect.height;

            var y = i * (characterInfoHeight + CHAR_INFO_PADDING) + CHAR_INFO_PADDING;
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, y, rectTransform.rect.height);

            var characterInfo = charactersInfo.ElementAtOrDefault(i);
            RefreshPlayerGameObject(placeholder, characterInfo);

            collection[i] = placeholder;
        }

        return collection;
    }

    public void Populate(CharacterInfo[] characterInfos)
    {
        this.charactersInfo.Clear();
        for (int i = 0; i < MAX_PLACEHOLDERS && i < characterInfos.Length; i++)
        {
            this.charactersInfo.Add(characterInfos[i]);
        }

        UpdatePlaceholders();
    }

    private void UpdatePlaceholders()
    {
        for (var i = 0; i < placeholders.Length; i++)
        {
            var characterInfo = charactersInfo.ElementAtOrDefault(i);
            var placeholder = placeholders[i];
            
            RefreshPlayerGameObject(placeholder, characterInfo);
        }
    }

    private void RefreshPlayerGameObject(GameObject placeholder, CharacterInfo characterInfo)
    {
        if (characterInfo == null)
        {
            placeholder.SetActive(false);
            return;
        }

        var pictureContainer = placeholder.transform.Find("PictureContainer");
        var containerImg = pictureContainer.GetComponent<Image>();
        containerImg.color = characterInfo.Team == Team.Team1 ? GlobalResources.Team1Color : GlobalResources.Team2Color;

        var picture = pictureContainer.Find("Picture");
        var pictureImg = picture.GetComponent<Image>();
        pictureImg.sprite = characterInfo.Picture;
        pictureImg.preserveAspect = true;

        var hpBar = placeholder.transform.Find("HPBar");
        var hpBarWidth = hpBar.GetComponent<RectTransform>().rect.width;
        
        var hpText = hpBar.Find("HPText");
        hpText.GetComponent<TextMeshProUGUI>().text = $"{characterInfo.CurrentHP}/{characterInfo.TotalHP}";

        var currentHPBar = hpBar.Find("CurrentHP");
        var currentHPRect = currentHPBar.GetComponent<RectTransform>();
        var hpPercentage = Math.Max(0.15f, (float) characterInfo.CurrentHP / characterInfo.TotalHP);
        currentHPRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpPercentage * hpBarWidth);

        placeholder.SetActive(true);
    }

}
