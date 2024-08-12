using System;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace CustomAircraftTemplateJAS39.Patches.TranspilerStuff
{
    public static class CodeInstructionExtensions
    {

    	/// <summary>Returns if an <see cref="OpCode"/> is initialized and valid</summary>
    	/// <param name="code">The <see cref="OpCode"/></param>
    	/// <returns></returns>
    	public static bool IsValid(this OpCode code) => code.Size > 0;

    	/// <summary>Shortcut for testing whether the operand is equal to a non-null value</summary>
    	/// <param name="code">The <see cref="CodeInstruction"/></param>
    	/// <param name="value">The value</param>
    	/// <returns>True if the operand has the same type and is equal to the value</returns>
    	///


    	/// <summary>Tests if the code instruction calls the method/constructor</summary>
    	/// <param name="code">The <see cref="CodeInstruction"/></param>
    	/// <param name="method">The method</param>
    	/// <returns>True if the instruction calls the method or constructor</returns>
    	///
    	public static bool Calls(this CodeInstruction code, MethodInfo method)
    	{
    		if (method is null) throw new ArgumentNullException(nameof(method));
    		if (code.opcode != OpCodes.Call && code.opcode != OpCodes.Callvirt) return false;
    		return Equals(code.operand, method);
    	}

    	/// <summary>Tests if the code instruction loads an integer constant</summary>
    	/// <param name="code">The <see cref="CodeInstruction"/></param>
    	/// <param name="number">The integer constant</param>
    	/// <returns>True if the instruction loads the constant</returns>
    	///
    	public static bool LoadsConstant(this CodeInstruction code, long number)
    	{
    		var op = code.opcode;
    		if (number == -1 && op == OpCodes.Ldc_I4_M1) return true;
    		if (number == 0 && op == OpCodes.Ldc_I4_0) return true;
    		if (number == 1 && op == OpCodes.Ldc_I4_1) return true;
    		if (number == 2 && op == OpCodes.Ldc_I4_2) return true;
    		if (number == 3 && op == OpCodes.Ldc_I4_3) return true;
    		if (number == 4 && op == OpCodes.Ldc_I4_4) return true;
    		if (number == 5 && op == OpCodes.Ldc_I4_5) return true;
    		if (number == 6 && op == OpCodes.Ldc_I4_6) return true;
    		if (number == 7 && op == OpCodes.Ldc_I4_7) return true;
    		if (number == 8 && op == OpCodes.Ldc_I4_8) return true;
    		if (op != OpCodes.Ldc_I4 && op != OpCodes.Ldc_I4_S && op != OpCodes.Ldc_I8) return false;
    		return Convert.ToInt64(code.operand) == number;
    	}

    	/// <summary>Tests if the code instruction loads a floating point constant</summary>
    	/// <param name="code">The <see cref="CodeInstruction"/></param>
    	/// <param name="number">The floating point constant</param>
    	/// <returns>True if the instruction loads the constant</returns>
    	///
    	public static bool LoadsConstant(this CodeInstruction code, double number)
    	{
    		if (code.opcode != OpCodes.Ldc_R4 && code.opcode != OpCodes.Ldc_R8) return false;
    		var val = Convert.ToDouble(code.operand);
    		return val == number;
    	}

    	/// <summary>Tests if the code instruction loads an enum constant</summary>
    	/// <param name="code">The <see cref="CodeInstruction"/></param>
    	/// <param name="e">The enum</param>
    	/// <returns>True if the instruction loads the constant</returns>
    	///
    	public static bool LoadsConstant(this CodeInstruction code, Enum e) => code.LoadsConstant(Convert.ToInt64(e));

    	/// <summary>Tests if the code instruction loads a string constant</summary>
    	/// <param name="code">The <see cref="CodeInstruction"/></param>
    	/// <param name="str">The string</param>
    	/// <returns>True if the instruction loads the constant</returns>
    	///
    	public static bool LoadsConstant(this CodeInstruction code, string str)
    	{
    		if (code.opcode != OpCodes.Ldstr) return false;
    		var val = Convert.ToString(code.operand);
    		return val == str;
    	}

    	/// <summary>Tests if the code instruction loads a field</summary>
    	/// <param name="code">The <see cref="CodeInstruction"/></param>
    	/// <param name="field">The field</param>
    	/// <param name="byAddress">Set to true if the address of the field is loaded</param>
    	/// <returns>True if the instruction loads the field</returns>
    	///
    	public static bool LoadsField(this CodeInstruction code, FieldInfo field, bool byAddress = false)
    	{
    		if (field is null) throw new ArgumentNullException(nameof(field));
    		var ldfldCode = field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld;
    		if (byAddress is false && code.opcode == ldfldCode && Equals(code.operand, field)) return true;
    		var ldfldaCode = field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda;
    		if (byAddress is true && code.opcode == ldfldaCode && Equals(code.operand, field)) return true;
    		return false;
    	}

    	/// <summary>Tests if the code instruction stores a field</summary>
    	/// <param name="code">The <see cref="CodeInstruction"/></param>
    	/// <param name="field">The field</param>
    	/// <returns>True if the instruction stores this field</returns>
    	///
    	public static bool StoresField(this CodeInstruction code, FieldInfo field)
    	{
    		if (field is null) throw new ArgumentNullException(nameof(field));
    		var stfldCode = field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld;
    		return code.opcode == stfldCode && Equals(code.operand, field);
    	}
    }
}
