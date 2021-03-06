﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;



 public interface ILevelConfigurationVisitor  {

	void Visit (Level visitable);
	void Visit (BaseConfiguration visitable);
	void Visit (PlanetConfiguration visitable);
	void Visit (WormHoleConfiguration visitable);
	void Visit (SunConfiguration visitable);
	void Visit (ConstellationLineConfiguration visitable);
	void Visit (LevelMapItemConfiguration visitable);
	void Visit (PlanetResourceConfiguration visitable);
	void Visit (SpawnConfiguration visitable);

}