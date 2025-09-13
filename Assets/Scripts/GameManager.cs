using System;
using System.Collections;
using System.Collections.Generic;
using BrunoMikoski.TextJuicer;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    public string Enter;
    public List<string> QrTexts = new List<string>();
    public List<string> BackTexts = new List<string>();
    public List<string> Zagolovok = new List<string>();
    
    public TMP_TextJuicer ZagolovokJuicer;
    public List<ThemeButton> themeButtons = new List<ThemeButton>();
    public List<Sprite> Active_Uzb = new List<Sprite>();
    [HideInInspector] public List<Sprite> Active_Arab = new List<Sprite>();
    [HideInInspector] public List<Sprite> Active_Eng = new List<Sprite>();
    public List<Sprite> Active_Rus = new List<Sprite>();

    public Button Lang_Uzb;
    public Button Lang_Rus;
    
    public Image Gradient;

    public Image BG;
    public Sprite bg_Uzb;
    [HideInInspector] public Sprite bg_Eng;
    public Sprite bg_Rus;
    [HideInInspector] public Sprite bg_Arab;

    [HideInInspector] public int CurrentLang;
    [HideInInspector] public ThemePanel CurrentTheme;
    
    public GameObject DefaultScreen;
    public List<AnimText> animTexts = new List<AnimText>();
    public List<string> Default_Rus = new List<string>();
    public List<string> Default_Uzb = new List<string>();
    [HideInInspector] public List<string> Default_Eng = new List<string>();
    [HideInInspector] public List<string> Default_Arab = new List<string>();
    
    public string Main_Rus;
    public string Main_Uzb;

    public Button ClickDefault;
    public SecondSCreen SecondScreen;
    
    private float _time = 0;
    private Coroutine _coroutine;
    public float Speed = 10f;
    private Color _colorLang;

    public float TimeOut;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        _colorLang = Lang_Uzb.image.color;
        _colorLang.a = 1f;
        Gradient.enabled = false;
        for (int i = 0; i < themeButtons.Count; i++)
        {
            themeButtons[i].Init();
        }
        
        foreach (var animText in animTexts)
        {
            animText.Init();
        }
       
        
        Lang_Uzb.onClick.AddListener(OnLangUzb);
        Lang_Rus.onClick.AddListener(OnLangRus);
       
        ClickDefault.onClick.AddListener(OffDefault);
        OnLangRus();
        OnDefault();
        ChangeLangDefault();
    }
    
    void Update()
    {
        if (Input.anyKeyDown)
        {
            _time = Time.time;
        }

        if (!DefaultScreen.activeSelf && Time.time - _time > TimeOut)
        {
            CurrentTheme.HideAll();
            OnDefault();
        }
    }

    public void OnLangUzb()
    {
        Lang_Uzb.enabled = false;
        Lang_Rus.enabled = true;
        CurrentLang = 0;
        Lang_Uzb.image.DOColor(_colorLang, 0.2f).OnComplete(ChangeLang);
        Lang_Rus.image.DOColor(new Color(_colorLang.r, _colorLang.g,_colorLang.b, 0f), 0.2f);
    }

    public void OnLangRus()
    { 
        Lang_Uzb.enabled = true;
        Lang_Rus.enabled = false;
        Lang_Uzb.image.DOColor(new Color(_colorLang.r, _colorLang.g,_colorLang.b, 0f), 0.2f);
        CurrentLang = 1;
        Lang_Rus.image.DOColor(_colorLang, 0.2f).OnComplete(ChangeLang);
    }

    private void ChangeLang()
    {
        OnOffLangButtons(false);
        StartCoroutine(StartAnimationChangeLanguage());
    }

    IEnumerator StartAnimationChangeLanguage()
    {
        float progress = 1f;
        while (progress > 0f)
        {
            progress -= Time.deltaTime * Speed;
            foreach (var button in themeButtons)
            {
                button.TextJuicer.SetProgress(progress);
                button.TextJuicer.Update();
            }

            foreach (var textJuicer in SecondScreen.TextJuicers)
            {
                textJuicer.SetProgress(progress);
                textJuicer.Update();
            }
            
            ZagolovokJuicer.SetProgress(progress);
            ZagolovokJuicer.Update();

            if (CurrentTheme == null)
            {
                yield return null;
                continue;
            }
            foreach (var juicer in CurrentTheme.TextJuicers)
            {
                juicer.SetProgress(progress);
                juicer.Update();
                foreach (var theme in CurrentTheme.SubThemes)
                {
                    foreach (var textJuicer in theme.TextJuicers)
                    {
                        textJuicer.SetProgress(progress);
                        textJuicer.Update();
                    }
                }
            }

            yield return null;
        }
        
        foreach (var themeButton in themeButtons)
        {
            themeButton.ChangeLang();
        }

        ZagolovokJuicer.Text = Zagolovok[CurrentLang];
        SecondScreen.ChangeLeng();

        if (CurrentTheme != null)
        {
            CurrentTheme.ChangeLang();
            foreach (var theme in CurrentTheme.SubThemes)
            {
                theme.ChangeLang();
            }
        }

        progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * Speed;
            foreach (var button in themeButtons)
            {
                button.TextJuicer.SetProgress(progress);
                button.TextJuicer.Update();
            }
            
            foreach (var textJuicer in SecondScreen.TextJuicers)
            {
                textJuicer.SetProgress(progress);
                textJuicer.Update();
            }
            
            ZagolovokJuicer.SetProgress(progress);
            ZagolovokJuicer.Update();
            
            if (CurrentTheme == null)
            {
                yield return null;
                continue;
            }
            
            foreach (var juicer in CurrentTheme.TextJuicers)
            {
                juicer.SetProgress(progress);
                juicer.Update();
                foreach (var theme in CurrentTheme.SubThemes)
                {
                    foreach (var textJuicer in theme.TextJuicers)
                    {
                        textJuicer.SetProgress(progress);
                        textJuicer.Update();
                    }
                }
            }

            yield return null;
        }
        OnOffLangButtons(true);
    }

    private void OnOffLangButtons(bool value)
    {
        Lang_Rus.enabled = value;
        Lang_Uzb.enabled = value;
    }

    private void ChangeLangDefault()
    {
        Debug.Log(CurrentLang);
        switch (CurrentLang)
        {
            case 0:
            {
                for (int i = 0; i < animTexts.Count; i++)
                {
                    animTexts[i].SetText(Default_Uzb[i]);
                }
                
                break;
            }
            case 1:
            {
                for (int i = 0; i < animTexts.Count; i++)
                {
                    animTexts[i].SetText(Default_Rus[i]);
                }
                
                break;
            }
        }
        
    }

    private void OnDefault()
    {
        SecondScreen.gameObject.SetActive(true);
        DefaultScreen.SetActive(true);
        if(_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(StartAnimation());
    }

    private void OffDefault()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        DefaultScreen.SetActive(false);
    }
    
    IEnumerator StartAnimation()
    {
        float timer = Time.time;
        while (DefaultScreen.activeSelf)
        {
            
            yield return new WaitForSeconds(0.3f);

            animTexts[Random.Range(0, animTexts.Count)].PlayEffect();
            
            yield return null;
            if (Time.time - timer > 5f)
            {
                timer = Time.time;
                if (CurrentLang == 1)
                    CurrentLang = 0;
                else
                {
                    CurrentLang = 1;
                }

                ChangeLangDefault();
            }
        }
    }

    private void OffActiveButtons()
    {
        foreach (var button in themeButtons)
        {
            button.OffActive();
        }
    }
}
