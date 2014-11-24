using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ws.winx.unity;
using ws.winx.bmachine.extensions;


namespace ws.winx.editor.bmachine.extensions
{
	[Serializable]
	internal class MecanimNodeEventTimeLine
	{
		//
		// Fields
		//
		private int m_HoverEvent = -1;
		
		private bool m_DirtyTooltip;
		
		private Vector2 m_InstantTooltipPoint = Vector2.zero;
		
		private string m_InstantTooltipText;
		
		private bool[] m_EventsSelected;
		
		//private EditorWindow m_Owner;
		private IMecanimEvents m_Owner;
		
		[NonSerialized]
		private AnimationEvent[] m_EventsAtMouseDown;
		
		[NonSerialized]
		private float[] m_EventTimes;
		
		//
		// Constructors
		//
//		public MecanimNodeEventTimeLine(EditorWindow owner)
//		{
//			this.m_Owner = owner;
//		}

		//
		// Constructors
		//
		public MecanimNodeEventTimeLine(IMecanimEvents owner)
		{
			this.m_Owner = owner;
			AnimationUtility.SetAnimationEvents (state.m_ActiveAnimationClip, new AnimationEvent[]{new AnimationEvent ()});
			//state.m_ActiveAnimationClip.AddEvent (new AnimationEvent ());
			this.m_Owner.SetAnimationEvents(new AnimationEvent[]{new AnimationEvent()});
		}
		
		//
		// Methods
		//
		//public void AddEvent (AnimationWindowState state)
		public void AddEvent (AnimationState state)
		{
//			float time = (float)state.m_Frame / state.frameRate;
//			int index = AnimationEventPopup.Create (state.m_RootGameObject, state.m_ActiveAnimationClip, time, this.m_Owner);
//			this.Select (state.m_ActiveAnimationClip, index);
		}
		
		private void CheckRectsOnMouseMove (Rect eventLineRect, AnimationEvent[] events, Rect[] hitRects)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			bool flag = false;
			if (events.Length == hitRects.Length)
			{
				for (int i = hitRects.Length - 1; i >= 0; i--)
				{
					if (hitRects [i].Contains (mousePosition))
					{
						flag = true;
						if (this.m_HoverEvent != i)
						{
							this.m_HoverEvent = i;
							this.m_InstantTooltipText = events [this.m_HoverEvent].functionName;
							this.m_InstantTooltipPoint = new Vector2 (hitRects [this.m_HoverEvent].xMin + (float)((int)(hitRects [this.m_HoverEvent].width / 2f)) + eventLineRect.x, eventLineRect.yMax);
							this.m_DirtyTooltip = true;
						}
					}
				}
			}
			if (!flag)
			{
				this.m_HoverEvent = -1;
				this.m_InstantTooltipText = string.Empty;
			}
		}
		
		private void DeleteEvents (AnimationClip clip, bool[] deleteIndices)
		{
			bool flag = false;
			List<AnimationEvent> list = new List<AnimationEvent> (AnimationUtility.GetAnimationEvents (clip));
			for (int i = list.Count  - 1; i >= 0; i--)
			//for (int i = list.get_Count () - 1; i >= 0; i--)
			{
				if (deleteIndices [i])
				{
					list.RemoveAt (i);
					flag = true;
				}
			}
			if (flag)
			{
				Debug.Log("AnimationEventPopup.ClosePopup ()");
				Undo.RegisterCompleteObjectUndo (clip, "Delete Event");
				AnimationUtility.SetAnimationEvents (clip, list.ToArray ());
				this.m_EventsSelected = new bool[list.Count];
				this.m_DirtyTooltip = true;
			}
		}
		
		public void DeselectAll ()
		{
			this.m_EventsSelected = null;
		}
		
		public void DrawInstantTooltip (Rect position)
		{
			if (this.m_InstantTooltipText != null && this.m_InstantTooltipText != string.Empty)
			{
				GUIStyle gUIStyle = "AnimationEventTooltip";
				Vector2 vector = gUIStyle.CalcSize (new GUIContent (this.m_InstantTooltipText));
				Rect position2 = new Rect (this.m_InstantTooltipPoint.x - 10f, this.m_InstantTooltipPoint.y + 24f, vector.x, vector.y);
				if (position2.xMax > position.width)
				{
					position2.x = position.width - position2.width;
				}
				GUI.Label (position2, this.m_InstantTooltipText, gUIStyle);
				position2 = new Rect (this.m_InstantTooltipPoint.x - 3f, this.m_InstantTooltipPoint.y, 7f, 25f);
				GUI.Label (position2, string.Empty, "AnimationEventTooltipArrow");
			}
		}
		
