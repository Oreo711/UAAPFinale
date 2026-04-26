using Assets._Project.Develop.Runtime.UI.Core;
using TMPro;
using UnityEngine;


namespace _Project.Develop.Runtime.UI.CommonViews
{
	public class RatioView : MonoBehaviour, IView
	{
		[SerializeField] private TMP_Text _numerator;
		[SerializeField] private TMP_Text _denomenator;

		public void SetNumerator (int value) => _numerator.text = value.ToString();

		public void SetDenomenator(int value) => _denomenator.text = value.ToString();
	}
}
