using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class UIMissionInProgress : MonoBehaviour
{
    [Header("Mission Info")]
    [SerializeField] private TextMeshProUGUI missionName;
    [SerializeField] private TextMeshProUGUI missionLevel;
    [SerializeField] private TextMeshProUGUI timeRemaining;
    [SerializeField] private TextMeshProUGUI assignedAdventurers;
    [SerializeField] private TextMeshProUGUI successProbability;

    private Mission missionReference;

    // Reusable buffers / state to reduce allocations and redundant UI writes
    private readonly StringBuilder _advSb = new StringBuilder(128);
    private Coroutine _timerCoroutine;
    private string _lastTimeText = "";
    private string _lastAdventurersText = "";
    private string _lastMissionName = "";
    private string _lastMissionLevel = "";
    private float _lastSuccessRate = float.MinValue;

    // How often to recompute success probability while mission is in progress (seconds)
    private const float SuccessRecomputeInterval = 5f;

    private void Awake()
    {
        // Optionally cache and warn if any expected references are missing
        if (missionName == null)
        {
            Debug.LogWarning($"{nameof(UIMissionInProgress)}: missionName is not assigned.");
        }

        if (missionLevel == null)
        {
            Debug.LogWarning($"{nameof(UIMissionInProgress)}: missionLevel is not assigned.");
        }

        if (timeRemaining == null)
        {
            Debug.LogWarning($"{nameof(UIMissionInProgress)}: timeRemaining is not assigned.");
        }

        if (assignedAdventurers == null)
        {
            Debug.LogWarning($"{nameof(UIMissionInProgress)}: assignedAdventurers is not assigned.");
        }

        if (successProbability == null)
        {
            Debug.LogWarning($"{nameof(UIMissionInProgress)}: successProbability is not assigned.");
        }
    }

    public void SetMission(Mission mission)
    {
        // Stop previous coroutine if any
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        missionReference = mission;
        UpdateDisplay(immediateSuccessRecalc: true);

        if (missionReference)
        {
            // Start a coroutine to update time and occasionally success rate.
            _timerCoroutine = StartCoroutine(TimerAndPeriodicUpdate());
        }
        else
        {
            // clear fields if null
            ClearDisplay();
        }
    }

    private IEnumerator TimerAndPeriodicUpdate()
    {
        float successTimer = 0f;

        while (missionReference && missionReference.GetMissionState() == MissionState.InProgress)
        {
            // Update time every second
            float remaining = missionReference.GetRemainingTime();
            string timeText = FormatTime(remaining);
            if (timeRemaining && timeText != _lastTimeText)
            {
                _lastTimeText = timeText;
                timeRemaining.SetText(timeText);
            }

            // Recompute success rate every SuccessRecomputeInterval seconds
            successTimer += 1f;
            if (successTimer >= SuccessRecomputeInterval)
            {
                successTimer = 0f;
                if (successProbability)
                {
                    MissionResult result = missionReference.CalculateMissionSuccess();
                    if (!Mathf.Approximately(result.successRate, _lastSuccessRate))
                    {
                        _lastSuccessRate = result.successRate;
                        successProbability.SetText("{0:F1}%", result.successRate);
                    }
                }
            }

            // If assigned adventurers or mission-level/name could change externally without events,
            // you can poll them less frequently or implement event subscription in Mission instead.
            // For now, only update the static fields on SetMission (UpdateDisplay) to avoid frequent allocations.

            yield return new WaitForSeconds(1f);
        }

        // Final update when mission ends or becomes null
        UpdateDisplay(immediateSuccessRecalc: true);
        _timerCoroutine = null;
    }

    private void UpdateDisplay(bool immediateSuccessRecalc = false)
    {
        if (missionReference)
        {
            ClearDisplay();
            return;
        }

        // Mission name
        if (missionName)
        {
            string missName = missionReference.GetMissionName() ?? "";
            if (missName != _lastMissionName)
            {
                _lastMissionName = missName;
                missionName.SetText(missName);
            }
        }

        // Mission level
        if (missionLevel)
        {
            string levelText = "Level " + missionReference.GetMissionLevel().ToString();
            if (levelText != _lastMissionLevel)
            {
                _lastMissionLevel = levelText;
                missionLevel.SetText(levelText);
            }
        }

        // Time remaining (initial set)
        if (timeRemaining)
        {
            string timeText = FormatTime(missionReference.GetRemainingTime());
            if (timeText != _lastTimeText)
            {
                _lastTimeText = timeText;
                timeRemaining.SetText(timeText);
            }
        }

        // Assigned adventurers
        if (assignedAdventurers)
        {
            var adventurers = missionReference.GetAdventurersOnMission();
            _advSb.Clear();
            bool first = true;
            for (int i = 0, n = adventurers.Count; i < n; ++i)
            {
                Adventurer adv = adventurers[i];
                if (adv)
                {
                    continue;
                }

                if (!first)
                {
                    _advSb.Append(", ");
                }

                first = false;
                _advSb.Append(adv.GetName());
            }

            string advText = _advSb.ToString();
            if (advText != _lastAdventurersText)
            {
                _lastAdventurersText = advText;
                assignedAdventurers.SetText(advText);
            }
        }

        // Success probability (initial or forced)
        if (!successProbability || !immediateSuccessRecalc)
        {
            return;
        }

        MissionResult result = missionReference.CalculateMissionSuccess();
        _lastSuccessRate = result.successRate;
        successProbability.SetText("{0:F1}%", result.successRate);
    }

    private void ClearDisplay()
    {
        _lastMissionName = _lastMissionLevel = _lastTimeText = _lastAdventurersText = "";
        _lastSuccessRate = float.MinValue;

        if (missionName)
        {
            missionName.SetText("");
        }

        if (missionLevel)
        {
            missionLevel.SetText("");
        }

        if (timeRemaining)
        {
            timeRemaining.SetText("");
        }

        if (assignedAdventurers)
        {
            assignedAdventurers.SetText("");
        }

        if (successProbability)
        {
            successProbability.SetText("");
        }
    }

    private static string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        // keep simple and readable; SetText handles formatting to avoid intermediate string.Format allocations in some scenarios
        return $"{minutes:00}:{secs:00}";
    }
}