using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkQuest.scripts.Models
{
    public class DialogueData
    {
        public string Name;
        public string Text;
        public string[] Choices;
    }

    public class DialogueList
    {
        public DialogueData[] Dialogue;
        public DialogueData[] Dialogue_Exhausted;
    }
}