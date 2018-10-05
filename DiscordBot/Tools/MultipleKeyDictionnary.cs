using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace DiscordBot.Tools
{
    public class MultipleKeyDictionnary<T1, T2, T3>
    {

        private readonly Dictionary<T1, T3> _dictionary;

        private readonly Dictionary<T1, T2> _forwardDictionary;
        
        private readonly Dictionary<T2, T1> _reverseDictionary;
        
        public MultipleKeyDictionnary()
        {
            _dictionary = new Dictionary<T1, T3>();
            _forwardDictionary = new Dictionary<T1, T2>();
            _reverseDictionary = new Dictionary<T2, T1>();
        }

        public IEnumerator<KeyValuePair<T1, T3>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public bool Remove(T1 key)
        {
            return _dictionary.Remove(key) && _reverseDictionary.Remove(_forwardDictionary[key]) && 
                   _forwardDictionary.Remove(key);
        }
        
        public bool Remove(T2 key)
        {
            return _dictionary.Remove(_reverseDictionary[key]) && _forwardDictionary.Remove(_reverseDictionary[key]) &&
                   _reverseDictionary.Remove(key);
        }
        
        public bool ContainsKey(T2 key)
        {
            return _dictionary.ContainsKey(_reverseDictionary[key]);
        }

        public bool ContainsKey(T1 key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool TryGetValue(T2 key, out T3 value)
        {
            return _dictionary.TryGetValue(_reverseDictionary[key], out value);
        }

        public bool TryGetValue(T1 key, out T3 value)
        {
            return _dictionary.TryGetValue(key, out value);
        }
        
        public void Add(T1 key, T2 key2 ,T3 value)
        {
            _dictionary.Add(key, value);
            _forwardDictionary.Add(key, key2);
            _reverseDictionary.Add(key2, key);
        }
        
        public T3 this[T2 key] => _dictionary[_reverseDictionary[key]];

        public T3 this[T1 key] => _dictionary[key];

        public IEnumerable<T1> Keys => _dictionary.Keys;
        public IEnumerable<T3> Values => _dictionary.Values;

        public int Count => _dictionary.Count;
    }
}