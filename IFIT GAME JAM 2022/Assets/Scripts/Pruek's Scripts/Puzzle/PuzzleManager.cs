using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private RotatePuzzle logo1;
    [SerializeField] private RotatePuzzle logo2;

    public static event Action OnLogoCorrected;

    public static bool isLogoCorrected = false;

    private void Awake()
    {
        logo1.OnCorrected += CheckLogoPuzzle;
        logo2.OnCorrected += CheckLogoPuzzle;
    }

    private void CheckLogoPuzzle()
    {
        if (logo1.isCorrected && logo2.isCorrected)
        {
            isLogoCorrected = true;
            OnLogoCorrected?.Invoke();
        }
    }

    private void OnDisable()
    {
        logo1.OnCorrected -= CheckLogoPuzzle;
        logo2.OnCorrected -= CheckLogoPuzzle;
    }
}