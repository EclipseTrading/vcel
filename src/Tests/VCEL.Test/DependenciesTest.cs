using NUnit.Framework;
using System.Linq;
using VCEL.Core.Lang;

namespace VCEL.Test
{
    public class DependenciesTest
    {
        [TestCase("null")]
        [TestCase("5")]
        [TestCase("'X'")]
        [TestCase("X", "X")]
        [TestCase("X + Y", "X", "Y")]
        [TestCase("A / B", "A", "B")]
        [TestCase("X ? (A + B) : (C / D)", "X", "A", "B", "C", "D")]
        [TestCase("X between {1, 2}", "X")]
        [TestCase("X between {Y, 2}", "X", "Y")]
        [TestCase("X between {Y, Z}", "X", "Y", "Z")]
        [TestCase("X in {A, B}", "X", "A", "B")]
        [TestCase("let X = A / B in X + C", "A", "B", "C")]
        [TestCase("match | X == A = B | X == C = D | otherwise E", "X", "A", "B", "C", "D", "E")]
        public void PropertyDeps(string exprString, params string[] deps)
        {
            var expr = VCExpression.ParseDefault(exprString);
            var depList = expr.Expression.Dependencies.ToList();
            Assert.That(depList, Is.All.TypeOf<PropDependency>());
            var propDefs = depList.OfType<PropDependency>().ToList();
            Assert.That(propDefs.Select(p => p.Name), Is.EquivalentTo(deps));
        }

        [Test]
        public void NowDep()
        {
            var expr = VCExpression.ParseDefault("A / now()");
            var deps = expr.Expression.Dependencies.ToList();
            Assert.That(deps, Is.EquivalentTo(new IDependency[] { new PropDependency("A"), TemporalDependency.Now }));
        }
        [Test]
        public void TodayDep()
        {
            var expr = VCExpression.ParseDefault("A / today()");
            var deps = expr.Expression.Dependencies.ToList();
            Assert.That(deps, Is.EquivalentTo(new IDependency[] { new PropDependency("A"), TemporalDependency.Today }));
        }

        [Test]
        public void MixedDependencies()
        {
            var expr = VCExpression.ParseDefault("A ? now() : today()");
            var deps = expr.Expression.Dependencies.ToList();
            Assert.That(deps, Is.EquivalentTo(new IDependency[] { new PropDependency("A"), TemporalDependency.Now, TemporalDependency.Today }));
        }

        [Test]
        public void FuncDependency()
        {
            var expr = VCExpression.ParseDefault("1 + f()", ("f", _ => 1));
            var deps = expr.Expression.Dependencies.ToList();
            Assert.That(deps, Is.EquivalentTo(new IDependency[] { new FuncDependency("f") }));
        }

        [Test]
        public void NoDependency()
        {
            var expr = VCExpression.ParseDefault("1 + 1");
            var deps = expr.Expression.Dependencies.ToList();
            Assert.That(deps, Is.Empty);
        }
    }
}
