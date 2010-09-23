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
        public Grid Grid
        {
            get
            {
                return _grid;
            }
        }

        private Picker _picker;

        private static string MAP_MAGIC_STRING = "UWRPG";

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
        /// Create an empty map with the default name of "new_map"
        /// </summary>
        public Map(Grid grid, GameObjectModel groundModel)
        {
            _pathName = "Contents/Map/new_map.map";

            _models = new LinkedList<GameObjectModel>();
            _grid = grid;
            
            _models.AddLast(groundModel);

            _xmlDocument = new XmlDocument();

            XmlNode xmlDec = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _xmlDocument.AppendChild(xmlDec);

            _rootNode = _xmlDocument.CreateElement(MAP_MAGIC_STRING);
            _xmlDocument.AppendChild(_rootNode);

            _picker = new Picker(_grid);
        }

        /// <summary>
        /// Add a new background model to the map
        /// </summary>
        /// <param name="gom"></param>
        public void AddModel(GameObjectModel gom)
        {
            _models.AddLast(gom);
        }

        #region XML Load & Save
        public void Load()
        {
            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(_pathName + ".map");

            _rootNode = _xmlDocument.DocumentElement;

            if (_rootNode.Name != MAP_MAGIC_STRING) {
                throw new ApplicationException("Attempted to load an invalid map file.");
            }

            _grid = Grid.Load(_xmlDocument, _rootNode["Grid"]);

            _models = new LinkedList<GameObjectModel>();
            XmlNodeList models = _xmlDocument.GetElementsByTagName("Model");
            foreach (XmlNode model in models) {
                GameObjectModel gom = GameObjectModel.Load(_xmlDocument, model);
                Game1.console.Log(gom.Name + ": " +  gom.BoundingBox.ToString());
                _models.AddLast(gom);
            }
        }

        public void Save()
        {
            SaveTo(_pathName);
        }

        public void SaveTo(string pathName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode xmlDec = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(xmlDec);

            XmlNode rootNode = xmlDocument.CreateElement(MAP_MAGIC_STRING);
            xmlDocument.AppendChild(rootNode);

            _grid.Save(xmlDocument, rootNode);

            foreach (GameObjectModel gom in _models) {
                gom.Save(xmlDocument, rootNode);
            }

            xmlDocument.Save(pathName);
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            _picker.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            foreach (GameObjectModel gom in _models) {
                gom.Draw(gameTime);
            }

            _grid.Draw(gameTime);
        }

        public void Unload()
        {
            SaveTo(_pathName + ".out");
        }
    }
}
