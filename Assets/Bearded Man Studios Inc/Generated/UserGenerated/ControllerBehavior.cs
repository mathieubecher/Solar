using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"Quaternion\"][\"Vector3\"][\"float\"][\"Vector3\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"rotation\"][\"position\"][\"rotation\"][\"velocity\"]]")]
	public abstract partial class ControllerBehavior : NetworkBehavior
	{
		public const byte RPC_SET_ROTATE = 0 + 5;
		public const byte RPC_SET_POSITION = 1 + 5;
		public const byte RPC_SET_SUN_ROTATE = 2 + 5;
		public const byte RPC_SET_VELOCITY = 3 + 5;
		
		public ControllerNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (ControllerNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("SetRotate", SetRotate, typeof(Quaternion));
			networkObject.RegisterRpc("SetPosition", SetPosition, typeof(Vector3));
			networkObject.RegisterRpc("SetSunRotate", SetSunRotate, typeof(float));
			networkObject.RegisterRpc("SetVelocity", SetVelocity, typeof(Vector3));

			networkObject.onDestroy += DestroyGameObject;

			if (!obj.IsOwner)
			{
				if (!skipAttachIds.ContainsKey(obj.NetworkId)){
					uint newId = obj.NetworkId + 1;
					ProcessOthers(gameObject.transform, ref newId);
				}
				else
					skipAttachIds.Remove(obj.NetworkId);
			}

			if (obj.Metadata != null)
			{
				byte transformFlags = obj.Metadata[0];

				if (transformFlags != 0)
				{
					BMSByte metadataTransform = new BMSByte();
					metadataTransform.Clone(obj.Metadata);
					metadataTransform.MoveStartIndex(1);

					if ((transformFlags & 0x01) != 0 && (transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() =>
						{
							transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform);
							transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform);
						});
					}
					else if ((transformFlags & 0x01) != 0)
					{
						MainThreadManager.Run(() => { transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform); });
					}
					else if ((transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() => { transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform); });
					}
				}
			}

			MainThreadManager.Run(() =>
			{
				NetworkStart();
				networkObject.Networker.FlushCreateActions(networkObject);
			});
		}

		protected override void CompleteRegistration()
		{
			base.CompleteRegistration();
			networkObject.ReleaseCreateBuffer();
		}

		public override void Initialize(NetWorker networker, byte[] metadata = null)
		{
			Initialize(new ControllerNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new ControllerNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// Quaternion rotation
		/// </summary>
		public abstract void SetRotate(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// Vector3 position
		/// </summary>
		public abstract void SetPosition(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// float rotation
		/// </summary>
		public abstract void SetSunRotate(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// Vector3 velocity
		/// </summary>
		public abstract void SetVelocity(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}