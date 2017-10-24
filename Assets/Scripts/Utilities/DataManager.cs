using System.IO;
using System.Xml.Serialization;

public class DataManager : MonoBehaviourSingleton<DataManager> {
    
    public static void XMLMarshalling(string path, object item) {
        using (FileStream fs = new FileStream(path, FileMode.Create)) {
            XmlSerializer xml = new XmlSerializer(item.GetType());
            xml.Serialize(fs, item);
        }
    }

    public static T XMLUnmarshalling<T>(string path) {
        using (FileStream fs = new FileStream(path, FileMode.Open)) {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            return (T)xml.Deserialize(fs);
        }
    }    
}
