
using Evva.Models;
using System.IO;
using System.Xml.Serialization;

namespace Evva.XMLSerializer
{
    public static class XmlSerializeWrapper<T> where T : new()
    {
        public static FileInfo FileInfo; 
        public static void Serialize (T obj, string filename)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));

            if (!FileInfo.Exists)
            {
                FileInfo.Create().Close();
            }
            FileInfo.Attributes = FileAttributes.Normal;
            using (FileStream fs = new FileStream(FileInfo.FullName, FileMode.Create, FileAccess.ReadWrite))
            {
                formatter.Serialize(fs, obj);
            }
            FileInfo.Attributes = FileAttributes.System | FileAttributes.Hidden | FileAttributes.ReadOnly;
        }

        public static T Deserialize(string filename, FileMode fileMode)
        {
            T obj = new T();
            FileInfo = new FileInfo(filename);

            if (!FileInfo.Exists)
            {
                Serialize(obj, filename);
            }
            FileInfo.Attributes = FileAttributes.Normal;
            using (FileStream fstream = new FileStream(filename, fileMode, FileAccess.ReadWrite))
            {              
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                if (fstream.Length != 0)
                {
                    obj = (T)formatter.Deserialize(fstream);
                }
                else
                {
                    obj = new T();
                }
            }
            FileInfo.Attributes = FileAttributes.System | FileAttributes.Hidden | FileAttributes.ReadOnly;
            return obj;
        }
    }
}

