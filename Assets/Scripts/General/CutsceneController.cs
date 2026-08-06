using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    private bool _inCutscene, _dialogStarted;

    public List<Image> _cutsceneBackground = new List<Image>();

    private int _cutsceneIndexSelected = -1;

    private FadeController _fadeController;

    private Image _imageObject;

    private DialogHandler _dialogHandler;

    void Start()
    {
        _fadeController = gameObject.GetComponent<FadeController>();
        _imageObject = GameObject.Find("CutscenePanel").GetComponent<Image>();
        _dialogHandler = gameObject.GetComponent<DialogHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_inCutscene && _cutsceneIndexSelected >= 0)
        {
            if(!_dialogStarted)
            {
                _fadeController.MakePanelFade(true);

                if (_fadeController.ReturnFadeHasFinished()[0] && _fadeController.ReturnFadeHasFinished()[1])
                {
                    if (!ReturnImageAlphaAtExtreme(true))
                    {
                        ChangeImageObjectAlpha(true);
                    }
                    else
                    {
                        _dialogHandler.StartNewConversation(_cutsceneIndexSelected);
                        _dialogStarted = true;
                    }
                }
            }
            else
            {
                if (!_dialogHandler.ReturnConversationStatus())
                {
                    ChangeImageObjectAlpha(false);
                
                    _fadeController.MakePanelFade(false);

                    if(ReturnImageAlphaAtExtreme(false) && !_fadeController.ReturnFadeHasFinished()[0] && _fadeController.ReturnFadeHasFinished()[1])
                    {
                        _inCutscene = false;
                        _cutsceneIndexSelected = -1;
                        _dialogStarted = false;
                    }
                }
            }
        }
    }

    private void ChangeImageObjectAlpha(bool fadeIn)
    {
        var oldcolor = _imageObject.color;
        var newAlpha = oldcolor.a + (0.5f * Time.deltaTime);

        if (!fadeIn)
        {
            newAlpha = oldcolor.a - (0.5f * Time.deltaTime);
        }

        _imageObject.color = new Color(oldcolor.r, oldcolor.g, oldcolor.b, newAlpha);
    }

    private bool ReturnImageAlphaAtExtreme(bool fadeIn)
    {
        if(_imageObject.color.a >= 1 && fadeIn)
        {
            return true;
        }
        else if(_imageObject.color.a <= 0 && !fadeIn)
        {
            return true;
        }

        return false;
    }

    public void StartCutscene(int cutsceneIndex)
    {
        _inCutscene = true;
        _cutsceneIndexSelected = cutsceneIndex;
    }

    public bool ReturnCutsceneActive()
    {
        return _inCutscene;
    }
}
