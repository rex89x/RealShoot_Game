using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public void OnStartGame(string ScneneName)
    {
      
        Application.LoadLevel(ScneneName);

    }
}