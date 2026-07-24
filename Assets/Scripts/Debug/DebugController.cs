using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugController : MonoBehaviour
{
    private bool DebugOn = false;

    private bool DebugEnabled = false;

    private int OptionsIndex = 0;

    private GameObject DebugMenu;

    public List<DebugOption> DebugOptions = new List<DebugOption>();

    void Start()
    {
        if (Application.isEditor || Debug.isDebugBuild)
        {
            DebugEnabled = true;
        }

        for (int x = 0; x < DebugOptions.Count; x++)
        {
            DebugOptions[x].OptionQuit();
        }
    }

    void Update()
    {
        if(DebugEnabled){
            if (Input.GetKeyDown(KeyCode.F1))
            {
                DebugOn = !DebugOn;
            }

            if (DebugMenu != null && Input.GetKeyDown(KeyCode.F2))
            {
                OptionsIndex++;

                if(OptionsIndex >= DebugOptions.Count)
                {
                    OptionsIndex = 0;
                }
    
            }

            CreateOrDestroyMenu();

            UpdateDebugMenuText();

            SetupOption();

            RunDebugOptions();
        }
    }

    private void UpdateDebugMenuText()
    {
        if(DebugMenu != null)
        {   
            var textObj = GameObject.Find("OptionNameForDebug").GetComponentInParent<TextMeshProUGUI>();

            var currentOpt = DebugOptions[OptionsIndex];

            textObj.text = "Debug Menu - Option [" + OptionsIndex + "] - " + currentOpt.OptionReturnName() + " - " + currentOpt.OptionReturnActivation();
        }
    }

    private void CreateOrDestroyMenu()
    {
        if(DebugOn && DebugMenu == null)
        {
            DebugMenu = new GameObject();
            DebugMenu.name = "DebugMenu";
            DebugMenu.AddComponent<Canvas>();
            DebugMenu.GetComponent<Canvas>().sortingOrder = 100;
            DebugMenu.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            DebugMenu.AddComponent<CanvasScaler>();
            DebugMenu.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            DebugMenu.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1366, 768);
            DebugMenu.GetComponent<CanvasScaler>().screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

            var OptionName = new GameObject();

            OptionName.name = "OptionNameForDebug";

            OptionName.transform.parent = DebugMenu.transform;

            OptionName.transform.localPosition = new Vector3(-500, -250, 0);

            OptionName.AddComponent<TextMeshProUGUI>();

            OptionName.GetComponent<TextMeshProUGUI>().margin = new Vector4(0, 0, -850, 0);

            OptionName.GetComponent<TextMeshProUGUI>().fontSize = 40;      

            var currentPos = OptionName.transform.position;

            OptionName.transform.position = new Vector3(currentPos.x, currentPos.y - 50, currentPos.z);
        }
        else if (!DebugOn && DebugMenu != null)
        {
            Destroy(DebugMenu);
        }
    }

    private void SetupOption()
    {
        if (DebugMenu != null && Input.GetKeyDown(KeyCode.F3))
        {
            var currentOpt = DebugOptions[OptionsIndex];

            if (currentOpt.OptionReturnActivation())
            {
                currentOpt.OptionQuit();
            }
            else
            {
                currentOpt.OptionSetup();
            }
        }
    }

    private void RunDebugOptions()
    {
        if(DebugOptions.Count > 0)
        {
            for(int x = 0; x < DebugOptions.Count; x++)
            {
                var currentOpt = DebugOptions[x];

                if (currentOpt.OptionReturnActivation())
                {
                    currentOpt.OptionExec();
                }
            }
        }
    }
}
