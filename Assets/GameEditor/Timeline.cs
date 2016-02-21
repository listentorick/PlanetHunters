using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public delegate void SlotClickedHandler(int when);

public class Timeline : MonoBehaviour {

	public TimelineSlot slotPrefab;
	//public Button filledSlotPrefab;
	//public int timeSpan;
	private IList<RectTransform> slots = new List<RectTransform>();
	private List<int> whens = new List<int>();
	public Transform content;


	// Use this for initialization
	void Start () {
	
	}

	private int maxWhen = 0;

	public void AddEvent(int when)
	{
		if (when > maxWhen) {
			maxWhen = when;
		}
		whens.Add (when);

		if (this.content.transform.childCount > when) {
			this.content.transform.GetChild (when).GetComponent<TimelineSlot>().Fill();
		}

	//	Transform currentButton = this.transform.GetChild (when);
		//this.content.SetSiblingIndex(
		//if (currentButton != null) {
		//	currentButton.parent = null;
		//	Destroy (currentButton);
		//	AddSlot (when);
		//}
	}

	public event SlotClickedHandler SlotClicked;

	private void AddSlot(int when){
		TimelineSlot slot = Instantiate(slotPrefab);

		if (whens.Contains (when)) {
			slot.Fill ();
		} else {
			slot.Empty();
		}
		slot.GetComponent<Button>().onClick.AddListener(()=> SlotClicked(when));
		slot.transform.SetParent(content,false);
		slot.transform.SetSiblingIndex (when);
	}

	public void AddSlotToEnd(){
	
		maxWhen++;
		AddSlot (maxWhen);
	
	}
	
	public void Build() {


	
		for (int i=0; i<maxWhen; i++) {
		//	int copy = i;

			AddSlot(i);
			//Button slot = null;
			//if(whens.Contains(i)){
		//		slot = Instantiate(filledSlotPrefab);
		//	} else {
		//		slot = Instantiate(slotPrefab);
		//	}
		//	slot.onClick.AddListener(()=> SlotClicked(copy));
		//	slot.transform.SetParent(content,false);
		}
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
