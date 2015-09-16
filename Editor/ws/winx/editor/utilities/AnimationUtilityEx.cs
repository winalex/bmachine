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
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace ws.winx.editor.utilities
{
		public static class AnimationUtilityEx
		{
				static Type TransformType = typeof(Transform);
				public static EditorCurveBinding EditorCurveBinding_PosX = EditorCurveBinding.FloatCurve ("", TransformType, "m_LocalPosition.x");
				public static EditorCurveBinding EditorCurveBinding_PosY = EditorCurveBinding.FloatCurve ("", TransformType, "m_LocalPosition.y");
				public static EditorCurveBinding EditorCurveBinding_PosZ = EditorCurveBinding.FloatCurve ("", TransformType, "m_LocalPosition.z");
				public static EditorCurveBinding EditorCurveBinding_RotX = EditorCurveBinding.FloatCurve ("", TransformType, "m_LocalRotation.x");
				public static EditorCurveBinding EditorCurveBinding_RotY = EditorCurveBinding.FloatCurve ("", TransformType, "m_LocalRotation.y");
				public static EditorCurveBinding EditorCurveBinding_RotZ = EditorCurveBinding.FloatCurve ("", TransformType, "m_LocalRotation.z");
				public static EditorCurveBinding EditorCurveBinding_SclX = EditorCurveBinding.FloatCurve ("", TransformType, "m_LocalScale.x");
				public static EditorCurveBinding EditorCurveBinding_SclY = EditorCurveBinding.FloatCurve ("", TransformType, "m_LocalScale.y");
				public static EditorCurveBinding EditorCurveBinding_SclZ = EditorCurveBinding.FloatCurve ("", TransformType, "m_LocalScale.z");


				/// <summary>
				/// Moves the key to time.
				/// </summary>
				/// <param name="binding">Binding.</param>
				/// <param name="clip">Clip.</param>
				/// <param name="keyframeInx">L.</param>
				/// <param name="time">New Time to be moved keyframe to</param>
				public static void MoveKeyToTime (EditorCurveBinding binding, AnimationClip clip, int keyframeInx, float time)
				{
						if (keyframeInx < 0) {
								Debug.LogWarning ("Try to move keyframe at index " + keyframeInx + " from prop:" + binding.propertyName + " from " + clip.name);
								return;
						}
			
						//if property is "m_LocalRotation.y" create "m_LocalRotation.x" "m_LocalRotation.z" and "m_LocalRotation.w"
						EditorCurveBinding[] array = RotationCurveInterpolationW.RemapAnimationBindingForAddKey (binding, clip);
						if (array != null) {
								for (int j = 0; j < array.Length; j++) {
										MoveKey (array [j], clip, keyframeInx, time);
								}
						} else {
								MoveKey (binding, clip, keyframeInx, time);
						}
			
				}

				private static void MoveKey (EditorCurveBinding binding, AnimationClip clip, int keyframeInx, float time)
				{
						AnimationCurve curve;
			
			
			
						Type type = null;
			
						int frameCurrent = (int)(time * clip.frameRate);
						Component componentCurrent;
			
			
			
			
		
			

			
						if (binding.isPPtrCurve) {
				
								ObjectReferenceKeyframe[] keyframesCurveReferenced = AnimationUtility.GetObjectReferenceCurve (clip, binding);
				
								if (keyframesCurveReferenced != null && keyframesCurveReferenced.Length > keyframeInx) {
										keyframesCurveReferenced [keyframeInx].time = time;

										AnimationUtility.SetObjectReferenceCurve (clip, binding, keyframesCurveReferenced);
								}
				
				
					
				
				
				
						} else {
				
				
				
								curve = AnimationUtility.GetEditorCurve (clip, binding);
				
								if (curve != null) {
										Keyframe key = curve.keys [keyframeInx];
										key.time = time;
										curve.MoveKey (keyframeInx, key);

										AnimationModeUtility.SaveCurve (curve, clip, binding);
								}
					
					

				
						}
				}
		
		
				/// <summary>
				/// Adds the keyframe to clip at time based of current values of root GameObject.
				/// <param name="curveBindingCurrent">Curve binding current.</param>
				/// </summary>
				/// <param name="root">Root.</param>
				/// <param name="clip">Clip.</param>
				/// <param name="time">Time.</param>
				public static void AddKeyframeTo (EditorCurveBinding binding, GameObject root, AnimationClip clip, float time)
				{

						object valueCurrent = AnimationModeUtility.GetCurrentValue (root, binding);

						//if property is "m_LocalRotation.y" create "m_LocalRotation.x" "m_LocalRotation.z" and "m_LocalRotation.w"
						EditorCurveBinding[] array = RotationCurveInterpolationW.RemapAnimationBindingForAddKey (binding, clip);
						if (array != null) {
								for (int j = 0; j < array.Length; j++) {
										AddKey (array [j], root, clip, valueCurrent, time);
								}
						} else {
								AddKey (binding, root, clip, valueCurrent, time);
						}
						
					




						
				}

				private static void AddKey (EditorCurveBinding curveBindingCurrent, GameObject root, AnimationClip clip, object valueCurrent, float time)
				{
						AnimationCurve curve;
						
						
						
						Type type = null;
						
						int frameCurrent = (int)(time * clip.frameRate);
						Component componentCurrent;
						
						
						
						
						UnityEngine.Object objectAnimated = AnimationUtility.GetAnimatedObject (root, curveBindingCurrent);
						
						if (curveBindingCurrent.type == TransformType)
								type = typeof(float);
						else
								type = objectAnimated.GetType ().GetField (curveBindingCurrent.propertyName).FieldType;
						
						if (curveBindingCurrent.isPPtrCurve) {
							
								ObjectReferenceKeyframe[] keyframesCurveReferenced = AnimationUtility.GetObjectReferenceCurve (clip, curveBindingCurrent);
							
								if (keyframesCurveReferenced == null)
										keyframesCurveReferenced = new ObjectReferenceKeyframe[0];
							
							
								//if you put first frame somewhere in time there should be one at 0f so curve can be made
								//also if you put first frame in 0f then create one in 1f
								if (keyframesCurveReferenced.Length == 0) {
										if (frameCurrent != 0) 
												AnimationModeUtility.AddKeyframeToObjectReferenceCurve (keyframesCurveReferenced, clip, curveBindingCurrent, valueCurrent, type, 0);
										else 
												AnimationModeUtility.AddKeyframeToObjectReferenceCurve (keyframesCurveReferenced, clip, curveBindingCurrent, valueCurrent, type, 1f);
								
								}
							
							
								AnimationModeUtility.AddKeyframeToObjectReferenceCurve (keyframesCurveReferenced, clip, curveBindingCurrent, valueCurrent, type, time);
							
							
							
						} else {
							
							
							
								curve = AnimationUtility.GetEditorCurve (clip, curveBindingCurrent);
							
								if (curve == null)
										curve = new AnimationCurve ();
							
							
							
							
							
								//if you put first frame somewhere in time there should be one at 0f so curve can be made
								//also if you put first frame in 0f then create one in 1f
								if (curve.length == 0) {
										if (frameCurrent != 0) 
									
												AnimationModeUtility.AddKeyframeToCurve (curve, clip, curveBindingCurrent, (float)valueCurrent, type, 0);
										else
												AnimationModeUtility.AddKeyframeToCurve (curve, clip, curveBindingCurrent, (float)valueCurrent, type, 1f);
								}
							
								//Debug.Log("Add key at some other frame");
								AnimationModeUtility.AddKeyframeToCurve (curve, clip, curveBindingCurrent, (float)valueCurrent, type, time);
							
						}
				}
				
				
		
		
				/// <summary>
				/// Removes the keyframe from clip (!!! Inserts hidden keyframes in between)
				/// </summary>
				/// <param name="clip">Clip.</param>
				/// <param name="keyframeInx">Keyframe inx.</param>
				public static void RemoveKeyframeFrom (AnimationClip clip, EditorCurveBinding binding, int keyframeInx)
				{

						if (keyframeInx < 0) {
								Debug.LogWarning ("Try to remove keyframe at index " + keyframeInx + " from prop:" + binding.propertyName + " from " + clip.name);
								return;
						}

						EditorCurveBinding[] curveBindings = RotationCurveInterpolationW.RemapAnimationBindingForAddKey (binding, clip);
						AnimationCurve curve;


						int curveBindingsNum = curveBindings.Length;
						EditorCurveBinding curveBindingCurrent;
						if (curveBindings != null) {
								for (int i=0; i<curveBindingsNum; i++) {

										curveBindingCurrent = curveBindings [i];
										curve = AnimationUtility.GetEditorCurve (clip, curveBindingCurrent);

										if (curve != null && curve.length > keyframeInx) {
												curve.RemoveKey (keyframeInx);
												
												AnimationModeUtility.SaveCurve (curve, clip, curveBindingCurrent);
										}
								}
						} else {
								curve = AnimationUtility.GetEditorCurve (clip, binding);
				
								if (curve != null && curve.length > keyframeInx) {
										curve.RemoveKey (keyframeInx);
										
										AnimationModeUtility.SaveCurve (curve, clip, binding);
								}
						}
				
				
				
				
				}
			

				/// <summary>
				/// Gets the time at clip, keyFrameInx and curveBinding.
				/// </summary>
				/// <returns>The <see cref="System.Single"/>.</returns>
				/// <param name="clip">Clip.</param>
				/// <param name="keyFrameInx">Key frame inx.</param>
				/// <param name="curveBinding">Curve binding.</param>
				public static float GetTimeAt (AnimationClip clip, int keyFrameInx, EditorCurveBinding curveBinding)
				{
						AnimationCurve curve = AnimationUtility.GetEditorCurve (clip, curveBinding);
						if (keyFrameInx < 0 || keyFrameInx > curve.length - 1)
								return 0f;
					
						return curve.keys [keyFrameInx].time;


				}





				/// <summary>
				/// Gets the times.
				/// </summary>
				/// <returns>The times.</returns>
				/// <param name="clip">Clip.</param>
				/// <param name="curveBinding">Curve binding.</param>
				/// <param name="clipLength">Clip length.</param>
				public static float[] GetTimes (AnimationClip clip, EditorCurveBinding curveBinding, float clipLength=1f)
				{
						AnimationCurve curve = AnimationUtility.GetEditorCurve (clip, curveBinding);
						if (curve == null)
								return new float[0];
						return curve.keys.Select ((itm) => itm.time).ToArray ();
			
				}
		
				/// <summary>
				/// Gets the times.(normalized 0-1)
				/// </summary>
				/// <returns>The times.</returns>
				/// <param name="clip">Clip.</param>
				/// <param name="curveBinding">Curve binding.</param>
				/// <param name="clipLength">Clip length.</param>
				public static float[] GetTimesNormalized (AnimationClip clip, EditorCurveBinding curveBinding, float clipLength=1f)
				{
						AnimationCurve curve = AnimationUtility.GetEditorCurve (clip, curveBinding);
						if (curve == null)
								return new float[0];
						return curve.keys.Select ((itm) => itm.time / clipLength).ToArray ();
					
				}

				

				/// <summary>
				/// Converts the time to whole FPS.
				/// </summary>
				/// <returns>The time to whole frames per second.</returns>
				/// <param name="time">Time.</param>
				/// <param name="frameRate">Frame rate.</param>
				public static int TimeToFrame (float time, int frameRate)
				{
					
						return (int)Mathf.Ceil (time * frameRate);
				}



				/// <summary>
				/// Gets the keyframe positions. If keyframes are rotation or scale, for time of keyframe position value is
				/// calculated on Positon curve
				/// its is expected curves to exist for every property or postion,rotation on scale and
				/// to have equal num of keyframes ex. for x,y,z position or x,y,z, rotation
				/// </summary>
				/// <returns>The keyframe positions.</returns>
				/// <param name="clip">Clip.</param>
				/// <param name="path">Path.</param>
				/// <param name="transform">Transform.</param>
//		public static Vector3[] GetKeyframesPositions (AnimationClip clip, string path="", Transform transform=null)
//		{
//			
//			int keyCurrent = 0;
//			int keysNumber = 0;
//			Keyframe keyFrameCurrent;
//			
//			EditorCurveBinding_PosX.path = path;
//			EditorCurveBinding_PosY.path = path;
//			EditorCurveBinding_PosZ.path = path;
//			
//			
//			AnimationCurve curvePosX = AnimationUtility.GetEditorCurve (clip, EditorCurveBinding_PosX);
//			AnimationCurve curvePosY = AnimationUtility.GetEditorCurve (clip, EditorCurveBinding_PosY);
//			AnimationCurve curvePosZ = AnimationUtility.GetEditorCurve (clip, EditorCurveBinding_PosZ);
//			
//			List<Keyframe> keyframesX = new List<Keyframe> ();
//			List<Keyframe> keyframesY = new List<Keyframe> ();
//			List<Keyframe> keyframesZ = new List<Keyframe> ();
//			
//			
//			
//			if (curvePosX != null && curvePosY != null && curvePosZ != null) {
//				
//				keyframesX.Concat (curvePosX.keys);
//				keyframesY.Concat (curvePosY.keys);
//				keyframesZ.Concat (curvePosZ.keys);
//			} else {
//				
//				
//			}
//			
//			EditorCurveBinding_RotX.path = path;
//			EditorCurveBinding_RotY.path = path;
//			EditorCurveBinding_RotZ.path = path;
//			
//			AnimationCurve curveRotX = AnimationUtility.GetEditorCurve (clip, EditorCurveBinding_RotX);
//			AnimationCurve curveRotY = AnimationUtility.GetEditorCurve (clip, EditorCurveBinding_RotY);
//			AnimationCurve curveRotZ = AnimationUtility.GetEditorCurve (clip, EditorCurveBinding_RotZ);
//			
//			
//			if (!((curveRotX != null && curveRotY != null && curveRotZ != null) || (curveRotX == null && curveRotY == null && curveRotZ == null))) {
//				Debug.LogError ("Missing rotation curve! Logic expected curves for rotation on all axes - x,y,z.");
//				return new Vector3[0];
//			}
//			
//			
//			
//			
//			if (curveRotX != null && curveRotY != null && curveRotZ != null) {
//				
//				
//				
//				
//				keysNumber = curveRotX.length; 
//				
//				if (curveRotY.length != keysNumber || curveRotZ.length != keysNumber) {
//					Debug.LogError ("Incorrect number of keyframes in rotation curves! Number of keyframes in rotation curves on all axes - x,y,z should be equal.");
//					return new Vector3[0];
//				}
//				
//				
//				
//				for (keyCurrent=0; keyCurrent<keysNumber; keyCurrent++) {
//					
//					keyFrameCurrent = curveRotX [keyCurrent];
//					
//					//if this keyframe is unique 
//					if (keyframesX.FindIndex ((itm) => Mathf.Approximately (itm.time, keyFrameCurrent.time)) < 0) {
//						keyframesX.Add (new Keyframe (keyFrameCurrent.time, curvePosX.Evaluate (keyFrameCurrent.time)));
//						keyframesY.Add (new Keyframe (keyFrameCurrent.time, curvePosY.Evaluate (keyFrameCurrent.time)));
//						keyframesZ.Add (new Keyframe (keyFrameCurrent.time, curvePosZ.Evaluate (keyFrameCurrent.time)));
//					}
//					
//					
//				}
//			}
//			
//			EditorCurveBinding_SclX.path = path;
//			EditorCurveBinding_SclY.path = path;
//			EditorCurveBinding_SclZ.path = path;
//			
//			AnimationCurve curveSclX = AnimationUtility.GetEditorCurve (clip, EditorCurveBinding_SclX);
//			AnimationCurve curveSclY = AnimationUtility.GetEditorCurve (clip, EditorCurveBinding_SclY);
//			AnimationCurve curveSclZ = AnimationUtility.GetEditorCurve (clip, EditorCurveBinding_SclZ);
//			
//			
//			if (!((curveSclX != null && curveSclY != null && curveSclZ != null) || (curveSclX == null && curveSclY == null && curveSclZ == null))) {
//				Debug.LogError ("Missing rotation curve! Logic expected curves for rotation on all axes - x,y,z.");
//				return new Vector3[0];
//			}
//			
//			
//			if (curveSclX != null && curveSclY != null && curveSclZ != null) {
//				
//				
//				
//				
//				keysNumber = curveSclX.length; 
//				
//				if (curveSclY.length != keysNumber || curveSclZ.length != keysNumber) {
//					Debug.LogError ("Incorrect number of keyframes in scale curves! Number of keyframes in scale curves on all axes - x,y,z should be equal.");
//					return new Vector3[0];
//				}
//				
//				
//				
//				for (keyCurrent=0; keyCurrent<keysNumber; keyCurrent++) {
//					
//					keyFrameCurrent = curveSclX [keyCurrent];
//					
//					//if this keyframe is unique 
//					if (keyframesX.FindIndex ((itm) => Mathf.Approximately (itm.time, keyFrameCurrent.time)) < 0) {
//						keyframesX.Add (new Keyframe (keyFrameCurrent.time, curvePosX.Evaluate (keyFrameCurrent.time)));
//						keyframesY.Add (new Keyframe (keyFrameCurrent.time, curvePosY.Evaluate (keyFrameCurrent.time)));
//						keyframesZ.Add (new Keyframe (keyFrameCurrent.time, curvePosZ.Evaluate (keyFrameCurrent.time)));
//					}
//					
//					
//				}
//			}
//			
//			//sort by time
//			
//			keyframesX.Sort ((itm1,itm2) => { 
//				
//				if (Mathf.Approximately (itm1.time, itm2.time))
//					return 0;
//				if (itm1.time > itm2.time)
//					return 1;
//				else
//					return -1;
//				
//				
//			});
//			
//			keyframesY.Sort ((itm1,itm2) => { 
//				
//				if (Mathf.Approximately (itm1.time, itm2.time))
//					return 0;
//				if (itm1.time > itm2.time)
//					return 1;
//				else
//					return -1;
//				
//				
//			});
//			
//			
//			keyframesZ.Sort ((itm1,itm2) => { 
//				
//				if (Mathf.Approximately (itm1.time, itm2.time))
//					return 0;
//				if (itm1.time > itm2.time)
//					return 1;
//				else
//					return -1;
//				
//				
//			});
//			
//			
//			Vector3[] positionVectors = null;
//			
//			//expect every keyframe to have x,y,z
//			if ((keysNumber = keyframesX.Count) > 0) {
//				
//				
//				///group
//				
//				
//				
//				
//				
//				positionVectors = new Vector3[keysNumber];
//				
//				
//				
//				for (keyCurrent=0; keyCurrent<keysNumber; keyCurrent++) 
//					
//				if (transform != null) {//conver local transform to world
//					
//					positionVectors [keyCurrent] = transform.TransformPoint (new Vector3 (keyframesX [keyCurrent].value, keyframesY [keyCurrent].value, keyframesY [keyCurrent].value));
//				} else {
//					
//					positionVectors [keyCurrent] = new Vector3 (keyframesX [keyCurrent].value, keyframesY [keyCurrent].value, keyframesZ [keyCurrent].value);
//				}
//				
//				
//			}
//			
//			
//			
//			
//			
//			
//			
//			
//			return positionVectors;
//		}



				/// <summary>
				/// Gets the positions x,y,z by keyframes transformed from local to world space
				/// //expect every clip to have x,y,z curves
				/// </summary>
				/// <returns>The positions.</returns>
				/// <param name="clip">Clip.</param>
				/// <param name="path">Path.</param>
				/// <param name="transform">Transform.</param>
				public static Vector3[] GetPositions (AnimationClip clip, string path="", Transform transform=null)
				{

						EditorCurveBinding_PosX.path = path;
						EditorCurveBinding_PosY.path = path;
						EditorCurveBinding_PosZ.path = path;
						
						
						AnimationCurve curvePosX = AnimationUtility.GetEditorCurve (clip, EditorCurveBinding_PosX);
						AnimationCurve curvePosY = AnimationUtility.GetEditorCurve (clip, EditorCurveBinding_PosY);
						AnimationCurve curvePosZ = AnimationUtility.GetEditorCurve (clip, EditorCurveBinding_PosZ);
						

						Vector3[] positionVectors = null;

						//expect every keyframe to have x,y,z
						if (curvePosX != null && curvePosY != null && curvePosZ != null) {

								int keysNumber = curvePosX.length;


								positionVectors = new Vector3[keysNumber];
									
								
								for (int keyCurrent=0; keyCurrent<keysNumber; keyCurrent++) {

										if (transform != null)//conver local transform to world
												positionVectors [keyCurrent] = transform.TransformPoint (new Vector3 (curvePosX.keys [keyCurrent].value, curvePosY.keys [keyCurrent].value, curvePosZ.keys [keyCurrent].value));
										else
												positionVectors [keyCurrent] = new Vector3 (curvePosX.keys [keyCurrent].value, curvePosY.keys [keyCurrent].value, curvePosZ.keys [keyCurrent].value);
								}
								

						}


					


						return positionVectors;
				}



					

		}
}

