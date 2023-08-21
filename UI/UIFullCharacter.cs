using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIFullCharacter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI charSpecies;
    [SerializeField] private TextMeshProUGUI charProfession;
    [SerializeField] private TextMeshProUGUI combat;
    [SerializeField] private TextMeshProUGUI healing;
    [SerializeField] private TextMeshProUGUI social;
    [SerializeField] private TextMeshProUGUI subterfuge;
    [SerializeField] private TextMeshProUGUI hunting;
    [SerializeField] private TextMeshProUGUI magic;
    [SerializeField] private TextMeshProUGUI craftType;
    [SerializeField] private TextMeshProUGUI craft;
    [SerializeField] private TextMeshProUGUI charValue;
    [SerializeField] private TextMeshProUGUI equippedWeapon;
    [SerializeField] private TextMeshProUGUI equippedOutfit;
    [SerializeField] private TextMeshProUGUI equippedAccessory;

    public void SetStats(Adventurer adventurer)
    {
        charName.text = adventurer.GetName();
        charSpecies.text = adventurer.GetSpecies().ToString();
        charProfession.text = adventurer.GetProfession().ToString();
        combat.text = adventurer.GetCombat().ToString();
        healing.text = adventurer.GetHealing().ToString();
        social.text = adventurer.GetSocial().ToString();
        subterfuge.text = adventurer.GetSubterfuge().ToString();
        hunting.text = adventurer.GetHunting().ToString();
        magic.text = adventurer.GetMagic().ToString();
        craftType.text = adventurer.GetCraftType().ToString() + ":";
        craft.text = adventurer.GetCraft().ToString();
        charValue.text = adventurer.GetCharacterValue().ToString();
        if (adventurer.GetWeaponSlot() != null)
        {
            equippedWeapon.text = adventurer.GetWeaponSlot().GetItemName();
        }
        else
        {
            equippedWeapon.text = "Empty";
        }
        if (adventurer.GetOutfitSlot() != null)
        {
            equippedOutfit.text = adventurer.GetOutfitSlot().GetItemName();
        }
        else
        {
            equippedOutfit.text = "Empty";
        }
        if (adventurer.GetAccessorySlot() != null)
        {
            equippedAccessory.text = adventurer.GetAccessorySlot().GetItemName();
        }
        else
        {
            equippedAccessory.text = "Empty";
        }
    }
}
