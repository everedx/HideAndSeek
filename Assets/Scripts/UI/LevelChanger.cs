using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{

    public Animator animator;
    private string levelToLoad;

    public void changeToLevel(string levelName)
    {
        animator.SetTrigger("EndLevel");
        levelToLoad = levelName;
    }

    public void onFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
