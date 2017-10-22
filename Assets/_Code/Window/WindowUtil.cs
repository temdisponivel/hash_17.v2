using System.Text;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH.Window
{
    public static class WindowUtil
    {
        public static SimpleList<Window> CurrentlyOpenWindows;
        public static int WindowIds;

        public static void Initialize()
        {
            WindowIds = 0;
            CurrentlyOpenWindows = SList.Create<Window>(5);
        }

        public static Window CreateWindow(WindowType type)
        {
            var references = DataHolder.WindowReferences;

            var windowObj = NGUITools.AddChild(DataHolder.TerminalReferences.WindowsPanel.gameObject, references.WindowPrefab.gameObject);

            var windowComponent = windowObj.GetComponent<WindowComponent>();
            var textWindow = CreateTextWindow(windowComponent.ContentParent);

            windowComponent.DragObject.contentRect = windowComponent.WindowWidget;
            windowComponent.DragObject.panelRegion = DataHolder.TerminalReferences.WindowsPanel;

            var window = new Window();

            window.WindowId = WindowIds++;
            window.SceneWindow = windowComponent;
            window.Type = type;
            window.WindowContent = textWindow;

            windowObj.SetActive(true);

            SList.Add(CurrentlyOpenWindows, window);

            return window;
        }

        public static TextWindowComponent CreateTextWindow(UIPanel parent)
        {
            var textWindowObj = NGUITools.AddChild(parent.gameObject, DataHolder.WindowReferences.TextWindowPrefab.gameObject);
            var textWindow = textWindowObj.GetComponent<TextWindowComponent>();

            GameObjectUtil.AnchorToParentKeepingValues(parent.gameObject, textWindow.MainWidget);

            textWindowObj.SetActive(true);

            return textWindow;
        }

        public static void CloseWindow(Window window)
        {
            GameObject.Destroy(window.SceneWindow.gameObject);
            SList.Remove(CurrentlyOpenWindows, window);
        }

        public static void CloseWindowComponent(WindowComponent windowComponent)
        {
            var window = GetWindowFromWindowComponent(windowComponent);
            CloseWindow(window);
        }

        public static void MaximizeWindowComponent(WindowComponent window)
        {
            if (window.Minimized)
                MinimizeWindowComponent(window);
            
            if (window.Maximized)
            {
                window.WindowWidget.width = Mathf.RoundToInt(window.MaximizedProperties.PreviousSize.x);
                window.WindowWidget.height = Mathf.RoundToInt(window.MaximizedProperties.PreviousSize.y);
                window.transform.position = window.MaximizedProperties.PreviousPosition;
            }
            else
            {
                window.MaximizedProperties.PreviousSize = new Vector2(window.WindowWidget.width, window.WindowWidget.height);
                window.MaximizedProperties.PreviousPosition = window.transform.position;

                window.WindowWidget.width = Screen.currentResolution.width;
                window.WindowWidget.height = Screen.currentResolution.height;
            }
            
            window.Maximized = !window.Maximized;
            var bounds = GameObjectUtil.GetDragObjectBounds(window.DragObject);
            window.DragObject.panelRegion.ConstrainTargetToBounds(window.transform, ref bounds, true);
        }

        public static void MinimizeWindowComponent(WindowComponent window)
        {
            if (window.Maximized)
                MaximizeWindowComponent(window);
            
            if (window.Minimized)
            {
                window.WindowWidget.width = Mathf.RoundToInt(window.MinimizedProperties.PreviousSize.x);
                window.WindowWidget.height = Mathf.RoundToInt(window.MinimizedProperties.PreviousSize.y);
                window.transform.position = window.MinimizedProperties.PreviousPosition;
            }
            else
            {
                window.MinimizedProperties.PreviousSize = new Vector2(window.WindowWidget.width, window.WindowWidget.height);
                window.MinimizedProperties.PreviousPosition = window.transform.position;

                window.WindowWidget.height = window.ControlBox.height;
            }

            window.Minimized = !window.Minimized;
            var bounds = GameObjectUtil.GetDragObjectBounds(window.DragObject);
            window.DragObject.panelRegion.ConstrainTargetToBounds(window.transform, ref bounds, true);
        }

        public static Window GetWindowFromWindowComponent(WindowComponent windowComponent)
        {
            var window = SList.Find(CurrentlyOpenWindows, w => w.SceneWindow == windowComponent);
            DebugUtil.Assert(window == null, "CLOSING A INVALID WINDOW");
            return window;
        }
    }
}