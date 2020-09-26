using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingParser
{
    public class CKParser:IDisposable
    {
        private bool _disposed = false;

        private StreamReader file;
        public string FilePath { get; }

        public CKParser(string filePath)
        {
            FilePath = filePath;
            file = new StreamReader(FilePath);
        }

        public IEnumerable<SettingElement> ReadSettings()
        {
            var scope = new Stack<SettingElement>();
            string line;
            while((line = file.ReadLine()) != null)
            {
                // コメントを除去
                var formula = line.Split('#')[0].Trim('\t');
                // 空白行なら飛ばす
                if (string.IsNullOrWhiteSpace(formula))
                    continue;

                // 関心の要素
                var setElm = new SettingElement();
                var data = formula.Split('=');

                setElm.Name = data[0].Trim();
                if (setElm.Name == "}")
                {
                    // スコープから出る
                    var elm = scope.Pop();

                    // 上位にポップした値を投げる
                    if (scope.Any())
                        scope.Peek().Attributes.Add(elm);
                    else
                        yield return elm;

                    continue;
                }

                string value = data[1].Trim();
                if (value == "{")
                {
                    // スコープに入る
                    scope.Push(setElm);
                }
                else
                {
                    // リテラルを作る
                    setElm.Attributes.Add(new SettingElement() { Name = value });

                    // スコープに格納する
                    if (scope.Any())
                        scope.Peek().Attributes.Add(setElm);
                    else
                        yield return setElm;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    file.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
