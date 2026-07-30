using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    private bool _inCutscene;

    public List<Image> _cutsceneBackground = new List<Image>();

    private int _indexSelected = -1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_inCutscene && _indexSelected >= 0)
        {
            
        }
    }

    public void StartCutscene(int cutsceneIndex)
    {
        _inCutscene = true;
        _indexSelected = cutsceneIndex;
    }
}
