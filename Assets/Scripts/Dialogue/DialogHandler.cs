using System.Collections.Generic;
using Rewired;
using TMPro;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
    public List<ConversationContainer> AllConversations = new List<ConversationContainer>();

    private Animator DialogPanelAnimator;

    private TextAsset CsvFile;

    private Player RewiredPlayer;

    private int CurrentConversationIndex, CurrentPhraseSelected, CurrentChar;

    private bool ConversationStarted, SelectedNextPhrase, FinishedCurrentPhrasePrint;

    public float PrintTimeTarget;

    private float PrintTime;

    private string PhraseToPrint;

    //TEXT PANEL OUT POSITION TOP: 815 BOT: -305
    //IN POSITION TOP: 500 BOT: 10
    //IN ANIMATION ANCHORED INITIAL POS: -245 LAST POS: -600 

    void Start()
    {
        var currentFileName = "DialogueTest";

        CsvFile = Resources.Load("Imported\\" + currentFileName) as TextAsset;

        ParseTextIntoConversations();

        DialogPanelAnimator = GameObject.Find("DialogPanel").GetComponent<Animator>();

        RewiredPlayer = ReInput.players.GetPlayer(0);
    }

    void Update()
    {
        if (ConversationStarted)
        {
            if (!SelectedNextPhrase)
            {
                CurrentChar = 0;
                SelectedNextPhrase = true;
                var currentConvo = AllConversations[CurrentConversationIndex];
                PhraseToPrint = currentConvo.ReturnPhraseFromIndex(CurrentPhraseSelected);
                DialogPanelAnimator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                DialogPanelAnimator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = currentConvo.ReturnColorFromIndex(CurrentPhraseSelected);
            }
            else
            {
                var textBox = DialogPanelAnimator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                
                PrintTime += Time.deltaTime;

                if(CurrentChar < PhraseToPrint.Length){
                    if(PrintTime >= PrintTimeTarget)
                    {
                        textBox.text += PhraseToPrint[CurrentChar];
                        PrintTime = 0;
                        CurrentChar++;
                    }

                    if (RewiredPlayer.GetAnyButtonDown())
                    {
                        textBox.text = PhraseToPrint;
                        CurrentChar = PhraseToPrint.Length;
                    }
                }
                else
                {
                    if (!FinishedCurrentPhrasePrint)
                    {
                        Debug.Log("Finished writing Phrase!");
                        FinishedCurrentPhrasePrint = true;
                    }

                    if (RewiredPlayer.GetAnyButtonDown())
                    {
                        CurrentPhraseSelected++;

                        if (CurrentPhraseSelected >= AllConversations[CurrentConversationIndex].ReturnConversationSize())
                        {
                            EndConversation();
                        }
                        else
                        {
                            SelectedNextPhrase = false;
                            FinishedCurrentPhrasePrint = false;
                        }
                    }
                }
            }
        }
    }

    public void StartNewConversation(int index)
    {
        ConversationStarted = true;

        CurrentConversationIndex = index;

        DialogPanelAnimator.SetBool("ShowUp", true);

        //Debug.Log("Starting conversation [" + index + "] - " + AllConversations[index].ReturnConversationName() + "!");
    }

    private void EndConversation()
    {
        ConversationStarted = false;

        CurrentPhraseSelected = 0;

        SelectedNextPhrase = false;

        FinishedCurrentPhrasePrint = false;

        DialogPanelAnimator.SetBool("ShowUp", false);

        Debug.Log("Ending conversation...");
    }

    public bool ReturnConversationStatus()
    {
        return ConversationStarted;
    }

    private void ParseTextIntoConversations()
    {
        string[] extractedText = CsvFile.text.Split(new string[] {";","\n"}, System.StringSplitOptions.RemoveEmptyEntries);
    
        bool dialogFound = false, stillDialog = true;

        string dialogName = "";

        List<string> dialogs = new List<string>();

        List<Color> colors = new List<Color>();

        for(int x = 0; x < extractedText.Length; x++)
        {
            //Debug.Log("ExtractedText: " + extractedText[x] + " | StilDialogValue: " + stillDialog);

            if (!dialogFound && extractedText[x].Contains("DialogueIntro-")) //14 chars
            {
                Debug.Log("New conversation found in extracted Text! Name: " + extractedText[x].Remove(0, 14));
                dialogFound = true;
                dialogName = extractedText[x].Remove(0, 14);
            }
            else if(dialogFound)
            {
                if (extractedText[x].Contains("DialogueEnd"))
                {
                    Debug.Log("Found ending of the dialogue! Finishing creating conversation!");
                    
                    ConversationContainer ConversationToAdd = new ConversationContainer();

                    ConversationToAdd.SetupNewConversation(dialogName, dialogs, colors);

                    AllConversations.Add(ConversationToAdd);

                    dialogFound = false;
                    stillDialog = true;
                    dialogName = "";
                    dialogs.Clear();
                    colors.Clear();
                }
                else
                {
                    if (stillDialog)
                    {
                        dialogs.Add(extractedText[x]);
                    }
                    else
                    {
                        ColorUtility.TryParseHtmlString(extractedText[x], out Color color);
                        colors.Add(color);
                    }

                    stillDialog = !stillDialog;
                }
            }
        }
    }
}