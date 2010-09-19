using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using UnderworldEngine.Graphics;

namespace UnderworldEngine.Game
{
    public class Map
    {
        private string _pathName;
        private XmlDocument _xmlDocument;
        private XmlNode _rootNode;

        private LinkedList<GameObjectModel> _models;
        private Grid _grid;

        private Picker _picker;

        /// <summary>
        /// Load a map from the specified URL/local pathname
        /// </summary>
        /// <param name="mapPathName"></param>
        public Map(string mapPathName)
        {
            mapPathName = "Content/Maps/" + mapPathName;
            _pathName = mapPathName;
            Load();
            _picker = new Picker(_grid);
        }

        /// <summary>
        /// Create an empty map
        /// </summary>
        public Map(Grid grid, GameObjectModel groundModel)
        {
            _models = new LinkedList<GameObjectModel>();
            _grid = grid;
            _picker = new Picker(_grid);
            _models.AddLast(groundModel);

            _xmlDocument = new XmlDocument();

            XmlNode xmlDec = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _xmlDocument.AppendChild(xmlDec);

            _rootNode = _xmlDocument.CreateElement("UWRPG");
            _xmlDocument.AppendChild(_rootNode);
        }

        public void AddModel(GameObjectModel gom)
        {
            _models.AddLast(gom);
        }

        public void Load()
        {
            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(_pathName);

            _rootNode = _xmlDocument.DocumentElement;

            _grid = Grid.Load(_xmlDocument, _rootNode["Grid"]);

            _models = new LinkedList<GameObjectModel>();
            XmlNodeList models = _xmlDocument.GetElementsByTagName("Model");
            foreach (XmlNode model in models) {
                _models.AddLast(GameObjectModel.Load(_xmlDocument, model));
            }
        }

        public void Save()
        {
            SaveTo(_pathName);
        }

        public void SaveTo(string pathName)
        {
            // Save Grid
            _grid.Save(_xmlDocument, _rootNode);

            // Save Models
            foreach (GameObjectModel gom in _models) {
                gom.Save(_xmlDocument, _rootNode);
            }

            // Write out document
            _xmlDocument.Save(pathName);
        }

        public void Update(GameTime gameTime)
        {
            _picker.Update(gameTime);
        }

        public void Draw()
        {
            foreach (GameObjectModel gom in _models) {
                gom.Draw();
            }

            _grid.Draw();
        }

        internal void Unload()
        {
            //throw new NotImplementedException();
        }
    }
}
