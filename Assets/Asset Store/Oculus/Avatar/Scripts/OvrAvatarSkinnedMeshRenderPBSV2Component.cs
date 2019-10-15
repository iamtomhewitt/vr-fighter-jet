using UnityEngine;
using System.Collections;
using System;
using Oculus.Avatar;

public class OvrAvatarSkinnedMeshPBSV2RenderComponent : OvrAvatarRenderComponent
{
	public OvrAvatarMaterialManager AvatarMaterialManager;
	bool PreviouslyActive = false;
	bool IsCombinedMaterial = false;

	private readonly FingerBone Phalanges = new FingerBone(0.01f, 0.03f);
	private readonly FingerBone Metacarpals = new FingerBone(0.01f, 0.05f);

	internal void Initialize(IntPtr renderPart, ovrAvatarRenderPart_SkinnedMeshRenderPBS_V2 skinnedMeshRender, OvrAvatarMaterialManager materialManager, int thirdPersonLayer,
								int firstPersonLayer, int sortOrder, bool isCombinedMaterial, ovrAvatarAssetLevelOfDetail lod)
	{
		AvatarMaterialManager = materialManager;
		IsCombinedMaterial = isCombinedMaterial;

		mesh = CreateSkinnedMesh(	skinnedMeshRender.meshAssetID,
									skinnedMeshRender.visibilityMask,
									thirdPersonLayer,
									firstPersonLayer,
									sortOrder);

#if UNITY_ANDROID
        var singleComponentShader = "OvrAvatar/Avatar_Mobile_SingleComponent";
#else
		var singleComponentShader = "OvrAvatar/Avatar_PC_SingleComponent";
#endif

		var shader = IsCombinedMaterial ? Shader.Find("OvrAvatar/Avatar_Mobile_CombinedMesh") : Shader.Find(singleComponentShader);

		AvatarLogger.Log("Shader is: " + shader.name);

		mesh.sharedMaterial = CreateAvatarMaterial(gameObject.name + "_material", shader);
		mesh.sharedMaterial.renderQueue = OvrAvatarMaterialManager.RENDER_QUEUE;

		bones = mesh.bones;

		// Updated - added a collider for each of the finger joints
		foreach (Transform bone in bones)
		{
			if (!bone.name.Contains("ignore"))
			{
				CreateCollider(bone);
			}
		}

		if (IsCombinedMaterial)
		{
			AvatarMaterialManager.SetRenderer(mesh);
			InitializeCombinedMaterial(renderPart, (int)lod - 1);
			AvatarMaterialManager.OnCombinedMeshReady();
		}
	}

	/// <summary>
	/// Creates colliders for each of the finger bones, thanks to 
	/// http://www.rgbschemes.com/blog/oculus-touch-and-finger-stuff-part-2/
	/// </summary>
	private void CreateCollider(Transform t)
	{
		// Check that there is no collider, and that it is part of the hand bones
		if (!t.gameObject.GetComponent(typeof(CapsuleCollider)) &&
			!t.gameObject.GetComponent(typeof(SphereCollider)) &&
			t.name.Contains("hands"))
		{
			// Check its a finger tip (denoted by the number '3')
			if (t.name.Contains("thumb3") ||
				t.name.Contains("index3") ||
				t.name.Contains("middle3") ||
				t.name.Contains("ring3") ||
				t.name.Contains("pinky3"))
			{
				// Pinky finger has additional bone that we dont need, so check here
				if (!t.name.EndsWith("0"))
				{
					// Create the type of finger bone collider - "_l_" indicates left hand
					CapsuleCollider collider = t.gameObject.AddComponent<CapsuleCollider>();
					if (!t.name.EndsWith("1"))
					{
						collider.radius = Phalanges.Radius;
						collider.height = Phalanges.Height;
						collider.center = Phalanges.GetCenter(t.name.Contains("_l_"));
						collider.direction = 0;
						collider.isTrigger = true;
					}
					else
					{
						collider.radius = Metacarpals.Radius;
						collider.height = Metacarpals.Height;
						collider.center = Metacarpals.GetCenter(t.name.Contains("_l_"));
						collider.direction = 0;
						collider.isTrigger = true;
					}

					// Now create a tag for the finger
					t.tag = "VR Finger";
				}
				else if (transform.name.Contains("grip"))
				{
					SphereCollider collider = t.gameObject.AddComponent<SphereCollider>();
					collider.radius = 0.04f;
					collider.center = new Vector3(((t.name.Contains("_l_")) ? -1 : 1) * 0.01f, 0.01f, 0.02f);
					collider.isTrigger = true;
				}
			}
		}
	}

