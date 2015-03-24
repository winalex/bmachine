//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------
using UnityEngine;
using UnityEditor;
using BehaviourMachine;

using ws.winx.bmachine;
using BehaviourMachineEditor;
using UnityEditorInternal;
using ws.winx.unity;
using System;
using System.Collections.Generic;
using ws.winx.editor.extensions;
using System.Linq;
using ws.winx.csharp.utilities;
using System.Runtime.Serialization;
using System.Reflection;
using ws.winx.csharp.extensions;
using ws.winx.editor.windows;
using UnityEngine.Events;

namespace ws.winx.editor.bmachine
{

		/// <summary>
		/// Wrapper class for BehaviourTreeEditor.
		/// <seealso cref="BehaviourMachine.BehaviourTree" />
		/// </summary>
		[CustomEditor(typeof(BlackboardCustom))]
		public class BlackboardCustomEditor :Editor
		{
				
				ReorderableList  __variablesReordableList;
				GenericMenu genericMenu;
				string _typeNameSelected = "None";
				List<Type> typesCustom;

				private void OnEnable ()
				{


						//serializedObject.FindProperty("variablesList").objectReferenceValue
						if (__variablesReordableList == null) {

								
								SerializedProperty variableListSerialized = serializedObject.FindProperty ("variablesList");

								typesCustom = ((BlackboardCustom)serializedObject.targetObject).typesCustom;
								if (typesCustom == null)
										typesCustom = ((BlackboardCustom)serializedObject.targetObject).typesCustom = new List<Type> ();
								
				
				
								__variablesReordableList = new ReorderableList (serializedObject, variableListSerialized, 
				                                       true, true, true, true);
								__variablesReordableList.drawElementCallback = onDrawElement;

								__variablesReordableList.onAddDropdownCallback = onAddDropdownCallback;

								__variablesReordableList.drawHeaderCallback = onDrawHeaderElement;

								__variablesReordableList.onRemoveCallback = onRemoveCallback;

								__variablesReordableList.onSelectCallback = onSelectCallback;

								__variablesReordableList.elementHeight=32f;
				

								genericMenu = EditorGUILayoutEx.GeneraterGenericMenu<Type> (EditorGUILayoutEx.unityTypesDisplayOptions, EditorGUILayoutEx.unityTypes, onTypeSelection);

								fillMenuCustomTypes ();

								

							





								


						}

				}

				void fillMenuCustomTypes ()
				{
						genericMenu.AddSeparator (string.Empty);
						genericMenu.AddItem (new GUIContent ("Any UnityObject"), false, onTypeSelection, typeof(UnityEngine.Object)



			                     );


						Type[] derivedTypes = TypeUtility.GetDerivedTypes (typeof(System.Object));
						int i = 0;
						Type typeUnityObject = typeof(UnityEngine.Object);
						for (i = 0; i < derivedTypes.Length; i++) {
				
								if (!derivedTypes [i].IsSubclassOf (typeUnityObject)) {
										string text2 = derivedTypes [i].ToString ();
										genericMenu.AddItem (new GUIContent ("Custom Object/" + text2.Replace ('.', '/')), _typeNameSelected == text2, onTypeCustomSelected, derivedTypes [i]);
								}
						}

						genericMenu.AddSeparator (string.Empty);
						genericMenu.AddItem (new GUIContent ("Reload"), false, delegate {
								fillMenuCustomTypes ();
						});

						
			
			
						genericMenu.AddSeparator (string.Empty);
						genericMenu.AddItem (new GUIContent ("Global Blackboard"), false, delegate {
								EditorApplication.ExecuteMenuItem ("Tools/BehaviourMachine/Global Blackboard");
						});
				}
		
				void AddVariableToList<T> (string name, T value, ReorderableList list)
				{
						AddVariableToList1 (name, value, list);
				}

				void AddVariableToList1 (string name, object value, ReorderableList list)
				{


						var index = list.serializedProperty.arraySize;
						list.serializedProperty.arraySize++;
						list.index = index;
			
						UnityVariable variable = (UnityVariable)ScriptableObject.CreateInstance<UnityVariable> ();
						variable.name = name;
						
						variable.Value = value;
			
						var element = list.serializedProperty.GetArrayElementAtIndex (index);
						element.objectReferenceValue = variable;
			
						serializedObject.ApplyModifiedProperties ();

						
				}

