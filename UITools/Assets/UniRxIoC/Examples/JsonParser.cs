using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using QF.PackageKit.Example;
using QFramework;
using UniRx;
using UnityEngine;
using UniRxIoC.Base;

namespace UniRxIoC.Example
{
    public class JsonParser : MonoBehaviour, ISingleton
    {
        private void Start()
        {
            UIKit.OpenPanel<HomeView>();
        }

        public static QFrameworkContainer Container
        {
            get { return MonoSingletonProperty<JsonParser>.Instance.mContainer; }
        }

        QFrameworkContainer mContainer;

        public void OnSingletonInit()
        {
            mContainer = new QFrameworkContainer();

            #region 注册服务

            mContainer.Register<INetWorkService, NetWorkService>();
            mContainer.Register<IFileLoader, FileLoader>();
            #endregion

            #region 注册数据层

            var model = new HomeViewModel();
            mContainer.RegisterInstance(model);

            #endregion

            #region 注册消息

            #endregion
        }
    }
}

public class ParserMsg
{
    public string From;
    public string To;

    public ParserMsg(string from, string to)
    {
        To = to;
        From = from;
    }
}