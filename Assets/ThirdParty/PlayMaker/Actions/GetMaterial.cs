// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
// Thanks to: Giyomu
// http://hutonggames.com/playmakerforum/index.php?topic=400.0

using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory(ActionCategory.Material)]
[Tooltip("Get a material at index on a gameObject and store it in a variable")]
public class GetMaterial : FsmStateAction
{
	[RequiredField]
	[CheckForComponent(typeof(Renderer))]
	public FsmOwnerDefault gameObject;
	public FsmInt materialIndex;
	[RequiredField]
	[UIHint(UIHint.Variable)]
	public FsmMaterial material;
	[Tooltip("Get the shared material of this object. NOTE: Modifying the shared material will change the appearance of all objects using this material, and change material settings that are stored in the project too.")]
	public bool getSharedMaterial;

	public override void Reset()
	{
		gameObject = null;
		material = null;
		materialIndex = 0;
		getSharedMaterial = false;
	}

	public override void OnEnter ()
	{
		DoGetMaterial();
		Finish();
	}
	
	void DoGetMaterial()
	{
		var go = Fsm.GetOwnerDefaultTarget(gameObject);
		if (go == null)
		{
			return;
		}

		if (go.renderer == null)
		{
			LogError("Missing Renderer!");
			return;
		}
		
		if (materialIndex.Value == 0 && !getSharedMaterial)
		{
			material.Value = go.renderer.material;
		}
		
		else if(materialIndex.Value == 0 && getSharedMaterial)
		{
			material.Value = go.renderer.sharedMaterial;
		}
	
		else if (go.renderer.materials.Length > materialIndex.Value && !getSharedMaterial)
		{
			var materials = go.renderer.materials;
			material.Value = materials[materialIndex.Value];
			go.renderer.materials = materials;
		}

		else if (go.renderer.materials.Length > materialIndex.Value && getSharedMaterial)
		{
			var materials = go.renderer.sharedMaterials;
			material.Value = materials[materialIndex.Value];
			go.renderer.sharedMaterials = materials;
		}
	}
}
