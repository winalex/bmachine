// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using BehaviourMachine;
using UnityEngine;
using ws.winx.unity;
using UnityEngine.Events;
using ws.winx.unity.attributes;


namespace ws.winx.bmachine.extensions
{
		[NodeInfo ( category = "Extensions/Mecanim/", icon = "StateMachine",  description ="To be used in conjuction with MecanimNode for sending events in animation points in time")]
		public class SendEventNormalizedNode:ActionNode
		{
				//
				// Fields
				//

				Animator _animator;
				float _timeNormalizedLast;
				[UnityVariablePropertyAttribute(typeof(float),"Time:")]
				public UnityVariable
						timeNormalized;

				[UnityVariablePropertyAttribute(typeof(UnityEvent),"Events:")]
				public UnityVariable
					unityEvent;


				
		
		
				public override void Awake ()
				{
				
						_animator = this.self.GetComponent<Animator> ();
						
				}

				public override Status Update ()
				{
						AnimatorState animatorStateSelected = ((MecanimNode)this.branch).animatorStateSelected;
						int layer= ((MecanimNode)this.branch).layer;
						AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo (layer);

						//Debug.Log ("onUpdate");
					
						if (currentAnimatorStateInfo.shortNameHash == animatorStateSelected.nameHash) {
								float timeNormalizedCurrent = currentAnimatorStateInfo.normalizedTime;

						
								timeNormalizedCurrent = timeNormalizedCurrent - (int)timeNormalizedCurrent;


								//Debug.Log ("timeNormalizedCurrent "+timeNormalizedCurrent);

								if (timeNormalizedCurrent > (float)timeNormalized.Value && _timeNormalizedLast < (float)timeNormalized.Value) {

									//	Debug.Log ("Event [" + name + "] sent at:" + timeNormalized.Value);
										((UnityEvent)this.unityEvent.Value).Invoke ();
										_timeNormalizedLast = timeNormalizedCurrent;
										return Status.Success;
								}

								_timeNormalizedLast = timeNormalizedCurrent;
						}

						//this.status = Status.Running;
						
						return Status.Running;
				}
			
				public override void Reset ()
				{
						//!!! if UnityVariables aren't Init here KABOOM when added to MecaniNode
						this.timeNormalized = UnityVariable.CreateInstanceOf (typeof(float));
						
			
						//this.unityEvent = (UnityVariable)ScriptableObject.CreateInstance< UnityVariable> ();
						//this.unityEvent.Value = new UnityEvent ();
						//this.unityEvent.OnBeforeSerialize ();
						this.unityEvent = UnityVariable.CreateInstanceOf (typeof(UnityEngine.Events.UnityEvent));
						
						
						_timeNormalizedLast = 0f;
				
				}

				public override string ToString ()
				{
						return string.Format ("[SendEventNormalized]{0} {1}", name, timeNormalized.Value);
				}
		}
}

