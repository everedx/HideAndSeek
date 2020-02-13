using UnityEngine;
using UnityEngine.UI;


	public class ScorePanel : MonoBehaviour
	{
		/// <summary>
		/// Objects that represent the stars
		/// </summary>
		public Image[] starImages;

		public Sprite achievedStarSprite;

		/// <summary>
		/// Show the correct number of stars for the score
		/// </summary>
		/// <param name="score">The final score</param>
		public void SetStars(int score)
		{
			if (score <= 0)
			{
				return;
			}
			score = Mathf.Clamp(score, 0, starImages.Length);
			for (int i = 0; i < score; i++)
			{
                Vector3 originalDelta = starImages[i].rectTransform.sizeDelta; //get the original Image's rectTransform's size delta and store it in a Vector called originalDelta
                starImages[i].sprite = achievedStarSprite; //swap out the new image
                starImages[i].rectTransform.sizeDelta = originalDelta; //size it as the old one was.
                //starImages[i].sprite = achievedStarSprite;
			}
		}
	}