				void onTypeSelection (object userData)
				{

						
						Type type = (Type)userData;
						if (type == typeof(Texture2D))
								AddVariableToList ("New " + type.Name, new Texture2D (2, 2), __variablesReordableList);
						else if (type == typeof(string))
								AddVariableToList ("New " + type.Name, String.Empty, __variablesReordableList);
						else if (type == typeof(Material))
								AddVariableToList ("New " + type.Name, new Material (Shader.Find ("Diffuse")), __variablesReordableList);
						else if (type == typeof(AnimationCurve))
								AddVariableToList ("New " + type.Name, new AnimationCurve (new Keyframe (0f, 0f, 0f, 0f), new Keyframe (1f, 1f, 0f, 0f)), __variablesReordableList);
						else if (type == typeof(Texture3D))
								AddVariableToList ("New " + type.Name, new Texture3D (2, 2, 2, TextureFormat.ARGB32, false), __variablesReordableList);
						else
								AddVariableToList ("New " + type.Name, FormatterServices.GetUninitializedObject (type), __variablesReordableList);

					
				}

				void onSelectCallback (ReorderableList list)
				{
						UnityVariable variable = list.serializedProperty.GetArrayElementAtIndex (list.index).objectReferenceValue as UnityVariable;

						//show popup just for complex types and UnityEvent
						if (variable != null
								&& (Array.IndexOf (EditorGUILayoutEx.unityTypes, variable.ValueType) < 0
								|| variable.ValueType == typeof(UnityEvent)) 
			    ) {
								UnityObjectEditorWindow.Show (variable);


						}
				}

				void onRemoveCallback (ReorderableList list)
				{
						if (UnityEditor.EditorUtility.DisplayDialog ("Warning!", 
			                                "Are you sure you want to delete the Unity Variable?", "Yes", "No")) {
								ReorderableList.defaultBehaviours.DoRemoveButton (list);
								
								list.serializedProperty.DeleteArrayElementAtIndex (list.index);
								

								serializedObject.ApplyModifiedProperties ();
								
						}
				}

				void onAddDropdownCallback (Rect buttonRect, ReorderableList list)
				{

						GUIUtility.hotControl = 0;
						GUIUtility.keyboardControl = 0;
						int i;

						if (typesCustom != null && typesCustom.Count > 0) {
								genericMenu.AddSeparator (string.Empty);
								for (i=0; i<typesCustom.Count; i++) {
										genericMenu.AddItem (new GUIContent (typesCustom [i].Name), true, onTypeCustomSelected, typesCustom [i]);
								}
							    
						}

						genericMenu.ShowAsContext ();
		
				}
		
				void onDrawElement (Rect rect, int index, bool isActive, bool isFocused)
				{

						SerializedProperty property = __variablesReordableList.serializedProperty.GetArrayElementAtIndex (index); 

						if (property == null || property.objectReferenceValue == null) {
								return;
						}

						DrawVariables (rect, property);



				}

				void onDrawHeaderElement (Rect rect)
				{
						EditorGUI.LabelField (rect, "Blackboard Variables:");
				}

				public Rect DrawVariables (Rect position, SerializedProperty property)
				{
						
					
						UnityVariable currentVariable;
					

		
				

						currentVariable = (UnityVariable)property.objectReferenceValue;

						
						
						if (currentVariable != null && currentVariable.serializedProperty != null) {

								Type type = currentVariable.ValueType;

								if (Array.IndexOf (EditorGUILayoutEx.unityTypes, type) < 0
										|| type == typeof(UnityEvent)) {

										currentVariable.name = EditorGUI.TextField (position, currentVariable.name);

								} else {
										PropertyDrawer drawer;

										
										drawer=EditorUtilityEx.GetDrawer(type);
										
										if(drawer==null)
										 drawer= EditorUtilityEx.GetDefaultDrawer ();
			
			
										Rect pos = new Rect (32, position.y, 80, 16);

										currentVariable.name = EditorGUI.TextField (pos, currentVariable.name);
										position.x = 113f;
										position.width -= 80f;

					EditorGUI.BeginChangeCheck ();
				


										drawer.OnGUI (position, currentVariable.serializedProperty, new GUIContent (""));
			
					if (EditorGUI.EndChangeCheck ()) {
						
						currentVariable.ApplyModifiedProperties ();

						
						EditorUtility.SetDirty (currentVariable);


						
					}
										
								}
						}
			
					



			
					



		
						return position;
		
				}

				void onTypeCustomSelected (object userData)
				{
						Type type = (Type)userData;


						//or use
						//Activator.CreateInstance ((Type)userData);
						

		
						if (typesCustom.IndexOf (type) < 0) {
								typesCustom.Add (type);

						}

				
						AddVariableToList ("New " + type.Name, FormatterServices.GetUninitializedObject (type), __variablesReordableList);


				}

				public override void OnInspectorGUI ()
				{

						base.DrawDefaultInspector ();

						serializedObject.Update ();

						__variablesReordableList.DoLayoutList ();

						serializedObject.ApplyModifiedProperties ();
				}
		}
}