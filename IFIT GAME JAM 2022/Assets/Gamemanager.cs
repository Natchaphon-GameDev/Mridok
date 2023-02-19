using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    [Header("2D")]
    [SerializeField] private List<GameObject> Map2D;
    [SerializeField] private  List<GameObject> Cameara2D;
    [Header("3D")]
    [SerializeField] private List<GameObject> Cameara3D;
    [SerializeField] private GameObject puzzle;
    [SerializeField] private GameObject character3D;
    [SerializeField] private GameObject canvas3D;

    private void Awake()
    {
        GoTo3D.On3D += SwitchTo3D;
        
        foreach (var obj in Cameara3D)
        {
            obj.SetActive(false);
        }
        puzzle.SetActive(false);
        character3D.SetActive(false);
        canvas3D.SetActive(false);
    }

    private void SwitchTo3D()
    {
        foreach (var obj in Map2D)
        {
            obj.SetActive(false);
        }

        foreach (var obj in Cameara2D)
        {
            obj.SetActive(false);
        }

        foreach (var obj in Cameara3D)
        {
            obj.SetActive(true);
        }
        
        canvas3D.SetActive(true);
        puzzle.SetActive(true);
        character3D.SetActive(true);
    }
}
