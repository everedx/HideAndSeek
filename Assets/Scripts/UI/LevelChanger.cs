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

    public void changeToNextLevel()
    {
        animator.SetTrigger("EndLevel");
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        string path = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);
        //levelToLoad = scene.name;
        string[] pathArray = path.Split('/');
        levelToLoad = pathArray[pathArray.Length - 1].Split('.')[0];
        Debug.Log(levelToLoad);
    }

    public void resetLevel()
    {
        animator.SetTrigger("EndLevel");
        levelToLoad = SceneManager.GetActiveScene().name;
    }

    public void onFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void onFadeInComplete()
    {
        //GameObject.Find("GameController").GetComponent<SceneController>().stopScene();
        //Move camera
        GameObject.Find("Main Camera").GetComponent<Animator>().SetTrigger("MoveCamera");
    }

    public void onFadeInStart()
    {
        GameObject.Find("GameController").GetComponent<SceneController>().stopScene(false);
    }
}
