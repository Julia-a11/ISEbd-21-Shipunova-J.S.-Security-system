using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;

namespace SecuritySystemBusinessLogic.BusinessLogics
{
    public abstract class BackUpAbstractLogic
    {
        public void CreateArchive(string folderName)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(folderName);
                if (directoryInfo.Exists)
                {
                    foreach (FileInfo file in directoryInfo.GetFiles())
                    {
                        file.Delete();
                    }
                }

                string fileName = $"{folderName}.zip";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                // Берём сборку, чтобы от неё создавать объекты
                Assembly assembly = GetAssembly();

                // Вытаскиваем список классов для сохранения
                var dbsets = GetFullList();

                // Берём метод для сохранения(из базового абстрактного класса)
                MethodInfo method = GetType().BaseType.GetTypeInfo().GetDeclaredMethod("SaveToFile");
                foreach (var set in dbsets)
                {
                    // Создаём объект из класса для сохранения
                    var elem = assembly.CreateInstance(set.PropertyType.GenericTypeArguments[0].FullName);

                    // Генерируем метод исходя из класса 
                    MethodInfo generic = method.MakeGenericMethod(elem.GetType());

                    // Вызываем метод на выполнение
                    generic.Invoke(this, new object[] { folderName });
                }

                // Архивируем
                ZipFile.CreateFromDirectory(folderName, fileName);

                // Удаляем папку
                directoryInfo.Delete(true);
            }
            catch (Exception)
            {
                // Делаем проброс
                throw;
            }
        }

        private void SaveToFile<T>(string folderName) where T : class, new()
        {
            var records = GetList<T>();
            T obj = new T();
            var typeName = obj.GetType().Name;
            if (records != null)
            {
                var root = new XElement(typeName + 's');
                foreach (var record in records)
                {
                    var elem = new XElement(typeName);
                    foreach (var member in obj.GetType().GetMembers()
                        .Where(rec => rec.MemberType != MemberTypes.Method && 
                        rec.MemberType != MemberTypes.Constructor &&
                        !rec.ToString().Contains(".Models.")))
                    {
                        elem.Add(new XElement(member.Name, record.GetType().GetProperty(member.Name)?.GetValue(record) ?? "null"));
                    }
                    root.Add(elem);
                }
                XDocument xDocument = new XDocument(root);
                xDocument.Save(string.Format("{0}/{1}.xml", folderName, typeName));
            } 
        }

        protected abstract Assembly GetAssembly();

        protected abstract List<PropertyInfo> GetFullList();

        protected abstract List<T> GetList<T>() where T : class, new();
    }
}
