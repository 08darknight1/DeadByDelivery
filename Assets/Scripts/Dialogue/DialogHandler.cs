using System.Collections.Generic;
using Rewired;
using TMPro;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
    public List<ConversationContainer> AllConversations = new List<ConversationContainer>();

    public float PrintTimeTarget;

    private Animator _dialogPanelAnimator;

    private TextAsset _csvFile;

    private Player _playerInput;

    private int _currentConversationIndex, _currentPhraseSelected, _currentChar;

    private bool _conversationStarted, _selectedNextPhrase, _finishedCurrentPhrasePrint, _finishConversation;

    private float PrintTime;

    private string PhraseToPrint;

    //TEXT PANEL OUT POSITION TOP: 815 BOT: -305
    //IN POSITION TOP: 500 BOT: 10
    //IN ANIMATION ANCHORED INITIAL POS: -245 LAST POS: -600 

    void Start()
    {
        var currentFileName = "DialogueTest";

        _csvFile = Resources.Load("Imported\\" + currentFileName) as TextAsset;

        ParseTextIntoConversations();

        _dialogPanelAnimator = GameObject.Find("DialogPanel").GetComponent<Animator>();

        _playerInput = ReInput.players.GetPlayer(0);
    }

    void Update()
    {
        if (_conversationStarted)
        {
            if (!_finishConversation)
            {
                if (!_selectedNextPhrase)
                {
                    _currentChar = 0;
                    _selectedNextPhrase = true;
                    var currentConvo = AllConversations[_currentConversationIndex];
                    PhraseToPrint = currentConvo.ReturnPhraseFromIndex(_currentPhraseSelected);
                    _dialogPanelAnimator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                    _dialogPanelAnimator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = currentConvo.ReturnColorFromIndex(_currentPhraseSelected);
                }
                else
                {
                    var textBox = _dialogPanelAnimator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    
                    PrintTime += Time.deltaTime;

                    if(_currentChar < PhraseToPrint.Length){
                        if(PrintTime >= PrintTimeTarget)
                        {
                            textBox.text += PhraseToPrint[_currentChar];
                            PrintTime = 0;
                            _currentChar++;
                        }

                        if (_playerInput.GetButtonDown("Interact"))
                        {
                            textBox.text = PhraseToPrint;
                            _currentChar = PhraseToPrint.Length;
                        }
                    }
                    else
                    {
                        if (!_finishedCurrentPhrasePrint)
                        {
                            Debug.Log("Finished writing Phrase!");
                            _finishedCurrentPhrasePrint = true;
                        }

                        if (_playerInput.GetButtonDown("Interact"))
                        {
                            _currentPhraseSelected++;

                            if (_currentPhraseSelected >= AllConversations[_currentConversationIndex].ReturnConversationSize())
                            {
                                _finishConversation = true;
                            }
                            else
                            {
                                _selectedNextPhrase = false;
                                _finishedCurrentPhrasePrint = false;
                            }
                        }
                    }
                }
            }
            else
            {
                EndConversation();
            }
        }
    }

    public void StartNewConversation(int index)
    {
        _conversationStarted = true;

        _currentConversationIndex = index;

        _dialogPanelAnimator.SetBool("ShowUp", true);
    }

    private void EndConversation()
    {
        _dialogPanelAnimator.SetBool("ShowUp", false);

        if(_dialogPanelAnimator.gameObject.transform.localScale.x <= 0.75f)
        {
            Debug.Log("Ending conversation...");

            _conversationStarted = false;

            _currentPhraseSelected = 0;

            _selectedNextPhrase = false;

            _finishedCurrentPhrasePrint = false;

            _finishConversation = false;
        }
    }

    public bool ReturnConversationStatus()
    {
        return _conversationStarted;
    }

    private void ParseTextIntoConversations()
    {
        string[] extractedText = _csvFile.text.Split(new string[] {";","\n"}, System.StringSplitOptions.RemoveEmptyEntries);
    
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