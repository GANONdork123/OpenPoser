using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFKPoser : MonoBehaviour
{
	public GameObject iKTargetContainer;
	
	[System.Serializable]
	public class Limb
	{
		[Header("FK Bones")]
		public GameObject parentBone;
		public GameObject childBone;
		public GameObject endBone;
		[Header("IK Controllers")]
		public GameObject iKTarget;
		public GameObject poleTarget;
	}
	
	public Limb leftArm;
	public Limb rightArm;
	public Limb leftLeg;
	public Limb rightLeg;
}
