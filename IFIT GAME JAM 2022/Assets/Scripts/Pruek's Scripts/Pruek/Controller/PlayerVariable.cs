using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerVariable : MonoBehaviour
{
    public bool isSplitScreen;
    public bool isCantMove;
    public bool isStopCamera;
    public bool isWalking;
    public bool isJumping;
    [FormerlySerializedAs("isIdel")] public bool isIdle;
    public bool isfalling;

    public bool isPreparing;
    public bool isGrabbing;
    public bool isBoostUp;
    public bool isThrowing;

}
