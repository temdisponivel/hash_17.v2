//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// This script can be used to stretch objects relative to the screen's width and height.
/// The most obvious use would be to create a full-screen background by attaching it to a sprite.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Stretch")]
public class UIStretch : MonoBehaviour
{
	public enum Style
	{
		None,
		Horizontal,
		Vertical,
		Both,
		BasedOnHeight,
		FillKeepingRatio, // Fits the image so that it entirely fills the specified container keeping its ratio
		FitInternalKeepingRatio // Fits the image/item inside of the specified container keeping its ratio
	}

	/// <summary>
	/// Camera used to determine the anchor bounds. Set automatically if none was specified.
	/// </summary>

	public Camera uiCamera = null;

	/// <summary>
	/// Object used to determine the container's bounds. Overwrites the camera-based anchoring if the value was specified.
	/// </summary>

	public GameObject container = null;

	/// <summary>
	/// Stretching style.
	/// </summary>

	public Style style = Style.None;

	/// <summary>
	/// Whether the operation will occur only once and the script will then be disabled.
	/// Screen size changes will still cause the script's logic to execute.
	/// </summary>

	public bool runOnlyOnce = true;

	/// <summary>
	/// Relative-to-target size.
	/// </summary>

	public Vector2 relativeSize = Vector2.one;

	/// <summary>
	/// The size that the item/image should start out initially.
	/// Used for FillKeepingRatio, and FitInternalKeepingRatio.
	/// Contributed by Dylan Ryan.
	/// </summary>

	public Vector2 initialSize = Vector2.one;

	/// <summary>
	/// Padding applied after the size of the stretched object gets calculated. This value is in pixels.
	/// </summary>

	public Vector2 borderPadding = Vector2.zero;

	// Deprecated legacy functionality
	[HideInInspector][SerializeField] UIWidget widgetContainer;

	Transform mTrans;
	UIWidget mWidget;
	UISprite mSprite;
	UIPanel mPanel;
	UIRoot mRoot;
	Animation mAnim;
	Rect mRect;
	bool mStarted = false;

	void Awake ()
	{
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
		mAnim = animation;
#else
		mAnim = GetComponent<Animation>();
#endif
		mRect = new Rect();
		mTrans = transform;
		mWidget = GetComponent<UIWidget>();
		mSprite = GetComponent<UISprite>();
		mPanel = GetComponent<UIPanel>();

		UICamera.onScreenResize += ScreenSizeChanged;
	}

	void OnDestroy () { UICamera.onScreenResize -= ScreenSizeChanged; }

	void ScreenSizeChanged () { if (mStarted && runOnlyOnce) Update(); }

	void Start ()
	{
		if (container == null && widgetContainer != null)
		{
			container = widgetContainer.gameObject;
			widgetContainer = null;
#if UNITY_EDITOR
			NGUITools.SetDirty(this);
#endif
		}

		if (uiCamera == null) uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
		mRoot = NGUITools.FindInParents<UIRoot>(gameObject);
		
		Update();
		
		mStarted = true;
	}

