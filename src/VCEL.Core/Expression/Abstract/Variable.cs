using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;
public class Variable : IExpressionNode
{
    public Variable(string name)
    {
        Name = name;
    }

    [JsonProperty("$type")]
    public NodeType Type => NodeType.Variable;
    [JsonProperty("name")]
    public string Name { get; }
}
