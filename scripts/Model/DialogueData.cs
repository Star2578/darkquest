using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkQuest.scripts.Model
{
    public class DialogueData
    {
        public string name;
        public string text;
        public string[] choices;
    }

    public class DialogueList
    {
        public DialogueData[] dialogue;
    }
}