	void Update ()
	{
		if (mAnim != null && mAnim.isPlaying) return;

		if (style != Style.None)
		{
			var wc = (container == null) ? null : container.GetComponent<UIWidget>();
			var pc = (container == null && wc == null) ? null : container.GetComponent<UIPanel>();
			var adjustment = 1f;

			if (wc != null)
			{
				var b = wc.CalculateBounds(transform.parent);

				mRect.x = b.min.x;
				mRect.y = b.min.y;

				mRect.width = b.size.x;
				mRect.height = b.size.y;
			}
			else if (pc != null)
			{
				if (pc.clipping == UIDrawCall.Clipping.None)
				{
					// Panel has no clipping -- just use the screen's dimensions
					var ratio = (mRoot != null) ? (float)mRoot.activeHeight / Screen.height * 0.5f : 0.5f;
					mRect.xMin = -Screen.width * ratio;
					mRect.yMin = -Screen.height * ratio;
					mRect.xMax = -mRect.xMin;
					mRect.yMax = -mRect.yMin;
				}
				else
				{
					// Panel has clipping -- use it as the mRect
					var cr = pc.finalClipRegion;
					mRect.x = cr.x - (cr.z * 0.5f);
					mRect.y = cr.y - (cr.w * 0.5f);
					mRect.width = cr.z;
					mRect.height = cr.w;
				}
			}
			else if (container != null)
			{
				var root = transform.parent;
				var b = (root != null) ? NGUIMath.CalculateRelativeWidgetBounds(root, container.transform) :
					NGUIMath.CalculateRelativeWidgetBounds(container.transform);

				mRect.x = b.min.x;
				mRect.y = b.min.y;

				mRect.width = b.size.x;
				mRect.height = b.size.y;
			}
			else if (uiCamera != null)
			{
				mRect = uiCamera.pixelRect;
				if (mRoot != null) adjustment = mRoot.pixelSizeAdjustment;
			}
			else return;

			var rectWidth = mRect.width;
			var rectHeight = mRect.height;

			if (adjustment != 1f && rectHeight > 1f)
			{
				var scale = mRoot.activeHeight / rectHeight;
				rectWidth *= scale;
				rectHeight *= scale;
			}

			var size = (mWidget != null) ? new Vector3(mWidget.width, mWidget.height) : mTrans.localScale;

			if (style == Style.BasedOnHeight)
			{
				size.x = relativeSize.x * rectHeight;
				size.y = relativeSize.y * rectHeight;
			}
			else if (style == Style.FillKeepingRatio)
			{
				// Contributed by Dylan Ryan
				var screenRatio = rectWidth / rectHeight;
				var imageRatio = initialSize.x / initialSize.y;

				if (imageRatio < screenRatio)
				{
					// Fit horizontally
					var scale = rectWidth / initialSize.x;
					size.x = rectWidth;
					size.y = initialSize.y * scale;
				}
				else
				{
					// Fit vertically
					var scale = rectHeight / initialSize.y;
					size.x = initialSize.x * scale;
					size.y = rectHeight;
				}
			}
			else if (style == Style.FitInternalKeepingRatio)
			{
				// Contributed by Dylan Ryan
				var screenRatio = rectWidth / rectHeight;
				var imageRatio = initialSize.x / initialSize.y;

				if (imageRatio > screenRatio)
				{
					// Fit horizontally
					var scale = rectWidth / initialSize.x;
					size.x = rectWidth;
					size.y = initialSize.y * scale;
				}
				else
				{
					// Fit vertically
					var scale = rectHeight / initialSize.y;
					size.x = initialSize.x * scale;
					size.y = rectHeight;
				}
			}
			else
			{
				if (style != Style.Vertical)
					size.x = relativeSize.x * rectWidth;

				if (style != Style.Horizontal)
					size.y = relativeSize.y * rectHeight;
			}

			if (mSprite != null)
			{
				var multiplier = (mSprite.atlas != null) ? mSprite.atlas.pixelSize : 1f;
				size.x -= borderPadding.x * multiplier;
				size.y -= borderPadding.y * multiplier;

				if (style != Style.Vertical)
					mSprite.width = Mathf.RoundToInt(size.x);

				if (style != Style.Horizontal)
					mSprite.height = Mathf.RoundToInt(size.y);

				size = Vector3.one;
			}
			else if (mWidget != null)
			{
				if (style != Style.Vertical)
					mWidget.width = Mathf.RoundToInt(size.x - borderPadding.x);

				if (style != Style.Horizontal)
					mWidget.height = Mathf.RoundToInt(size.y - borderPadding.y);

				size = Vector3.one;
			}
			else if (mPanel != null)
			{
				var cr = mPanel.baseClipRegion;

				if (style != Style.Vertical)
					cr.z = size.x - borderPadding.x;
				
				if (style != Style.Horizontal)
					cr.w = size.y - borderPadding.y;

				mPanel.baseClipRegion = cr;
				size = Vector3.one;
			}
			else
			{
				if (style != Style.Vertical)
					size.x -= borderPadding.x;
				
				if (style != Style.Horizontal)
					size.y -= borderPadding.y;
			}
			
			if (mTrans.localScale != size)
				mTrans.localScale = size;

			if (runOnlyOnce && Application.isPlaying) enabled = false;
		}
	}
}
