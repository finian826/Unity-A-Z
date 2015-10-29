using UnityEngine;
using System.Collections;

public class MatchingItem : MonoBehaviour
{
		public AudioClip dragAudioClip;//drag audio clip
		public AudioClip dropAudioClip;//drop audio clip
		public int id;//the id of the matching item
		public bool connected;//is connected flag
		public ItemType itemType;//item type

		public enum ItemType
		{
				SOURCE,
				DESTINATION
		}
}