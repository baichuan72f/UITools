

using System.Collections.Generic;
using UnityEngine;

namespace WWHY.Common
{
    /// <summary>
    /// 摄像机状态管理器
    /// </summary>
    // [AddComponentMenu(AddComponentMenuConfig.companyTitle+"/Common/CameraStateManager"),RequireComponent(typeof(OrbitCamera)),DisallowMultipleComponent]
    public class CameraStateManager : QFramework.MonoSingleton<CameraStateManager>
    {
        [Header("Start摄像机镜头状态")]
        public int defaultState = 0;

        [SerializeField]
        private List<CameraState> states;

        private void Start()
        {
            if (states != null && states.Count > 0)
                Invoke("SwitchDefaultState", 0.1f);
        }
        private void SwitchDefaultState(){ Switch(States[defaultState].name); }

        /// <summary>
        /// 镜头切换
        /// </summary>
        /// <param name="stateName">镜头状态名称</param>
        /// <param name="tween">是否动画</param>
        public void Switch(string stateName, bool tween = false)
        {
            foreach (CameraState state in this.States)
                if (state.name == stateName)
                    OrbitCamera.Instance.SetCameraState(state, tween);
        }


        /// <summary>
        /// 获取摄像机镜头状态集合
        /// </summary>
        public List<CameraState> States{ get{ return states == null ? states = new List<CameraState>() : states; }}
    }

}
