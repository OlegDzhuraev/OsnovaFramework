using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OsnovaFramework
{
    public sealed class Filter<X> : IEnumerable<X> where X : BaseComponent
    {
        readonly IEnumerable<X> results;
        
        public Filter() => results = BaseComponent.Filter<X>();
        public Filter(IEnumerable<X> collection) => results = collection;

        public X First() => results.FirstOrDefault();
        public X FirstWith<T>() where T : BaseComponent => results.FirstOrDefault(x => x && x.Entity.Has<T>());
        public X FirstWithout<T>() where T : BaseComponent => results.FirstOrDefault(x => x && !x.Entity.Has<T>());
       
        public Filter<X> With<T>() where T : BaseComponent => new (results.Where(x => x.Entity.Has<T>()));
        public Filter<X> Without<T>() where T : BaseComponent => new (results.Where(x => !x.Entity.Has<T>()));
        public Filter<X> WithSignal<T>() where T : Signal => new (results.Where(x => x.Entity.GetSignal<T>()));
        public Filter<X> WithoutSignal<T>() where T : Signal => new (results.Where(x => !x.Entity.GetSignal<T>()));
        
        public Filter<T> GetAllWith<T>() where T : BaseComponent => new (results.Select(x => x as T));

        public IEnumerator<X> GetEnumerator() => results.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}