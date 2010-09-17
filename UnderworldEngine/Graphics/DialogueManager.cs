using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using UnderworldEngine.Scripting;

namespace UnderworldEngine.Graphics
{
    class DialogueManager
    {
        private LinkedList<string> _lines;

        private int _xSize;
        private int _ySize;
        private int _xPosition;
        private int _yPosition;
        private Vector2 _position;
        private Dialogue _dialogue;

        private string[] _linesToWrite;

        public DialogueManager(int xSize, int ySize, int xPosition, int yPosition)
        {
            _lines = new LinkedList<string>();
            _xSize = xSize;
            _ySize = ySize;
            _xPosition = xPosition;
            _yPosition = yPosition;
            _position = new Vector2(xPosition, yPosition);

            Dialogue.Initialize();
            _dialogue = new Dialogue(this, xSize, ySize, xPosition, yPosition);
        }

        public void AddLines(string line)
        {
            string remainingText = line;
            Vector2 textStart = new Vector2(_position.X + 5, _position.Y + 5);
            Vector2 textEnd = new Vector2(_position.X + _xSize - 5, _position.Y + _ySize - 5);
            float maxTextWidth = _xSize - 5;

            while (remainingText.Length > 0) {
                Vector2 measurement = Dialogue.Font.MeasureString(remainingText);

                // Find estimated number of characters can fit width;
                int estimatedChars = (int)(maxTextWidth / Dialogue.AverageCharSize.X);
                int numChars = estimatedChars > remainingText.Length ?
                    remainingText.Length : estimatedChars;
                String candidate = new String(remainingText.ToCharArray(), 0, numChars);
                // Increment until string fits
                while ((Dialogue.Font.MeasureString(candidate).X < maxTextWidth) &&
                       (remainingText.Length > candidate.Length)
                      ) {
                    candidate = new String(remainingText.ToCharArray(), 0, candidate.Length + 1);
                }
                // Decrement until string fits
                while (Dialogue.Font.MeasureString(candidate).X > maxTextWidth) {
                    candidate = new String(candidate.ToCharArray(), 0, candidate.Length - 1);
                }

                // Adaptive averaging
                Dialogue.AverageCharSize = (Dialogue.Font.MeasureString(candidate));
                Dialogue.AverageCharSize.X *= (1.0f / (float)candidate.Length);

                // Queue line
                _lines.AddLast(candidate);

                // Cut out processed
                numChars = remainingText.Length - candidate.Length > 0 ?
                    remainingText.Length - candidate.Length : remainingText.Length;
                if (numChars != remainingText.Length) {
                    remainingText =
                        new String(remainingText.ToCharArray(),
                        candidate.Length, numChars
                        );
                }
                else {
                    remainingText = "";
                }

                // move "cursor" to next line
                textStart.Y += Dialogue.FontHeight + Dialogue.FontVerticalSpacing;
            }

            figureOutLinesToWrite();
        }

        private void figureOutLinesToWrite()
        {
            // Figure out Lines to Write
            LinkedList<string> linesToWriteList = new LinkedList<string>();

            Vector2 textStart = new Vector2(_position.X + 5, _position.Y + 5);
            Vector2 textEnd = new Vector2(_position.X + _xSize - 5, _position.Y + _ySize - 5);

            foreach (string s in _lines) {
                if (textStart.Y > textEnd.Y) {
                    break;
                }

                linesToWriteList.AddLast(s);

                textStart.Y += Dialogue.FontHeight + Dialogue.FontVerticalSpacing;
            }

            _linesToWrite = linesToWriteList.ToArray<string>(); ;
        }

        public string[] GetLinesToWrite()
        {
            return _linesToWrite;
        }

        public void Draw()
        {
            if (_lines.Count() <= 0) {
                return;
            }
            _dialogue.DrawLine();
        }

        public void Advance()
        {
            foreach (string s in _linesToWrite) {
                _lines.RemoveFirst();
            }

            figureOutLinesToWrite();
        }

        private static Keys lastKeyPress = Keys.F13;
        internal void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Space)) {
                lastKeyPress = Keys.Space;
            }
            else if (lastKeyPress == Keys.Space && keyState.IsKeyUp(Keys.Space)) {
                lastKeyPress = Keys.F13;
                Advance();
            }
        }
    }
}
