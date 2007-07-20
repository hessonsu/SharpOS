using System;
using SharpOS.AOT.X86;

namespace SharpOS
{
	public static class MemUtils
	{
		public static unsafe void MemSet32(uint value, uint dst, uint count)
		{
			Asm.MOV(R32.ECX, &count);
			Asm.MOV(R32.EAX, &value);
			Asm.MOV(R32.EDI, &dst);
			Asm.REP();
			Asm.STOSD();
		}

		public static unsafe void MemCopy32(uint src, uint dst, uint count)
		{
			Asm.MOV(R32.ECX, &count);
			Asm.MOV(R32.ESI, &src);
			Asm.MOV(R32.EDI, &dst);
			Asm.REP();
			Asm.MOVSD();
		}
	}
}
