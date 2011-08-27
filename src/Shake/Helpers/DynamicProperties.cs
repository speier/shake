//
// Shake - C# Make
//
// Dynamic properties
//
// Author:
//   Kalman Speier (kalman.speier@gmail.com)
//
// Licensed under the terms of the MIT X11
//
// Copyright (c) 2010 Kalman Speier
//
using System.Collections.Generic;
using System.Dynamic;

namespace Shake.Helpers
{
    /// <summary>
    /// Dynamic properties.
    /// </summary>
    public class DynamicProperties : DynamicObject
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        public object this[string key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }

        public int Count
        {
            get { return _properties.Count; }
        }

        public Dictionary<string, object> GetMembers()
        {
            return _properties;
        }

        public object Get(string key)
        {
            object result;

            _properties.TryGetValue(key.ToLower(), out result);

            if (result != null)
            {
                // try if result is a number
                int number;
                if (int.TryParse(result.ToString(), out number))
                    result = number;
            }

            return result;
        }

        public void Set(string key, object value)
        {
            if (!_properties.ContainsKey(key.ToLower()))
                _properties.Add(key.ToLower(), value);
            else
                _properties[key.ToLower()] = value;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = Get(binder.Name);

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Set(binder.Name, value);

            return true;
        }
    }
}