using System;
using Newtonsoft.Json.Linq;
using UniRx;

namespace UniRxIoC.Example
{
    public class HomeViewModel
    {
        public ReactiveProperty<string> someData = new ReactiveProperty<string>(String.Empty);
        
        public ReactiveProperty<JObject> leftData = new ReactiveProperty<JObject>(null);
        public ReactiveProperty<JObject> ParserData = new ReactiveProperty<JObject>(null);
        public ReactiveProperty<JObject> rightData = new ReactiveProperty<JObject>(null);

        public void Init()
        {
        }
    }
}