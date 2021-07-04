using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meun : MonoBehaviour
{
    [SerializeField] private GameObject _GameTitle = null;

    [SerializeField] private GameObject _GameStart = null;

    public void CreateGametitle()
    {
        _GameTitle.SetActive(true);
    }

    public void CreateGameStart()
    {
        _GameStart.SetActive(true);
    }
}
