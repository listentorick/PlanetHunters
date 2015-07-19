using UnityEngine;
using System.Collections;

public class SunConfiguration :BaseConfiguration {

	public override void Accept(ILevelConfigurationVisitor visitor) {
		visitor.Visit(this);
	}
}
