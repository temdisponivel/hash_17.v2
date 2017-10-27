using System.Text;
using HASH.GUI;
using NUnit.Framework.Api;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH.Window
{
    public static class WindowUtil
    {
        public static SimpleList<Window> CurrentlyOpenWindows;
        public static int WindowIds;

        public static WindowState DefaultTextState;
        public static WindowState DefaultImageState;

        public static void Initialize()
        {
            WindowIds = 0;
            CurrentlyOpenWindows = SList.Create<Window>(5);
            
            DefaultTextState = new WindowState();
            DefaultTextState.CanBeClosed = true;
            DefaultTextState.CanBeMoved = true;
            DefaultTextState.CanBeResized = true;
            
            DefaultImageState = new WindowState();
            DefaultImageState.CanBeClosed = true;
            DefaultImageState.CanBeMoved = true;
            DefaultImageState.CanBeResized = false;
        }

        public static void CreateTextWindow(string content)
        {
            var window = CreateWindow(DefaultTextState);
            window.Type = WindowType.TextWindow;
            
            var textWindow = CreateTextWindow(window.SceneWindow.ContentParent);
            textWindow.TextComponent.text = content;
            window.WindowContent = textWindow;
        }

        public static void CreateImageWindow(Texture2D content)
        {
            var window = CreateWindow(DefaultImageState);
            window.Type = WindowType.ImageWindow;
            
            var texture = content;

            var imageWindow = CreateImageWindow(window.SceneWindow.ContentParent);
            imageWindow.ImageHolder.mainTexture = texture;

            SetWindowSize(window.SceneWindow, texture.width, texture.height);

            window.WindowContent = imageWindow;
        }

        private static Window CreateWindow(WindowState defaultState)
        {
            var references = DataHolder.GUIReferences;

            var windowObj = NGUITools.AddChild(DataHolder.TerminalReferences.WindowsPanel.gameObject, references.WindowPrefab.gameObject);

            var windowComponent = windowObj.GetComponent<WindowComponent>();

            windowComponent.ControlBar.DragObject.contentRect = windowComponent.WindowWidget;
            windowComponent.ControlBar.DragObject.panelRegion = DataHolder.TerminalReferences.WindowsPanel;

            var window = new Window();

            window.WindowId = WindowIds++;
            window.SceneWindow = windowComponent;
            
            SetWindowState(window, defaultState);

            windowObj.SetActive(true);

            SList.Add(CurrentlyOpenWindows, window);

            return window;
        }

        public static TextWindowComponent CreateTextWindow(UIPanel parent)
        {
            var textWindowObj = NGUITools.AddChild(parent.gameObject, DataHolder.GUIReferences.TextWindowPrefab.gameObject);
            var textWindow = textWindowObj.GetComponent<TextWindowComponent>();

            GameObjectUtil.AnchorToParentKeepingValues(parent.gameObject, textWindow.MainWidget);

            textWindowObj.SetActive(true);

            return textWindow;
        }

        public static ImageWindowComponent CreateImageWindow(UIPanel parent)
        {
            var textWindowObj = NGUITools.AddChild(parent.gameObject, DataHolder.GUIReferences.ImageWindowPrefab.gameObject);
            var textWindow = textWindowObj.GetComponent<ImageWindowComponent>();

            GameObjectUtil.AnchorToParentKeepingValues(parent.gameObject, textWindow.MainWidget);

            textWindowObj.SetActive(true);

            return textWindow;
        }

        public static void SetWindowSize(WindowComponent window, int width, int height)
        {
            if (width >= 0)
                window.WindowWidget.width = width;
            
            if (height >= 0)
                window.WindowWidget.height = height;

            var bounds = GameObjectUtil.GetDragObjectBounds(window.ControlBar.DragObject);
            window.ControlBar.DragObject.panelRegion.ConstrainTargetToBounds(window.transform, ref bounds, true);
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

        public static void MaximizeWindowComponent(WindowComponent windowComponent)
        {
            if (windowComponent.Minimized)
                MinimizeWindowComponent(windowComponent);

            var window = GetWindowFromWindowComponent(windowComponent);
            if (!window.State.CanBeResized)
                return;

            int width;
            int height;

            if (windowComponent.Maximized)
            {
                width = Mathf.RoundToInt(windowComponent.MaximizedProperties.PreviousSize.x);
                height = Mathf.RoundToInt(windowComponent.MaximizedProperties.PreviousSize.y);

                windowComponent.transform.position = windowComponent.MaximizedProperties.PreviousPosition;
            }
            else
            {
                windowComponent.MaximizedProperties.PreviousSize =
                    new Vector2(windowComponent.WindowWidget.width, windowComponent.WindowWidget.height);
                windowComponent.MaximizedProperties.PreviousPosition = windowComponent.transform.position;

                width = Screen.currentResolution.width;
                height = Screen.currentResolution.height;
            }

            windowComponent.Maximized = !windowComponent.Maximized;
            SetWindowSize(windowComponent, width, height);
        }

        public static void MinimizeWindowComponent(WindowComponent window)
        {
            if (window.Maximized)
                MaximizeWindowComponent(window);

            int width = -1;
            int height = 0;

            if (window.Minimized)
            {
                width = Mathf.RoundToInt(window.MinimizedProperties.PreviousSize.x);
                height = Mathf.RoundToInt(window.MinimizedProperties.PreviousSize.y);
                window.transform.position = window.MinimizedProperties.PreviousPosition;
            }
            else
            {
                window.MinimizedProperties.PreviousSize = new Vector2(window.WindowWidget.width, window.WindowWidget.height);
                window.MinimizedProperties.PreviousPosition = window.transform.position;

                height = window.ControlBar.ControlBox.height;
            }

            window.Minimized = !window.Minimized;
            SetWindowSize(window, width, height);
        }

        public static Window GetWindowFromWindowComponent(WindowComponent windowComponent)
        {
            var window = SList.Find(CurrentlyOpenWindows, w => w.SceneWindow == windowComponent);
            DebugUtil.Assert(window == null, "CLOSING A INVALID WINDOW");
            return window;
        }

        public static void KeepWindowInsideScreen(WindowComponent windowComponent)
        {
            var bounds = GameObjectUtil.GetDragObjectBounds(windowComponent.ControlBar.DragObject);
            windowComponent.ControlBar.DragObject.panelRegion.ConstrainTargetToBounds(windowComponent.transform, ref bounds, true);
        }

        public static void SetWindowMovable(Window window, bool canMove)
        {
            if (window.State.CanBeMoved == canMove)
                return;

            window.State.CanBeMoved = canMove;
            window.SceneWindow.ControlBar.DragObject.enabled = canMove;
        }

        public static void SetWindowResizable(Window window, bool canResize)
        {
            window.State.CanBeResized = canResize;
            for (int i = 0; i < window.SceneWindow.Resizers.Length; i++)
            {
                var resizer = window.SceneWindow.Resizers[i];
                resizer.DragResize.enabled = canResize;
            }

            if (canResize)
                window.SceneWindow.ControlBar.MaximizeButton.IgnoreClick = false;
            else
                window.SceneWindow.ControlBar.MaximizeButton.IgnoreClick = true;
            
            GUIUtil.UpdateButtonCursor(window.SceneWindow.ControlBar.MaximizeButton);
        }

        public static void SetWindowClosable(Window window, bool canBeClosed)
        {
            window.State.CanBeClosed = canBeClosed;
            if (canBeClosed)
                window.SceneWindow.ControlBar.CloseButton.IgnoreClick = false;
            else
                window.SceneWindow.ControlBar.CloseButton.IgnoreClick = true;
            
            GUIUtil.UpdateButtonCursor(window.SceneWindow.ControlBar.CloseButton);
        }

        public static void SetWindowState(Window window, WindowState state)
        {
            window.State = state;
            
            SetWindowMovable(window, state.CanBeMoved);
            SetWindowClosable(window, state.CanBeClosed);
            SetWindowResizable(window, state.CanBeResized);
        }
    }
}