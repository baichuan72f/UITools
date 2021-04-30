using UnityEngine;
using DG.Tweening;


namespace WWHY.Common
{
    /// <summary>
    /// 环绕摄像机缩放事件
    /// </summary>
    [System.Serializable]
    public class OrbitCameraZoom : UnityEngine.Events.UnityEvent<float>
    {
    }

    /// <summary>
    /// 环绕摄像机
    /// </summary>
    // [AddComponentMenu(AddComponentkMenuConfig.companyTitle+"/Common/OrbitCamera"),RequireComponent(typeof(Camera)),DisallowMultipleComponent]
    public class OrbitCamera : QFramework.MonoSingleton<OrbitCamera>
    {
        #region 可执行的交互能力

        [Header("能旋转")] public bool _canRot = true;
        [Header("能移动")] public bool _canTrans = true;
        [Header("能缩放")] public bool _canZoom = true;
        [Header("受UI层影响")] public bool _pointerOnUI = false;

        #endregion

        #region 摄像机状态

        [Header("当前相机状态")] public Vector3 _curState;
        [Header("初始像机状态")] public Vector3 _initState = new Vector3(0.0f, 30f, 500f);
        [Header("初始标记状态")] public Vector3 _initIconState = new Vector3(0f, 30f, 20f);

        #endregion

        #region 范围取值

        [Header("距目标点最小距离")] // 缩放交互
        public float _minDis = 10f;

        [Header("距目标点最大距离")] // 缩放交互
        public float _maxDis = 1000f;

        [Header("垂直角度最小值")] // 旋转部分交互
        public float _yMinLimit = 0f;

        [Header("垂直角度最大值")] // 旋转部分交互
        public float _yMaxLimit = 90f;

        #endregion

        #region 平滑交互 交互平滑效果

        [Header("交互是否启用平滑")] public bool _smoothMotion = true;
        [Header("移动平滑速度")] public float _smoothTransSpeed = 4f;
        [Header("旋转X平滑速度")] public float _smoothXSpeed = 4f;
        [Header("旋转Y平滑速度")] public float _smoothYSpeed = 4f;
        [Header("缩放平滑速度")] public float _smoothZoomSpeed = 3f;

        #endregion

        #region 交互速度

        [Header("移动速度")] public float _transSpeed = 100f;
        [Header("旋转X速度")] public float _xSpeed = 250f;
        [Header("旋转Y速度")] public float _ySpeed = 120f;
        [Header("缩放速度")] public float _zoomSpeed = 100f;

        #endregion

        #region 其他参数

        // 动画插值行为
        private bool bTweenning;

        // 操作的摄像机
        public Camera cameraView;


        // 画布组
        public CanvasGroup canvasGroup;


        // 初始目标位置 用来做重置时使用
        public Vector3 initTargetPos;

        // 鼠标滑动信息
        private Vector3 mouseDelta = Vector3.zero;

        // 环绕摄像机目标点
        public Transform target;

        // 交互的目标位置和状态信息
        public Vector3 targetPosition;
        public Vector3 targetState;

        // 临时交互信息 能旋转 能移动 能缩放 临时控制行为
        private bool canRotT, canTransT, canZoomT, tem;

        #endregion

        #region 事件

        [Header("当环绕摄像机缩放")] public OrbitCameraZoom onZoomChange = new OrbitCameraZoom();

        #endregion


        #region 私有方法

        /// <summary>/// 应用/// </summary>
        private void Apply()
        {
            // 如果使用平滑 并且 执行动画插值执行中
            if (_smoothMotion && bTweenning)
            {
                // 计算并设置目标点位置信息
                mTarget.position = Vector3.Lerp(mTarget.position, targetPosition, _smoothTransSpeed * Time.deltaTime);

                // 计算出摄像机当前状态
                _curState.x = Mathf.Lerp(_curState.x, targetState.x, _smoothXSpeed * Time.deltaTime);
                _curState.y = Mathf.Lerp(_curState.y, targetState.y, _smoothYSpeed * Time.deltaTime);
                _curState.z = Mathf.Lerp(_curState.z, targetState.z, _smoothZoomSpeed * Time.deltaTime);

                // 操作
                if (Mathf.Abs(_curState.z - targetState.z) <= 0.1f &&
                    Mathf.Abs(_curState.x - targetState.x) <= 0.1f &&
                    Mathf.Abs(_curState.y - targetState.y) <= 0.1f &&
                    Vector3.Distance(mTarget.position, targetPosition) < 0.02f)
                {
                    _curState = targetState; // 摄像机当前状态
                    mTarget.position = targetPosition; // 目标点对象位置设置
                    bTweenning = false; // 动画插值执行完毕
                }

                // 事件
                onZoomChange?.Invoke(DistanceRate);
            }

            // 计算摄像机位置
            CalculateCamPos();
        }

