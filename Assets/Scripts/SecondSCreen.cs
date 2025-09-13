using System.Collections;
using System.Collections.Generic;
using BrunoMikoski.TextJuicer;
using UnityEngine;
using UnityEngine.UI;

public class SecondSCreen : MonoBehaviour
{

    public List<string> Portal = new List<string>();
    public List<string> Starting = new List<string>();
    public List<string> Text = new List<string>();
    
    public List<TMP_TextJuicer> TextJuicers = new List<TMP_TextJuicer>();
    
    public Button StartButton;
    
    void Start()
    {
        StartButton.onClick.AddListener(OnClick);
    }

    public void ChangeLeng()
    {
        TextJuicers[0].Text = Portal[GameManager.instance.CurrentLang];
        TextJuicers[1].Text = Starting[GameManager.instance.CurrentLang];
        TextJuicers[2].Text = Text[GameManager.instance.CurrentLang];
    }

    private void OnClick()
    {
        gameObject.SetActive(false);
    }
}
