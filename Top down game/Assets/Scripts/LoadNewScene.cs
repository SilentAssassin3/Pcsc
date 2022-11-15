using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadNewScene : MonoBehaviour

{
    public static void loadscene(string Scenename)
    {
        SceneManager.LoadScene(Scenename);
    }
    public static void loadscene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
