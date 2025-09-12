using System.Collections;
using System.Collections.Generic;
using BrunoMikoski.TextJuicer;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ThemeButton : MonoBehaviour
{

    public List<string> LangText = new List<string>();
    
    public ThemePanel ThemePanel;
    
    private Button button;
    private GameManager _manager;
    [HideInInspector] public TMP_TextJuicer TextJuicer;
    
    public void Init()
    {
        _manager = GameManager.instance;
        button = GetComponent<Button>();
        TextJuicer = GetComponentInChildren<TMP_TextJuicer>(true);
        button.onClick.AddListener(OnClick);
        ThemePanel?.Init();
    }

    public void OffActive()
    {
        button.image.color = new Color(1, 1, 1, 0f);
    }

    private void OnClick()
    {
        button.image.DOColor(Color.white, 0.2f).OnComplete(OffColor);
    }

    private void OffColor()
    {
        button.image.DOColor(new Color(1f,1f,1f,0f), 0.2f).OnComplete(OnShowTheme);
    }

    private void OnShowTheme()
    {
        ThemePanel?.Show();
    }

    public void SetDefault()
    {
        ThemePanel?.HideAll();
        button.image.color = Color.clear;
    }

    public void ChangeLang()
    {
        TextJuicer.Text = LangText[_manager.CurrentLang];
    }
    
    

}