		public void EventLineContextMenuAdd (object obj)
		{
			Debug.Log ("EventLineContextMenuAdd");
			MecanimNodeEventTimeLine.EventLineContextMenuObject eventLineContextMenuObject = (MecanimNodeEventTimeLine.EventLineContextMenuObject)obj;

			int num = 0;// AnimationEventPopup.Create (eventLineContextMenuObject.m_Animated, eventLineContextMenuObject.m_Clip, eventLineContextMenuObject.m_Time, this.m_Owner);
			this.Select (eventLineContextMenuObject.m_Clip, num);
			this.m_EventsSelected = new bool[AnimationUtility.GetAnimationEvents (eventLineContextMenuObject.m_Clip).Length];
			this.m_EventsSelected [num] = true;
		}
		
		public void EventLineContextMenuDelete (object obj)
		{
			MecanimNodeEventTimeLine.EventLineContextMenuObject eventLineContextMenuObject = (MecanimNodeEventTimeLine.EventLineContextMenuObject)obj;
			AnimationClip clip = eventLineContextMenuObject.m_Clip;
			if (clip == null)
			{
				return;
			}
			int index = eventLineContextMenuObject.m_Index;
			if (this.m_EventsSelected [index])
			{
				this.DeleteEvents (clip, this.m_EventsSelected);
			}
			else
			{
				bool[] array = new bool[this.m_EventsSelected.Length];
				array [index] = true;
				this.DeleteEvents (clip, array);
			}
		}
		
		public void EventLineContextMenuEdit (object obj)
		{
			MecanimNodeEventTimeLine.EventLineContextMenuObject eventLineContextMenuObject = (MecanimNodeEventTimeLine.EventLineContextMenuObject)obj;


			Debug.Log("AnimationEventPopup.Edit (eventLineContextMenuObject.m_Animated, eventLineContextMenuObject.m_Clip, eventLineContextMenuObject.m_Index, this.m_Owner);");
			this.Select (eventLineContextMenuObject.m_Clip, eventLineContextMenuObject.m_Index);
		}

		 AnimationWindowState state=new AnimationWindowState();

		class AnimationWindowState{

			public AnimationClip m_ActiveAnimationClip = new AnimationClip ();
			public GameObject m_RootGameObject = null;

			public float frameRate = 60f;

			public float frameSpan
			{
				get
				{
					return this.timeSpan * this.frameRate;
				}
			}

			public float maxTime=5f;
			public float minTime=0f;

			public float timeSpan
			{
				get
				{
					return this.maxTime - this.minTime;
				}
			}


			
			public float PixelToTime (float pixelX, Rect rect)
			{
				return pixelX * this.timeSpan / rect.width + this.minTime;
			}

			public float FrameDeltaToPixel (Rect rect)
			{
				return rect.width / this.frameSpan;
			}

			public float minFrame
			{
				get
				{
					return this.minTime * this.frameRate;
				}
			}
			
			public float FrameToPixel (float i, Rect rect)
			{
				return (i - this.minFrame) * rect.width / this.frameSpan;
			}


			public float PixelDeltaToTime (Rect rect)
			{
				return this.timeSpan / rect.width;
			}
		}
		
