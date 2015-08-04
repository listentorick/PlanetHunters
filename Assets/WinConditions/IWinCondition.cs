using UnityEngine;
using System.Collections;

public interface IWinCondition  {
	event WinConditionHandler Win;
}
public delegate void WinConditionHandler();