﻿using NUnit.Framework;
using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.Test;

public class DefaultOperatorTests
{
    private readonly BinaryExprBase<object?> add = new AddExpr<object?>(ExprMonad.Instance, null!, null!);
    private readonly BinaryExprBase<object?> subtract = new SubtractExpr<object?>(ExprMonad.Instance, null!, null!);
    private readonly BinaryExprBase<object?> mult = new MultExpr<object?>(ExprMonad.Instance, null!, null!);
    private readonly BinaryExprBase<object?> div = new DivideExpr<object?>(ExprMonad.Instance, null!, null!);

    [TestCase(0, 0, 0)]
    [TestCase(1, 1, 2)]
    [TestCase(1L, 1, 2L)]
    [TestCase(0x01, 0x01, 0x02)]
    [TestCase(1L, 1d, 2d)]
    [TestCase(1.1f, 1.1d, 2.2d)]
    [TestCase("X", 1.1d, "X1.1")]
    public void Add(object l, object r, object expected)
    {
        var res = add.Evaluate(l, r);
        Assert.That(res, Is.EqualTo(expected).Within(0.000001));
    }

    [TestCase(0, 0, 0)]
    [TestCase(3, 1, 2)]
    [TestCase(3L, 1, 2L)]
    [TestCase(0x03, 0x01, 0x02)]
    [TestCase(3L, 1d, 2d)]
    [TestCase(3.3f, 1.1d, 2.2d)]
    public void Subtract(object l, object r, object expected)
    {
        var res = subtract.Evaluate(l, r);
        Assert.That(res, Is.EqualTo(expected).Within(0.000001));
    }

    [TestCase(0, 0, 0)]
    [TestCase(2, 5, 10)]
    [TestCase(2L, 5, 10L)]
    [TestCase(0x02, 0x05, 0x0A)]
    [TestCase(2L, 5d, 10d)]
    [TestCase(2.2f, 5.5d, 12.1d)]
    public void Multiply(object l, object r, object expected)
    {
        var res = mult.Evaluate(l, r);
        Assert.That(res, Is.EqualTo(expected).Within(0.000001));
    }

    [TestCase(6, 2, 3)]
    [TestCase(5, 2, 2.5)]
    [TestCase(10L, 5, 2L)]
    [TestCase(0x0A, 0x02, 0x05)]
    [TestCase(10L, 2d, 5d)]
    [TestCase(12.1f, 5.5d, 2.2d)]
    public void Divide(object l, object r, object expected)
    {
        var res = div.Evaluate(l, r);
        Assert.That(res, Is.EqualTo(expected).Within(0.000001));
    }
}