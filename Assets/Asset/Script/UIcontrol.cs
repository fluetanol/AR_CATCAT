using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIcontrol : MonoBehaviour
{
    // Start is called before the first frame update
    public string scenename;
    public GameObject CubeSpawner;
    ARPlaceOnPlane arplaceclass;

    void Start()
    {
        arplaceclass = CubeSpawner.GetComponent<ARPlaceOnPlane>();
    }

    public void onSceneChangeButtonclick()
    {
        SceneManager.LoadScene(scenename);
    }

    public void onLandARAutoModeChange()
    {
        arplaceclass.ModeInitialize();
        arplaceclass.SetFlag(1);
    }

    public void onLandARTouchModeChange()
    {
        arplaceclass.ModeInitialize();
        arplaceclass.SetFlag(2);
    }

    public void onLandARMoveModeChange()
    {
        arplaceclass.SetFlag(3);
    }

}
