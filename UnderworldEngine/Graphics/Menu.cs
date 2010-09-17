using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using UnderworldEngine.Scripting;
/*
namespace UnderworldEngine.Graphics
{
    class Menu
    {
        public enum MenuEntryType
        {
            Label,
            Editable,
            Separator
        }

        public struct MenuEntry
        {
            MenuEntryType menuEntryType;
            string text;
            public string Text
            {
                get
                {
                    return text;
                }
            }

            public MenuEntry(MenuEntryType met, string s)
            {
                menuEntryType = met;
                text = s;
            }
        }

        

        
        // content
        public List<MenuEntry> MenuEntries;
        
        private int _menuEntryToDisplay;
        private string _menuRemainingText;

        public Menu()
        {
            

            MenuEntries = new List<MenuEntry>();
            _averageCharSize = calculateAverageCharLength();
            _menuEntryToDisplay = 0;
            _menuRemainingText = "";
        }

        

        public void MoveTo(Vector2 pos)
        {
            _position = pos;
        }

        public virtual void Draw()
        {
            
        }

        

        public void AddEntry(MenuEntryType menuEntryType, string s)
        {
            MenuEntry me = new MenuEntry(menuEntryType, s);
            MenuEntries.Add(me);
        }

        public void Advance()
        {
            ;
        }

        
    }

    
}
*/