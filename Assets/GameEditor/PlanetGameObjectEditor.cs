using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlanetGameObjectEditor : GameObjectEditor {

	public PlanetType planetType;
//	public Image image;
	//public InputField mass;

	public Sprite redPlanetSprite;
	public Sprite bluePlanetSprite;
	public Sprite sunSprite;
	public Sprite gasGiantSprite;

	public void Start(){

		if (planetType == PlanetType.Red) {
			this.SetSprite (redPlanetSprite);
		} else if (planetType == PlanetType.Blue) {
			this.SetSprite (bluePlanetSprite);
		} else if (planetType == PlanetType.GasGiant) {
			this.SetSprite (gasGiantSprite);
		} else {
			this.SetSprite(sunSprite);
		}

		this.editorPanel.AddInput ("mass","0");
		this.editorPanel.AddInput ("soi","0");
	
	}

}
