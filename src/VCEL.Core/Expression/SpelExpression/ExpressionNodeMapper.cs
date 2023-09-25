using System;
using Spring.Expressions;
using VCEL.Core.Expression.Impl;

namespace VCEL.Core.Expression.SpelExpression
{
    public class SpelExpressionNodeMapper<T>
    {
        public BaseNode ToExpressionNode(IExpression<T>? node)
        {
            return node switch
            {
                Ternary<T> e => Setup(new TernaryNode(), ToExpressionNode(e.ConditionExpr), ToExpressionNode(e.TrueExpr), ToExpressionNode(e.FalseExpr)),
                LetExpr<T> e => throw new Exception("Let expressions are not supported in SPEL"),
                GuardExpr<T> e => throw new Exception("Guard expressions are not supported in SPEL"),
                LessThan<T> e => Setup(new OpLess(), ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                GreaterThan<T> e => Setup(new OpGreater(), ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                LessOrEqual<T> e => Setup(new OpLessOrEqual(), ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                GreaterOrEqual<T> e => Setup(new OpGreaterOrEqual(), ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                BetweenExpr<T> e => Setup(new OpBetween(), ToExpressionNode(e.Left), ToExpressionNode(e.Lower), ToExpressionNode(e.Upper)),
                InSetExpr<T> e => throw new Exception("InSet expressions are not supported in SPEL"), // TODO - can we make this work?
                InExpr<T> e => Setup(new OpIn(), ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                SpreadExpr<T> e => throw new Exception("Spread expressions are not supported in SPEL"),
                MatchesExpr<T> e => Setup(new OpMatches(), ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                AndExpr<T> e => new OpAND(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                OrExpr<T> e => new OpOR(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                NotExpr<T> e => new OpNOT(ToExpressionNode(e.Expr)),
                ListExpr<T> e => throw new Exception("List expressions are not supported in SPEL"),
                AddExpr<T> e => Setup(new OpADD(), ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                MultExpr<T> e => Setup(new OpMULTIPLY(), ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                SubtractExpr<T> e => Setup(new OpSUBTRACT(), ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                DivideExpr<T> e => Setup(new OpDIVIDE(), ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                PowExpr<T> e => Setup(new OpPOWER(), ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                // ParenExpr<T> e => Setup(new Spring.Expressions.(), ToExpressionNode(e.Expr)),
                // Property<T> e => Setup(new PropertyNode(), new StringNode(e.Name)),
                // FunctionExpr<T> e => new Function(e.Name, e.Args.Select(ToExpressionNode).ToArray()),
                UnaryMinusExpr<T> e => Setup(new OpNotEqual(), ToExpressionNode(e.Expr)),
                NullExpr<T> _ => new NullLiteralNode(),
                EqExpr<T> e => Setup(new OpEqual(), ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                NotEqExpr<T> e => Setup(new OpNotEqual(), ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                ObjectMember<T> e => throw new Exception("ObjectMember expressions are not supported in SPEL"),
                BoolExpr<T> e => new BooleanLiteralNode(e.Value.ToString()), // TODO - to string??
                DoubleExpr<T> e => new RealLiteralNode(), // TODO - to string?? 
                IntExpr<T> e => Setup(new IntLiteralNode(), e.Value.ToString()), // TODO - to string??
                LongExpr<T> e => Setup(new IntLiteralNode(), e.Value.ToString()), // TODO - to string??
                StringExpr<T> e => new StringLiteralNode(e.Value),
                DateTimeOffsetExpr<T> e => throw new Exception("DateTimeOffset expressions are not supported in SPEL"),
                TimeSpanExpr<T> e => throw new Exception("TimeSpan expressions are not supported in SPEL"),
                SetExpr<T> e => throw new Exception("Set expressions are not supported in SPEL"),
                ValueExpr<T, object> e => throw new Exception("Value expressions are not supported in SPEL"),
                _ => throw new Exception($"Expression node type not handled '{node?.GetType()}'"),
            };
        }
        
        private BaseNode Setup(BaseNode node, BaseNode firstChild, params BaseNode[] nextSiblings)
        {
            node.setFirstChild(firstChild);
            BaseNode current = firstChild;
            foreach (var nextSibling in nextSiblings)
            {
                current.setNextSibling(nextSibling);
                current = nextSibling;
            }

            return node;
        }

        private BaseNode Setup(BaseNode node, string? text)
        {
            node.setText(text);
            return node;
        }
    }

    // class SpringASTFactory : ASTFactory
    // {
    //     static readonly SpringASTCreator Creator = new SpringASTCreator();
    //     private static readonly Type BASENODE_TYPE;
    //     private static readonly Hashtable Typename2Creator;
    //
    //     static SpringASTFactory()
    //     {
    //         BASENODE_TYPE = typeof (SpringAST);
    //
    //         Typename2Creator = new Hashtable();
    //         foreach (Type type in typeof(Spring.Expressions.TernaryNode).Assembly.GetTypes())
    //         {
    //             if (BASENODE_TYPE.IsAssignableFrom(type) && !type.IsAbstract)
    //             {
    //                 ConstructorInfo ctor = type.GetConstructor(System.Type.EmptyTypes);
    //                 if (ctor != null)
    //                 {
    //                     ASTNodeCreator creator = new ASTNodeCreator(ctor);
    //                     Typename2Creator[creator.ASTNodeTypeName] = creator;
    //                 }
    //             }
    //         }
    //         Typename2Creator[BASENODE_TYPE.FullName] = Creator;
    //     }
    //
    //     public SpringASTFactory() : base(BASENODE_TYPE)
    //     {
    //         base.defaultASTNodeTypeObject_ = BASENODE_TYPE;
    //         base.typename2creator_ = Typename2Creator;
    //     }
    // }
    //
    // class SpringASTCreator
    // {
    //     public AST Create()
    //     {
    //         return new SpringAST();
    //     }
    //
    //     public string ASTNodeTypeName => typeof(SpringAST).FullName;
    // }
    //
    // class ASTNodeCreator
    // {
    //     private readonly SafeConstructor ctor;
    //
    //     public ASTNodeCreator(ConstructorInfo ctor)
    //     {
    //         this.ctor = new SafeConstructor(ctor);
    //         this.ASTNodeTypeName = ctor.DeclaringType!.FullName!;
    //     }
    //
    //     public AST Create()
    //     {
    //         return (AST) ctor.Invoke(Array.Empty<object>());
    //     }
    //
    //     public string ASTNodeTypeName { get; }
    // }
}
