using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace CsvHelper.FastDynamicReader
{
    internal sealed class CsvRecordMetaObject : DynamicMetaObject
    {
        private static readonly MethodInfo _getValueMethod = typeof(IDictionary<string, object>).GetProperty("Item").GetGetMethod();
        private static readonly MethodInfo _setValueMethod = typeof(CsvRecord).GetMethod("SetValue", new[] { typeof(string), typeof(object) });

        public CsvRecordMetaObject(Expression expression, BindingRestrictions restrictions, object value)
            : base(expression, restrictions, value)
        {
        }

        private DynamicMetaObject CallMethod(MethodInfo method, Expression[] parameters)
        {
            var callMethod = new DynamicMetaObject(
                Expression.Call(Expression.Convert(Expression, LimitType), method, parameters),
                BindingRestrictions.GetTypeRestriction(Expression, LimitType)
            );

            return callMethod;
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            var parameters = new Expression[]
            {
                Expression.Constant(binder.Name)
            };

            return CallMethod(_getValueMethod, parameters);
        }

        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            var parameters = new Expression[]
            {
                Expression.Constant(binder.Name)
            };

            return CallMethod(_getValueMethod, parameters);
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            var parameters = new Expression[]
            {
                Expression.Constant(binder.Name),
                value.Expression,
            };

            return CallMethod(_setValueMethod, parameters);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return HasValue && Value is IDictionary<string, object> lookup ? lookup.Keys : Array.Empty<string>();
        }
    }
}