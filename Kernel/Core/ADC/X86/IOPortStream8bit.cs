﻿// 
// (C) 2006-2007 The SharpOS Project Team (http://www.sharpos.org)
//
// Authors:
//	Sander van Rossen <sander.vanrossen@gmail.com>
//
// Licensed under the terms of the GNU GPL v3,
//  with Classpath Linking Exception for Libraries
//

using System;
using SharpOS.Kernel.DriverSystem;

namespace SharpOS.Kernel.ADC.X86 {

	public class IOPortStream8bit : IOPortStream {

		#region Constructor
		internal IOPortStream8bit(IO.Port _port) 
		{
			port = _port;
		}
		#endregion
		
		#region Port
		protected IO.Port	port;
		#endregion

		#region Read

		#region ReadSByte
		public sbyte	ReadSByte()
		{
			return IO.ReadSByte(port);
		}
		#endregion
		
		#region ReadByte
		public byte	ReadByte()
		{
			return IO.ReadByte(port);
		}
		#endregion
		
		#region ReadInt16
		public Int16	ReadInt16()
		{
			byte	value0 = IO.ReadByte(port);
			byte	value1 = IO.ReadByte(port);

			unchecked
			{
				return  (Int16)(
						((UInt16)value0 << 0) +
						((UInt16)value1 << 8));
			}
		}
		#endregion
		
		#region ReadUInt16
		public UInt16	ReadUInt16()
		{
			byte	value0 = IO.ReadByte(port);
			byte	value1 = IO.ReadByte(port);

			return  (UInt16)(
					((UInt16)value0 << 0) +
					((UInt16)value1 << 8));
		}
		#endregion
		
		#region ReadInt32
		public Int32	ReadInt32()
		{
			byte	value0 = IO.ReadByte(port);
			byte	value1 = IO.ReadByte(port);
			byte	value2 = IO.ReadByte(port);
			byte	value3 = IO.ReadByte(port);

			unchecked
			{
				return  (Int32)(
						((UInt32)value0 <<  0) +
						((UInt32)value1 <<  8) +
						((UInt32)value2 << 16) +
						((UInt32)value3 << 24));
			}
		}
		#endregion
		
		#region ReadUInt32
		public UInt32	ReadUInt32()
		{
			byte	value0 = IO.ReadByte(port);
			byte	value1 = IO.ReadByte(port);
			byte	value2 = IO.ReadByte(port);
			byte	value3 = IO.ReadByte(port);
			
			return  (UInt32)(
					((UInt32)value0 <<  0) +
					((UInt32)value1 <<  8) +
					((UInt32)value2 << 16) +
					((UInt32)value3 << 24));
		}
		#endregion
		
		#region ReadInt64
		public Int64	ReadInt64()
		{
			byte	value0 = IO.ReadByte(port);
			byte	value1 = IO.ReadByte(port);
			byte	value2 = IO.ReadByte(port);
			byte	value3 = IO.ReadByte(port);
			byte	value4 = IO.ReadByte(port);
			byte	value5 = IO.ReadByte(port);
			byte	value6 = IO.ReadByte(port);
			byte	value7 = IO.ReadByte(port);

			unchecked
			{
				return  (Int64)(
						((UInt64)value0 <<  0) +
						((UInt64)value1 <<  8) +
						((UInt64)value2 << 16) +
						((UInt64)value3 << 24) +
						((UInt64)value4 << 32) +
						((UInt64)value5 << 40) +
						((UInt64)value6 << 48) +
						((UInt64)value7 << 56));
			}
		}
		#endregion
		
		#region ReadUInt64
		public UInt64	ReadUInt64()
		{
			byte	value0 = IO.ReadByte(port);
			byte	value1 = IO.ReadByte(port);
			byte	value2 = IO.ReadByte(port);
			byte	value3 = IO.ReadByte(port);
			byte	value4 = IO.ReadByte(port);
			byte	value5 = IO.ReadByte(port);
			byte	value6 = IO.ReadByte(port);
			byte	value7 = IO.ReadByte(port);
			
			return  (UInt64)(
					((UInt64)value0 <<  0) +
					((UInt64)value1 <<  8) +
					((UInt64)value2 << 16) +
					((UInt64)value3 << 24) +
					((UInt64)value4 << 32) +
					((UInt64)value5 << 40) +
					((UInt64)value6 << 48) +
					((UInt64)value7 << 56));
		}
		#endregion

		#endregion
				
		#region Write
		
		#region Write SByte
		public void Write(sbyte value)
		{
			IO.WriteSByte(port, value);
		}
		#endregion
		
		#region Write Byte
		public void Write(byte value)
		{
			IO.WriteByte(port, value);
		}
		#endregion
		
		#region Write Int16
		public void Write(Int16 value)
		{
			unchecked
			{
				IO.WriteByte(port, (byte)(((UInt16)value      ) & 255));
				IO.WriteByte(port, (byte)(((UInt16)value >>  8) & 255));
			}
		}
		#endregion
		
		#region Write UInt16
		public void Write(UInt16 value)
		{
			IO.WriteByte(port, (byte)((value      ) & 255));
			IO.WriteByte(port, (byte)((value >>  8) & 255));
		}
		#endregion
		
		#region Write Int32
		public void Write(Int32 value)
		{
			unchecked
			{
				IO.WriteByte(port, (byte)(((UInt32)value      ) & 255));
				IO.WriteByte(port, (byte)(((UInt32)value >>  8) & 255));
				IO.WriteByte(port, (byte)(((UInt32)value >> 16) & 255));
				IO.WriteByte(port, (byte)(((UInt32)value >> 24) & 255));
			}
		}
		#endregion
		
		#region Write UInt32
		public void Write(UInt32 value)
		{
			IO.WriteByte(port, (byte)((value      ) & 255));
			IO.WriteByte(port, (byte)((value >>  8) & 255));
			IO.WriteByte(port, (byte)((value >> 16) & 255));
			IO.WriteByte(port, (byte)((value >> 24) & 255));
		}
		#endregion
		
		#region Write Int64
		public void Write(Int64 value)
		{
			unchecked
			{
				IO.WriteByte(port, (byte)(((UInt64)value      ) & 255));
				IO.WriteByte(port, (byte)(((UInt64)value >>  8) & 255));
				IO.WriteByte(port, (byte)(((UInt64)value >> 16) & 255));
				IO.WriteByte(port, (byte)(((UInt64)value >> 24) & 255));
				IO.WriteByte(port, (byte)(((UInt64)value >> 32) & 255));
				IO.WriteByte(port, (byte)(((UInt64)value >> 40) & 255));
				IO.WriteByte(port, (byte)(((UInt64)value >> 48) & 255));
				IO.WriteByte(port, (byte)(((UInt64)value >> 56) & 255));
			}
		}
		#endregion
		
		#region Write UInt64
		public void Write(UInt64 value)
		{
			IO.WriteByte(port, (byte)((value      ) & 255));
			IO.WriteByte(port, (byte)((value >>  8) & 255));
			IO.WriteByte(port, (byte)((value >> 16) & 255));
			IO.WriteByte(port, (byte)((value >> 24) & 255));
			IO.WriteByte(port, (byte)((value >> 32) & 255));
			IO.WriteByte(port, (byte)((value >> 40) & 255));
			IO.WriteByte(port, (byte)((value >> 48) & 255));
			IO.WriteByte(port, (byte)((value >> 56) & 255));
		}
		#endregion
		
		#region Write byte[]
		public void Write(byte[] buffer)
		{
			foreach(Byte value in buffer)
				IO.WriteByte(port, value);
		}
		#endregion
		
		#endregion
	}
}