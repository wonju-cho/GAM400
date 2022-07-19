using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditUI : MonoBehaviour
{
    public GameObject creditUI;
    public Texture2D cursorTexture;
    private Bow bow;
    private PlayerManager pm;
    private PlayerController pc;
    public bool isCreditOn = false;
    public bool isMainGameOn = false;
    public GameObject otherUI;

    private void Start()
    {
        creditUI.SetActive(false);

        if(SceneManager.GetActiveScene().name == "MainScene")
        {
            isMainGameOn = true;
            bow = GameObject.FindWithTag("Bow").GetComponent<Bow>();
            pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            pm = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }
    }

    public void OpenCreditUI()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);

        if(isMainGameOn)
        {
            pc.enabled = false;
            pm.enabled = false;
            bow.enabled = false;
            otherUI.SetActive(false);
        }
        else
        {
            otherUI.SetActive(false);
        }

        creditUI.SetActive(true);
        isCreditOn = true;
    }

    public void CloseCreditUI()
    {
        creditUI.SetActive(false);
        
        if(isMainGameOn)
        {
            pc.enabled = true;
            pm.enabled = true;
            bow.enabled = true;
            otherUI.SetActive(true);
        }
        else
        {
            otherUI.SetActive(true);
        }
        isCreditOn = false;
    }
}