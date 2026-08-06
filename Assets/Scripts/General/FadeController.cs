using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public float _valueForFading;

    private bool _activateFadePanel, _fadeIn;

    private Image _fadePanel;

    void Start()
    {
        _fadePanel = GameObject.Find("FadePanel").GetComponent<Image>();
    }

    void Update()
    {
        if (_activateFadePanel)
        {
            if (_fadeIn && _fadePanel.GetComponent<Image>().color.a < 1)
            {
                var newAlpha = _fadePanel.GetComponent<Image>().color.a + (_valueForFading * Time.deltaTime);
                _fadePanel.GetComponent<Image>().color = new Color(0, 0, 0, newAlpha);
            }
            else if(!_fadeIn && _fadePanel.GetComponent<Image>().color.a > 0)
            {
                var newAlpha = _fadePanel.GetComponent<Image>().color.a - (_valueForFading * Time.deltaTime);
                _fadePanel.GetComponent<Image>().color = new Color(0, 0, 0, newAlpha);
            }
            else
            {
                _activateFadePanel = false;
            }
        }
    }

    public void MakePanelFade(bool fadeIn)
    {
        _activateFadePanel = true;
        _fadeIn = fadeIn;
    }

    public List<bool> ReturnFadeHasFinished()
    {
        var listToReturn = new List<bool>();

        listToReturn.Clear();

        listToReturn.Add(_fadeIn);

        if (_fadeIn && _fadePanel.color.a >= 1)
        {
            listToReturn.Add(true);
        }
        else if(!_fadeIn && _fadePanel.color.a <= 0)
        {
            listToReturn.Add(true);
        }
        else
        {
            listToReturn.Add(false);
        }

        return listToReturn;
    }
}
