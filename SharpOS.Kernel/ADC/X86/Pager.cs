/*
 * SharpOS.ADC.X86/PagerImpl.cs
 * N:SharpOS.ADC
 *
 * (C) 2007 William Lahti. This software is licensed under the terms of the
 * GNU General Public License.
 *
 * Author: William Lahti <xfurious@gmail.com>
 *
 */

using SharpOS;
using SharpOS.AOT.X86;
using SharpOS.Memory;
using ADC = SharpOS.ADC;

namespace SharpOS.ADC.X86 {
	public unsafe class Pager {
	
		#region Global State

		private static uint *PageDirectory;	// page directory
		private static uint *PageTables;	// page tables

		#endregion
		#region Enumerations
		
		[System.Flags]
		private enum PageAttr: uint {
			Present = 1,
			ReadWrite = (1<<1),
			User = (1<<2),
			Accessed = (1<<5),
			Dirty = (1<<6),
			
			A1 = (1<<9),
			A2 = (1<<10),
			A3 = (1<<11),
			
			FrameMask = 0xFFFFF000,
			AttributeMask = 0x00000FFF
		}

		#endregion
		#region AOT Stubs

		[SharpOS.AOT.Attributes.String]
		private static byte *_ (string s)
		{
			return null;
		}

		#endregion
		#region Implementation Details
		
		private static PageAttr GetNativePMA (PageAttributes attr)
		{
			if ((uint)attr == 0xFFFFFFFF)
				return (PageAttr)attr;
			
			PageAttr attrMask = 0;
			
			if ((attr & PageAttributes.ReadWrite) != 0)
				attrMask |= PageAttr.ReadWrite;
				
			if ((attr & PageAttributes.User) != 0)
				attrMask |= PageAttr.User;
				
			if ((attr & PageAttributes.Present) != 0)
				attrMask |= PageAttr.Present;
		
			return attrMask;
		}
		
		private static PageAttributes GetAbstractPMA (PageAttr attr)
		{
			if ((uint) attr == 0xFFFFFFFF)
				return (PageAttributes)attr;
				
			PageAttributes ret = PageAttributes.None;
			
			if ((attr & PageAttr.ReadWrite) != 0)
				ret |= PageAttributes.ReadWrite;
				
			if ((attr & PageAttr.User) != 0)
				ret |= PageAttributes.User;
				
			if ((attr & PageAttr.Present) != 0)
				ret |= PageAttributes.Present;
			
			return ret;
		}
		
		private static uint ReadCR0()
		{
			uint val = 0;
			
			Asm.PUSH (R32.EAX);
				Asm.MOV (R32.EAX, CR.CR0);
				Asm.MOV (&val, R32.EAX);
			Asm.POP (R32.EAX);
			
			return val;
		}
		
		private static void *ReadCR3()
		{
			uint val = 0;
			
			Asm.PUSH (R32.EAX);
				Asm.MOV (R32.EAX, CR.CR3);
				Asm.MOV (&val, R32.EAX);
			Asm.POP (R32.EAX);
			
			return (void*)val;
		}
		
		private unsafe static void WriteCR0 (uint value)
		{
			Asm.PUSH (R32.EAX);
				Asm.MOV (R32.EAX, &value);
				Asm.MOV (CR.CR0, R32.EAX);
			Asm.POP (R32.EAX);
		}
		
		private static void WriteCR3 (uint ptr)
		{
			Asm.PUSH (R32.EAX);
				Asm.MOV (R32.EAX, &ptr);
				Asm.MOV (CR.CR3, R32.EAX);
			Asm.POP (R32.EAX);
		}
		
		private static void PagePtrToTables (void *page, uint *ret_pde, uint *ret_pte)
		{
			*ret_pde = (uint)page / 4194304;
			*ret_pte = ((uint)page - (*ret_pde * 4194304)) / 4096;
		}
		
		private static uint ComputeControlReq (uint totalMem)
		{
			PageAllocator.Errors err;
			
			return (totalMem / (GetGranularitySize (0, &err) / 1024) / 1024 + 1);
		}

		#endregion
		#region ADC Stub Implementations
		
		public static uint GetGranularitySize (uint granularity, PageAllocator.Errors *ret_err)
		{
			if (granularity < 0 || granularity > 1) {
				*ret_err = PageAllocator.Errors.UnsupportedGranularity;
				return 0;
			}

			*ret_err = PageAllocator.Errors.Success;
			
			switch (granularity) {
			case 0:
				return 4096;
			case 1:
				return 131072;
			default:
				*ret_err = PageAllocator.Errors.UnsupportedGranularity;
				return 0xFFFFFFFF;
			}
		}

		public static uint GetBigGranularity ()
		{
			return 1;
		}

		public static void GetMemoryRequirements (uint totalMem, PagingMemoryRequirements *req)
		{
			req->AtomicPages = ComputeControlReq (totalMem);
			req->Start = null;
			req->Error = PageAllocator.Errors.Success;
		}
		