	public void UpdateSkinnedMeshRender( OvrAvatarComponent component, OvrAvatar avatar, IntPtr renderPart)
	{
		ovrAvatarVisibilityFlags visibilityMask = CAPI.ovrAvatarSkinnedMeshRenderPBSV2_GetVisibilityMask(renderPart);

		ovrAvatarTransform localTransform = CAPI.ovrAvatarSkinnedMeshRenderPBSV2_GetTransform(renderPart);

		UpdateSkinnedMesh(avatar, bones, localTransform, visibilityMask, renderPart);

		bool isActive = gameObject.activeSelf;

		if (mesh != null && !PreviouslyActive && isActive)
		{
			if (!IsCombinedMaterial)
			{
				InitializeSingleComponentMaterial(renderPart, (int)avatar.LevelOfDetail - 1);
			}
		}

		PreviouslyActive = isActive;
	}

	private void InitializeSingleComponentMaterial(IntPtr renderPart, int lodIndex)
	{
		ovrAvatarPBSMaterialState materialState = CAPI.ovrAvatarSkinnedMeshRenderPBSV2_GetPBSMaterialState(renderPart);

		int componentType = (int)OvrAvatarMaterialManager.GetComponentType(gameObject.name);

		var defaultProperties = AvatarMaterialManager.DefaultAvatarConfig.ComponentMaterialProperties;

		var diffuseTexture = OvrAvatarComponent.GetLoadedTexture(materialState.albedoTextureID);
		var normalTexture = OvrAvatarComponent.GetLoadedTexture(materialState.normalTextureID);
		var metallicTexture = OvrAvatarComponent.GetLoadedTexture(materialState.metallicnessTextureID);

		if (diffuseTexture == null)
		{
			diffuseTexture = AvatarMaterialManager.DiffuseFallbacks[lodIndex];
		}

		if (normalTexture == null)
		{
			normalTexture = AvatarMaterialManager.NormalFallbacks[lodIndex];
		}

		if (metallicTexture == null)
		{
			metallicTexture = AvatarMaterialManager.DiffuseFallbacks[lodIndex];
		}

		mesh.sharedMaterial.SetTexture(OvrAvatarMaterialManager.AVATAR_SHADER_MAINTEX, diffuseTexture);
		mesh.sharedMaterial.SetTexture(OvrAvatarMaterialManager.AVATAR_SHADER_NORMALMAP, normalTexture);
		mesh.sharedMaterial.SetTexture(OvrAvatarMaterialManager.AVATAR_SHADER_ROUGHNESSMAP, metallicTexture);

		mesh.sharedMaterial.SetVector(OvrAvatarMaterialManager.AVATAR_SHADER_COLOR, materialState.albedoMultiplier);

		mesh.sharedMaterial.SetFloat(OvrAvatarMaterialManager.AVATAR_SHADER_DIFFUSEINTENSITY, defaultProperties[componentType].DiffuseIntensity);

		mesh.sharedMaterial.SetFloat(OvrAvatarMaterialManager.AVATAR_SHADER_RIMINTENSITY, defaultProperties[componentType].RimIntensity);

		mesh.sharedMaterial.SetFloat(OvrAvatarMaterialManager.AVATAR_SHADER_BACKLIGHTINTENSITY, defaultProperties[componentType].BacklightIntensity);

		mesh.sharedMaterial.SetFloat(OvrAvatarMaterialManager.AVATAR_SHADER_REFLECTIONINTENSITY, defaultProperties[componentType].ReflectionIntensity);

		mesh.GetClosestReflectionProbes(AvatarMaterialManager.ReflectionProbes);
		if (AvatarMaterialManager.ReflectionProbes != null && AvatarMaterialManager.ReflectionProbes.Count > 0)
		{
			mesh.sharedMaterial.SetTexture(OvrAvatarMaterialManager.AVATAR_SHADER_CUBEMAP, AvatarMaterialManager.ReflectionProbes[0].probe.texture);
		}

#if UNITY_EDITOR
		mesh.sharedMaterial.EnableKeyword("FIX_NORMAL_ON");
#endif
		mesh.sharedMaterial.EnableKeyword("PBR_LIGHTING_ON");
	}

