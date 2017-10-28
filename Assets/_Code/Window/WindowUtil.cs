using HASH.GUI;
using SimpleCollections.Lists;
using SimpleCollections.Util;
using UnityEngine;

namespace HASH.Window
{
    public static class WindowUtil
    {
        public static SimpleList<Window> CurrentlyOpenWindows;
        public static int WindowIds;

        public static WindowState DefaultTextState;
        public static WindowState DefaultImageState;

        public const float UpdateScrollBarsInterval = .1f;

        public static WindowComponent CurrentlyFocusedWindow;

        public const int FocusedWindowDepthIncrement = 999;

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

            LoopUtil.CallForever(UpdateWindowsScrollBars, UpdateScrollBarsInterval);
        }

        public static void CreateTextWindow(string content)
        {
            var window = CreateWindow(DefaultTextState);
            window.Type = WindowType.TextWindow;

            var textWindow = CreateTextWindow(window.SceneWindow.ContentParent);
            textWindow.TextComponent.text = content;
            window.WindowContent = textWindow;

            CacheDepths(window.SceneWindow);
            Focus(window.SceneWindow);
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

            CacheDepths(window.SceneWindow);
            Focus(window.SceneWindow);
        }

        private static Window CreateWindow(WindowState defaultState)
        {
            var references = DataHolder.GUIReferences;

            var windowObj = NGUITools.AddChild(DataHolder.GUIReferences.WindowsPanel.gameObject, references.WindowPrefab.gameObject);

            var windowComponent = windowObj.GetComponent<WindowComponent>();

            windowComponent.ControlBar.DragObject.contentRect = windowComponent.WindowWidget;
            windowComponent.ControlBar.DragObject.panelRegion = DataHolder.GUIReferences.WindowsPanel;

            var window = new Window();

            window.WindowId = WindowIds++;
            window.SceneWindow = windowComponent;

            SetWindowState(window, defaultState);

            windowObj.SetActive(true);

            SList.Add(CurrentlyOpenWindows, window);

            windowComponent.WidgetsDefaultDepth = SList.Create<Pair<UIWidget, int>>(3);
            windowComponent.PanelsDefaultDepth = SList.Create<Pair<UIPanel, int>>(3);

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

        public static void SetWindowSize(WindowComponent windowComponent, int width, int height)
        {
            if (width >= 0)
                windowComponent.WindowWidget.width = width;

            if (height >= 0)
                windowComponent.WindowWidget.height = height;

            var bounds = GameObjectUtil.GetDragObjectBounds(windowComponent.ControlBar.DragObject);
            windowComponent.ControlBar.DragObject.panelRegion.ConstrainTargetToBounds(windowComponent.transform, ref bounds, true);
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

        public static void MinimizeWindowComponent(WindowComponent windowComponent)
        {
            if (windowComponent.Maximized)
                MaximizeWindowComponent(windowComponent);

            int width = -1;
            int height = 0;

            if (windowComponent.Minimized)
            {
                width = Mathf.RoundToInt(windowComponent.MinimizedProperties.PreviousSize.x);
                height = Mathf.RoundToInt(windowComponent.MinimizedProperties.PreviousSize.y);
                windowComponent.transform.position = windowComponent.MinimizedProperties.PreviousPosition;

                var window = GetWindowFromWindowComponent(windowComponent);
                SetWindowResizable(window, true);
            }
            else
            {
                windowComponent.MinimizedProperties.PreviousSize =
                    new Vector2(windowComponent.WindowWidget.width, windowComponent.WindowWidget.height);
                windowComponent.MinimizedProperties.PreviousPosition = windowComponent.transform.position;

                height = windowComponent.ControlBar.ControlBox.height;

                var window = GetWindowFromWindowComponent(windowComponent);
                SetWindowResizable(window, false);
            }

            windowComponent.Minimized = !windowComponent.Minimized;
            SetWindowSize(windowComponent, width, height);
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

        public static void UpdateWindowsScrollBars()
        {
            for (int i = 0; i < CurrentlyOpenWindows.Count; i++)
            {
                var window = CurrentlyOpenWindows[i];
                if (window.Type == WindowType.TextWindow)
                {
                    var textWindow = window.WindowContent as TextWindowComponent;
                    var size = new Vector2(textWindow.MainWidget.width, textWindow.MainWidget.height);
                    var diff = textWindow.LastSize - size;
                    if (diff.sqrMagnitude > 0.1f)
                    {
                        GUIUtil.UpdateScrollBar(textWindow.ScrollView);
                        textWindow.LastSize = size;
                    }
                }
            }
        }

        public static void CacheWidgetDepths(WindowComponent windowComponent)
        {
            SList.Clear(windowComponent.WidgetsDefaultDepth);

            var widgets = windowComponent.gameObject.GetComponentsInChildren<UIWidget>(true);
            for (int i = 0; i < widgets.Length; i++)
            {
                var widget = widgets[i];

                var pair = new Pair<UIWidget, int>();
                pair.Key = widget;
                pair.Value = widget.depth;

                SList.Add(windowComponent.WidgetsDefaultDepth, pair);
            }
        }

        public static void CachePanelDepths(WindowComponent windowComponent)
        {
            SList.Clear(windowComponent.PanelsDefaultDepth);

            var panels = windowComponent.gameObject.GetComponentsInChildren<UIPanel>(true);
            for (int i = 0; i < panels.Length; i++)
            {
                var panel = panels[i];

                var pair = new Pair<UIPanel, int>();
                pair.Key = panel;
                pair.Value = panel.depth;

                SList.Add(windowComponent.PanelsDefaultDepth, pair);
            }
        }

        public static void CacheDepths(WindowComponent windowComponent)
        {
            CacheWidgetDepths(windowComponent);
            CachePanelDepths(windowComponent);
        }

        public static void SetDepthsToCachedValue(WindowComponent windowComponent)
        {
            var widgets = windowComponent.WidgetsDefaultDepth;
            for (int i = 0; i < widgets.Count; i++)
            {
                var widget = widgets[i];
                widget.Key.depth = widget.Value;
            }

            var panels = windowComponent.PanelsDefaultDepth;
            for (int i = 0; i < panels.Count; i++)
            {
                var panel = panels[i];
                panel.Key.depth = panel.Value;
            }
        }

        public static void SetWidgetDepths(WindowComponent windowComponent, int depth, SetDepthMode mode)
        {
            var widgets = windowComponent.WidgetsDefaultDepth;
            for (int i = 0; i < widgets.Count; i++)
            {
                var widget = widgets[i];
                switch (mode)
                {
                    case SetDepthMode.Absolute:
                        widget.Key.depth = depth;
                        break;
                    case SetDepthMode.Increment:
                        widget.Key.depth = widget.Value + depth;
                        break;
                    case SetDepthMode.Decrement:
                        widget.Key.depth = widget.Value - depth;
                        break;
                }
            }
        }
        
        public static void SetPanelDepths(WindowComponent windowComponent, int depth, SetDepthMode mode)
        {
            var panels = windowComponent.PanelsDefaultDepth;
            for (int i = 0; i < panels.Count; i++)
            {
                var panel = panels[i];
                switch (mode)
                {
                    case SetDepthMode.Absolute:
                        panel.Key.depth = depth;
                        break;
                    case SetDepthMode.Increment:
                        panel.Key.depth = panel.Value + depth;
                        break;
                    case SetDepthMode.Decrement:
                        panel.Key.depth = panel.Value - depth;
                        break;
                }
            }
        }

        public static void SetDepths(WindowComponent windowComponent, int depth, SetDepthMode mode)
        {
            SetWidgetDepths(windowComponent, depth, mode);
            SetPanelDepths(windowComponent, depth, mode);
        }

        public static void Focus(WindowComponent windowComponent)
        {
            if (windowComponent == CurrentlyFocusedWindow)
                return;

            if (CurrentlyFocusedWindow)
                SetDepthsToCachedValue(CurrentlyFocusedWindow);
            
            SetDepths(windowComponent, FocusedWindowDepthIncrement, SetDepthMode.Increment);
            CurrentlyFocusedWindow = windowComponent;
        }
    }
}