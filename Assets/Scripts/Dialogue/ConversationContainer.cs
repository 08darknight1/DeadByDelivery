using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class ConversationContainer
{
    [SerializeField] private string ConversationName;

    private List<string> Dialog = new List<string>();

    private List<Color> DialogColor = new List<Color>();

    public void SetupNewConversation(string name, List<string> dialog, List<Color> dialogColors)
    {
        ConversationName = name;

        for(int x = 0; x < dialog.Count; x++)
        {
            Dialog.Add(dialog[x]);
            DialogColor.Add(dialogColors[x]);
        }
    }

    public string ReturnConversationName()
    {
        return ConversationName;
    }

    public string ReturnPhraseFromIndex(int index)
    {
        return Dialog[index];
    }

    public int ReturnConversationSize()
    {
        return Dialog.Count;
    }

    public Color ReturnColorFromIndex(int index)
    {
        return DialogColor[index];
    }
}
