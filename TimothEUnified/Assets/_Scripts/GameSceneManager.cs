using UnityEngine;
using UnityEngine.SceneManagement;


public class GameSceneManager : MonoBehaviour
{
    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
