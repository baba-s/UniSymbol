﻿using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Kogane.Internal
{
	/// <summary>
	/// UniSymbolSettings の Inspector の表示を変更するエディタ拡張
	/// </summary>
	[CustomEditor( typeof( UniSymbolSettings ) )]
	internal sealed class UniSymbolSettingsInspector : Editor
	{
		//==============================================================================
		// 変数
		//==============================================================================
		private SerializedProperty m_property;
		private ReorderableList    m_reorderableList;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 有効になった時に呼び出されます
		/// </summary>
		private void OnEnable()
		{
			m_property = serializedObject.FindProperty( "m_list" );

			m_reorderableList = new ReorderableList( serializedObject, m_property )
			{
				onAddCallback       = OnAdd,
				drawElementCallback = OnDrawElement,
			};
		}

		/// <summary>
		/// 無効になった時に呼び出されます
		/// </summary>
		private void OnDisable()
		{
			m_property        = null;
			m_reorderableList = null;
		}

		/// <summary>
		/// 要素を追加する時に呼び出されます
		/// </summary>
		private void OnAdd( ReorderableList list )
		{
			var index = m_property.arraySize;

			m_property.InsertArrayElementAtIndex( index );

			var property        = m_property.GetArrayElementAtIndex( index );
			var nameProperty    = property.FindPropertyRelative( "m_name" );
			var commentProperty = property.FindPropertyRelative( "m_comment" );
			var colorProperty   = property.FindPropertyRelative( "m_color" );

			nameProperty.stringValue    = string.Empty;
			commentProperty.stringValue = string.Empty;
			colorProperty.colorValue    = Color.white;
		}

		/// <summary>
		/// リストの要素を描画する時に呼び出されます
		/// </summary>
		private void OnDrawElement
		(
			Rect rect,
			int  index,
			bool isActive,
			bool isFocused
		)
		{
			var element = m_property.GetArrayElementAtIndex( index );
			rect.height -= 4;
			rect.y      += 2;
			EditorGUI.PropertyField( rect, element );
		}

		/// <summary>
		/// GUI を表示する時に呼び出されます
		/// </summary>
		public override void OnInspectorGUI()
		{
			if ( GUILayout.Button( "Open UniSymbol" ) )
			{
				UniSymbolWindow.Open();
			}

			serializedObject.Update();
			m_reorderableList.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}
	}
}