using UnityEngine;

public class SceneWrapper : MonoBehaviour
{

    public void LoadScene(string sceneName)
    {
        GameManager.GetInstance().LoadScene(sceneName);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }

}
