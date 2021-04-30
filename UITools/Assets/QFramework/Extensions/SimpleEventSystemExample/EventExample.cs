using QFramework;
using UnityEngine;
using UniRx;

namespace QF.PackageKit.Example
{
    public class A
    {
    }

    public class B
    {
    }

    public class EventExample : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            SimpleEventSystem.GetEvent<A>()
                .Subscribe(a => Log.I("a message"));
            
            SimpleEventSystem.GetEvent<B>()
                .Select(b=>"b message") // 支持 UniRx 的 LINQ 操作符
                .Subscribe(bMsg => Log.I(bMsg));
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                SimpleEventSystem.Publish(new A());
                
            }

            if (Input.GetMouseButtonUp(1))
            {
                SimpleEventSystem.Publish(new B());
            }
        }
    }
}