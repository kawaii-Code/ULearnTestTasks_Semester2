using System;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{		
			RegisterIOCommands(vm, read, write);
			RegisterBasicOperations(vm);			
			RegisterLetterCommands(vm);
		}

        #region Internal registration
        private static void RegisterBasicOperations(IVirtualMachine vm)
        {
			vm.RegisterCommand('+', b =>
			{
				b.Memory[ b.MemoryPointer ] =
					CompensateByteOverflow(b.Memory[ b.MemoryPointer ], x => x + 1);
			});
			vm.RegisterCommand('-', b =>
			{
				b.Memory[ b.MemoryPointer ] =
					CompensateByteOverflow(b.Memory[ b.MemoryPointer ], x => x - 1);
			});

			vm.RegisterCommand('>', b =>
			{
				b.MemoryPointer = ++b.MemoryPointer % b.Memory.Length;
			});
			vm.RegisterCommand('<', b =>
			{
				b.MemoryPointer = --b.MemoryPointer < 0 ? b.Memory.Length - 1 : b.MemoryPointer;
			});
		}

		private static void RegisterIOCommands(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
			vm.RegisterCommand('.', b => write((char) b.Memory[ b.MemoryPointer ]));
			vm.RegisterCommand(',', b => b.Memory[ b.MemoryPointer ] = (byte) read());
		}

		private static void RegisterLetterCommands(IVirtualMachine vm)
        {
			for (char c = 'a'; c <= 'z'; c++)
			{
				var lower = c;
				var upper = char.ToUpper(c);

				vm.RegisterCommand(lower, b => b.Memory[ b.MemoryPointer ] = (byte) lower);
				vm.RegisterCommand(upper, b => b.Memory[ b.MemoryPointer ] = (byte) upper);
			}

			for (char n = '0'; n <= '9'; n++)
			{
				var temp = n;

				vm.RegisterCommand(temp, b => b.Memory[ b.MemoryPointer ] = (byte) temp);
			}
		}
		#endregion

		private static byte CompensateByteOverflow(byte value, Func<byte, int> operation)
		{
			int operationResult = operation(value);

			if (operationResult == byte.MaxValue + 1)
				operationResult = byte.MinValue;
			else if (operationResult == byte.MinValue - 1)
				operationResult = byte.MaxValue;

			return (byte) operationResult;
		}
	}
}