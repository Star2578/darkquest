using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace DarkQuest.scripts.Global
{
    public static class ChoiceImpact
    {
        public static void Impact(string code)
        {
            if (code == "none")
            {
                GD.Print("No impact");
            }
            // TODO :  Add more here
            else
            {
                GD.Print("Not specified");
            }
        }
    }
}