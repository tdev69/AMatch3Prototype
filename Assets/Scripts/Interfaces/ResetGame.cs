using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    public void RestartGame()
    {
        print("poutou");
        DOTween.Clear(destroy: true);
        print ("pouet");
        SceneManager.LoadScene(0);
    }
}
