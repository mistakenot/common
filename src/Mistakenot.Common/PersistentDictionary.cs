using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mistakenot.Common
{
    /// A dictionary that persists items to disc. FOR TESTING USES ONLY! HERE BE DRAGONS!
    public class PersistentDictionary<T, S>  : IDictionary<T, S>
    {
        private readonly string _folder;
        private readonly Func<S, string> _serializer;
        private readonly Func<string, S> _deserializer;
        private readonly Func<T, string> _getKey;
        private readonly Dictionary<string, T> _keyCache;

        public PersistentDictionary(
            string folder,
            Func<S, string> serializer, 
            Func<string, S> deserializer,
            Func<T, string> getKey = null)
        {
            _folder = folder;
            _serializer = serializer;
            _deserializer = deserializer;
            _getKey = getKey ?? (t => t.ToString());
            _keyCache = new Dictionary<string, T>();
        }

        public S this[T key] 
        {
            get => Read(key);
            set => Write(key, value);
        }

        public ICollection<T> Keys => this.Select(kv => kv.Key).ToList();

        public ICollection<S> Values => this.Select(kv => kv.Value).ToList();

        public int Count => this.Count();

        public bool IsReadOnly => false;

        public void Add(T key, S value) => Write(key, value);

        public void Add(KeyValuePair<T, S> item) => Add(item.Key, item.Value);

        public void Clear()
        {
            foreach (var file in this)
            {
                Delete(file.Key);
            }
        }

        public bool Contains(KeyValuePair<T, S> item) => ContainsKey(item.Key);

        public bool ContainsKey(T key) => Exists(key);

        public void CopyTo(KeyValuePair<T, S>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<T, S>> GetEnumerator()
        {
            return Directory.EnumerateFiles(_folder).Select(file =>
            {
                var path = Path.Combine(_folder, file);
                var text = File.ReadAllText(path);
                var value = _deserializer(text);
                var key = _keyCache[file];

                return new KeyValuePair<T, S>(key, value);
            })
            .GetEnumerator();
        }

        public bool Remove(T key) => Delete(key);

        public bool Remove(KeyValuePair<T, S> item) => Remove(item.Key);

        public bool TryGetValue(T key, out S value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void Write(T key, S value)
        {
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }

            var keyString = _getKey(key);
            var path = Path.Combine(_folder, keyString);
            var serializedValue = _serializer(value);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, serializedValue);
            _keyCache[keyString] = key;
        }

        private S Read(T key)
        {
            var keyString = _getKey(key);
            var path = Path.Combine(_folder, keyString);
            var text = File.ReadAllText(path);

            if (text == null)
            {
                throw new KeyNotFoundException(key.ToString());
            }

            return _deserializer(text);
        }

        private bool Delete(T key)
        {
            var keyString = _getKey(key);
            var path = Path.Combine(_folder, keyString);
            var found = false;

            if (Exists(key))
            {
                File.Delete(path);
                found = true;
            }

            if (_keyCache.ContainsKey(keyString))
            {
                _keyCache.Remove(keyString);
            }

            return found;
        }

        private bool Exists(T key)
        {
            var keyString = _getKey(key);
            var path = Path.Combine(_folder, keyString);

            return File.Exists(path);
        }
    }
}