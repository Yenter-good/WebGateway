using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Service.Common
{
    public static class ObjectExtend
    {
        public static string GetEnumDescription<TEnum>(this object obj, TEnum value) where TEnum : Enum
        {
            FieldInfo[] fields = typeof(TEnum).GetFields();
            for (int i = 1, count = fields.Length; i < count; i++)
            {
                if (fields[i].Name == value.ToString())
                {
                    DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])fields[i].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (EnumAttributes.Length > 0)
                    {
                        return EnumAttributes[0].Description;
                    }
                }
            }
            return "";
        }
    }
}
