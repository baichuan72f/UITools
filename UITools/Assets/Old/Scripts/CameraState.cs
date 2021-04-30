using System;
using UnityEngine;

namespace WWHY.Common
{
    /// <summary>
    /// 摄像机设置状态
    /// </summary>
    [Serializable]
    public class CameraState
    {
        public bool canInteraction = false;// 能否相互作用
        public string name = string.Empty; // 状态名称
        public Vector3 state = Vector3.zero;// 状态位置       注意：x-> 沿着y轴的角度  y->沿着x轴角度  z->据目标点保持的距离
        public Vector3 targetPos = Vector3.zero;// 目标位置
    }
}
