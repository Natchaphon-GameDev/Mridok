using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    private Inventory _inventory;

    public GameObject itemButton;

    [SerializeField] private Transform inpectScreen;
    [SerializeField] private View_Item vi;
    [SerializeField] private GameObject ui;
    [SerializeField] private InpectAnim insAnim;
    [SerializeField] private float scaleInInspector;

    private GameObject buttonOItem;

    // Start is called before the first frame update
    void Start()
    {
        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        
        //_unityAction = ShowObject(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < _inventory.slots.Length; i++)   
            {
                if (_inventory.isFull[i] == false)
                {
                    _inventory.isFull[i] = true;
                    buttonOItem = Instantiate(itemButton, _inventory.slots[i].transform, false);
                    
                    buttonOItem.GetComponent<Button>().onClick.AddListener(ShowObject);

                    int uiLayer = LayerMask.NameToLayer("UI");
                    gameObject.layer = uiLayer;
                    
                    transform.SetParent(inpectScreen);

                    transform.localPosition = new Vector3(0, 0, -10);
                    transform.localScale = new Vector3(scaleInInspector, scaleInInspector, scaleInInspector);
                    Quaternion.Euler(0f, 0f, 0f);

                    vi.enabled = true;

                    gameObject.SetActive(false);
                    
                    break; 
                }
            }
        }
    }

    private void ShowObject()
    {
        SoundManager.instance.Play(SoundManager.SoundName.UIClick);
        insAnim.RunIn();
        ui.SetActive(false);
        transform.gameObject.SetActive(true);
    }
}
