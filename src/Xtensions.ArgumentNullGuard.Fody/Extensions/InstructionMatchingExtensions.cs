namespace Xtensions.ArgumentNullGuard.Fody.Extensions
{
    using System.Diagnostics.CodeAnalysis;
    using Mono.Cecil;
    using Mono.Cecil.Cil;

    public static class InstructionMatchingExtensions
    {
        public static bool IsLoadLocal(this Instruction instruction, VariableDefinition variable)
        {
            return instruction.OpCode == OpCodes.Ldloc && instruction.Operand == variable;
        }

        public static bool IsLoadInt32Constant(this Instruction instruction, int value)
        {
            return instruction.OpCode == OpCodes.Ldc_I4 && (int)instruction.Operand == value;
        }

        [ExcludeFromCodeCoverage] // Never called with wrong field.
        public static bool IsSetField(this Instruction instruction, string fieldName)
        {
            return instruction.OpCode == OpCodes.Stfld && (instruction.Operand as FieldReference)!.Name == fieldName;
        }

        public static bool IsBranchFalse(this Instruction instruction)
        {
            return instruction.OpCode == OpCodes.Brfalse;
        }
    }
}