	private void InitializeCombinedMaterial(IntPtr renderPart, int lodIndex)
	{
		ovrAvatarPBSMaterialState[] materialStates = CAPI.ovrAvatar_GetBodyPBSMaterialStates(renderPart);

		if (materialStates.Length == (int)ovrAvatarBodyPartType.Count)
		{
			AvatarMaterialManager.CreateTextureArrays();

			AvatarMaterialManager.LocalAvatarConfig = AvatarMaterialManager.DefaultAvatarConfig;
			var localProperties = AvatarMaterialManager.LocalAvatarConfig.ComponentMaterialProperties;

			AvatarLogger.Log("InitializeCombinedMaterial - Loading Material States");

			for (int i = 0; i < materialStates.Length; i++)
			{
				localProperties[i].TypeIndex = (ovrAvatarBodyPartType)i;
				localProperties[i].Color = materialStates[i].albedoMultiplier;

				var diffuse = OvrAvatarComponent.GetLoadedTexture(materialStates[i].albedoTextureID);
				var normal = OvrAvatarComponent.GetLoadedTexture(materialStates[i].normalTextureID);
				var roughness = OvrAvatarComponent.GetLoadedTexture(materialStates[i].metallicnessTextureID);

				localProperties[i].Textures[(int)OvrAvatarMaterialManager.TextureType.DiffuseTextures] = diffuse == null ? AvatarMaterialManager.DiffuseFallbacks[lodIndex] : diffuse;
				localProperties[i].Textures[(int)OvrAvatarMaterialManager.TextureType.NormalMaps] = normal == null ? AvatarMaterialManager.NormalFallbacks[lodIndex] : normal;
				localProperties[i].Textures[(int)OvrAvatarMaterialManager.TextureType.RoughnessMaps] = roughness == null ? AvatarMaterialManager.DiffuseFallbacks[lodIndex] : roughness;

				AvatarLogger.Log(localProperties[i].TypeIndex.ToString());
				AvatarLogger.Log(AvatarLogger.Tab + "Diffuse: " + materialStates[i].albedoTextureID);
				AvatarLogger.Log(AvatarLogger.Tab + "Normal: " + materialStates[i].normalTextureID);
				AvatarLogger.Log(AvatarLogger.Tab + "Metallic: " + materialStates[i].metallicnessTextureID);
			}

			AvatarMaterialManager.ValidateTextures();
		}

#if UNITY_EDITOR
		mesh.sharedMaterial.EnableKeyword("FIX_NORMAL_ON");
#endif
	}

	/// <summary>
	/// Holds information about finger joints to be used when constructing colliders for the joints.
	/// </summary>
	private struct FingerBone
	{
		public readonly float Radius;
		public readonly float Height;

		public FingerBone(float radius, float height)
		{
			Radius = radius;
			Height = height;
		}

		public Vector3 GetCenter(bool isLeftHand)
		{
			return new Vector3(((isLeftHand) ? -1 : 1) * Height / 2.0f, 0, 0);
		}
	};
}
