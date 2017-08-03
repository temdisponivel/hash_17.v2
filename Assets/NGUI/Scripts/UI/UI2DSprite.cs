//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 2D Sprite is capable of drawing sprites added in Unity 4.3. When importing your textures,
/// import them as Sprites and you will be able to draw them with this widget.
/// If you provide a Packing Tag in your import settings, your sprites will get automatically
/// packed into an atlas for you, so creating an atlas beforehand is not necessary.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Unity2D Sprite")]
public class UI2DSprite : UIBasicSprite
{
	[HideInInspector][SerializeField] UnityEngine.Sprite mSprite;
	[HideInInspector][SerializeField] Material mMat;
	[HideInInspector][SerializeField] Shader mShader;
	[HideInInspector][SerializeField] Vector4 mBorder = Vector4.zero;
	[HideInInspector][SerializeField] bool mFixedAspect = false;
	[HideInInspector][SerializeField] float mPixelSize = 1f;

	/// <summary>
	/// To be used with animations.
	/// </summary>

	public UnityEngine.Sprite nextSprite;

	[System.NonSerialized] int mPMA = -1;

	/// <summary>
	/// UnityEngine.Sprite drawn by this widget.
	/// </summary>

	public UnityEngine.Sprite sprite2D
	{
		get
		{
			return mSprite;
		}
		set
		{
			if (mSprite != value)
			{
				RemoveFromPanel();
				mSprite = value;
				nextSprite = null;
				CreatePanel();
			}
		}
	}

	/// <summary>
	/// Material used by the widget.
	/// </summary>

	public override Material material
	{
		get
		{
			return mMat;
		}
		set
		{
			if (mMat != value)
			{
				RemoveFromPanel();
				mMat = value;
				mPMA = -1;
				MarkAsChanged();
			}
		}
	}

	/// <summary>
	/// Shader used by the texture when creating a dynamic material (when the texture was specified, but the material was not).
	/// </summary>

	public override Shader shader
	{
		get
		{
			if (mMat != null) return mMat.shader;
			if (mShader == null) mShader = Shader.Find("Unlit/Transparent Colored");
			return mShader;
		}
		set
		{
			if (mShader != value)
			{
				RemoveFromPanel();
				mShader = value;

				if (mMat == null)
				{
					mPMA = -1;
					MarkAsChanged();
				}
			}
		}
	}
	
	/// <summary>
	/// Texture used by the UITexture. You can set it directly, without the need to specify a material.
	/// </summary>

	public override Texture mainTexture
	{
		get
		{
			if (mSprite != null) return mSprite.texture;
			if (mMat != null) return mMat.mainTexture;
			return null;
		}
	}

	/// <summary>
	/// Whether the texture is using a premultiplied alpha material.
	/// </summary>

	public override bool premultipliedAlpha
	{
		get
		{
			if (mPMA == -1)
			{
				var sh = shader;
				mPMA = (sh != null && sh.name.Contains("Premultiplied")) ? 1 : 0;
			}
			return (mPMA == 1);
		}
	}

	/// <summary>
	/// Size of the pixel -- used for drawing.
	/// </summary>

	override public float pixelSize { get { return mPixelSize; } }

	/// <summary>
	/// Widget's dimensions used for drawing. X = left, Y = bottom, Z = right, W = top.
	/// This function automatically adds 1 pixel on the edge if the texture's dimensions are not even.
	/// It's used to achieve pixel-perfect sprites even when an odd dimension widget happens to be centered.
	/// </summary>

	public override Vector4 drawingDimensions
	{
		get
		{
			var offset = pivotOffset;

			var x0 = -offset.x * mWidth;
			var y0 = -offset.y * mHeight;
			var x1 = x0 + mWidth;
			var y1 = y0 + mHeight;

			if (mSprite != null && mType != UISprite.Type.Tiled)
			{
				var w = Mathf.RoundToInt(mSprite.rect.width);
				var h = Mathf.RoundToInt(mSprite.rect.height);
				var padLeft = Mathf.RoundToInt(mSprite.textureRectOffset.x);
				var padBottom = Mathf.RoundToInt(mSprite.textureRectOffset.y);
				var padRight = Mathf.RoundToInt(mSprite.rect.width - mSprite.textureRect.width - mSprite.textureRectOffset.x);
				var padTop = Mathf.RoundToInt(mSprite.rect.height - mSprite.textureRect.height - mSprite.textureRectOffset.y);

				var px = 1f;
				var py = 1f;

				if (w > 0 && h > 0 && (mType == UISprite.Type.Simple || mType == UISprite.Type.Filled))
				{
					if ((w & 1) != 0) ++padRight;
					if ((h & 1) != 0) ++padTop;

					px = (1f / w) * mWidth;
					py = (1f / h) * mHeight;
				}

				if (mFlip == UISprite.Flip.Horizontally || mFlip == UISprite.Flip.Both)
				{
					x0 += padRight * px;
					x1 -= padLeft * px;
				}
				else
				{
					x0 += padLeft * px;
					x1 -= padRight * px;
				}

				if (mFlip == UISprite.Flip.Vertically || mFlip == UISprite.Flip.Both)
				{
					y0 += padTop * py;
					y1 -= padBottom * py;
				}
				else
				{
					y0 += padBottom * py;
					y1 -= padTop * py;
				}
			}

			float fw, fh;

			if (mFixedAspect)
			{
				fw = 0f;
				fh = 0f;
			}
			else
			{
				var br = border * pixelSize;
				fw = (br.x + br.z);
				fh = (br.y + br.w);
			}

			var vx = Mathf.Lerp(x0, x1 - fw, mDrawRegion.x);
			var vy = Mathf.Lerp(y0, y1 - fh, mDrawRegion.y);
			var vz = Mathf.Lerp(x0 + fw, x1, mDrawRegion.z);
			var vw = Mathf.Lerp(y0 + fh, y1, mDrawRegion.w);

			return new Vector4(vx, vy, vz, vw);
		}
	}

