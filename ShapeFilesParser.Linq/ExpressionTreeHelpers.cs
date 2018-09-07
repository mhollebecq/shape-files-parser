using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Linq
{
    internal class ExpressionTreeHelpers
    {
        internal static bool IsMemberEqualsValueExpression(Expression exp, Type declaringType, string memberName)
        {
            if (exp.NodeType != ExpressionType.Equal)
                return false;

            BinaryExpression be = (BinaryExpression)exp;

            // Assert. 
            if (ExpressionTreeHelpers.IsSpecificMemberExpression(be.Left, declaringType, memberName) &&
                ExpressionTreeHelpers.IsSpecificMemberExpression(be.Right, declaringType, memberName))
                throw new Exception("Cannot have 'member' == 'member' in an expression!");

            return (ExpressionTreeHelpers.IsSpecificMemberExpression(be.Left, declaringType, memberName) ||
                ExpressionTreeHelpers.IsSpecificMemberExpression(be.Right, declaringType, memberName));
        }

        internal static bool IsSpecificMemberExpression(Expression exp, Type declaringType, string memberName)
        {
            return ((exp is MemberExpression) &&
                (((MemberExpression)exp).Member.DeclaringType.Name == declaringType.Name) &&
                (((MemberExpression)exp).Member.Name == memberName));
        }

        internal static (TParam, TRet) GetValueFromMethodCallExpression<TParam, TRet>(BinaryExpression be,
            Type declaringType,
            string memberName,
            string methodName)
        {
            if (be.NodeType != ExpressionType.Equal)
                throw new Exception("There is a bug in this program.");

            if (be.Left.NodeType == ExpressionType.Call)
            {
                MethodCallExpression me = (MethodCallExpression)be.Left;
                if (((MemberExpression)me.Object).Member.Name == memberName 
                    && ((MemberExpression)me.Object).Expression.Type.Name == declaringType.Name)
                {
                    var parameter = GetValueFromExpression<TParam>(me.Arguments[0]);
                    var value = GetValueFromExpression<TRet>(be.Right);
                    return (parameter, value);
                }
            }
            else if (be.Right.NodeType == ExpressionType.Call)
            {
                MethodCallExpression me = (MethodCallExpression)be.Right;

                if (((MemberExpression)me.Object).Member.Name == memberName
                    && ((MemberExpression)me.Object).Expression.Type.Name == declaringType.Name)
                {
                    var parameter = GetValueFromExpression<TParam>(me.Arguments[0]);
                    var value = GetValueFromExpression<TRet>(be.Left);
                    return (parameter, value);
                }
            }

            // We should have returned by now. 
            throw new Exception("There is a bug in this program.");
        }

        internal static TRet GetValueFromEqualsExpression<TRet>(BinaryExpression be, Type memberDeclaringType, string memberName)
        {
            if (be.NodeType != ExpressionType.Equal)
                throw new Exception("There is a bug in this program.");

            if (be.Left.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression me = (MemberExpression)be.Left;

                if (me.Member.DeclaringType.Name == memberDeclaringType.Name && me.Member.Name == memberName)
                {
                    return GetValueFromExpression<TRet>(be.Right);
                }
            }
            else if (be.Right.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression me = (MemberExpression)be.Right;

                if (me.Member.DeclaringType.Name == memberDeclaringType.Name && me.Member.Name == memberName)
                {
                    return GetValueFromExpression<TRet>(be.Left);
                }
            }

            // We should have returned by now. 
            throw new Exception("There is a bug in this program.");
        }

        internal static bool IsMethodCallEqualsValueExpression(BinaryExpression be, 
            Type declaringType,
            string memberName, 
            string methodName)
        {
            if (be.NodeType != ExpressionType.Equal)
                return false;


            //// Assert. 
            //if (ExpressionTreeHelpers.IsSpecificMemberExpression(be.Left, declaringType, memberName) &&
            //    ExpressionTreeHelpers.IsSpecificMemberExpression(be.Right, declaringType, memberName))
            //    throw new Exception("Cannot have 'member' == 'member' in an expression!");

            return (ExpressionTreeHelpers.IsSpecificMethodCallExpression(be.Left, declaringType, memberName, methodName) !=
                ExpressionTreeHelpers.IsSpecificMethodCallExpression(be.Right, declaringType, memberName, methodName));
        }

        internal static bool IsSpecificMethodCallExpression(Expression exp, Type declaringType, string memberName, string methodName)
        {
            return ((exp is MethodCallExpression) &&
                AssertDeclaringTypeForMethodCall((MethodCallExpression)exp, declaringType) &&
                AssertMemberNameForMethodCall((MethodCallExpression)exp, memberName) &&
                (((MethodCallExpression)exp).Method.Name == methodName));
        }

        private static bool AssertDeclaringTypeForMethodCall(MethodCallExpression exp, Type declaringType)
        {
            return (((MemberExpression)exp.Object).Expression.Type.Name == declaringType.Name);
        }

        private static bool AssertMemberNameForMethodCall(MethodCallExpression exp, string memberName)
        {
            return (((MemberExpression)exp.Object).Member.Name == memberName);
        }

        internal static TRet GetValueFromExpression<TRet>(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Constant)
                return (TRet)(((ConstantExpression)expression).Value);
            else
                throw new InvalidQueryException(
                    String.Format("The expression type {0} is not supported to obtain a value.", expression.NodeType));
        }
    }
}
