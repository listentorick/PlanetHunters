using UnityEngine;
using System.Collections;

public class WormHoleConfiguration : BaseConfiguration {

	public override void Accept(ILevelConfigurationVisitor visitor) {
		visitor.Visit(this);
	}

}
