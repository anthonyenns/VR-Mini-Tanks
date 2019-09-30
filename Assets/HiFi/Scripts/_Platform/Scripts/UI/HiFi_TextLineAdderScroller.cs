using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//*** Add to UI text. Prints a line of text to text UI object with automatic line breaks and scrolling.

namespace HiFi
{
    public class HiFi_TextLineAdderScroller : MonoBehaviour
    {
        public Text textBox;
        public int maxLines = 1;
        public bool displayWorldDebug;

        List<string> allLines = new List<string>();
        string summedText;
        string lastEntry;

        private void Awake()
        {
            if (displayWorldDebug)
                HiFi_Utilities.OnDebugEvent += Print;
        }

        public void Print(string incomingText, bool muteDuplicates = true)  //prints text to each object in textPanels, if filter is true then does not print immediately repeating lines
        {
            if (!muteDuplicates)
            {
                AddText(incomingText);
            }
            else
            {
                if (lastEntry != incomingText)
                {
                    AddText(incomingText);
                }
                lastEntry = incomingText;
            }
        }

        private void AddText(string textToAdd)
        {
            summedText = null;
            allLines.Add(textToAdd);

            if (allLines.Count > maxLines)
            {
                allLines.RemoveAt(0);
            }


            foreach (string line in allLines)
            {
                summedText += line + '\n';
            }
            textBox.text = summedText;
        }

        private void OnDestroy()
        {
            HiFi_Utilities.OnDebugEvent -= Print;
        }
        private void OnDisable()
        {
            HiFi_Utilities.OnDebugEvent -= Print;
        }
    }
}