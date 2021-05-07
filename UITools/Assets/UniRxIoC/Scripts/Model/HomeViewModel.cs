using System;
using Newtonsoft.Json.Linq;
using QFramework;
using UniRx;

namespace UniRxIoC.Example
{
    public class HomeViewModel 
    {
        public ReactiveProperty<JObject> leftData = new ReactiveProperty<JObject>(new JObject());
        public ReactiveProperty<JObject> rightData = new ReactiveProperty<JObject>(new JObject());
        public ReactiveProperty<JObject> editItem = new ReactiveProperty<JObject>(new JObject());
        public ReactiveProperty<JObject> result = new ReactiveProperty<JObject>(new JObject());
    }
}