		public static PageAllocator.Errors Init (uint totalMem, byte *pagemap, uint pagemapLen)
		{
			if (pagemapLen < ComputeControlReq (totalMem))
				return PageAllocator.Errors.UnusablePageControlBuffer;
			
			PageTables = (uint*)pagemap;
			PageDirectory = (uint*) (pagemap + (totalMem / 4 / 1024));
			
			uint addr = 0;
			byte *table = (byte*)PageTables;
			uint totalPages = totalMem / 4;
			
			// enough page tables to cover all existing pages
			
			for (int x = 0; x < (totalPages / 1024); ++x) {
				for (int i = 0; i < 1024; ++i) {
					uint val = addr | (uint)PageAttr.ReadWrite;
					
					if (x * 1024 * 4096 + i * 4096 <= totalMem * 1024)
						val = addr | (uint)PageAttr.Present;
						
					PageTables[i] = val;
					addr += 4096;
				}
				
				table += 1024;
			}
			
			// top-level page directory (level-1)
			
			table = (byte*)PageTables;
			
			for (uint i = 0; i < (totalPages / 1024); ++i) {
				PageDirectory[i] = (uint)table | (uint)(PageAttr.ReadWrite |
					PageAttr.Present);
				table += 1024;
			}
			
			for (uint i = (totalPages / 1024) + 1; i < 1024; ++i)
				PageDirectory[i] = 0;
		
			// Reserve memory below 0x100000 (1MB) for the BIOS/video memory
			
			uint ceil = 0x100000;
			byte *page = null;
			
			while ((uint)page < ceil) {
				PageAllocator.ReservePage (page);
				page += 4096;
			}

			return PageAllocator.Errors.Success;
		}
		
		public static PageAllocator.Errors Enable ()
		{
			uint value = ReadCR0 ();
			uint mod = (uint) CR0.PG;
			
			WriteCR3 ((uint) PageDirectory);
			WriteCR0 (value | mod);

			return PageAllocator.Errors.Success;
		}
		
		/**
			<summary>
				Changes the mapping of an individual page.
			</summary>
		*/
		public static PageAllocator.Errors MapPage (void *page, void *phys_page, uint granularity,
							PageAttributes attr)
		{
			uint nativeAttr = 0, pde = 0, pte = 0;
			uint *table = null;
			
			// validity checks

			Kernel.Assert (ADC.Pager.GetPointerGranularity (page) == granularity,
				_("X86.Pager::MapPage(): bad alignment on virtual page pointer"));

			Kernel.Assert (ADC.Pager.GetPointerGranularity (phys_page) == granularity,
				_("X86.Pager::MapPage(): bad alignment on physical page pointer"));
				
			Kernel.Assert (page == PageTables, 
				_("X86.Pager::MapPage(): tried to change mapping of the page table!"));
				
			Kernel.Assert (page == PageDirectory,
				_("X86.Pager::MapPage(): tried to change mapping of the page directory!"));
			
			Kernel.Assert (PageAllocator.IsPageReserved (page),
				_("X86.Pager::MapPage(): tried to change mapping on a reserved page."));

			// perform mapping
			
			nativeAttr = (uint) GetNativePMA (attr);
			table = PageDirectory;
			
			PagePtrToTables (page, &pde, &pte);
			
			if (granularity == 0)
				table = (uint*)(table [pde] & (uint)PageAttr.FrameMask);
			
			if (nativeAttr == 0xFFFFFFFF)
				nativeAttr = table[pte] & (uint)PageAttr.AttributeMask;

			// set our table entry to it's new target
			
			table[pte] = (uint)phys_page | nativeAttr;
			
			return PageAllocator.Errors.Success;
		}
		
		public static PageAllocator.Errors SetPageAttributes (void *page, uint granularity,
								      PageAttributes attr)
		{
			uint pde = 0, pte = 0;
			uint *table = null;
			uint nativeAttr = (uint)GetNativePMA (attr);

			if (granularity < 0 || granularity > 1)
				return PageAllocator.Errors.UnsupportedGranularity;
			
			Kernel.Assert (nativeAttr == 0xFFFFFFFF, 
				_("X86.Pager::SetPageAttributes(): bad page map attributes"));

			Kernel.Assert (ADC.Pager.GetPointerGranularity (page) == granularity,
				_("X86.Pager::SetPageAttributes(): bad alignment on page pointer"));
			
			PagePtrToTables (page, &pde, &pte);

			table = PageDirectory;

			if (granularity == 0)
				table = (uint*)(PageDirectory [pde] & (uint)PageAttr.FrameMask);
			
			table[pte] = (table [pte] & (uint)PageAttr.FrameMask) | nativeAttr;
			
			return PageAllocator.Errors.Success;
		}
		
		public static PageAttributes GetPageAttributes (void *page, uint granularity,
								      PageAllocator.Errors *ret_err)
		{
			uint pde = 0, pte = 0;
			uint *table = null;

			if (granularity < 0 || granularity > 1) {
				*ret_err = PageAllocator.Errors.UnsupportedGranularity;
				return PageAttributes.None;
			}
			
			Kernel.Assert (ADC.Pager.GetPointerGranularity(page) == granularity,
				_("X86.Pager.GetPageAttributes(): bad page pointer alignment!"));
			
			PagePtrToTables(page, &pde, &pte);
			table = PageDirectory;
			
			if (granularity == 0)
				table = (uint*)(table[pde] & (uint)PageAttr.FrameMask);

			*ret_err = PageAllocator.Errors.Success;
			return GetAbstractPMA ((PageAttr)(table[pde] &
					(uint)PageAttr.AttributeMask));
		}

		#endregion
	}
}