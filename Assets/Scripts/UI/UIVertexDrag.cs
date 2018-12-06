using UnityEngine;

namespace UI {
    public class UIVertexDrag : UIDragObject {
	Vector3 mTargetPos;
        
	void Move (Vector3 worldDelta)
	{
		if (panelRegion != null)
		{
			Debug.Log("Moving");
			mTargetPos += worldDelta;
			Transform parent = target.parent;
			Rigidbody rb = target.GetComponent<Rigidbody>();

			if (parent != null)
			{
				Vector3 after = parent.worldToLocalMatrix.MultiplyPoint3x4(mTargetPos);
				after.x = Mathf.Round(after.x);
				after.y = Mathf.Round(after.y);

				if (rb != null)
				{
					// With a lot of colliders under the rigidbody, moving the transform causes some crazy overhead.
					// Moving the rigidbody is much cheaper, but it does seem to have a side effect of causing
					// widgets to detect movement relative to the panel, when in fact they should not be moving.
					// This is why it's best to keep the panel as 'static' if at all possible.
					// NOTE: Immediate constraints will also fail with a rigidbody because transform doesn't get updated.
					// It is strongly not advisable to have a rigidbody in this case.
					after = parent.localToWorldMatrix.MultiplyPoint3x4(after);
					rb.position = after;
#if UNITY_EDITOR
					if (restrictWithinPanel && dragEffect != DragEffect.MomentumAndSpring)
						Debug.LogWarning("Constraining doesn't work properly when there is a rigidbody present because rigidbodies move in FixedUpdate, not Update.\n" +
							"Please remove it, or use the MomentumAndSpring type drag effect.", rb);
#endif
				}
				else target.localPosition = after;
			}
			else if (rb != null)
			{
				rb.position = mTargetPos;
			}
			else target.position = mTargetPos;

			UIScrollView ds = panelRegion.GetComponent<UIScrollView>();
			if (ds != null) ds.UpdateScrollbars(true);
		}
		else target.position += worldDelta;
	}
    }
}