using UnityEngine;
using System.Collections;

public interface IFailCondition  {
	event FailConditionHandler Fail;
}
public delegate void FailConditionHandler();