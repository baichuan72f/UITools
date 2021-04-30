using UnityEditor;

namespace UITools.Editor
{
    public static class DataWayEditor
    {
        [MenuItem("GameObject/ShowDatWayWindow", false, -1)]
        public static void ShowDatWay()
        {
            var window = EditorWindow.GetWindow<DawaWayEditlorWinow>();
            window.Show();
        }
    }
}