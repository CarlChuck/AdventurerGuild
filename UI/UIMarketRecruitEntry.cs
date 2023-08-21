using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMarketRecruitEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI charProfession;
    [SerializeField] private TextMeshProUGUI combat;
    [SerializeField] private TextMeshProUGUI healing;
    [SerializeField] private TextMeshProUGUI social;
    [SerializeField] private TextMeshProUGUI subterfuge;
    [SerializeField] private TextMeshProUGUI hunting;
    [SerializeField] private TextMeshProUGUI magic;
    [SerializeField] private TextMeshProUGUI craft;
    [SerializeField] private TextMeshProUGUI charValue;
    [SerializeField] private Adventurer adventurerReference;

    public void SetStats(Adventurer adventurer)
    {
        adventurerReference = adventurer;
        charName.text = adventurer.GetName();
        charProfession.text = adventurer.GetProfession();
        combat.text = adventurer.GetCombat().ToString();
        healing.text = adventurer.GetHealing().ToString();
        social.text = adventurer.GetSocial().ToString();
        subterfuge.text = adventurer.GetSubterfuge().ToString();
        hunting.text = adventurer.GetHunting().ToString();
        magic.text = adventurer.GetMagic().ToString();
        craft.text = adventurer.GetCraft().ToString();
        charValue.text = adventurer.GetCharacterValue().ToString();
        SetAdventurerColour();
    }

    public void OnButtonBuy()
    {
        UIManager.Instance.OnButtonBuyRecruit(adventurerReference);
    }

    private void SetAdventurerColour()
    {
        int advLevel = adventurerReference.GetLevel();
        //TODO ColourStuff
    }
}
