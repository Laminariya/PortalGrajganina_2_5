using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.TextJuicer;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SubTheme : MonoBehaviour
{

    public List<string> UzbTexts = new List<string>();
    public List<string> RusTexts = new List<string>();
    public List<Sprite> QrSprites = new List<Sprite>();
    
    private Image QRImage;
    private ThemePanel _themePanel;
    private GameManager _manager;
    
    [HideInInspector] public List<TMP_TextJuicer> TextJuicers = new List<TMP_TextJuicer>();
    
    private Image _image;
    private Button[] _buttons;
    
    public void Init(ThemePanel themePanel)
    {
        _image = GetComponent<Image>();
        _manager = GameManager.instance;
        _themePanel = themePanel;
        _buttons = GetComponentsInChildren<Button>(true);
        _buttons[0].onClick.AddListener(OnBack);
        _buttons[1].onClick.AddListener(OnHome);
        TextJuicers = GetComponentsInChildren<TMP_TextJuicer>(true).ToList();
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        foreach (var transform1 in transforms)
        {
            if(transform1.name == "qr") QRImage = transform1.GetComponent<Image>();
        }
    }

    private void OnBack()
    {
        _themePanel.Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        ChangeLang();
        foreach (var textJuicer in TextJuicers)
        {
            textJuicer.SetProgress(0f);
            textJuicer.Update();
        }
        
        _image.DOFade(1, 0.3f).OnComplete(ShowText);
        QRImage.DOFade(1, 0.3f);
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

    public void Hide()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(HideTextCoroutine());
        }
    }

    public void ChangeLang()
    {
        if (_manager.CurrentLang == 0)
        {
            TextJuicers[0].Text = UzbTexts[0];
            TextJuicers[1].Text = UzbTexts[1];
            TextJuicers[2].Text = _manager.BackTexts[0];
            TextJuicers[3].Text = _manager.QrTexts[0];
            _buttons[0].GetComponentInChildren<TMP_TextJuicer>().Text = _manager.BackTexts[0];
        }

        if (_manager.CurrentLang == 1)
        {
            TextJuicers[0].Text = RusTexts[0];
            TextJuicers[1].Text = RusTexts[1];
            TextJuicers[2].Text = _manager.BackTexts[1];
            TextJuicers[3].Text = _manager.QrTexts[1];
            _buttons[0].GetComponentInChildren<TMP_TextJuicer>().Text = _manager.BackTexts[1];
        }
        
        QRImage.sprite = QrSprites[_manager.CurrentLang];
        
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
        
        _image.DOFade(0f, 0.3f).OnComplete(EndHide);
        QRImage.DOFade(0f, 0.3f);
    }

    private void EndHide()
    {
        gameObject.SetActive(false);
    }

    private void OnHome()
    {
        _themePanel.HideAll();
    }

}
