using Validtron.Builders;
using Validtron.Configurations;
using Validtron.Exceptions;

namespace Validtron.Extensions;

public static partial class ValidatorExtensions
{
    public static IRuleBuilder<T, TEnum> IsInEnum<T, TEnum>(
    this IRuleBuilder<T, TEnum> ruleBuilder,
    string? errorMessage = null)
    where TEnum : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);

        return ruleBuilder.Must(
            value => IsDefinedEnumValue(value),
            errorMessage ??
            ValidationMessages.Get("IsInEnum"));
    }

    public static IRuleBuilder<T, TEnum?> IsInEnum<T, TEnum>(
        this IRuleBuilder<T, TEnum?> ruleBuilder,
        string? errorMessage = null)
        where TEnum : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);

        return ruleBuilder.Must(
            value =>
                !value.HasValue ||
                IsDefinedEnumValue(value.Value),
            errorMessage ??
            ValidationMessages.Get("IsInEnum"));
    }

    private static bool IsDefinedEnumValue<TEnum>(TEnum value)
    where TEnum : struct, Enum
    {
        var enumType = typeof(TEnum);

        if (!enumType.IsDefined(typeof(FlagsAttribute), inherit: false))
        {
            return Enum.IsDefined(enumType, value);
        }

        var numericValue = ToUInt64Bits(value);

        if (numericValue == 0)
        {
            return Enum.IsDefined(enumType, value);
        }

        ulong allDefinedFlags = 0;

        foreach (var enumValue in Enum.GetValues<TEnum>())
        {
            allDefinedFlags |= ToUInt64Bits(enumValue);
        }

        return (numericValue & ~allDefinedFlags) == 0;
    }

    private static ulong ToUInt64Bits<TEnum>(TEnum value)
    where TEnum : struct, Enum
    {
        var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));

        return Type.GetTypeCode(underlyingType) switch
        {
            TypeCode.SByte =>
                unchecked((byte)Convert.ToSByte(value)),

            TypeCode.Byte =>
                Convert.ToByte(value),

            TypeCode.Int16 =>
                unchecked((ushort)Convert.ToInt16(value)),

            TypeCode.UInt16 =>
                Convert.ToUInt16(value),

            TypeCode.Int32 =>
                unchecked((uint)Convert.ToInt32(value)),

            TypeCode.UInt32 =>
                Convert.ToUInt32(value),

            TypeCode.Int64 =>
                unchecked((ulong)Convert.ToInt64(value)),

            TypeCode.UInt64 =>
                Convert.ToUInt64(value),

            _ => throw new UnsupportedEnumUnderlyingTypeException(underlyingType)
        };
    }
}