		//public void EventLineGUI (Rect rect, AnimationSelection selection, AnimationWindowState state, CurveEditor )
		public void EventLineGUI (Rect rect)
		{
			AnimationClip activeAnimationClip = state.m_ActiveAnimationClip;
			GameObject rootGameObject = state.m_RootGameObject;
			GUI.BeginGroup (rect);
			Color color = GUI.color;
			Rect rect2 = new Rect (0f, 0f, rect.width, rect.height);
			float time = (float)Mathf.RoundToInt (state.PixelToTime (Event.current.mousePosition.x, rect) * state.frameRate) / state.frameRate;
			if (activeAnimationClip != null)
			{
				AnimationEvent[] animationEvents = AnimationUtility.GetAnimationEvents (activeAnimationClip);
				Texture image = EditorGUIUtility.IconContent ("Animation.EventMarker").image;
				Rect[] array = new Rect[animationEvents.Length];
				Rect[] array2 = new Rect[animationEvents.Length];
				int num = 1;
				int num2 = 0;
				for (int i = 0; i < animationEvents.Length; i++)
				{
					AnimationEvent animationEvent = animationEvents [i];
					if (num2 == 0)
					{
						num = 1;
						while (i + num < animationEvents.Length && animationEvents [i + num].time == animationEvent.time)
						{
							num++;
						}
						num2 = num;
					}
					num2--;
					float num3 = Mathf.Floor (state.FrameToPixel (animationEvent.time * activeAnimationClip.frameRate, rect));
					int num4 = 0;
					if (num > 1)
					{
						float num5 = (float)Mathf.Min ((num - 1) * (image.width - 1), (int)(state.FrameDeltaToPixel (rect) - (float)(image.width * 2)));
						num4 = Mathf.FloorToInt (Mathf.Max (0f, num5 - (float)((image.width - 1) * num2)));
					}
					Rect rect3 = new Rect (num3 + (float)num4 - (float)(image.width / 2), (rect.height - 10f) * (float)(num2 - num + 1) / (float)Mathf.Max (1, num - 1), (float)image.width, (float)image.height);
					array [i] = rect3;
					array2 [i] = rect3;
				}
				if (this.m_DirtyTooltip)
				{
					if (this.m_HoverEvent >= 0 && this.m_HoverEvent < array.Length)
					{
						Debug.Log ("AnimationEventPopup.FormatEvent... tooltip text");
					//	this.m_InstantTooltipText = AnimationEventPopup.FormatEvent (rootGameObject, animationEvents [this.m_HoverEvent]);
						this.m_InstantTooltipPoint = new Vector2 (array [this.m_HoverEvent].xMin + (float)((int)(array [this.m_HoverEvent].width / 2f)) + rect.x - 30f, rect.yMax);
					}
					this.m_DirtyTooltip = false;
				}
				if (this.m_EventsSelected == null || this.m_EventsSelected.Length != animationEvents.Length)
				{
					this.m_EventsSelected = new bool[animationEvents.Length];
					Debug.Log("AnimationEventPopup.ClosePopup ();");
				}
				Vector2 zero = Vector2.zero;
				int num6;
				float num7;
				float num8;
				//HighLevelEvent highLevelEvent = EditorGUIExt.MultiSelection (rect, array2, new GUIContent (image), array, ref this.m_EventsSelected, null, out num6, out zero, out num7, out num8, GUIStyleX.none);


				HighLevelEvent highLevelEvent = EditorGUIExtW.MultiSelection (rect, array2, new GUIContent (image), array, ref this.m_EventsSelected, null, out num6, out zero, out num7, out num8, GUIStyle.none);

				if (highLevelEvent != HighLevelEvent.None)
				{
					switch (highLevelEvent)
					{
					case HighLevelEvent.DoubleClick:
						if (num6 != -1)
						{
							Debug.Log ("AnimationEventPopup.Edit (rootGameObject, selection.clip, num6, this.m_Owner);");
						}
						else
						{
							this.EventLineContextMenuAdd (new MecanimNodeEventTimeLine.EventLineContextMenuObject (rootGameObject, activeAnimationClip, time, -1));
						}
						break;
					case HighLevelEvent.ContextClick:
					{
						GenericMenu genericMenu = new GenericMenu ();
						MecanimNodeEventTimeLine.EventLineContextMenuObject userData = new MecanimNodeEventTimeLine.EventLineContextMenuObject (rootGameObject, activeAnimationClip, animationEvents [num6].time, num6);
						genericMenu.AddItem (new GUIContent ("Edit Animation Event"), false, new GenericMenu.MenuFunction2 (this.EventLineContextMenuEdit), userData);
						genericMenu.AddItem (new GUIContent ("Add Animation Event"), false, new GenericMenu.MenuFunction2 (this.EventLineContextMenuAdd), userData);
						genericMenu.AddItem (new GUIContent ("Delete Animation Event"), false, new GenericMenu.MenuFunction2 (this.EventLineContextMenuDelete), userData);
						genericMenu.ShowAsContext ();
						this.m_InstantTooltipText = null;
						this.m_DirtyTooltip = true;
						//state.Repaint ();
						break;
					}
					case HighLevelEvent.BeginDrag:
						this.m_EventsAtMouseDown = animationEvents;
						this.m_EventTimes = new float[animationEvents.Length];
						for (int j = 0; j < animationEvents.Length; j++)
						{
							this.m_EventTimes [j] = animationEvents [j].time;
						}
						break;
					case HighLevelEvent.Drag:
					{
						for (int k = animationEvents.Length - 1; k >= 0; k--)
						{
							if (this.m_EventsSelected [k])
							{
								AnimationEvent animationEvent2 = this.m_EventsAtMouseDown [k];
								animationEvent2.time = this.m_EventTimes [k] + zero.x * state.PixelDeltaToTime (rect);
								animationEvent2.time = Mathf.Max (0f, animationEvent2.time);
								animationEvent2.time = (float)Mathf.RoundToInt (animationEvent2.time * activeAnimationClip.frameRate) / activeAnimationClip.frameRate;
							}
						}
						int[] array3 = new int[this.m_EventsSelected.Length];
						for (int l = 0; l < array3.Length; l++)
						{
							array3 [l] = l;
						}
						Array.Sort (this.m_EventsAtMouseDown, array3, new MecanimNodeEventTimeLine.EventComparer ());
						bool[] array4 = (bool[])this.m_EventsSelected.Clone ();
						float[] array5 = (float[])this.m_EventTimes.Clone ();
						for (int m = 0; m < array3.Length; m++)
						{
							this.m_EventsSelected [m] = array4 [array3 [m]];
							this.m_EventTimes [m] = array5 [array3 [m]];
						}
						Undo.RegisterCompleteObjectUndo (activeAnimationClip, "Move Event");
						AnimationUtility.SetAnimationEvents (activeAnimationClip, this.m_EventsAtMouseDown);
						this.m_DirtyTooltip = true;
						break;
					}
					case HighLevelEvent.Delete:
						this.DeleteEvents (activeAnimationClip, this.m_EventsSelected);
						break;
					case HighLevelEvent.SelectionChanged:
						//curveEditor.SelectNone ();
						if (num6 != -1)
						{

							Debug.Log("AnimationEventPopup.UpdateSelection (rootGameObject, selection.clip, num6, this.m_Owner);");
						}
						break;
					}
				}
				this.CheckRectsOnMouseMove (rect, animationEvents, array);
			}

			if (Event.current.type == EventType.ContextClick && rect2.Contains (Event.current.mousePosition) )
			//if (Event.current.type == EventType.ContextClick && rect2.Contains (Event.current.mousePosition) && selection.EnsureClipPresence ())
			{
				Event.current.Use ();
				GenericMenu genericMenu2 = new GenericMenu ();
				genericMenu2.AddItem (new GUIContent ("Add Animation Event"), false, new GenericMenu.MenuFunction2 (this.EventLineContextMenuAdd), new MecanimNodeEventTimeLine.EventLineContextMenuObject (rootGameObject, activeAnimationClip, time, -1));
				genericMenu2.ShowAsContext ();
			}
			GUI.color = color;
			GUI.EndGroup ();
		}
		
		private void Select (AnimationClip clip, int index)
		{
			this.m_EventsSelected = new bool[AnimationUtility.GetAnimationEvents (clip).Length];
			this.m_EventsSelected [index] = true;
		}
		
		//
		// Nested Types
		//
		public class EventComparer : IComparer
		{
			int IComparer.Compare (object objX, object objY)
			{
				AnimationEvent animationEvent = (AnimationEvent)objX;
				AnimationEvent animationEvent2 = (AnimationEvent)objY;
				float time = animationEvent.time;
				float time2 = animationEvent2.time;
				if (time != time2)
				{
					return (int)Mathf.Sign (time - time2);
				}
				int hashCode = animationEvent.GetHashCode ();
				int hashCode2 = animationEvent2.GetHashCode ();
				return hashCode - hashCode2;
			}
		}
		
		private class EventLineContextMenuObject
		{
			public GameObject m_Animated;
			public AnimationClip m_Clip;
			public float m_Time;
			public int m_Index;
			public EventLineContextMenuObject (GameObject animated, AnimationClip clip, float time, int index)
			{
				this.m_Animated = animated;
				this.m_Clip = clip;
				this.m_Time = time;
				this.m_Index = index;
			}
		}
	}
}