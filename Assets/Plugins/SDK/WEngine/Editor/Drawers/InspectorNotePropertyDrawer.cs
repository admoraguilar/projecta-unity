using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using WEngine;

/*
    Author: Admor Aguilar
    Twitter: @_wateeeeeeeer
    Email: aguilar.water@gmail.com

    CLASS DESCRIPTION:
    <Insert class description here...>
*/

namespace WEditor {
    [CustomPropertyDrawer(typeof(InspectorNoteAttribute))]
    public class InspectorNotePropertyDrawer : DecoratorDrawer {
        public override float GetHeight() {
            GUISkin inspectorNoteStyle = WEditorResources.GetSkinInspectorNote();
            InspectorNoteAttribute note = attribute as InspectorNoteAttribute;

            float headerHeight = inspectorNoteStyle.customStyles[0].CalcHeight(new GUIContent(note.Header), EditorGUIUtility.currentViewWidth - 33f) + inspectorNoteStyle.customStyles[0].fontSize;
            float messageHeight = inspectorNoteStyle.customStyles[1].CalcHeight(new GUIContent(note.Message), EditorGUIUtility.currentViewWidth - 33f) + inspectorNoteStyle.customStyles[1].fontSize;

            return (string.IsNullOrEmpty(note.Message) ? 0 : 0 + messageHeight) + headerHeight;
        }

        public override void OnGUI(Rect position) {
            GUISkin inspectorNoteStyle = WEditorResources.GetSkinInspectorNote();
            InspectorNoteAttribute note = attribute as InspectorNoteAttribute;

            // Background box
            Rect posBgBox = position;
            posBgBox.height -= 5f;
            posBgBox.y += 5f;
            GUI.Box(posBgBox, "", inspectorNoteStyle.customStyles[2]);

            float headerHeight = inspectorNoteStyle.customStyles[0].CalcHeight(new GUIContent(note.Header), position.width);
            //float messageHeight = inspectorNoteStyle.customStyles[1].CalcHeight(new GUIContent(note.Message), position.width); // not used

            // our header is always present
            Rect posLabel = position;
            posLabel.y += 13;
            posLabel.x += 5f;
            posLabel.height += 13;
            EditorGUI.LabelField(posLabel, ">" + note.Header, inspectorNoteStyle.customStyles[0]);

            // do we have a message too?
            if(!string.IsNullOrEmpty(note.Message)) {
                //Color color = GUI.color;
                //Color faded = color;
                //faded.a = 0.5f;

                Rect posExplain = posLabel;
                posExplain.y += headerHeight;
                posExplain.width -= 13f;

                //GUI.color = faded;
                EditorGUI.LabelField(posExplain, note.Message, inspectorNoteStyle.customStyles[1]);
                //GUI.color = color;
            }

            //Rect posLine = position;
            //posLine.y += (string.IsNullOrEmpty(note.message) ? 17 : 17 + messageHeight) + headerHeight;
            //posLine.x += 0;
            //posLine.height = 2;
            //GUI.Box(posLine, "");
        }
    }
}