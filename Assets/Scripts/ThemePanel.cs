using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.TextJuicer;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ThemePanel : MonoBehaviour
{
    
    public List<string> UzbTexts = new List<string>();
    public List<string> RusText = new List<string>();
    
    [HideInInspector] public List<SubTheme> SubThemes = new List<SubTheme>();
    private Image MainImage;
    [HideInInspector] public List<TMP_TextJuicer> TextJuicers = new List<TMP_TextJuicer>();
    private GameManager _manager;

    public bool IsAnyMain;
    public List<Sprite> QRCodes = new List<Sprite>();
    private Image QRImage;
    private Button[] _buttons;
    
    public void Init()
    {
        _manager = GameManager.instance;
        gameObject.SetActive(true);
        MainImage = GetComponentInChildren<Image>(true);
        SubThemes = GetComponentsInChildren<SubTheme>(true).ToList();
        _buttons = MainImage.GetComponentsInChildren<Button>(true);

        for (int i = 0; i < SubThemes.Count; i++)
        {
            SubThemes[i].Init(this);
            _buttons[i].onClick.AddListener(SubThemes[i].Show);
            //buttons[i].onClick.AddListener(Hide);
        }
        
        TextJuicers = MainImage.GetComponentsInChildren<TMP_TextJuicer>(true).ToList();

        if (IsAnyMain)
        {
            _buttons[1].onClick.AddListener(HideAll); //TODO Home
            _buttons[0].onClick.AddListener(HideAll); //TODO Back
            Transform[] transforms = GetComponentsInChildren<Transform>(true);
            foreach (var transform1 in transforms)
            {
                if(transform1.name == "qr") QRImage = transform1.GetComponent<Image>();
            }
        }
        else
        {
            _buttons[_buttons.Length - 1].onClick.AddListener(HideAll);
            _buttons[_buttons.Length - 2].onClick.AddListener(HideAll);
        }
        
        
        
        ChangeLang();
        HideAll();
    }

    public void ChangeLang()
    {

        if (IsAnyMain)
        {
            if (_manager.CurrentLang == 0)
            {
                TextJuicers[0].Text = UzbTexts[0];
                TextJuicers[1].Text = UzbTexts[1];
                TextJuicers[2].Text = _manager.BackTexts[0];
                TextJuicers[3].Text = _manager.QrTexts[0];
                QRImage.sprite = QRCodes[0];
                //_buttons[_buttons.Length - 2].GetComponentInChildren<TMP_TextJuicer>().Text = _manager.BackTexts[0];
            }

            if (_manager.CurrentLang == 1)
            {
                TextJuicers[0].Text = RusText[0];
                TextJuicers[1].Text = RusText[1];
                TextJuicers[2].Text = _manager.BackTexts[1];
                TextJuicers[3].Text = _manager.QrTexts[1];
                QRImage.sprite = QRCodes[1];
                //_buttons[_buttons.Length - 2].GetComponentInChildren<TMP_TextJuicer>().Text = _manager.BackTexts[1];
            }
        }
        else
        {
            for (int i = 0; i < TextJuicers.Count-1; i++)
            {
                if (_manager.CurrentLang == 0)
                {
                    TextJuicers[i].Text = UzbTexts[i];
                }

                if (_manager.CurrentLang == 1)
                {
                    TextJuicers[i].Text = RusText[i];
                }
            }
            TextJuicers[TextJuicers.Count-1].Text = _manager.BackTexts[_manager.CurrentLang];
        }

        
        
        foreach (var subTheme in SubThemes)
        {
            subTheme.ChangeLang();
        }
    }

    public void Show()
    {
        ChangeLang();
        _manager.CurrentTheme = this;
        _manager.Gradient.enabled = true;
        MainImage.gameObject.SetActive(true);
        foreach (var subTheme in SubThemes)
        {
            subTheme.Hide();
        }
        foreach (var textJuicer in TextJuicers)
        {
            textJuicer.SetProgress(0f);
            textJuicer.Update();
        }
        
        _manager.Gradient.DOFade(1, 0.2f);
        if(IsAnyMain)
            QRImage.DOFade(1, 0.5f).SetDelay(0.2f);
        MainImage.DOFade(1, 0.5f).SetDelay(0.2f).OnComplete(ShowText);
        
    }

    private void ShowText()
    {
        StartCoroutine(ShowTextCoroutine());
    }

    IEnumerator ShowTextCoroutine()
    {
        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * _manager.Speed;
            foreach (var textJuicer in TextJuicers)
            {
                textJuicer.SetProgress(progress);
                textJuicer.Update();
            }
            

            yield return null;
        }
    }

    public void HideAll()
    {
        Hide();
        foreach (var subTheme in SubThemes)
        {
            subTheme.Hide();
        }
    }

    public void Hide()
    {
        StartCoroutine(HideTextCoroutine());
    }
    
    IEnumerator HideTextCoroutine()
    {
        float progress = 1f;
        while (progress > 0f)
        {
            progress -= Time.deltaTime * _manager.Speed;
            foreach (var textJuicer in TextJuicers)
            {
                textJuicer.SetProgress(progress);
                textJuicer.Update();
            }

            yield return null;
        }
        
        _manager.Gradient.DOFade(0f, 0.3f);
        MainImage.DOFade(0f, 0.3f).OnComplete(EndHide);
        if(IsAnyMain)
            QRImage.DOFade(0, 0.3f);
    }

    private void EndHide()
    {
        MainImage.gameObject.SetActive(false);
        _manager.Gradient.enabled = false;
    }
    
}
