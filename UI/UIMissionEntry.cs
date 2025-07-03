using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMissionEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionName;
    [SerializeField] private TextMeshProUGUI missionLevel;
    [SerializeField] private TextMeshProUGUI combat;
    [SerializeField] private TextMeshProUGUI healing;
    [SerializeField] private TextMeshProUGUI social;
    [SerializeField] private TextMeshProUGUI subterfuge;
    [SerializeField] private TextMeshProUGUI hunting;
    [SerializeField] private TextMeshProUGUI magic;
    [SerializeField] private TextMeshProUGUI craft;
    [SerializeField] private TextMeshProUGUI craftType;
    [SerializeField] private Mission missionReference;
    [SerializeField] private Quality missionRarity;

    public void SetStats(Mission mission)
    {
        missionReference = mission;
        missionName.text = mission.GetMissionName();
        mission.GetStats(out int mCombat, out int mHealing, out int mSocial, out int mSubterfuge, out int mHunting, 
            out int mMagic, out int mCraft, out IfCrafter mCraftType);
        combat.text = mCombat.ToString();
        healing.text = mHealing.ToString();
        social.text = mSocial.ToString();
        subterfuge.text = mSubterfuge.ToString();
        hunting.text = mHunting.ToString();
        magic.text = mMagic.ToString();
        craft.text = mCraft.ToString();
        craftType.text = mCraftType.ToString();

    }
    public void OnButtonPress()
    {
        UIManager.Instance.OnButtonOpenTask(missionReference);
    }
}