        /// <summary>/// 计算摄像机位置-旋转的计算/// </summary>
        private void CalculateCamPos()
        {
            // 角度
            transform.rotation = Quaternion.Euler(_curState.y, _curState.x, 0.0f);
            // 位置
            transform.position = transform.rotation * new Vector3(0.0f, 0.0f, -_curState.z) + mTarget.position;
            // 事件
            onZoomChange?.Invoke(DistanceRate);
        }

        /// <summary>/// 角度卡钳/// </summary>
        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f)
                angle += 360f;
            if (angle > 360f)
                angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }

        /// <summary>
        /// 旋转 鼠标左键
        /// </summary>
        /// <param name="xValue">鼠标水平轴</param>
        /// <param name="yValue">鼠标垂直轴</param>
        /// <param name="bCtlUserMouse">用户使用的是鼠标</param>
        private void Rotation(float xValue, float yValue, bool bCtlUserMouse = true)
        {
            // 鼠标左键按下 或者 用户不用鼠标交互(RotateDown() RotateLeft() RotateRight() RotateUp())
            if (Input.GetMouseButton(0) || !bCtlUserMouse)
            {
                // 目标状态X
                targetState.x += xValue * _xSpeed * Time.deltaTime;

                // 目标状态Y
                targetState.y -= yValue * _ySpeed * Time.deltaTime;

                // 角度卡钳
                targetState.y = ClampAngle(targetState.y, _yMinLimit, _yMaxLimit);
            }

            // 平滑操作
            if (_smoothMotion)
            {
                if (Mathf.Abs(_curState.z - targetState.z) <= 0.1f &&
                    Mathf.Abs(_curState.x - targetState.x) <= 0.1f &&
                    Mathf.Abs(_curState.y - targetState.y) <= 0.1f)
                    return;

                // 动画插值执行...
                bTweenning = true;
            }
            // 不平滑操作
            else
                _curState = targetState; // 摄像机当前状态
        }

        /// <summary>
        /// 平移 鼠标右键
        /// </summary>
        /// <param name="xValue">鼠标X 水平</param>
        /// <param name="yValue">鼠标Y 垂直</param>
        private void Translation(float xValue, float yValue)
        {
            // 计算出目标位置
            float num = _curState.z / _initState.z;
            targetPosition +=
                transform.TransformVector(new Vector3(xValue, yValue, 0.0f) * num * Time.deltaTime * -_transSpeed);

            // 平滑
            if (_smoothMotion)
                bTweenning = true;
            // 不平滑
            else
                mTarget.position = targetPosition;
        }

        /// <summary>
        /// 缩放 鼠标中键
        /// </summary>
        /// <param name="value"></param>
        private void ZoomInOut(float value)
        {
            targetState.z -= value * _zoomSpeed;
            targetState.z = Mathf.Clamp(targetState.z, _minDis, _maxDis);
            if (_smoothMotion)
                bTweenning = true;
            else
                _curState = targetState;
        }

        /// <summary>
        /// 指针是否在UI元素上
        /// </summary>
        /// <returns></returns>
        private bool PointerOnUI()
        {
            var c = UnityEngine.EventSystems.EventSystem.current;
            var b = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            return c != null && b;
        }

        #endregion


        #region UnityAPI

        private void Awake()
        {
            // 获取操作的摄像机
            cameraView = GetComponent<Camera>();

            // 当应用程序在后台运行时，播放器是否应该在运行？
            Application.runInBackground = true;
            // 帧率限制
            // 指示游戏尝试以指定的帧速率进行渲染。
            Application.targetFrameRate = 30;
        }

        private void Start()
        {
            // 初始化摄像机状态
            targetState = _curState = _initState;

            // 初始化目标点对象位置
            targetPosition = initTargetPos = mTarget.position;

            // 计算摄像机位置
            CalculateCamPos();
        }

        private void Update()
        {
            // 如果围绕目标空 退出不执行
            if (!mTarget)
                return;

            // 存在交互
            if (CanInteraction() && GetInteraction(out mouseDelta))
            {
                // 目标移动信息
                float xValue = mouseDelta.x;
                float yValue = mouseDelta.y;

                // 受UI层交互影响
                if (_pointerOnUI)
                {
                    xValue = PointerOnUI() ? 0.0f : mouseDelta.x;
                    yValue = PointerOnUI() ? 0.0f : mouseDelta.y;
                }

                // 移动/缩放/旋转
                if (_canTrans && Input.GetMouseButton(1))
                    Translation(xValue, yValue);
                if (_canZoom)
                    ZoomInOut(mouseDelta.z);
                if (_canRot)
                    Rotation(xValue, yValue, true);
            }

            Apply();
        }

        #endregion


        #region 属性

        /// <summary>
        /// 获取围绕旋转之间的距离
        /// </summary>
        public float DistanceRate
        {
            get { return (_curState.z - _minDis) / (_maxDis - _minDis); }
        }

        /// <summary>
        /// 获取摄像机围绕的目标对象
        /// </summary>
        public Transform mTarget
        {
            get
            {
                if (!target)
                {
                    GameObject ct = GameObject.Find("cameraTarget");
                    if (!ct)
                    {
                        target = new GameObject("cameraTarget").transform;

                        target.localPosition = Vector3.zero;
                        target.rotation = Quaternion.identity;
                        target.localScale = Vector3.one;
                    }
                    else
                        target = ct.transform;
                }

                return target;
            }
        }

        #endregion


        #region 公开方法

        /// <summary>
        /// 摄像机是否有交互能力
        /// </summary>
        /// <returns>true有交互能力(旋转/缩放/移动)</returns>
        public bool CanInteraction()
        {
            if (!_canRot && !_canZoom)
                return _canTrans;
            return true;
        }


        /// <summary>
        /// 获得交互
        /// </summary>
        /// <param name="delta">滑动信息</param>
        /// <returns>true 有交互执行</returns>
        public bool GetInteraction(out Vector3 delta)
        {
            // 虚拟X轴 旋转水平
            delta.x = Input.GetAxis("Mouse X");

            // 虚拟Y轴 旋转垂直
            delta.y = Input.GetAxis("Mouse Y");

            // 虚拟滚动轴 缩放
            delta.z = Input.GetAxis("Mouse ScrollWheel");

            // 返回状态信息
            if (delta.z == 0f && !Input.GetMouseButton(1) && !Input.GetMouseButton(0))
                return false;
            return true;
        }


        /// <summary>
        /// 当前脚本重置
        /// </summary>
        public void ResetMono()
        {
            // 目标点位置
            mTarget.position = initTargetPos;

            // 摄像机当前状态
            _curState = targetState = _initState;

            // 插值动画执行中
            bTweenning = false;

            // 计算摄像机位置
            CalculateCamPos();
        }


        /// <summary>
        /// 重置目标点位置
        /// </summary>
        public void ResetPos()
        {
            // 目标点位置
            targetPosition = initTargetPos;

            // 动画插值执行
            bTweenning = true;
        }


        /// <summary>
        /// 设置摄像机位置和状态
        /// </summary>
        /// <param name="state">摄像机状态</param>
        /// <param name="pos">摄像机位置</param>
        public void SetCameraPos(Vector3 state, Vector3 pos)
        {
            // 平滑
            if (_smoothMotion)
            {
                targetState = state;
                targetPosition = pos;
                bTweenning = true;
            }
            // 不平滑
            else
            {
                _curState = state;
                mTarget.position = pos;
                Apply();
            }
        }


        /// <summary>
        /// 设置摄像机状态
        /// 镜头管理器中的摄像机状态
        /// </summary>
        /// <param name="state">摄像机状态</param>
        /// <param name="tween">动画插值</param>
        public void SetCameraState(CameraState state, bool tween = true)
        {
            // 平滑
            if (_smoothMotion)
            {
                // 动画
                if (tween)
                {
                    // 杀死动画
                    DOTween.Kill("CameraView", false);
                    // 画布组透明通道
                    canvasGroup.alpha = 0.0f;
                    // 画布视口
                    cameraView.fieldOfView = 45f;
                    // 动画
                    TweenSettingsExtensions.SetId(
                        TweenSettingsExtensions.SetEase(
                            ShortcutExtensions.DOFieldOfView(cameraView, cameraView.fieldOfView * 0.8f, 0.8f),
                            (Ease) 1), "CameraView");

                    // 动画回调
                    TweenSettingsExtensions.OnComplete(
                        TweenSettingsExtensions.SetId(
                            TweenSettingsExtensions.SetEase(canvasGroup.DOFade(1f, 0.8f), (Ease) 12), "CameraView"),
                        new TweenCallback(() => SetCameraState(state, tween)));
                }
                // 不执行动画
                else
                {
                    targetState = state.state;
                    targetPosition = state.targetPos;
                    bTweenning = true;
                }
            }
            // 不平滑
            else
            {
                _curState = state.state;
                mTarget.position = state.targetPos;
                Apply();
            }

            SetInteraction(state.canInteraction);
        }


        /// <summary>
        /// 移动到参数目标点
        /// 场景标记/物体等...
        /// </summary>
        /// <param name="pos">目标点</param>
        /// <param name="state">摄像机状态</param>
        /// <param name="tween">动画执行</param>
        public void MoveToTrans(Transform pos, Vector3 state, bool tween = true)
        {
            targetState = state;
            targetPosition = pos.position;
            bTweenning = tween;
            SetInteraction(false);
        }


        /// <summary>
        /// 移动到参数目标点
        /// 场景标记/物体等...
        /// initIconState 状态参数用的是初始化标记摄像机状态
        /// </summary>
        /// <param name="pos">目标点</param>
        /// <param name="tween">动画执行</param>
        public void MoveToTrans(Transform pos, bool tween = true)
        {
            targetState = _initIconState;
            targetPosition = pos.position;
            bTweenning = tween;
            SetInteraction(false);
        }


        /// <summary>
        /// 设置交互能力
        /// </summary>
        /// <param name="state">交互能力(移动/旋转/缩放)</param>
        public void SetInteraction(bool state)
        {
            _canTrans = state;
            _canRot = state;
            _canZoom = state;
        }


        /// <summary>
        /// 设置环绕摄像机目标点对象位置
        /// </summary>
        /// <param name="pos">位置信息</param>
        public void SetTargetPos(Vector3 pos)
        {
            targetPosition = pos;
            bTweenning = true;
        }


        /// <summary>
        /// 暂停交互能力-临时行为
        /// </summary>
        public void PauseInteraction()
        {
            if (tem || !CanInteraction())
                return;
            tem = true;

            // 记录当前行为
            canRotT = _canRot;
            canTransT = _canTrans;
            canZoomT = _canZoom;

            // 暂停当前交互能力
            _canRot = _canTrans = _canZoom = false;
        }


        /// <summary>
        /// 恢复交互能力-临时行为
        /// </summary>
        public void ResumeInteraction()
        {
            if (!tem)
                return;
            tem = false;

            // 从记录中恢复交互行为
            _canRot = canRotT;
            _canTrans = canTransT;
            _canZoom = canZoomT;

            // 重置临时信息
            canRotT = canTransT = canZoomT = false;
        }


        #region 不使用鼠标时的交互行为

        // 上下左右旋转
        public void RotateDown()
        {
            Rotation(0f, -1f, false);
        }

        public void RotateLeft()
        {
            Rotation(-1f, 0f, false);
        }

        public void RotateRight()
        {
            Rotation(1f, 0f, false);
        }

        public void RotateUp()
        {
            Rotation(0f, 1f, false);
        }


        // 上下左右移动
        public void TransUp()
        {
            Translation(0.0f, 1f);
        }

        public void TransDown()
        {
            Translation(0.0f, -1f);
        }

        public void TransLeft()
        {
            Translation(-1f, 0.0f);
        }

        public void TransRight()
        {
            Translation(1f, 0.0f);
        }


        // 缩放
        public void ZoomIn()
        {
            ZoomInOut(1f);
        }

        public void ZoomOut()
        {
            ZoomInOut(-1f);
        }

        #endregion

        #endregion
    }
}