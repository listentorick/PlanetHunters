using UnityEngine;
using System.Collections;

public interface IFailCondition: IBuild, IReset  {
	event FailConditionHandler Fail;
}
public delegate void FailConditionHandler();