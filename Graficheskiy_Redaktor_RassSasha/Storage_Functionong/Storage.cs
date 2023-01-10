using Graficheskiy_Redaktor_RussAlex.Line_Functioning;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Graficheskiy_Redaktor_RussAlex.Storage_Functionong
{
    class Storage
    {
        public static void Save(SaveFileDialog _saveFD, List<LineClass> _lineList)
        {
            if (_lineList.Count == 0)
            {
                MessageBox.Show("File is empty!", "Warning");
            }
            else
            {
                if (_saveFD.ShowDialog() == DialogResult.OK)
                {
                    foreach (LineClass lineClass_List_Element in _lineList)
                        lineClass_List_Element.Selected = false;
                    string fileName = _saveFD.FileName;
                    Serializer.Save($"{fileName}", _lineList);
                }
            }
        }

        public static List<LineClass> Load(OpenFileDialog _openFD, List<LineClass> _lineList)
        {
            if (_openFD.ShowDialog() == DialogResult.OK)
            {
                string fileName = _openFD.FileName;
                _lineList = Serializer.Load<List<LineClass>>($"{fileName}");
            }
            return _lineList;
        }

        public static class Serializer
        {
            public static void Save(string filePath, object objToSerialize)
            {
                try
                {
                    using (Stream stream = File.Open(filePath, FileMode.Create))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, objToSerialize);
                    }
                }
                catch (IOException)
                {

                }
            }

            public static T Load<T>(string filePath) where T : new()
            {
                T rez = new T();

                try
                {
                    using (Stream stream = File.Open(filePath, FileMode.Open))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        rez = (T)bin.Deserialize(stream);
                    }
                }
                catch (IOException)
                {

                }

                return rez;
            }
        }
    }
}
