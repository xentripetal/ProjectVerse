using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace HappyHarvest
{
	public class RendererFader : MonoBehaviour
	{
		public AnimationCurve curve;
		public float time = 0.5f;
		[FormerlySerializedAs("renderer")] 
		public SpriteRenderer RendererToHide;
		public float finalAlpha = 0.2f;

		public Tilemap tilemap;
    
		private Color _initialColor;
		private Color col;

		void Start()
		{
			//curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
			curve.preWrapMode = WrapMode.Once;
			curve.postWrapMode = WrapMode.ClampForever;
			if (RendererToHide != null)
			{
				_initialColor = RendererToHide.color;
			}

			if (tilemap != null)
			{
				_initialColor = tilemap.color;
			}
		
			col = _initialColor;
		
		
		}


		private void OnTriggerEnter2D(Collider2D col)
		{
			StartCoroutine(AnimCurve(_initialColor.a, finalAlpha));
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			StartCoroutine(AnimCurve(finalAlpha, _initialColor.a));

		}

		private IEnumerator AnimCurve (float initialPosition, float finalPosition)
		{
			float i = 0;
			float rate = 1 / time;
			while (i < 1) {
				i += rate * Time.deltaTime;
				var resultValue = Mathf.Lerp (initialPosition, finalPosition, curve.Evaluate (i));
				col.a = resultValue;
				if (tilemap != null)
					tilemap.color = col;
				if(RendererToHide != null)
					RendererToHide.color = col;
				yield return 0;
			}
		} 

	}
}