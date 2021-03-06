﻿#region Licence
/* The MIT License (MIT)
Copyright © 2014 Ian Cooper <ian_hammond_cooper@yahoo.co.uk>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the “Software”), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */
#endregion

using System;
using System.Collections;
using System.Collections.Generic;

namespace paramore.brighter.commandprocessor
{
    public class SubscriberRegistry : IAmASubscriberRegistry, IEnumerable<KeyValuePair<Type, List<Type>>>
    {
        readonly Dictionary<Type, List<Type>> observers = new Dictionary<Type, List<Type>>(); 

        public IEnumerable<Type> Get<TRequest>() where TRequest : class, IRequest
        {
            var observed = observers.ContainsKey(typeof (TRequest));
            return observed ? observers[typeof (TRequest)] : new List<Type>();
        }

        public void Register<TRequest, TImplementation>() where TRequest: class, IRequest where TImplementation: class, IHandleRequests<TRequest>
        {
            Add(typeof(TRequest), typeof(TImplementation));
        }
        private void Add(Type requestType, Type handlerType)
        {
            var observed = observers.ContainsKey(requestType);
            if (!observed)
                observers.Add(requestType, new List<Type>{handlerType});
            else
                observers[requestType].Add(handlerType);
        }

        public IEnumerator<KeyValuePair<Type, List<Type>>> GetEnumerator()
        {
            return observers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
