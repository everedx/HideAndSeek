using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public OptionsMenu optionsMenu;
    public MainPage mainPage;
    public LevelSelectMenu levelSelectMenu;



    protected virtual void Awake()
    {
        mainPage.showTitleScreen();
    }

    public void showOptions()
    {
        mainPage.hideTitleScreen();
        optionsMenu.showOptions();
    }

    public void backFromOptions()
    {
        optionsMenu.hideOptions();
        optionsMenu.UpdateVolumesSave();
        mainPage.showTitleScreen();
    }

    public void showLevels()
    {
        mainPage.hideTitleScreen();
        levelSelectMenu.showLevelSelect();
    }

    public void backFromLevels()
    {
        levelSelectMenu.hideShowLevelSelect();
        mainPage.showTitleScreen();
    }

}
