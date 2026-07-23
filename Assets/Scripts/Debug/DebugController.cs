using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class DebugController : MonoBehaviour
{
    private bool DebugOn = false;

    private bool DebugEnabled = false;

    private int OptionsIndex = 0;

    private GameObject DebugMenu;

    public GameObject[] DebugOptions;

    void Start()
    {
        if (Application.isEditor || Debug.isDebugBuild)
        {
            DebugEnabled = true;

            for (int x = 0; x < DebugOptions.Length; x++)
            {
                DebugOptions[x].GetComponent<DebugOption>().OptionQuit();
            }
        }
    }

    // Update is called once per frame
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

                if(OptionsIndex >= DebugOptions.Length)
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

            var currentOpt = DebugOptions[OptionsIndex].GetComponent<DebugOption>();

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

            var OptionName = new GameObject();

            OptionName.name = "OptionNameForDebug";

            OptionName.transform.parent = DebugMenu.transform;

            OptionName.AddComponent<TextMeshProUGUI>();

            OptionName.GetComponent<TextMeshProUGUI>().margin = new Vector4(0, 0, -300, 0);

            OptionName.GetComponent<TextMeshProUGUI>().fontSize = 20;      

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
            var currentOpt = DebugOptions[OptionsIndex].GetComponent<DebugOption>();

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
        if(DebugOptions.Length > 0)
        {
            for(int x = 0; x < DebugOptions.Length; x++)
            {
                var currentOpt = DebugOptions[x].GetComponent<DebugOption>();

                if (currentOpt.OptionReturnActivation())
                {
                    currentOpt.OptionExec();
                }
            }
        }
    }
}
