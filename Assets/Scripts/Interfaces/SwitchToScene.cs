using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToScene : MonoBehaviour
{
    ///Pass the scene index of the scene you want to load from the Builder as arg 
    public void GoToScene(int aSceneIndex)
    {
        DOTween.Clear(true);
        SceneManager.LoadScene(aSceneIndex);
    }

    private void SwitchToGOScene()
    {
        GoToScene(2);
    }

    private void StopListening()
    {
        EventManager.onGameOver -= SwitchToGOScene;
    }

    private void OnDestroy()
    {
        StopListening();
    }

    private void OnDisable()
    {
        StopListening();
    }


    public void Awake()
    {
        EventManager.onGameOver += SwitchToGOScene; 
    }

}
