using System.Collections;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MasterAnalyticsDeadByDaylight.Utils.Helper
{
    public class FileHelper
    {
        public static async Task CreateJsonBackupFileAsync<T>(List<T> Data, string filePath)
        {
            await Task.Run(() => 
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    string json = JsonSerializer.Serialize(Data);
                    File.WriteAllText(filePath, json);
                }
                else
                {
                    throw new Exception("Не указан путь");
                }
            });
        }


        //public static async Task CreateXmlBackupFileAsync<T>(List<T> Data, string filePath)
        //{
        //    await Task.Run(() =>
        //    {
        //        if (Data == null || Data.Count == 0)
        //        {
        //            throw new ArgumentException("Список пуст");
        //        }

        //        XmlSerializer serializer = new XmlSerializer(typeof(T));

        //        using (StreamWriter writer = new StreamWriter(filePath))
        //        {
        //            serializer.Serialize(writer, Data);
        //        }
        //    });
        //}

        public static async Task CreateXmlBackupFileAsync<T>(List<T> Data, string filePath)
        {
            await Task.Run(() =>
            {
                using (XmlWriter writer = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true }))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Items");

                    foreach (var item in Data)
                    {
                        writer.WriteStartElement($"{typeof(T).Name}");

                        var properties = typeof(T).GetProperties();

                        foreach (var property in properties)
                        {
                            var value = property.GetValue(item);

                            writer.WriteStartElement(property.Name);

                            if (value is IEnumerable<object> collection)
                            {
                                foreach (var element in collection)
                                {
                                    writer.WriteStartElement("Item");
                                    writer.WriteString(element != null ? element.ToString() : string.Empty);
                                    writer.WriteEndElement();
                                }
                            }
                            else if (value is byte[] byteArray)
                            {
                                writer.WriteString(Convert.ToBase64String(byteArray));
                            }
                            else
                            {
                                writer.WriteString(value != null ? value.ToString() : string.Empty);
                            }

                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            });
        }

        public static async Task CreateExcelBackupFileAsync<T>(List<T> Data, string filePath)
        {
            await Task.Run(() =>
            {

            });
        }
    }
}
