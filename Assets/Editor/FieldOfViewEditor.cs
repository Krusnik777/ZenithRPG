using UnityEditor;
using UnityEngine;

namespace DC_ARPG
{
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : Editor
    {
        private void OnSceneGUI()
        {
            FieldOfView fov = (FieldOfView)target;

            Vector3 pos = fov.transform.position;
            pos.y = 0.5f;

            Handles.color = Color.white;
            Handles.DrawWireArc(pos, Vector3.up, Vector3.forward, 360, fov.Radius);

            Vector3 viewAngle01 = GetDirectionFromAngle(fov.transform.eulerAngles.y, -fov.Angle / 2);
            Vector3 viewAngle02 = GetDirectionFromAngle(fov.transform.eulerAngles.y, fov.Angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(pos, pos + viewAngle01 * fov.Radius);
            Handles.DrawLine(pos, pos + viewAngle02 * fov.Radius);

            if (fov.CanSeePlayer)
            {
                Handles.color = Color.green;
                Handles.DrawLine(pos, fov.PlayerGameObject.transform.position);
            }
        }

        private Vector3 GetDirectionFromAngle(float eulerY, float angle)
        {
            angle += eulerY;

            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
        }
    }
}
