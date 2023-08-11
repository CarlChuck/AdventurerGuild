using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIRosterEntry : MonoBehaviour
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

    public void SetCharName(string nameToSet)
    {
        charName.text = nameToSet;
    }
    public void SetProfession(string prof)
    {
        charProfession.text = prof;
    }
    public void SetStats(Adventurer adventurer)
    {
        adventurerReference = adventurer;
        combat.text = adventurer.GetCombat().ToString();
        healing.text = adventurer.GetHealing().ToString();
        social.text = adventurer.GetSocial().ToString();
        subterfuge.text = adventurer.GetSubterfuge().ToString();
        hunting.text = adventurer.GetHunting().ToString();
        magic.text = adventurer.GetMagic().ToString();
        craft.text = adventurer.GetCraft().ToString();
    }
    public void SetCharValue(int value)
    {
        charValue.text = value.ToString();
    }
    public void OnButtonPress()
    {
        UIManager.Instance.OnButtonAdventurer(adventurerReference);
    }
}
