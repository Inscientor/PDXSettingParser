using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingParser
{
    public class SettingElement
    {
        public string Name { get; set; }
        public List<SettingElement> Attributes { get; } = new List<SettingElement>();
        public bool HasAttribute => Attributes.Any();
        public bool IsLiteral => Attributes.Count == 1 && !Attributes.First().HasAttribute;

        public override string ToString() => ToString();

        public string ToString(string prepend = "") => IsLiteral
                ? $"{prepend}{Name}={Attributes.First().Name}"
                : Attributes.Aggregate($"{prepend}{Name}={{", (sum, elm) => $"{sum}\r\n{elm.ToString($"{prepend}\t")}") + $"\r\n{prepend}}}";
    }
}
