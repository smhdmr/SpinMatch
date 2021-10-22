using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadScene : MonoBehaviour
{

    int Level =0;

    void Start()
    {
        Level = PlayerPrefs.GetInt("level");

        if (Level == 0 || Level>9)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(Level);
        }
    }


}