	/// <summary>
	/// Sprite's border. X = left, Y = bottom, Z = right, W = top.
	/// </summary>

	public override Vector4 border
	{
		get
		{
			// Normally this would be enough... but there seems to be no way to SET the 'border' anywhere. Sigh, Unity.
			//return (mSprite != null) ? mSprite.border : Vector4.zero;
			return mBorder;
		}
		set
		{
			if (mBorder != value)
			{
				mBorder = value;
				MarkAsChanged();
			}
		}
	}

	/// <summary>
	/// Update the sprite in case it was animated.
	/// </summary>

	protected override void OnUpdate ()
	{
		if (nextSprite != null)
		{
			if (nextSprite != mSprite)
				sprite2D = nextSprite;
			nextSprite = null;
		}
		base.OnUpdate();

		if (mFixedAspect)
		{
			var tex = mainTexture;

			if (tex != null)
			{
				var w = Mathf.RoundToInt(mSprite.rect.width);
				var h = Mathf.RoundToInt(mSprite.rect.height);
				var padLeft = Mathf.RoundToInt(mSprite.textureRectOffset.x);
				var padBottom = Mathf.RoundToInt(mSprite.textureRectOffset.y);
				var padRight = Mathf.RoundToInt(mSprite.rect.width - mSprite.textureRect.width - mSprite.textureRectOffset.x);
				var padTop = Mathf.RoundToInt(mSprite.rect.height - mSprite.textureRect.height - mSprite.textureRectOffset.y);

				w += padLeft + padRight;
				h += padTop + padBottom;

				float widgetWidth = mWidth;
				float widgetHeight = mHeight;
				var widgetAspect = widgetWidth / widgetHeight;
				var textureAspect = (float)w / h;

				if (textureAspect < widgetAspect)
				{
					var x = (widgetWidth - widgetHeight * textureAspect) / widgetWidth * 0.5f;
					drawRegion = new Vector4(x, 0f, 1f - x, 1f);
				}
				else
				{
					var y = (widgetHeight - widgetWidth / textureAspect) / widgetHeight * 0.5f;
					drawRegion = new Vector4(0f, y, 1f, 1f - y);
				}
			}
		}
	}

#if UNITY_EDITOR
	/// <summary>
	/// Keep sane values.
	/// </summary>

	protected override void OnValidate ()
	{
		base.OnValidate();
		mBorder.x = Mathf.Max(mBorder.x, 0);
		mBorder.y = Mathf.Max(mBorder.y, 0);
		mBorder.z = Mathf.Max(mBorder.z, 0);
		mBorder.w = Mathf.Max(mBorder.w, 0);
	}
#endif

	/// <summary>
	/// Adjust the scale of the widget to make it pixel-perfect.
	/// </summary>

	public override void MakePixelPerfect ()
	{
		base.MakePixelPerfect();
		if (mType == Type.Tiled) return;

		var tex = mainTexture;
		if (tex == null) return;

		if (mType == Type.Simple || mType == Type.Filled || !hasBorder)
		{
			if (tex != null)
			{
				var rect = mSprite.rect;
				var w = Mathf.RoundToInt(pixelSize * rect.width);
				var h = Mathf.RoundToInt(pixelSize * rect.height);

				if ((w & 1) == 1) ++w;
				if ((h & 1) == 1) ++h;

				width = w;
				height = h;
			}
		}
	}

	/// <summary>
	/// Virtual function called by the UIPanel that fills the buffers.
	/// </summary>

	public override void OnFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
	{
		var tex = mainTexture;
		if (tex == null) return;

		var outer = (mSprite != null) ? mSprite.textureRect : new Rect(0f, 0f, tex.width, tex.height);
		var inner = outer;
		var br = border;
		inner.xMin += br.x;
		inner.yMin += br.y;
		inner.xMax -= br.z;
		inner.yMax -= br.w;

		var w = 1f / tex.width;
		var h = 1f / tex.height;

		outer.xMin *= w;
		outer.xMax *= w;
		outer.yMin *= h;
		outer.yMax *= h;

		inner.xMin *= w;
		inner.xMax *= w;
		inner.yMin *= h;
		inner.yMax *= h;

		var offset = verts.size;
		Fill(verts, uvs, cols, outer, inner);

		if (onPostFill != null)
			onPostFill(this, offset, verts, uvs, cols);
	}
}
