using UnityEngine;
using System.Collections;

public class InGameMenuScript : MonoBehaviour {

    private GameManager manager = null;

    public void Exit()
    {
        Application.Quit();
    }

    public void BackToGame()
    {
        manager.CallMenuToggleEvent();
        manager.CallPauseToggleEvent();
    }

    void OnEnable()
    {
        manager = GameManager.instance;
    }
}
