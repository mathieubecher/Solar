using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0]")]
	public partial class ControllerSunNetworkObject : NetworkObject
	{
		public const int IDENTITY = 3;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private float _life;
		public event FieldEvent<float> lifeChanged;
		public InterpolateFloat lifeInterpolation = new InterpolateFloat() { LerpT = 0f, Enabled = false };
		public float life
		{
			get { return _life; }
			set
			{
				// Don't do anything if the value is the same
				if (_life == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_life = value;
				hasDirtyFields = true;
			}
		}

		public void SetlifeDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_life(ulong timestep)
		{
			if (lifeChanged != null) lifeChanged(_life, timestep);
			if (fieldAltered != null) fieldAltered("life", _life, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			lifeInterpolation.current = lifeInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _life);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_life = UnityObjectMapper.Instance.Map<float>(payload);
			lifeInterpolation.current = _life;
			lifeInterpolation.target = _life;
			RunChange_life(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _life);

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
				if (lifeInterpolation.Enabled)
				{
					lifeInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					lifeInterpolation.Timestep = timestep;
				}
				else
				{
					_life = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_life(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (lifeInterpolation.Enabled && !lifeInterpolation.current.UnityNear(lifeInterpolation.target, 0.0015f))
			{
				_life = (float)lifeInterpolation.Interpolate();
				//RunChange_life(lifeInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public ControllerSunNetworkObject() : base() { Initialize(); }
		public ControllerSunNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public ControllerSunNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
