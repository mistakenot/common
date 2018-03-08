using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mistakenot.Common.Collections
{
    /// A dictionary that persists items to a backing store. FOR TESTING USES ONLY! HERE BE DRAGONS!
    public class PersistentDictionary<T, S>  : IDictionary<T, S>
    {
        private readonly IPersistentDictionaryStorage _storage;
        private readonly Func<S, string> _serializer;
        private readonly Func<string, S> _deserializer;
        private readonly Func<T, string> _getKey;
        private readonly Dictionary<string, T> _keyCache;

        public PersistentDictionary(
            IPersistentDictionaryStorage storage,
            Func<S, string> serializer, 
            Func<string, S> deserializer,
            Func<T, string> getKey = null)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
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
            return _keyCache.Select(kv => 
            {
                var valueString = _storage.Read(kv.Key);
                var value = _deserializer(valueString);
                return new KeyValuePair<T, S>(kv.Value, value);
            })
            .GetEnumerator();
        }

        public bool Remove(T key) => Delete(key);

        public bool Remove(KeyValuePair<T, S> item) => Remove(item.Key);

        public bool TryGetValue(T key, out S value)
        {
            if (Exists(key))
            {
                value = this[key];
                return true;
            }

            value = default(S);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void Write(T key, S value)
        {
            var keyString = _getKey(key);
            var valueString = _serializer(value);

            _storage.Write(keyString, valueString);
            _keyCache[keyString] = key;
        }

        private S Read(T key)
        {
            var keyString = _getKey(key);
            var text = _storage.Read(keyString);

            if (text == null)
            {
                throw new KeyNotFoundException(key.ToString());
            }

            return _deserializer(text);
        }

        private bool Delete(T key)
        {
            var keyString = _getKey(key);
            _storage.Delete(keyString);

            if (_keyCache.ContainsKey(keyString))
            {
                _keyCache.Remove(keyString);
                return true;
            }

            return false;
        }

        private bool Exists(T key) => _storage.Exists(_getKey(key));
    }
}