using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject mainGameCanvas;
    [SerializeField] private GameObject settingsCanvas;

    [SerializeField] private GameObject frontPane;
    [SerializeField] private GameObject startGamePane;
    [SerializeField] private GameObject loadPane;
    [SerializeField] private GameObject goButton;
    [SerializeField] private string guildName;

    private void Start()
    {
        mainGameCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
        frontPane.SetActive(true);
        startGamePane.SetActive(false);
        loadPane.SetActive(false);
        goButton.SetActive(false);
    }
    public void MenuScreenActive()
    {
        mainGameCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }
    public void GameScreenActive()
    {
        mainGameCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }
    public void OnSettingsActive()
    {
        settingsCanvas.SetActive(true);
    }
    public void OnSettingsDeActive()
    {
        settingsCanvas.SetActive(false);
    }

    public void OnNewGame()
    {
        frontPane.SetActive(false);
        startGamePane.SetActive(true);
    }
    public void OnLoadGame()
    {
        frontPane.SetActive(false);
        loadPane.SetActive(true);
    }
    public void OnGoBack()
    {
        frontPane.SetActive(true);
        loadPane.SetActive(false);
        startGamePane.SetActive(false);
    }

    public void SetGuildName(TextMeshProUGUI guildNameToSet)
    {
        guildName = guildNameToSet.text;
        goButton.SetActive(true);
    }
    public void StartNewGame()
    {
        OnGoBack();
        GameScreenActive();
        Guild guild = Guild.Instance;
        guild.StartGuild(guildName);
        UIManager.Instance.StartUI(guild.GetGuildName(), guild.GetGoldMil(), guild.GetGold());
    }
    public void LoadGame()
    {
        //TODO
    }
    public void OnExitGame()
    {
        //TODO
    }
}
