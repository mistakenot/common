using System;
using System.IO;

namespace Mistakenot.Common.Collections
{
    public class FileSystemPersistentDictionaryStorage : IPersistentDictionaryStorage
    {
        private readonly string _folder;

        public FileSystemPersistentDictionaryStorage(string path)
        {
            _folder = path ?? throw new ArgumentNullException(nameof(path));
        }

        public void Delete(string key)
        {
            var path = Path.Combine(_folder, key);

            if (Exists(key))
            {
                File.Delete(path);
            }
        }

        public bool Exists(string key)
        {
            var path = Path.Combine(_folder, key);
            return File.Exists(path);
        }

        public string Read(string key)
        {
            var path = Path.Combine(_folder, key);
            return File.ReadAllText(path);
        }

        public void Write(string key, string value)
        {
             if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }

            var path = Path.Combine(_folder, key);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, value);
        }
    }
}