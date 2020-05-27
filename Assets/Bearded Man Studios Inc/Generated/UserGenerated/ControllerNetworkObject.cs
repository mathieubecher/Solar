using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0,0,0.15,0,0]")]
	public partial class ControllerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 8;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private Vector3 _position;
		public event FieldEvent<Vector3> positionChanged;
		public InterpolateVector3 positionInterpolation = new InterpolateVector3() { LerpT = 0f, Enabled = false };
		public Vector3 position
		{
			get { return _position; }
			set
			{
				// Don't do anything if the value is the same
				if (_position == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_position = value;
				hasDirtyFields = true;
			}
		}

		public void SetpositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_position(ulong timestep)
		{
			if (positionChanged != null) positionChanged(_position, timestep);
			if (fieldAltered != null) fieldAltered("position", _position, timestep);
		}
		[ForgeGeneratedField]
		private Vector3 _velocity;
		public event FieldEvent<Vector3> velocityChanged;
		public InterpolateVector3 velocityInterpolation = new InterpolateVector3() { LerpT = 0f, Enabled = false };
		public Vector3 velocity
		{
			get { return _velocity; }
			set
			{
				// Don't do anything if the value is the same
				if (_velocity == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_velocity = value;
				hasDirtyFields = true;
			}
		}

		public void SetvelocityDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_velocity(ulong timestep)
		{
			if (velocityChanged != null) velocityChanged(_velocity, timestep);
			if (fieldAltered != null) fieldAltered("velocity", _velocity, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _rotation;
		public event FieldEvent<Quaternion> rotationChanged;
		public InterpolateQuaternion rotationInterpolation = new InterpolateQuaternion() { LerpT = 0.15f, Enabled = true };
		public Quaternion rotation
		{
			get { return _rotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_rotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_rotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetrotationDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_rotation(ulong timestep)
		{
			if (rotationChanged != null) rotationChanged(_rotation, timestep);
			if (fieldAltered != null) fieldAltered("rotation", _rotation, timestep);
		}
		[ForgeGeneratedField]
		private float _sunrotation;
		public event FieldEvent<float> sunrotationChanged;
		public InterpolateFloat sunrotationInterpolation = new InterpolateFloat() { LerpT = 0f, Enabled = false };
		public float sunrotation
		{
			get { return _sunrotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_sunrotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_sunrotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetsunrotationDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_sunrotation(ulong timestep)
		{
			if (sunrotationChanged != null) sunrotationChanged(_sunrotation, timestep);
			if (fieldAltered != null) fieldAltered("sunrotation", _sunrotation, timestep);
		}
		[ForgeGeneratedField]
		private int _typeserver;
		public event FieldEvent<int> typeserverChanged;
		public Interpolated<int> typeserverInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int typeserver
		{
			get { return _typeserver; }
			set
			{
				// Don't do anything if the value is the same
				if (_typeserver == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x10;
				_typeserver = value;
				hasDirtyFields = true;
			}
		}

		public void SettypeserverDirty()
		{
			_dirtyFields[0] |= 0x10;
			hasDirtyFields = true;
		}

		private void RunChange_typeserver(ulong timestep)
		{
			if (typeserverChanged != null) typeserverChanged(_typeserver, timestep);
			if (fieldAltered != null) fieldAltered("typeserver", _typeserver, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			positionInterpolation.current = positionInterpolation.target;
			velocityInterpolation.current = velocityInterpolation.target;
			rotationInterpolation.current = rotationInterpolation.target;
			sunrotationInterpolation.current = sunrotationInterpolation.target;
			typeserverInterpolation.current = typeserverInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _position);
			UnityObjectMapper.Instance.MapBytes(data, _velocity);
			UnityObjectMapper.Instance.MapBytes(data, _rotation);
			UnityObjectMapper.Instance.MapBytes(data, _sunrotation);
			UnityObjectMapper.Instance.MapBytes(data, _typeserver);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_position = UnityObjectMapper.Instance.Map<Vector3>(payload);
			positionInterpolation.current = _position;
			positionInterpolation.target = _position;
			RunChange_position(timestep);
			_velocity = UnityObjectMapper.Instance.Map<Vector3>(payload);
			velocityInterpolation.current = _velocity;
			velocityInterpolation.target = _velocity;
			RunChange_velocity(timestep);
			_rotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			rotationInterpolation.current = _rotation;
			rotationInterpolation.target = _rotation;
			RunChange_rotation(timestep);
			_sunrotation = UnityObjectMapper.Instance.Map<float>(payload);
			sunrotationInterpolation.current = _sunrotation;
			sunrotationInterpolation.target = _sunrotation;
			RunChange_sunrotation(timestep);
			_typeserver = UnityObjectMapper.Instance.Map<int>(payload);
			typeserverInterpolation.current = _typeserver;
			typeserverInterpolation.target = _typeserver;
			RunChange_typeserver(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _position);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _velocity);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _rotation);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _sunrotation);
			if ((0x10 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _typeserver);

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
				if (positionInterpolation.Enabled)
				{
					positionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					positionInterpolation.Timestep = timestep;
				}
				else
				{
					_position = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_position(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (velocityInterpolation.Enabled)
				{
					velocityInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					velocityInterpolation.Timestep = timestep;
				}
				else
				{
					_velocity = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_velocity(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (rotationInterpolation.Enabled)
				{
					rotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					rotationInterpolation.Timestep = timestep;
				}
				else
				{
					_rotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_rotation(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (sunrotationInterpolation.Enabled)
				{
					sunrotationInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					sunrotationInterpolation.Timestep = timestep;
				}
				else
				{
					_sunrotation = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_sunrotation(timestep);
				}
			}
			if ((0x10 & readDirtyFlags[0]) != 0)
			{
				if (typeserverInterpolation.Enabled)
				{
					typeserverInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					typeserverInterpolation.Timestep = timestep;
				}
				else
				{
					_typeserver = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_typeserver(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (positionInterpolation.Enabled && !positionInterpolation.current.UnityNear(positionInterpolation.target, 0.0015f))
			{
				_position = (Vector3)positionInterpolation.Interpolate();
				//RunChange_position(positionInterpolation.Timestep);
			}
			if (velocityInterpolation.Enabled && !velocityInterpolation.current.UnityNear(velocityInterpolation.target, 0.0015f))
			{
				_velocity = (Vector3)velocityInterpolation.Interpolate();
				//RunChange_velocity(velocityInterpolation.Timestep);
			}
			if (rotationInterpolation.Enabled && !rotationInterpolation.current.UnityNear(rotationInterpolation.target, 0.0015f))
			{
				_rotation = (Quaternion)rotationInterpolation.Interpolate();
				//RunChange_rotation(rotationInterpolation.Timestep);
			}
			if (sunrotationInterpolation.Enabled && !sunrotationInterpolation.current.UnityNear(sunrotationInterpolation.target, 0.0015f))
			{
				_sunrotation = (float)sunrotationInterpolation.Interpolate();
				//RunChange_sunrotation(sunrotationInterpolation.Timestep);
			}
			if (typeserverInterpolation.Enabled && !typeserverInterpolation.current.UnityNear(typeserverInterpolation.target, 0.0015f))
			{
				_typeserver = (int)typeserverInterpolation.Interpolate();
				//RunChange_typeserver(typeserverInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public ControllerNetworkObject() : base() { Initialize(); }
		public ControllerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public ControllerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
