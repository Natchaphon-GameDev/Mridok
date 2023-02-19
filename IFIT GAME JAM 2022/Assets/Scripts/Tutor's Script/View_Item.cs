using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_Item : MonoBehaviour
{

    private Vector3 posLastFrame;
    public Camera UICam;

    [SerializeField]private GameObject ui;
    [SerializeField] private InpectAnim insAnim;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) posLastFrame = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - posLastFrame;
            posLastFrame = Input.mousePosition;

            Vector3 axis = Quaternion.AngleAxis(-90, Vector3.forward) * delta;
            transform.rotation = Quaternion.AngleAxis(delta.magnitude * 0.1f, axis) * transform.rotation;
        }
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            insAnim.RunOut();
            ui.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
