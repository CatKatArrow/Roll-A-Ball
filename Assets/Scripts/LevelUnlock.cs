using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlock : MonoBehaviour
{

    public Button level1;
    public Button level2;
    public Button level3;

    void Start()
    {
        RestData();
    }
    // Start is called before the first frame update
    void RestData()
    {
        PlayerPrefs.SetInt("Level1",1);

        if(PlayerPrefs.GetInt("Level1")!= 0)
        {
            level1.interactable = true;
        }
        else
        {
            level1.interactable = false;
        }

        if(PlayerPrefs.GetInt("Level2")!= 0)
        {
            level2.interactable = true;
        }
        else
        {
            level2.interactable = false;
        }

        if(PlayerPrefs.GetInt("Level3")!= 0)
        {
            level3.interactable = true;
        }
        else
        {
            level3.interactable = false;
        }
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
        RestData();
    }

}
