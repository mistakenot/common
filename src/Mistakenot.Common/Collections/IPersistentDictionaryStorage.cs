namespace Mistakenot.Common.Collections
{
    public interface IPersistentDictionaryStorage
    {
        string Read(string key);

        void Write(string key, string value);

        bool Exists(string key);

        void Delete(string key);
    }
}