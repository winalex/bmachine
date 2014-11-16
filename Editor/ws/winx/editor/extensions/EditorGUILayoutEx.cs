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
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ws.winx.editor.extensions
{
		public class EditorGUILayoutEx
		{

		//		private static object _CustomPopup_SelectedObject;
				private static int _CustomPopup_ControlID = -1;
				private static int _CustomPopup_SelectedIndex = -1;

				public delegate void MenuCallaback<T> (int selectedIndex,T SelectedObject,int controlID);

				public delegate void EventCallback (int ownerControlID,Event e);


	

//				public static int CustomPopup<T>(GUIContent label, int selectedIndex, GUIContent[] displayOptions, T[] values,
//		                               MenuCallaback<T> onSelection=null,
//		                               EventCallback onEvent=null,
//		                               GUIStyle labelStyle=null
//				)
//				{
//						GUIContent content;
//						string buttonLabel = null;
//						int i = 0;
//						int len;
//						int inxd;
//			
//						EditorGUILayout.BeginHorizontal ();
//			
//			
//			
//						//add Label field
//						// Screen.width in insprector returns its width not Screen => so 35% for the lable and rest for the popup button
//						if (labelStyle != null)
//								EditorGUILayout.LabelField (label, labelStyle, GUILayout.Width (Screen.width * 0.35f));
//						else
//								EditorGUILayout.LabelField (label, GUILayout.Width (Screen.width * 0.35f));
//			
//			
////						//get current control ID
//						int controlID = GUIUtility.GetControlID (FocusType.Passive) + 1;
//			
//						//if current == previous selected control => asign "selectedObject" and reset global
//						if (controlID == EditorGUILayoutEx._CustomPopup_ControlID) {
//								selectedIndex = EditorGUILayoutEx._CustomPopup_SelectedIndex;
//				
//								//reset
//								EditorGUILayoutEx._CustomPopup_SelectedObject = null;
//								EditorGUILayoutEx._CustomPopup_SelectedIndex = -1;
//								EditorGUILayoutEx._CustomPopup_ControlID = -1;
//						}
//			
//						//if selectionObject is null on Init
//						if (selectedIndex < 0) {
//				
//				
//								inxd = len = displayOptions.Length;
//				
//								//set "selectedObject" to first values[i] that is Enable
//								for (i=0; i<len; i++) {
//										buttonLabel = displayOptions [i].text;
//					
//										//check if contains "*" disable mark
//										if (buttonLabel.LastIndexOf ('*', buttonLabel.Length - 1) > -1)
//												continue;
//					
//										inxd = i;
//										break;
//					
//								}
//				
//								//if we have found value[i] which is Enabled => set selectObject to it
//								if (inxd < len) {
//										//remove submenu's mark "/"
//										buttonLabel = buttonLabel.Substring (buttonLabel.LastIndexOf ("/") + 1);		
//										selectedIndex = inxd;
//								}
//				
//						} else {
//				
//								//find label on displayOptions[i] which is value[i] == selectedObject
//								buttonLabel = displayOptions [selectedIndex].text;
//				
//								//remove submenus mark "/"
//								buttonLabel = buttonLabel.Substring (buttonLabel.LastIndexOf ("/") + 1);
//				
//				
//						}
//			
//			
//			//Debug.Log ("SelectedIndex:"+selectedIndex);
//			
//			
//						//dispatch events
//						if (onEvent != null && controlID == GUIUtility.hotControl) {
//								onEvent (controlID, Event.current);
//						}
//						//								switch (Event.current.GetTypeForControl (controlID)) {
//						//								case EventType.mouseDown://never triggered
//						//										Debug.Log ("Event " + Event.current.type);
//						//
//						//										break;
//						//								case EventType.mouseUp:
//						//										Debug.Log ("Event " + Event.current.type);
//						//										break;
//						//			default:
//						//				Debug.Log ("Event "+Event.current.type);
//						//				break;
//						//			}
//			
//			
//			
//			
//			
//						if (GUILayout.Button (new GUIContent (buttonLabel), EditorStyles.popup, GUILayout.MinWidth (200))) {
//				
//								//shoot custom MouseDown event here!!!
//				
//								// Now create the menu, add items and show it
//								GenericMenu menu = new GenericMenu ();
//				
//								len = displayOptions.Length;
//				
//								for (i=0; i<len; i++) {
//					
//										content = displayOptions [i];
//					
//										// null mean AddSeparator
//										if (content==null)
//												menu.AddSeparator ("");
//										// "*" at the end => mean AddDisabledItem
//										else if (content.text.LastIndexOf ('*', content.text.Length - 1) > -1) {
//												content.text = content.text.Remove (content.text.Length - 1);
//												menu.AddDisabledItem (content);
//										} else
//												menu.AddItem (content, false, (obj) => {
//														int inx = (int)obj;
//														EditorGUILayoutEx._CustomPopup_SelectedObject = values [inx];
//														EditorGUILayoutEx._CustomPopup_ControlID = controlID;
//														EditorGUILayoutEx._CustomPopup_SelectedIndex = inx;
//							
//														//Debug.Log ("Selected:" + inx);	
//							
//														//dispatch selected
//														if (onSelection != null)
//																onSelection (inx, values [inx], controlID);
//							
//												}, i);
//								}
//				
//				
//				
//								menu.ShowAsContext ();
//						}
//			
//						EditorGUILayout.EndHorizontal ();
//						return selectedIndex;
//				}



		#region CustomPopup

		public static int CustomPopup<T>(GUIContent label, int selectedIndex, GUIContent[] displayOptions, IList<T> values,
		                                 MenuCallaback<T> onSelection=null,
		                                 EventCallback onEvent=null,
		                                 GUIStyle labelStyle=null
		                                 )
		{
			GUIContent content;
			string buttonLabel = null;
			int i = 0;
			int len;
			int inxd;
			
			EditorGUILayout.BeginHorizontal ();
			
			
			
			//add Label field
			// Screen.width in insprector returns its width not Screen => so 35% for the lable and rest for the popup button
			if (labelStyle != null)
				EditorGUILayout.LabelField (label, labelStyle, GUILayout.Width (Screen.width * 0.35f));
			else
				EditorGUILayout.LabelField (label, GUILayout.Width (Screen.width * 0.35f));
			
			
			//						//get current control ID
			int controlID = GUIUtility.GetControlID (FocusType.Passive) + 1;
			
			//if current == previous selected control => asign "selectedObject" and reset global
			if (controlID == EditorGUILayoutEx._CustomPopup_ControlID) {
				selectedIndex = EditorGUILayoutEx._CustomPopup_SelectedIndex;
				
				//reset
			//	EditorGUILayoutEx._CustomPopup_SelectedObject = null;
				EditorGUILayoutEx._CustomPopup_SelectedIndex = -1;
				EditorGUILayoutEx._CustomPopup_ControlID = -1;
			}
			
			//if selectionObject is null on Init
			if (selectedIndex < 0) {
				
				
				inxd = len = displayOptions.Length;
				
				//set "selectedObject" to first values[i] that is Enable
				for (i=0; i<len; i++) {
					buttonLabel = displayOptions [i].text;
					
					//check if contains "*" disable mark
					if (buttonLabel.LastIndexOf ('*', buttonLabel.Length - 1) > -1)
						continue;
					
					inxd = i;
					break;
					
				}
				
				//if we have found value[i] which is Enabled => set selectObject to it
				if (inxd < len) {
					//remove submenu's mark "/"
					buttonLabel = buttonLabel.Substring (buttonLabel.LastIndexOf ("/") + 1);		
					selectedIndex = inxd;
				}
				
			} else {
				
				//find label on displayOptions[i] which is value[i] == selectedObject
				buttonLabel = displayOptions [selectedIndex].text;
				
				//remove submenus mark "/"
				buttonLabel = buttonLabel.Substring (buttonLabel.LastIndexOf ("/") + 1);
				
				
			}
			
			
			//Debug.Log ("SelectedIndex:"+selectedIndex);
			
			
			//dispatch events
			if (onEvent != null && controlID == GUIUtility.hotControl) {
				onEvent (controlID, Event.current);
			}
			//								switch (Event.current.GetTypeForControl (controlID)) {
			//								case EventType.mouseDown://never triggered
			//										Debug.Log ("Event " + Event.current.type);
			//
			//										break;
			//								case EventType.mouseUp:
			//										Debug.Log ("Event " + Event.current.type);
			//										break;
			//			default:
			//				Debug.Log ("Event "+Event.current.type);
			//				break;
			//			}
			
			
			
			
			
			if (GUILayout.Button (new GUIContent (buttonLabel), EditorStyles.popup, GUILayout.MinWidth (200))) {
				
				//shoot custom MouseDown event here!!!
				
				// Now create the menu, add items and show it
				GenericMenu menu = new GenericMenu ();
				
				len = displayOptions.Length;
				
				for (i=0; i<len; i++) {
					
					content = displayOptions [i];
					
					// null mean AddSeparator
					if (content==null)
						menu.AddSeparator ("");
					// "*" at the end => mean AddDisabledItem
					else if (content.text.LastIndexOf ('*', content.text.Length - 1) > -1) {
						content.text = content.text.Remove (content.text.Length - 1);
						menu.AddDisabledItem (content);
					} else
						menu.AddItem (content, false, (obj) => {
							int inx = (int)obj;
							//EditorGUILayoutEx._CustomPopup_SelectedObject = values [inx];
							EditorGUILayoutEx._CustomPopup_ControlID = controlID;
							EditorGUILayoutEx._CustomPopup_SelectedIndex = inx;
							
							//Debug.Log ("Selected:" + inx);	
							
							//dispatch selected
							if (onSelection != null)
								onSelection (inx, values [inx], controlID);
							
						}, i);
				}
				
				
				
				menu.ShowAsContext ();
			}
			
			EditorGUILayout.EndHorizontal ();
			return selectedIndex;
		}
		#endregion

		#region CustomObjectPopup
		public static T CustomObjectPopup<T> (GUIContent label, T selectedObject,GUIContent[] displayOptions, IList<T> values,
		                                      MenuCallaback<T> onSelection=null,
		                                      EventCallback onEvent=null,
		                                      GUIStyle labelStyle=null
		                                      
		                                      )
		{

			int inxOfSelectedObject;
			int len;
			int i;
			string buttonLabel;
			
			
			
			
			
			//if selectionObject is null on Init
			if (selectedObject == null) {
				
				
				inxOfSelectedObject = len = displayOptions.Length;
				
				//find index of "selectedObject" that is Enable
				for (i=0; i<len; i++) {
					buttonLabel = displayOptions [i].text;
					
					//check if contains "*" disable mark
					if (buttonLabel.LastIndexOf ('*', buttonLabel.Length - 1) > -1)
						continue;
					
					inxOfSelectedObject = i;
					break;
					
				}
				
				//if we have found value[i] which is Enabled => set selectObject to it
				if (inxOfSelectedObject < len) {
					
					selectedObject = values [inxOfSelectedObject];
					CustomPopup(label,inxOfSelectedObject,displayOptions,values,onSelection,onEvent,labelStyle);
				}
				
			} else {
				
				//find label on displayOptions[i] which is value[i] == selectedObject
				inxOfSelectedObject=values.IndexOf(selectedObject);
					//Array.IndexOf (values, selectedObject);

				
				inxOfSelectedObject=CustomPopup(label,inxOfSelectedObject,displayOptions,values,onSelection,onEvent,labelStyle);
				
				selectedObject=values[inxOfSelectedObject];
				
				
			}
			
			
			
			
			
			
			return selectedObject;

		}

		#endregion

//		public static T CustomObjectPopup<T> (GUIContent label, T selectedObject, GUIContent[] displayOptions, T[] values,
//		                                      MenuCallaback<T> onSelection=null,
//		                                      EventCallback onEvent=null,
//		                                      GUIStyle labelStyle=null
//		                                      
//		                                      )
//		{
//			
//			int inxOfSelectedObject;
//			int len;
//			int i;
//			string buttonLabel;
//		
//
//
//								
//			
//			//if selectionObject is null on Init
//			if (selectedObject == null) {
//				
//				
//				inxOfSelectedObject = len = displayOptions.Length;
//				
//				//find index of "selectedObject" that is Enable
//				for (i=0; i<len; i++) {
//					buttonLabel = displayOptions [i].text;
//					
//					//check if contains "*" disable mark
//					if (buttonLabel.LastIndexOf ('*', buttonLabel.Length - 1) > -1)
//						continue;
//					
//					inxOfSelectedObject = i;
//					break;
//					
//				}
//				
//				//if we have found value[i] which is Enabled => set selectObject to it
//				if (inxOfSelectedObject < len) {
//											
//					selectedObject = values [inxOfSelectedObject];
//					CustomPopup(label,inxOfSelectedObject,displayOptions,values,onSelection,onEvent,labelStyle);
//				}
//				
//			} else {
//				
//				//find label on displayOptions[i] which is value[i] == selectedObject
//				inxOfSelectedObject=Array.IndexOf (values, selectedObject);
//
//
//				inxOfSelectedObject=CustomPopup(label,inxOfSelectedObject,displayOptions,values,onSelection,onEvent,labelStyle);
//
//				selectedObject=values[inxOfSelectedObject];
//				
//				
//			}
//
//
//
//		
//			
//			
//			return selectedObject;
//		}

	}
}