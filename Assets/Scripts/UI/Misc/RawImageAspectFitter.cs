using UnityEngine;
using UnityEngine.UI;

namespace UI.Misc
{
    public class RawImageAspectFitter : MonoBehaviour
    {
		[SerializeField] bool m_adjustOnStart = true;

		protected RawImage m_image;
		protected float m_aspectRatio = 1.0f;
		protected float m_rectAspectRatio = 1.0f;

		private void Start()
		{
			AdjustAspect();
		}

		void SetupImage()
		{
			m_image = GetComponent<RawImage>();
			CalculateImageAspectRatio();
			//CalculateTextureAspectRatio();
		}

		void CalculateImageAspectRatio()
		{
			

			RectTransform rt = transform as RectTransform;

			Rect rect = new Rect(0, 0, 1, 1);
			rect.height = rt.sizeDelta.y / 512f;
			rect.width = rt.sizeDelta.x / 512f;

			m_image.uvRect = rect;
		}

		void CalculateTextureAspectRatio()
		{
			if (m_image == null)
			{
				Debug.Log("CalculateAspectRatio: m_image is null");
				return;
			}

			Texture texture = (Texture)m_image.texture;
			if (texture == null)
			{
				Debug.Log("CalculateAspectRatio: texture is null");
				return;
			}


			m_aspectRatio = (float)texture.width / texture.height;
			//Debug.Log("textW=" + texture.width + " h=" + texture.height + " ratio=" + m_aspectRatio);
		}

		public void AdjustAspect()
		{
			SetupImage();

			bool fitY = m_aspectRatio < m_rectAspectRatio;

			SetAspectFitToImage(m_image, fitY, m_aspectRatio);
		}


		protected virtual void SetAspectFitToImage(RawImage _image,
						 bool yOverflow, float displayRatio)
		{
			if (_image == null)
			{
				return;
			}

			Rect rect = new Rect(0, 0, 1, 1);   // default
			if (yOverflow)
			{

				rect.height = m_aspectRatio / m_rectAspectRatio;
				rect.y = (1 - rect.height) * 0.5f;
			}
			else
			{
				rect.width = m_rectAspectRatio / m_aspectRatio;
				rect.x = (1 - rect.width) * 0.5f;

			}
			_image.uvRect = rect;
		}
	}
}
