using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VCEL.Core.Expression.Abstract
{
    public class ExpressionNodeTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(IExpressionNode);

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var typeName = jObject["$type"]?.Value<string>();
            if (Enum.TryParse<NodeType>(typeName, out var nodeType))
            {
                return nodeType switch
                {
                    NodeType.Ternary => jObject.ToObject<Ternary>(serializer),
                    NodeType.Let => jObject.ToObject<Let>(serializer),
                    NodeType.Guard => jObject.ToObject<Guard>(serializer),
                    NodeType.LessThan => jObject.ToObject<LessThan>(serializer),
                    NodeType.GreaterThan => jObject.ToObject<GreaterThan>(serializer),
                    NodeType.LessOrEqual => jObject.ToObject<LessOrEqual>(serializer),
                    NodeType.GreaterOrEqual => jObject.ToObject<GreaterOrEqual>(serializer),
                    NodeType.Between => jObject.ToObject<Between>(serializer),
                    NodeType.In => jObject.ToObject<In>(serializer),
                    NodeType.Matches => jObject.ToObject<Matches>(serializer),
                    NodeType.And => jObject.ToObject<And>(serializer),
                    NodeType.Or => jObject.ToObject<Or>(serializer),
                    NodeType.Not => jObject.ToObject<Not>(serializer),
                    NodeType.Value => jObject.ToObject<Value>(serializer),
                    NodeType.List => jObject.ToObject<List>(serializer),
                    NodeType.Add => jObject.ToObject<Add>(serializer),
                    NodeType.Multiply => jObject.ToObject<Multiply>(serializer),
                    NodeType.Subtract => jObject.ToObject<Subtract>(serializer),
                    NodeType.Divide => jObject.ToObject<Divide>(serializer),
                    NodeType.Pow => jObject.ToObject<Pow>(serializer),
                    NodeType.Paren => jObject.ToObject<Paren>(serializer),
                    NodeType.Property => jObject.ToObject<Property>(serializer),
                    NodeType.Function => jObject.ToObject<Function>(serializer),
                    NodeType.UnaryMinus => jObject.ToObject<UnaryMinus>(serializer),
                    NodeType.Null => jObject.ToObject<Null>(serializer),
                    NodeType.Eq => jObject.ToObject<Eq>(serializer),
                    NodeType.NotEq => jObject.ToObject<NotEq>(serializer),
                    NodeType.ObjectMember => jObject.ToObject<ObjectMember>(serializer),
                    _ => throw new ArgumentOutOfRangeException($"Node type not handled '{nodeType}'"),
                };
            }

            throw new Exception($"Type not supported '{typeName}'");
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
