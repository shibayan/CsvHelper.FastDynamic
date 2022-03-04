using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace CsvHelper.FastDynamic;

// Thanks, from https://github.com/StackExchange/Dapper/blob/master/Dapper/SqlMapper.DapperRowMetaObject.cs
internal sealed class CsvRecordMetaObject : DynamicMetaObject
{
    private static readonly string[] s_emptyArray = Array.Empty<string>();

    private static readonly MethodInfo s_getValueMethod = typeof(IDictionary<string, object>).GetProperty("Item").GetGetMethod();
    private static readonly MethodInfo s_setValueMethod = typeof(CsvRecord).GetMethod(nameof(CsvRecord.SetValue), new[] { typeof(string), typeof(object) });

    public CsvRecordMetaObject(Expression expression, BindingRestrictions restrictions, object value)
        : base(expression, restrictions, value)
    {
    }

    public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
    {
        var parameters = new[]
        {
            indexes[0].Expression
        };

        return CallMethod(s_getValueMethod, parameters);
    }

    public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
    {
        var parameters = new Expression[]
        {
            Expression.Constant(binder.Name)
        };

        return CallMethod(s_getValueMethod, parameters);
    }

    public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
    {
        var parameters = new Expression[]
        {
            Expression.Constant(binder.Name)
        };

        return CallMethod(s_getValueMethod, parameters);
    }

    public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
    {
        var parameters = new[]
        {
            indexes[0].Expression,
            value.Expression
        };

        return CallMethod(s_setValueMethod, parameters);
    }

    public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
    {
        var parameters = new[]
        {
            Expression.Constant(binder.Name),
            value.Expression
        };

        return CallMethod(s_setValueMethod, parameters);
    }

    public override IEnumerable<string> GetDynamicMemberNames()
    {
        return HasValue && Value is IDictionary<string, object> lookup ? lookup.Keys : s_emptyArray;
    }

    private DynamicMetaObject CallMethod(MethodInfo method, Expression[] parameters)
    {
        var callMethod = new DynamicMetaObject(
            Expression.Call(Expression.Convert(Expression, LimitType), method, parameters),
            BindingRestrictions.GetTypeRestriction(Expression, LimitType)
        );

        return callMethod;
    }
}
