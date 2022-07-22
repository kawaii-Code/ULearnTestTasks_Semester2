using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		private Dictionary<char, Action<IVirtualMachine>> _actions;

		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }

		public VirtualMachine(string program, int memorySize)
		{
			Instructions = program;
			InstructionPointer = 0;

			Memory = new byte[memorySize];
			MemoryPointer = 0;

			_actions = new Dictionary<char, Action<IVirtualMachine>>();
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			_actions.Add(symbol, execute);
		}

		public void Run()
		{
			for (; InstructionPointer < Instructions.Length; InstructionPointer++)
                ExecuteCommand(Instructions[InstructionPointer]);			
		}

		private void ExecuteCommand(char command)
        {
			if (!_actions.ContainsKey(command))			
				return; //exception would be better

			_actions[ command ](this);
        }
	}
}