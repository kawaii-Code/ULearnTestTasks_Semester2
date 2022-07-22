using System.Collections.Generic;

namespace func.brainfuck
{
	class BracketPositiongHolder
    {
		private Dictionary<int, int> _closingBracketPosByOpeningBracket;
		private Dictionary<int, int> _openingBracketPosByClosingBracket;

		public BracketPositiongHolder()
        {
			_closingBracketPosByOpeningBracket = new Dictionary<int, int>();
			_openingBracketPosByClosingBracket = new Dictionary<int, int>();
		}

		public int GetLoopEnd(int openingBracket)
        {
			return _closingBracketPosByOpeningBracket[ openingBracket ];
        }

		public int GetLoopBegin(int closingBracket)
        {
			return _openingBracketPosByClosingBracket[ closingBracket ];
        }

		public void PrepareBracketData(string program)
		{
			Stack<int> openingBracketPositions = new Stack<int>();
			for (int i = 0; i < program.Length; i++)
			{
				switch (program[ i ])
				{
					case '[':
					{
						openingBracketPositions.Push(i);
						break;
					}
					case ']':
					{
						int openingPos = openingBracketPositions.Pop();
						_closingBracketPosByOpeningBracket.Add(openingPos, i);
						_openingBracketPosByClosingBracket.Add(i, openingPos);
						break;
					}
				}
			}
		}
	}

	public class BrainfuckLoopCommands
	{
		public static void RegisterTo(IVirtualMachine vm)
		{
			BracketPositiongHolder holder = new BracketPositiongHolder();
			holder.PrepareBracketData(vm.Instructions);

			vm.RegisterCommand('[', b =>
            {
				if (b.Memory[ b.MemoryPointer ] == 0)
					b.InstructionPointer = holder.GetLoopEnd(b.InstructionPointer);
            } );

			vm.RegisterCommand(']', b => 
			{
				if (b.Memory[ b.MemoryPointer ] != 0)
					b.InstructionPointer = holder.GetLoopBegin(b.InstructionPointer);
			} );
		}
	}
}