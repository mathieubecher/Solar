using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.1]")]
	public partial class PlatformNetworkObject : NetworkObject
	{
		public const int IDENTITY = 6;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private float _progress;
		public event FieldEvent<float> progressChanged;
		public InterpolateFloat progressInterpolation = new InterpolateFloat() { LerpT = 0.1f, Enabled = true };
		public float progress
		{
			get { return _progress; }
			set
			{
				// Don't do anything if the value is the same
				if (_progress == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_progress = value;
				hasDirtyFields = true;
			}
		}

		public void SetprogressDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_progress(ulong timestep)
		{
			if (progressChanged != null) progressChanged(_progress, timestep);
			if (fieldAltered != null) fieldAltered("progress", _progress, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			progressInterpolation.current = progressInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _progress);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_progress = UnityObjectMapper.Instance.Map<float>(payload);
			progressInterpolation.current = _progress;
			progressInterpolation.target = _progress;
			RunChange_progress(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _progress);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (progressInterpolation.Enabled)
				{
					progressInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					progressInterpolation.Timestep = timestep;
				}
				else
				{
					_progress = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_progress(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (progressInterpolation.Enabled && !progressInterpolation.current.UnityNear(progressInterpolation.target, 0.0015f))
			{
				_progress = (float)progressInterpolation.Interpolate();
				//RunChange_progress(progressInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public PlatformNetworkObject() : base() { Initialize(); }
		public PlatformNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public PlatformNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
